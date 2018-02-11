using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using System.Web.Routing;
using refactor_me.Api;
using refactor_me.Models.Services;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.WebApi;


namespace refactor_me
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly Container Container = new Container();
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configuration.Filters.Add(new LogExceptionFilterAttribute());

            //Simple Injector 
            Container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            Container.Register<IConfigurationManagerWapper, ConfigurationManagerWapper>(Lifestyle.Singleton);
            Container.Register<IDataProviderFactory, DataProviderFactory>(Lifestyle.Singleton);
            Container.Register<IProductService, ProductService>(Lifestyle.Scoped);
            Container.Register<IProductOptionService, ProductOptionService>(Lifestyle.Scoped);

            Container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            Container.Verify();
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(Container);
        }

        protected void Application_End()
        {
            Container.Dispose();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            ErrorLogService.LogError(Server.GetLastError());
        }
    }
}
