using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCTest.Data;

namespace MVCTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class ProductAuditController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductAuditController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get(DateTime? from, DateTime? to)
        {
            var audits = _context.ProductAudits
                .Where(a => (!from.HasValue || a.ChangedAt >= from.Value) && (!to.HasValue || a.ChangedAt <= to.Value))
                .ToList();

            return Ok(audits);
        }
    }
}
