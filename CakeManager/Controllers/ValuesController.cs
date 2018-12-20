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
  public class ValuesController : ControllerBase
  {
    private readonly ISettingsService settings;
    public ValuesController(ISettingsService settings)
    {
      this.settings = settings;
    }
    // GET api/values
    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> GetAsync()
    {
      var settings = await this.settings.GetSettingsAsync();
      return new string[] { "value1", "value2", settings.CakeDir };
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public ActionResult<string> Get(int id)
    {
      return "value";
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
