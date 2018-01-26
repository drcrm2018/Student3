using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student3.Data;
using Student3.Models;
using Microsoft.Xrm.Sdk;

namespace Student3.API
{
    [Produces("application/json")]
    [Route("api/InquiriesAPI")]
    public class InquiriesAPIController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InquiriesAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/InquiriesAPI
        [HttpGet]
        public IEnumerable<Inquiry> GetInquiry()
        {
            return _context.Inquiry;
        }

        // GET: api/InquiriesAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInquiry([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inquiry = await _context.Inquiry.SingleOrDefaultAsync(m => m.InquiryId == id);

            if (inquiry == null)
            {
                return NotFound();
            }

            return Ok(inquiry);
        }

        // PUT: api/InquiriesAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInquiry([FromRoute] Guid id, [FromBody] Inquiry inquiry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inquiry.InquiryId)
            {
                return BadRequest();
            }

            _context.Entry(inquiry).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InquiryExists(id))
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

        // POST: api/InquiriesAPI
        [HttpPost]
        public async Task<IActionResult> PostInquiry([FromBody] Inquiry inquiry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Inquiry.Add(inquiry);
            await _context.SaveChangesAsync();

            Entity crmInquiry = new Entity("student3_inquiry");
            crmInquiry.Id = inquiry.InquiryId; //keep the ids the same
            crmInquiry["student3_question"] = inquiry.Question;
            crmInquiry["student3_name"] = "Record from API";

            var service = CRM.CrmService.GetServiceProvider();
            service.Create(crmInquiry);

            return CreatedAtAction("GetInquiry", new { id = inquiry.InquiryId }, inquiry);
        }

        // DELETE: api/InquiriesAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInquiry([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inquiry = await _context.Inquiry.SingleOrDefaultAsync(m => m.InquiryId == id);
            if (inquiry == null)
            {
                return NotFound();
            }

            _context.Inquiry.Remove(inquiry);
            await _context.SaveChangesAsync();

            return Ok(inquiry);
        }

        private bool InquiryExists(Guid id)
        {
            return _context.Inquiry.Any(e => e.InquiryId == id);
        }
    }
}