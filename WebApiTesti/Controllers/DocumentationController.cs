using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTesti.Models;

namespace WebApiTesti.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {

        [HttpGet]
        [Route("{keycode}")]  // osoite on host/documentation/keycode
        public ActionResult Documentation(string keycode)
        {
            northwindContext db = new northwindContext();

            try
            {
                var isInDatabase = db.Documentations.Any(r => r.Keycode == keycode);

                if (isInDatabase) // jos koodi löytyy tietokannasta
                {
                    List<Documentation> lista = (from x in db.Documentations
                                                 where x.Keycode == keycode
                                                 select x).ToList();
                    return Ok(lista);
                }
                else
                {
                    return BadRequest("Empty " + DateTime.Now.ToString());
                }
            }
            catch (Exception)
            {
                return BadRequest("Virhe.");
            }
            finally
            {
                db.Dispose();  // vapauttaa db objektin lopuksi
            }

        }

        // vaihtoehtoinen tapa hakea samat tiedot videon perusteella
        [HttpGet]
        [Route("vaihtoehto/{key}")]  // osoite on host/documentation/vaihtoehto/key
        public ActionResult GetDoc(string key)
        {
            northwindContext db = new northwindContext();

            List<Documentation> lista = (from x in db.Documentations
                                           where x.Keycode == key
                                           select x).ToList();
            if (lista.Count > 0)
            {
                db.Dispose();
                return Ok(lista);
            }
            else
            {
                db.Dispose();
                return BadRequest("Empty " + DateTime.Now.ToString());
            }
        }

        [HttpPost] // sallii vain POST-metodit eli uuden luomisen
        [Route("")] // Route tyhjä tässä eli ei ole edes tarpeellinen
        public ActionResult PostCreateNew([FromBody] Documentation uusi) // [FromBody] = HTTP-pyynnön Body:ssä välitetään JSON-muodossa oleva objekti ,joka on Documentation-tyyppinen uusi-niminen
        {
            northwindContext db = new northwindContext();
            try
            {
                db.Documentations.Add(uusi);
                db.SaveChanges();
                return Ok("Tallennettu id:llä " + uusi.DocumentationId);  // palautetaan äsken luodun ID
            }
            catch (Exception)
            {
                return BadRequest("Virhe lisättäessä.");
            }
            finally
            {
                db.Dispose();  // vapauttaa db objektin lopuksi
            }
        }

        [HttpPut] // Sallii vain PUT-metodit eli muokkaamisen
        [Route("{primarykey}")] // Routemääritys tietokannan pääavaimella /api/documentation/pääavain
        public ActionResult PutEdit(int primarykey, [FromBody] Documentation uudetTiedot) // [FromBody] = HTTP-pyynnön Body:ssä välitetään JSON-muodossa oleva objekti ,joka on Documentation-tyyppinen uudetTiedot-niminen
        { 
            northwindContext db = new northwindContext(); 
            try
            {
                Documentation muokattava = db.Documentations.Find(primarykey);   // etsitään tietokannasta pääavaimella
                if (muokattava != null)                                 	  // jos löytyy niin siihen sijoitetaan tuodut tiedot
                {
                    muokattava.AvailableRoute = uudetTiedot.AvailableRoute;
					muokattava.Method = uudetTiedot.Method;
					muokattava.Description = uudetTiedot.Description;
					muokattava.Keycode = uudetTiedot.Keycode;

                    db.SaveChanges();
                    return Ok("Muutokset tallennettu.");
                }
                else
                {
                    return NotFound("Ei löytynyt!");
                }
            }
            catch (Exception e)
            {
                    return BadRequest("Virhe päivitettäessä." + e);
            }
            finally
            {
                db.Dispose();
            }
        }

        [HttpDelete]  // Sallii vain Delete-metodit eli poistamisen
        [Route("{primarykey}")]  // Routemääritys tietokannan pääavaimella /api/documentation/pääavain
        public ActionResult Delete(int primarykey)
        {
            northwindContext db = new northwindContext();
            try
            {
                Documentation poistettava = db.Documentations.Find(primarykey);  // etsitään tietokannasta pääavaimella
                if (poistettava != null)                              			// jos löytyy niin yritetään poistoa
                {
                    try
                    {
                        db.Documentations.Remove(poistettava);
                        db.SaveChanges();
                        return Ok("Dokumentti id " + poistettava.DocumentationId + " poistettu.");
                    }
                    catch (Exception)
                    {
                        return BadRequest("Virhe poistettaessa.");
                    }
                }
                else
                {
                    return NotFound("Dokumenttiä ei löytynyt.");
                }
            }
            finally
            {
                db.Dispose();
            }
        }

    }
}
