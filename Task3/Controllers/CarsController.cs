using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.Cars;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Cars")]
    public class CarsController : Controller
    {
        // GET: api/Cars
        [HttpGet]
        public IEnumerable<Vehicle> Get()
        {
            return Parking.Parking.Instance.Cars;
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        public IActionResult Get(int? id)
        {
            if (id == null) return BadRequest();

            var foundCar = Parking.Parking.Instance.Cars.FirstOrDefault(car => car.Id == id);
            if (foundCar != null)
            {
                return Ok(foundCar);
            }

            return NotFound();
        }

        // POST: api/Cars
        [HttpPost]
        public IActionResult Post([FromBody]Car car)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(x => x.Value.Errors, (y, z) => z.Exception.Message);

                return BadRequest(errors);
            }

            var vehicle = Parking.Parking.Instance.CreateCar(car.Balance, car.CarType);
            Parking.Parking.Instance.AddCarToParking(vehicle);
            return Ok(201);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if (id == null) return BadRequest();
            var balance = Parking.Parking.Instance.RemoveCar(id.Value);
            if (!balance.HasValue)
                return NotFound();

            if (balance <= 0)
                return BadRequest();

            return NoContent();
        }
    }
}
