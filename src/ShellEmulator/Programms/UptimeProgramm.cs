using MireaConfigurationManagement.ShellEmulator.Programms.Base;
using MireaConfigurationManagement.ShellEmulator.System;

namespace MireaConfigurationManagement.ShellEmulator.Programms;

public class UptimeProgramm : IShellProgramm
{
    public string Key => "uptime";
    public async Task Execute(IEnumerable<string> args, ShellSystem system)
    {
        TimeSpan uptime = GetUptime();
        Console.WriteLine($"System Uptime: {FormatUptime(uptime)}");
    }
    
    public static TimeSpan GetUptime()
    {
        long uptimeMilliseconds = Environment.TickCount64;
        return TimeSpan.FromMilliseconds(uptimeMilliseconds);
    }
    public static string FormatUptime(TimeSpan uptime)
    {
        if (uptime.TotalDays >= 1)
        {
            return $"{(int)uptime.TotalDays} days, {uptime.Hours:00}:{uptime.Minutes:00}";
        }
        else
        {
            return $"{uptime.Hours:00}:{uptime.Minutes:00}";
        }
    }
}
