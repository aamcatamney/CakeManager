using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CakeManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CakeManager.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CakeController : ControllerBase
  {
    private readonly ICakeService cakeService;
    public CakeController(ICakeService cakeService)
    {
      this.cakeService = cakeService;
    }

    [HttpGet]
    public async Task<List<string>> GetFilesAsync() => await this.cakeService.GetFilesAsync();

    [HttpPost]
    public async Task RunFileAsync(string FilePath) => await this.cakeService.RunFileAsync(FilePath);
  }
}
