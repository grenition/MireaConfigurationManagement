using MireaConfigurationManagement.ShellEmulator.Config;
using SharpCompress.Archives.Tar;

namespace MireaConfigurationManagement.ShellEmulator.System;

public class ShellSystem : IDisposable
{
    public string PathPointer { get; set; } = "/";
    public IReadOnlyList<string> Entries => _entries;
    
    private ShellEmulatorConfig _config;
    private List<string> _entries = new();
    
    public ShellSystem()
    {
        Initialize();
    }

    private void Initialize()
    {
        using (var configLoader = new ShellEmulatorConfigLoader())
        {
            _config = configLoader.LoadConfig();
        }
        
        using (Stream stream = File.OpenRead(_config.FileSystemPath))
        using (var archive = TarArchive.Open(stream))
        {
            _entries = archive.Entries
                .Select(x => x.Key.Substring(1, x.Key.Length - 1))
                .ToList();
        }
    }
    
    public void Dispose()
    {
        
    }
}
