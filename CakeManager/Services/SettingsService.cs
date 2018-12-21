using System;
using System.IO;
using System.Threading.Tasks;
using CakeManager.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CakeManager.Services
{
  public interface ISettingsService
  {
    Task<Settings> GetSettingsAsync();
  }

  public class SettingsService : ISettingsService
  {
    private readonly IConfiguration config;
    private bool hasLoadedAndUnchanged = false;
    private bool watching = false;
    private Settings settings;
    public SettingsService(IConfiguration config)
    {
      this.config = config;
    }
    public async Task<Settings> GetSettingsAsync()
    {
      if (!hasLoadedAndUnchanged || settings != null)
      {
        var filePath = config["SettingsPath"];
        var dirPath = new FileInfo(filePath).DirectoryName;
        if (Directory.Exists(dirPath) && !File.Exists(filePath))
        {
          ScaffoldDefault();
        }
        if (File.Exists(filePath))
        {
          var json = await File.ReadAllTextAsync(filePath);
          settings = JsonConvert.DeserializeObject<Settings>(json);

          if (!watching)
          {
            new FileSystemWatcher(dirPath).Changed += (c, e) =>
            {
              if (e.FullPath.Equals(filePath, StringComparison.OrdinalIgnoreCase))
              {
                this.hasLoadedAndUnchanged = false;
              }
            };
            this.watching = true;
          }

          this.hasLoadedAndUnchanged = true;
        }
      }
      return settings;
    }

    private void ScaffoldDefault()
    {
      var filePath = config["SettingsPath"];
      var dirPath = new FileInfo(filePath).DirectoryName;
      var settings = new Settings();

      settings.WorkingDir = Path.Combine(dirPath, "Working");
      settings.LogDir = Path.Combine(dirPath, "Logs");
      settings.CakeDir = Path.Combine(dirPath, "Cake");
      settings.ProjectDir = Path.Combine(dirPath, "Projects");

      if (!Directory.Exists(settings.WorkingDir)) Directory.CreateDirectory(settings.WorkingDir);
      if (!Directory.Exists(settings.LogDir)) Directory.CreateDirectory(settings.LogDir);
      if (!Directory.Exists(settings.CakeDir)) Directory.CreateDirectory(settings.CakeDir);
      if (!Directory.Exists(settings.ProjectDir)) Directory.CreateDirectory(settings.ProjectDir);

      File.WriteAllText(filePath, JsonConvert.SerializeObject(settings));
    }
  }
}