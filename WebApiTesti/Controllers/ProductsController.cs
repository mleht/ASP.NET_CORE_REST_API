using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTesti.Models;

namespace WebApiTesti.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("nw/[controller]")]  // nw/products eli northwind products
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Haku pääavaimella / yksi rivi
        [HttpGet]
        [Route("{primarykey}")]
        public ActionResult GetProduct(int primarykey)
        {
            northwindContext db = new northwindContext();

            try
            {
                var tuote = db.Products.Find(primarykey);

                if(tuote != null)
                {
                    return Ok(tuote);
                }
                else
                {
                    return NotFound("Ei löytynyt");
                }
                
            }
            catch (Exception)
            {

                return BadRequest();
            }
            finally
            {
                db.Dispose(); 
            }
        }


        // Haku kaikki rivit
        [HttpGet]
        [Route("")]   
        public ActionResult GetAllProducts() 
        {
            northwindContext db = new northwindContext();

            try
            {
                List<Product> tuotteet = db.Products.ToList();
                return Ok(tuotteet);
            }
            catch (Exception)
            {

                return BadRequest();
            }
            finally
            {
                db.Dispose();
            }
              
        }

        // Haku kaikki rivit, mutta ottaa vastaan "sivutus" parametrit
        [HttpGet]
        [Route("R")]
        public IActionResult GetProducts(int offset, int limit)
        {
            northwindContext db = new northwindContext();
            try
            {
                List<Product> tuotteet = db.Products.Skip(offset).Take(limit).ToList();
                return Ok(tuotteet);
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



        // Haku jollain muulla, kuin pääavaimella, jolloin voi tulla 0-n riviä [SupplierID on käytetty hakuehto]
        [HttpGet]
        [Route("supplier/{id}")]
        public ActionResult GetSomeProducts(int id)
        {
            northwindContext db = new northwindContext();
            try
            {
                var isInDatabase = db.Products.Any(s => s.SupplierId == id);

                if (isInDatabase)
                {
                    var tuotteet = from p in db.Products
                                   where p.SupplierId == id
                                   select p;

                    var tuotelista = tuotteet.ToList();

                    return Ok(tuotelista);
                }
                else
                {
                    return NotFound("Toimittajakoodilla ei löytynyt toimittajaa.");
                }
                
            }
            catch
            {
                return BadRequest();
            }
            finally
            {
                db.Dispose();
            }
        }

        // Lisäys
        [HttpPost]
        [Route("")]
        public ActionResult PostCreateNew([FromBody] Product uusituote)
        {
            northwindContext db = new northwindContext();
            try
            {
                db.Products.Add(uusituote);
                db.SaveChanges();
                return Ok("Tuote lisätty id:llä " + uusituote.ProductId);
            }
            catch (Exception)
            {
                return BadRequest("Virhe lisättäessä.");
            }
            finally
            {
                db.Dispose();
            }

        }

        // Päivitys

        [HttpPut]
        [Route("{primarykey}")]
        public ActionResult PutEdit(int primarykey, [FromBody] Product uudetTiedot)
        {
            northwindContext db = new northwindContext();
            try
            {
                Product tuote = db.Products.Find(primarykey);
                if (tuote != null)
                {
                    tuote.ProductName = uudetTiedot.ProductName;
                    tuote.SupplierId = uudetTiedot.SupplierId;
                    tuote.CategoryId = uudetTiedot.CategoryId;
                    // tuote.QuantityPerUnit = uudetTiedot.QuantityPerUnit;
                    tuote.UnitPrice = uudetTiedot.UnitPrice;
                    // tuote.UnitsInStock = uudetTiedot.UnitsInStock;
                    // tuote.UnitsOnOrder = uudetTiedot.UnitsOnOrder;
                    // tuote.ReorderLevel = uudetTiedot.ReorderLevel;
                    tuote.Discontinued = uudetTiedot.Discontinued;
                    // tuote.ImageLink = uudetTiedot.ImageLink;

                    db.SaveChanges();
                    return Ok("Tuotteen ID " + tuote.ProductId + " tiedot päivitetty.");
                }
                else
                {
                    return NotFound("Tuotetta ei löytynyt.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Virhe päivitettäessä.");
            }
            finally
            {
                db.Dispose();
            }
        }

        // poisto
        [HttpDelete]
        [Route("{primarykey}")]
        public ActionResult Delete(int primarykey)
        {
            northwindContext db = new northwindContext();
            try
            {
                Product poistettava = db.Products.Find(primarykey);
                if (poistettava != null)
                {
                    db.Products.Remove(poistettava);
                    db.SaveChanges();
                    return Ok("Tuote id " + poistettava.ProductId + " poistettu.");
                }
                else
                {
                    return NotFound("Tuotetta ei löytynyt.");
                }
            }
            catch
            {
                return BadRequest("Virhe poistettaessa.");
            }
            finally
            {
                db.Dispose();
            }
        }

    }
}
