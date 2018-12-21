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
    public async Task<List<string>> GetFiles() => await this.cakeService.GetFilesAsync();

    [HttpGet, Route("run")]
    public async Task RunFile(string FilePath) => await this.cakeService.RunFileAsync(FilePath);
  }
}
