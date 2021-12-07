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
    public class DailyexpensesController : ControllerBase
    {
        private readonly DAILYEXPENSETRACKERContext _context;

        public DailyexpensesController(DAILYEXPENSETRACKERContext context)
        {
            _context = context;
        }

        // GET: api/Dailyexpenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetDailyexpenses()
        {
            var dailyExpenses = await _context.Dailyexpenses.OrderByDescending(x => x.ExpensesDate).ToListAsync();
            var categories = await _context.Category.ToListAsync();
            var paymentModes = await _context.Paymentmode.ToListAsync();



            //var result = (from de in dailyExpenses
            //              join ct in categories on de.Category equals ct.Id
            //              join pm in paymentModes on de.PaymentMode equals pm.Id
            //              where de.IsDeleted == false 

            //              select new
            //              {
            //                  de.Id,
            //                  de.Category,
            //                  ct.CategoryName,
            //                  de.ExpensesDate,
            //                  de.PaymentMode,
            //                  pm.PaymentMode1,
            //                  de.PaymentDate,
            //                  de.Amount

            //              }).ToList();

            var result = (from de in _context.Dailyexpenses
                          join ct in _context.Category on de.Category equals ct.Id
                          join pm in _context.Paymentmode on de.PaymentMode equals pm.Id
                          where de.IsDeleted == false
                          orderby de.ExpensesDate descending
                          select new
                          {
                              de.Id,
                              de.Category,
                              ct.CategoryName,
                              de.ExpensesDate,
                              de.PaymentMode,
                              pm.PaymentMode1,
                              de.PaymentDate,
                              de.Amount
                          }).ToList();
            return result;
        }

        // GET: api/Dailyexpenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetDailyexpenses(int id)
        {
            var dailyexpenses = await _context.Dailyexpenses.FindAsync(id);
            var paymentModes = await _context.Paymentmode.FindAsync(dailyexpenses.PaymentMode);
            var category = await _context.Category.FindAsync(dailyexpenses.Category);
            var filesList =  _context.Fileuploader.Where(x => x.DailyExpensesId == id);

            var result = new
            {
                dailyexpenses.Id,
                dailyexpenses.Category,
                dailyexpenses.ExpensesDate,
                dailyexpenses.PaymentMode,
                dailyexpenses.PaymentDate,
                dailyexpenses.Amount,
                dailyexpenses.Comments,
                dailyexpenses.Attachments,
                paymentModes.PaymentMode1,
                category.CategoryName,
                filesList


            };


            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // PUT: api/Dailyexpenses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDailyexpenses(int id, Dailyexpenses dailyexpenses)
        {
            if (id != dailyexpenses.Id)
            {
                return BadRequest();
            }

            _context.Entry(dailyexpenses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DailyexpensesExists(id))
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

        // POST: api/Dailyexpenses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Dailyexpenses>> PostDailyexpenses(Dailyexpenses dailyexpenses)
        {
            _context.Dailyexpenses.Add(dailyexpenses);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDailyexpenses", new { id = dailyexpenses.Id }, dailyexpenses);
        }

        // DELETE: api/Dailyexpenses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDailyexpenses(int id, [FromQuery]string reason)
        {
            var dailyexpenses = await _context.Dailyexpenses.FindAsync(id);
            dailyexpenses.IsDeleted = true;
            dailyexpenses.ReasonForDeleting = reason;
            _context.Entry(dailyexpenses).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DailyexpensesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        private bool DailyexpensesExists(int id)
        {
            return _context.Dailyexpenses.Any(e => e.Id == id);
        }

        [HttpGet("ExpenseCalculator")]
        public async Task<ActionResult<dynamic>> GetDashboardExpenses()
        {
            double totalExpenseTillDate = 0;
            double totalExpensesThisMonth = 0;
            double totalExpesnsesToday = 0;
            DateTime now = DateTime.Now;
            var startDateOfMonth = new DateTime(now.Year, now.Month, 1);
            var endDateOfMonth = startDateOfMonth.AddMonths(1).AddDays(-1);

            var dailyExpenses = await _context.Dailyexpenses.ToListAsync();
            

            var expensesTillDate = (from de in dailyExpenses
                          where de.IsDeleted == false
                          select new
                          {
                              de.Amount
                          }).ToList();
            var expensesThisMonth = (from de in dailyExpenses
                                     where de.IsDeleted == false &&
                                     (de.ExpensesDate >= startDateOfMonth && de.ExpensesDate <= endDateOfMonth)
                                     select new
                                     {
                                         de.Amount
                                     }).ToList();
            var expensesToday = (from de in dailyExpenses
                                     where de.IsDeleted == false &&
                                     (de.ExpensesDate == DateTime.Today)
                                     select new
                                     {
                                         de.Amount
                                     }).ToList();
            foreach (var amount in expensesTillDate)
            {
                totalExpenseTillDate = totalExpenseTillDate + Convert.ToDouble(amount.Amount);
            }

            foreach (var amount in expensesThisMonth)
            {
                totalExpensesThisMonth = totalExpensesThisMonth + Convert.ToDouble(amount.Amount);
            }

            foreach (var amount in expensesToday)
            {
                totalExpesnsesToday = totalExpesnsesToday + Convert.ToDouble(amount.Amount);
            }



            return new
            {
                totalExpenseTillDate = totalExpenseTillDate,
                totalExpensesThisMonth = totalExpensesThisMonth,
                totalExpesnsesToday = totalExpesnsesToday
            };
        }
    }
}
