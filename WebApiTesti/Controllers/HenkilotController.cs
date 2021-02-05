using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTesti.Models;

namespace WebApiTesti.Controllers
{
    [Route("omareitti/[controller]")]  // reitti voisi olla mitä tahansa. Tässä siis omareitti
    [ApiController]
    public class HenkilotController : ControllerBase
    {
        [HttpGet] // httpget filtteri
        [Route("merkkijono")]
        public string MerkkiJono()
        {
            return "Testi";
        }

        [HttpGet] // httpget filtteri
        [Route("paivamaara")]
        public DateTime pvm()
        {
            return DateTime.Now;
        }

        [HttpGet] 
        [Route("olio")]
        public Henkilo Olio()   // public henkilo viittaa tekemäämme Henkilo luokkaan. Tarvitsee using lauseen "using webapitesti.models".
        {
            return new Henkilo()  // palauttaa uuden tässä lennossa luotavan Henkilo-luokan olion
            { 
                Nimi = "James Bond",
                Osoite = "Salainentie 13",
                Ika = 33
            };
        }

        [HttpGet]
        [Route("oliolista")]
        public List<Henkilo> OlioLista() // palauttaa listan
        {
            List<Henkilo> henkilot = new List<Henkilo>() // luodaan lista ja annetaan samalla sille kolme oliota
            {
                new Henkilo()
                {
                    Nimi = "Aku Ankka",
                    Osoite = "Paratiisitie 13",
                    Ika = 40
                },
                new Henkilo()
                {
                    Nimi = "Teppo Tulppu",
                    Osoite = "Paratiisitie 11",
                    Ika = 38
                },
                new Henkilo()
                {
                    Nimi = "Sepe Susi",
                    Osoite = "Takametsäntie 22",
                    Ika = 30
                }

            };  // Tämä puolipiste päättää listan luontikomennon
            return henkilot; // Tässä palautetaan yllä luotu oliolista
        }
    }
}
