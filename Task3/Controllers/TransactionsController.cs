using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Transactions")]
    public class TransactionsController : Controller
    {

        // GET: api/Transactions/5
        [HttpGet("car/{id}")]
        public IActionResult Get(int? id)
        {
            if (id == null) return BadRequest();

            var result = Parking.Parking.Instance.PrintLogForMinute(id.Value);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet("{option}")]
        public IActionResult Get(string option)
        {
            if (option == null) return BadRequest();

            var result = SelectTransactionsOptions(option);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        private static string SelectTransactionsOptions(string option)
        {
            switch (option)
            {
                case "minute":
                    if (Parking.Parking.Instance.Trasactions.Count == Parking.Parking.Instance.TransactionCapacity)
                    {

                        return Parking.Parking.Instance.PrintLogForMinute();
                    }

                    return null;

                case "log":
                    return Parking.Parking.Instance.Logger.PrintLogFile();
            }

            return null;
        }

        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public IActionResult Put(int? id, [FromBody] decimal value)
        {
            if (id == null && value < 0)
                return BadRequest();

            var carBalance = Parking.Parking.Instance.TopUpBalance(id.Value, value);

            if (carBalance != null)
                return Ok();

            return NotFound(); 
        }

    }
}
