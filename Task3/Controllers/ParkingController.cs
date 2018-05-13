using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Parking")]
    public class ParkingController : Controller
    {

        // GET: api/Parking/5
        [HttpGet("{option}")]
        public IActionResult Get(string option)
        {
            if (option == null) return BadRequest();

            var result = SelectParkingOptions(option);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();

        }

        private static string SelectParkingOptions(string option)
        {
            switch (option)
            {
                case "free": 
                    return Parking.Parking.Instance.FreeSpaces().ToString();

                case "occupied":
                    return Parking.Parking.Instance.Cars.Count.ToString();

                case "balance":
                    return Parking.Parking.Instance.Balance.ToString();
            }

            return null;
        }

    }
}
