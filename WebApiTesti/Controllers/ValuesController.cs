using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTesti.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2", "testi" };  // palauttaa string taulukon
        }

        // GET api/values/Jokunimi
        [HttpGet("{nimi}")]
        public ActionResult<string> Get(string nimi)  // parametrin tyyppi sellainen joka otetaan vastaan eli tässä string
        {
            return "Terve " + nimi + "!";
        }

        // GET api/values/Etunimi/Sukunimi
        [HttpGet("{etunimi}/{sukunimi}")]
        public ActionResult<string> Get(string etunimi, string sukunimi)  // otetaan vastaan kaksi parametria
        {
            return "Moikka " + etunimi + " " + sukunimi + "!";
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
