using refactor_me.Helpers;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace refactor_me.Models.Services
{
    public class DataProviderFactory: IDataProviderFactory
    {
        private readonly IConfigurationManagerWapper _configurationManagerWapper;
        private static string _providerName;
        private static string _connectionString;

        public DataProviderFactory(IConfigurationManagerWapper configurationManagerWapper)
        {
            _configurationManagerWapper = configurationManagerWapper;
        }

        private IDbConnection CreateConnection(string dbName)
        {
            IDbConnection connection = null;
            _providerName = _configurationManagerWapper.GetDbProviderName(dbName);
            _connectionString = _configurationManagerWapper.GetConnectionString(dbName);
            var connstr = _connectionString.Replace(Constants.DataDirectory, HttpContext.Current.Server.MapPath("~/App_Data"));
            switch (_providerName)
            {
                case Constants.MsSqlProviderName:
                    connection = new SqlConnection(connstr);
                    break;
            }
            return connection;
        }

        private static IDbCommand CreateCommand( )
        {
            IDbCommand command = null;
            switch (_providerName)
            {
                case Constants.MsSqlProviderName:
                    command = new SqlCommand();
                    break;
            }
            return command;
        }

        public IDbCommand RunCommand(string commandText, string connectionName)
        {
            var conn = CreateConnection(connectionName);
            conn.Open();

            using (var cmd = CreateCommand())
            {
                cmd.CommandText = commandText;
                cmd.Connection = conn;
                return cmd;
            }
        }
    }
}