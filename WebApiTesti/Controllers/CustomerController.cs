using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApiTesti.Models;

namespace WebApiTesti.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("nw/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        [HttpGet]
        [Route("R")]
        public IActionResult GetSomeCustomers(int offset, int limit, string country)
        {
            northwindContext db = new northwindContext();
            try
            {
                if (country != null) // Jos parametri ei ole null eli on annettu
                {
                    List<Customer> asiakkaat = db.Customers.Where(c => c.Country == country).Take(limit).ToList();
                    return Ok(asiakkaat);
                }
                else 
                {
                    List<Customer> asiakkaat = db.Customers.Skip(offset).Take(limit).ToList();
                    return Ok(asiakkaat);
                }
            }
            catch
            {
                return BadRequest("Virhe.");
            }
            finally
            {
                db.Dispose();
            }
        }


        [HttpGet]
        [Route("")]   // esimerkkinä routen paikka vaikkakin tässä tyhjä
        public List<Customer> GetAllCustomers() //Hakee kaikki rivit
        {
            northwindContext db = new northwindContext();  // tietokantayhteys
            List<Customer> asiakkaat = db.Customers.ToList();  // tietokannaan customers sisältö asiakkaat nimiselle listalle
            db.Dispose(); // tietokantayhteysolio pois
            return asiakkaat;   // palautetaan luotu lista
        }

        [HttpGet]
        [Route("{id}")]
        public Customer GetOneCustomer(string id) //Find-metodi hakee aina vain PÄÄAVAIMELLA yhden tuloksen
        {
            northwindContext db = new northwindContext();
            Customer asiakas = db.Customers.Find(id);
            db.Dispose();
            return asiakas;
        }

        [HttpGet]
        [Route("country/{key}")]  // reittimääritykseen tehty eroa edelliseen tuon country/finland verran (finland esimerkki hausta
        public List<Customer> GetSomeCustomers(string key) //Hakee jollain tiedolla mätsäävät rivit. Voi olla enemmän kuin yksi, siksi lista.
        {
            northwindContext db = new northwindContext();

            var someCustomers = from c in db.Customers
                                where c.Country == key
                                select c;
 
            // db.Dispose(); // Tässä kohtaa Dispose ei toimi vaan kaataisi sovelluksen
            return someCustomers.ToList();
        }

        [HttpPost] // sallii vain POST-metodit eli uuden luomisen
        [Route("")] // Route tyhjä tässä eli ei ole edes tarpeellinen
        public ActionResult PostCreateNew([FromBody] Customer asiakas) // [FromBody] = HTTP-pyynnön Body:ssä välitetään JSON-muodossa oleva objekti ,joka on Customers-tyyppinen asiakas-niminen
        {
            northwindContext db = new northwindContext(); 
            try
            {
                db.Customers.Add(asiakas);
                db.SaveChanges();
                return Ok("Asiakas tallennettu id:llä "+ asiakas.CustomerId);  // palautetaan äsken luodun asiakkaan customerid
            }
            catch (Exception)
            {
                return BadRequest("Virhe asiakasta lisättäessä.");
            }
            finally
            {
                db.Dispose();  // vapauttaa db objektin lopuksi
            }

        }


        [HttpPut] // Sallii vain PUT-metodit eli muokkaamisen
        [Route("{customerid}")] // Routemääritys pääavaimella /nw/customer/pääavain
        public ActionResult PutEdit(string customerid, [FromBody] Customer asiakas) // [FromBody] = HTTP-pyynnön Body:ssä välitetään JSON-muodossa oleva objekti ,joka on Customers-tyyppinen asiakas-niminen
        {
            northwindContext db = new northwindContext(); 
            try
            {
                Customer muokattava = db.Customers.Find(customerid);   // etsitään asiakas tietokannasta pääavaimella
                if (muokattava != null)                                 // jos asiakas löytyy niin siihen sijoitetaan tuodut tiedot
                {
                    muokattava.CompanyName = asiakas.CompanyName;
                    muokattava.ContactName = asiakas.ContactName;
                    muokattava.ContactTitle = asiakas.ContactTitle;
                    muokattava.Country = asiakas.Country;
                    muokattava.Address = asiakas.Address;
                    muokattava.City = asiakas.City;
                    muokattava.PostalCode = asiakas.PostalCode;
                    muokattava.Phone = asiakas.Phone;
                    muokattava.Fax = asiakas.Fax;

                    db.SaveChanges();
                    return Ok("Asiakas id " + muokattava.CustomerId + " muutokset tallennettu.");
                }
                else
                {
                    return NotFound("Asiakasta ei löytynyt!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Virhe asiakasta päivitettäessä.");
            }
            finally
            {
                db.Dispose();
            }
        }


        [HttpDelete]  // Sallii vain Delete-metodit eli poistamisen
        [Route("{customerid}")]  // Routemääritys pääavaimella /nw/customer/pääavain
        public ActionResult DeleteCustomer(string customerid)
        {
            northwindContext db = new northwindContext();
            try
            {
                Customer poistettava = db.Customers.Find(customerid);  // etsitään asiakas tietokannasta pääavaimella
                if (poistettava != null)                              // jos asiakas löytyy niin yritetään poistoa
                {
                    try
                    {
                        db.Customers.Remove(poistettava);
                        db.SaveChanges();
                        return Ok("Asiakas id " + customerid + " poistettu.");
                    }
                    catch (Exception)
                    {
                        return BadRequest("Virhe asiakasta poistettaessa.");
                    }
                }
                else
                {
                    return NotFound("Asiakasta ei löytynyt.");
                }
            }
            finally
            {
                db.Dispose();
            }
        }

    }

}
