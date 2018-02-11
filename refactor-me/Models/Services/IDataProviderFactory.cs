using System.Data;

namespace refactor_me.Models.Services
{
    public interface IDataProviderFactory
    {
        IDbCommand RunCommand(string commandText, string connectionName);
    }
}
