namespace CakeManager.Services
{
  public abstract class BaseService
  {
    protected readonly ISettingsService settingsService;
    public BaseService(ISettingsService settingsService)
    {
      this.settingsService = settingsService;
    }
  }
}