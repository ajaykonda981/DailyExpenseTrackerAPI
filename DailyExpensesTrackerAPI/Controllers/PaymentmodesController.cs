using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DailyExpensesTrackerAPI.Models;

namespace DailyExpensesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentmodesController : ControllerBase
    {
        private readonly DAILYEXPENSETRACKERContext _context;

        public PaymentmodesController(DAILYEXPENSETRACKERContext context)
        {
            _context = context;
        }

        // GET: api/Paymentmodes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paymentmode>>> GetPaymentmode()
        {
            return await _context.Paymentmode.ToListAsync();
        }

        // GET: api/Paymentmodes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Paymentmode>> GetPaymentmode(int id)
        {
            var paymentmode = await _context.Paymentmode.FindAsync(id);

            if (paymentmode == null)
            {
                return NotFound();
            }

            return paymentmode;
        }

        // PUT: api/Paymentmodes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentmode(int id, Paymentmode paymentmode)
        {
            if (id != paymentmode.Id)
            {
                return BadRequest();
            }

            _context.Entry(paymentmode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentmodeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Paymentmodes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Paymentmode>> PostPaymentmode(Paymentmode paymentmode)
        {
            _context.Paymentmode.Add(paymentmode);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentmode", new { id = paymentmode.Id }, paymentmode);
        }

        // DELETE: api/Paymentmodes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Paymentmode>> DeletePaymentmode(int id)
        {
            var paymentmode = await _context.Paymentmode.FindAsync(id);
            if (paymentmode == null)
            {
                return NotFound();
            }

            _context.Paymentmode.Remove(paymentmode);
            await _context.SaveChangesAsync();

            return paymentmode;
        }

        private bool PaymentmodeExists(int id)
        {
            return _context.Paymentmode.Any(e => e.Id == id);
        }
    }
}
