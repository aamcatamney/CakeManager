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
        if (File.Exists(config["SettingsPath"]))
        {
          var json = await File.ReadAllTextAsync(config["SettingsPath"]);
          settings = JsonConvert.DeserializeObject<Settings>(json);

          if (!watching)
          {
            new FileSystemWatcher(new FileInfo(config["SettingsPath"]).DirectoryName).Changed += (c, e) =>
            {
              if (e.FullPath.Equals(config["SettingsPath"], StringComparison.OrdinalIgnoreCase))
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
  }
}