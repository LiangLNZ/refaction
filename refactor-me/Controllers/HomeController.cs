using System.Web.Mvc;

namespace refactor_me.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return Redirect("~/Swagger");
        }
    }
}