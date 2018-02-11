namespace refactor_me.Models.Services
{
    public interface IConfigurationManagerWapper
    {
        string GetConnectionString(string name);
        string GetDbProviderName(string name);
        string GetAppSettingValue(string key);
    }
}
