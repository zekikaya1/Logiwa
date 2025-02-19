namespace Logiwa.Web.Application.Configs;

[AttributeUsage(AttributeTargets.Class)]
public class ConfigModelAttribute : System.Attribute
{
    public readonly string ConfigKey;
    public ConfigModelAttribute(string configKey)
    {
        ConfigKey = configKey;
    }
}