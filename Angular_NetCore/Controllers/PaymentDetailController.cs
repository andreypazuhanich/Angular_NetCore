using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Angular_NetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Angular_NetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailController : ControllerBase
    {
        private readonly PaymentDetailContext _paymentDetailContext;

        public PaymentDetailController(PaymentDetailContext paymentDetailContext)
        {
            _paymentDetailContext = paymentDetailContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDetail>>> GetPaymentDetails() => 
            await _paymentDetailContext.PaymentDetails.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDetail>> GetPaymentDetailsById([FromQuery] int id) =>
            await _paymentDetailContext.PaymentDetails.FirstOrDefaultAsync(s => s.PMId == id);

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentDetail([FromQuery]int id, [FromBody]PaymentDetail paymentDetail)
        {
            if (id != paymentDetail.PMId)
                return BadRequest();
            
            _paymentDetailContext.Entry(paymentDetail).State = EntityState.Modified;

            try
            {
                await _paymentDetailContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PaymentDetailExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDetail>> PostPaymentDetaul(PaymentDetail paymentDetail)
        {
            _paymentDetailContext.PaymentDetails.Add(paymentDetail);
            await _paymentDetailContext.SaveChangesAsync();

            return CreatedAtAction("GetPaymentDetailsById", new {id = paymentDetail.PMId}, paymentDetail);
        }

        [HttpDelete]
        public async Task<ActionResult<PaymentDetail>> DeletePaymentDetail(int id)
        {
            var paymentDetail = await _paymentDetailContext.PaymentDetails.FindAsync(id);
            if (paymentDetail == null)
                return NotFound();
            _paymentDetailContext.PaymentDetails.Remove(paymentDetail);
            await _paymentDetailContext.SaveChangesAsync();
            return paymentDetail;
        }
        
        

        private bool PaymentDetailExists(int id)
        {
            return _paymentDetailContext.PaymentDetails.Any(s => s.PMId == id);
        }
    }
}