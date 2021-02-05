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
    [Route("nw/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        [HttpGet]
        [Route("")]   // esimerkkinä routen paikka vaikkakin tässä tyhjä
        public ActionResult GetAllUsers() //Hakee kaikki rivit
        {
            northwindContext db = new northwindContext();

            try
            {
                var users = from u in db.Logins
                                select new Login 
                                {
                                    LoginId = u.LoginId,
                                    UserName = u.UserName,
                                    Firstname = u.Firstname,
                                    Lastname = u.Lastname,
                                    Email = u.Email,
                                    AccessLevelId = u.AccessLevelId
                                };

                var loginsList = users.ToList();

                return Ok(loginsList);
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


        // Lisäys
        [HttpPost]
        [Route("")]
        public ActionResult PostCreateNew([FromBody] Login uusiuser)
        {

            northwindContext db = new northwindContext();
            try
            {
                uusiuser.PassWord = BCrypt.Net.BCrypt.HashPassword(uusiuser.PassWord);  // Tässä hashataan salana Brycptilla kantaan
                uusiuser.UserName = uusiuser.UserName;
                uusiuser.Firstname = uusiuser.Firstname;
                uusiuser.Lastname = uusiuser.Lastname;
                uusiuser.Email = uusiuser.Email;
                uusiuser.AccessLevelId = uusiuser.AccessLevelId;
                uusiuser.Token = "";

                db.Logins.Add(uusiuser);
                db.SaveChanges();
                return Ok("Käyttäjä lisätty id:llä " + uusiuser.LoginId);
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
        public ActionResult PutEdit(int primarykey, [FromBody] Login u)
        {
            northwindContext db = new northwindContext();
            try
            {
                Login user = db.Logins.Find(primarykey);
                if (user != null)
                {
                    if (u.PassWord == "")
                    {
                        user.UserName = u.UserName;
                        user.Firstname = u.Firstname;
                        user.Lastname = u.Lastname;
                        user.Email = u.Email;
                        user.AccessLevelId = u.AccessLevelId;
                        user.PassWord =user.PassWord;  // Salasanaa ei vaihdeta

                        db.SaveChanges();
                        return Ok("Käyttäjän ID " + user.LoginId + " tiedot päivitetty.");
                    } 
                    else
                    {
                        user.UserName = u.UserName;
                        user.Firstname = u.Firstname;
                        user.Lastname = u.Lastname;
                        user.Email = u.Email;
                        user.AccessLevelId = u.AccessLevelId;
                        user.PassWord = BCrypt.Net.BCrypt.HashPassword(u.PassWord);  // Tässä hashataan salana Brycptilla kantaan

                        db.SaveChanges();
                        return Ok("Käyttäjän ID " + user.LoginId + " tiedot päivitetty.");
                    }
                   
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
                Login poistettava = db.Logins.Find(primarykey);
                if (poistettava != null)
                {
                    db.Logins.Remove(poistettava);
                    db.SaveChanges();
                    return Ok("Käyttäjä id " + poistettava.LoginId + " poistettu.");
                }
                else
                {
                    return NotFound("Käyttäjää ei löytynyt.");
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
