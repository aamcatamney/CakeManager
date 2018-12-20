using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CakeManager.Services
{
  public interface ICakeService
  {
    Task<List<string>> GetFilesAsync();
    Task RunFileAsync(string FilePath);
  }

  public class CakeService : BaseService, ICakeService
  {
    private readonly ILogWritingService logWritingService;
    public CakeService(ISettingsService settingsService, ILogWritingService logWritingService) : base(settingsService)
    {
      this.logWritingService = logWritingService;
    }
    public async Task<List<string>> GetFilesAsync()
    {
      return Directory.GetFiles((await this.settingsService.GetSettingsAsync()).CakeDir).ToList();
    }
    public async Task RunFileAsync(string FilePath)
    {
      var settings = await this.settingsService.GetSettingsAsync();
      var logFile = Path.Combine(settings.LogDir, "MyFile.txt");
      await Task.Run(() =>
       {
         var psi = new ProcessStartInfo();
         psi.FileName = "powershell";
         psi.Arguments = $" {FilePath}";
         psi.WorkingDirectory = settings.CakeDir;
         psi.CreateNoWindow = true;
         psi.UseShellExecute = false;
         psi.RedirectStandardOutput = true;
         psi.RedirectStandardError = true;
         var proc = new Process();
         proc.EnableRaisingEvents = true;
         proc.StartInfo = psi;
         proc.OutputDataReceived += (s, d) =>
         {
           this.logWritingService.WriteToLog(logFile, d.Data);
         };
         proc.ErrorDataReceived += (s, d) =>
         {
           this.logWritingService.WriteToLog(logFile, d.Data);
         };
         proc.Start();
         proc.BeginOutputReadLine();
         proc.BeginErrorReadLine();
         proc.WaitForExit();
       });
    }
  }
}