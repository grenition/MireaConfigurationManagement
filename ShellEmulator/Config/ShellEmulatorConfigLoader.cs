using Tomlyn;

namespace MireaConfigurationManagement.ShellEmulator.Config;

public class ShellEmulatorConfigLoader : IDisposable
{
    public static string ConfigPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "shellEmulatorConfig.toml");

    public ShellEmulatorConfig LoadConfig()
    {
        var configText = File.ReadAllText(ConfigPath);
        var options = new TomlModelOptions
        {
            ConvertPropertyName = name => name
        };
        
        return Toml.ToModel<ShellEmulatorConfig>(configText, null, options);
    }
    
    public void Dispose()
    {
        
    }
}
