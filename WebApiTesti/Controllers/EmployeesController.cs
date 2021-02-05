using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTesti.Models;

namespace WebApiTesti.Controllers
{
    [Route("nw/[controller]")]  // nw/employees eli northwind employees
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        // Haku pääavaimella / yksi rivi
        [HttpGet]
        [Route("{primarykey}")]
        public ActionResult GetEmployee(int primarykey)
        {
            northwindContext db = new northwindContext();

            try
            {
                var employee = db.Employees.Find(primarykey);

                if (employee != null)
                {
                    return Ok(employee);
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
        public ActionResult GetAllEmployees()
        {
            northwindContext db = new northwindContext();

            try
            {
                List<Employee> employees = db.Employees.ToList();
                return Ok(employees);
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
        // TÄSSÄ TOINEN VERSIO HAUSTA, JOSSA KUVA ja reportstonavigation() poissa selkeyden vuoksi !!
        [HttpGet]
        [Route("all")]
        public ActionResult GetAllEmployees2()
        {
            northwindContext db = new northwindContext();

            try
            {
                var employees = from e in db.Employees
                                select new Employee
                                {
                                    EmployeeId = e.EmployeeId,
                                    LastName = e.LastName,
                                    FirstName = e.FirstName,
                                    Title = e.Title,
                                    TitleOfCourtesy = e.TitleOfCourtesy,
                                    BirthDate = e.BirthDate,
                                    HireDate = e.HireDate,
                                    Address = e.Address,
                                    City = e.City,
                                    Region = e.Region,
                                    PostalCode = e.PostalCode,
                                    Country = e.Country,
                                    HomePhone = e.HomePhone,
                                    Extension = e.Extension,
                                    Notes = e.Notes,
                                    ReportsTo = e.ReportsTo,
                                    PhotoPath = e.PhotoPath
                                };


                var emplist = employees.ToList();

                return Ok(emplist);
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

        // Haku jollain muulla, kuin pääavaimella, jolloin voi tulla 0-n riviä [Country on käytetty hakuehto]
        [HttpGet]
        [Route("country/{country}")]
        public ActionResult GetSomeEmployees(string country)
        {
            northwindContext db = new northwindContext();

            try
            {
                var isInDatabase = db.Employees.Any(s => s.Country == country);

                if (isInDatabase)
                {
                    var employees = from e in db.Employees
                                    where e.Country == country
                                    select e;

                    var emplist = employees.ToList();

                    return Ok(emplist);
                }
                else
                {
                    return NotFound("Tällä maalla ei löytynyt henkilöitä.");
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


        // Haku jollain muulla, kuin pääavaimella, jolloin voi tulla 0-n riviä [Country on käytetty hakuehto]
        // TÄSSÄ TOINEN VERSIO HAUSTA, JOSSA KUVA ja reportstonavigation() poissa selkeyden vuoksi !!
        [HttpGet]
        [Route("country2/{country}")]
        public ActionResult GetSomeEmployees2(string country)
        {
            northwindContext db = new northwindContext();

            try
            {
                var isInDatabase = db.Employees.Any(s => s.Country == country);

                if (isInDatabase)
                {
                    var employees = from e in db.Employees
                                    where e.Country == country
                                    select new Employee
                                    {
                                        EmployeeId = e.EmployeeId,
                                        LastName = e.LastName,
                                        FirstName = e.FirstName,
                                        Title = e.Title,
                                        TitleOfCourtesy = e.TitleOfCourtesy,
                                        BirthDate = e.BirthDate,
                                        HireDate = e.HireDate,
                                        Address = e.Address,
                                        City = e.City,
                                        Region = e.Region,
                                        PostalCode = e.PostalCode,
                                        Country = e.Country,
                                        HomePhone = e.HomePhone,
                                        Extension = e.Extension,
                                        Notes = e.Notes,
                                        ReportsTo = e.ReportsTo,
                                        PhotoPath = e.PhotoPath  
                                    };


                    var emplist = employees.ToList();

                    return Ok(emplist);
                }
                else
                {
                    return NotFound("Tällä maalla ei löytynyt henkilöitä.");
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
        public ActionResult PostCreateNew([FromBody] Employee uusiemployee)
        {
            northwindContext db = new northwindContext();
            try
            {
                db.Employees.Add(uusiemployee);
                db.SaveChanges();
                return Ok("Työntekijä lisätty id:llä " + uusiemployee.EmployeeId);
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
        public ActionResult PutEdit(int primarykey, [FromBody] Employee e)
        {
            northwindContext db = new northwindContext();
            try
            {
                Employee emp = db.Employees.Find(primarykey);
                if (emp != null)
                {
                    emp.LastName = e.LastName;
                    emp.FirstName = e.FirstName;
                    emp.Title = e.Title;
                    emp.TitleOfCourtesy = e.TitleOfCourtesy;
                    emp.BirthDate = e.BirthDate;
                    emp.HireDate = e.HireDate;
                    emp.Address = e.Address;
                    emp.City = e.City;
                    emp.Region = e.Region;
                    emp.PostalCode = e.PostalCode;
                    emp.Country = e.Country;
                    emp.HomePhone = e.HomePhone;
                    emp.Extension = e.Extension;
                    emp.Notes = e.Notes;
                    emp.ReportsTo = e.ReportsTo;
                    emp.PhotoPath = e.PhotoPath;

                    db.SaveChanges();
                    return Ok("Työntekijän ID " + emp.EmployeeId + " tiedot päivitetty.");
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
                Employee poistettava = db.Employees.Find(primarykey);
                if (poistettava != null)
                {
                    db.Employees.Remove(poistettava);
                    db.SaveChanges();
                    return Ok("Työntekijä id " + poistettava.EmployeeId + " poistettu.");
                }
                else
                {
                    return NotFound("Työntekijää ei löytynyt.");
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
