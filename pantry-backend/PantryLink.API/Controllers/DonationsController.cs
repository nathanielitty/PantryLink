using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryLink.Core.Entities;
using PantryLink.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;

namespace PantryLink.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DonationsController> _logger;

        public DonationsController(ApplicationDbContext context, ILogger<DonationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/donations/food
        [HttpPost("food")]
        public async Task<ActionResult<FoodDonation>> SubmitFoodDonation(FoodDonationRequest request)
        {
            try
            {
                // Validate that the pantry exists
                var pantry = await _context.Pantries.FindAsync(request.PantryId);
                if (pantry == null)
                {
                    return BadRequest("Pantry not found");
                }

                var donation = new FoodDonation
                {
                    PantryId = request.PantryId,
                    DonorName = request.ContactInfo.Name,
                    DonorEmail = request.ContactInfo.Email,
                    DonorPhone = request.ContactInfo.Phone,
                    Notes = request.Notes,
                    PreferredPickupTime = request.PreferredPickupTime,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Pending"
                };

                // Add donation items
                foreach (var item in request.Items)
                {
                    donation.Items.Add(new DonationItem
                    {
                        Name = item.Name,
                        Quantity = item.Quantity,
                        Unit = item.Unit,
                        Category = item.Category,
                        ExpirationDate = item.ExpirationDate
                    });
                }

                _context.FoodDonations.Add(donation);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Food donation submitted successfully with ID: {donation.Id}");

                return Ok(new { Id = donation.Id, Message = "Food donation submitted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting food donation");
                return StatusCode(500, "An error occurred while submitting the donation");
            }
        }

        // POST: api/donations/money
        [HttpPost("money")]
        public async Task<ActionResult<MonetaryDonation>> SubmitMonetaryDonation(MonetaryDonationRequest request)
        {
            try
            {
                // Validate that the pantry exists
                var pantry = await _context.Pantries.FindAsync(request.PantryId);
                if (pantry == null)
                {
                    return BadRequest("Pantry not found");
                }

                var donation = new MonetaryDonation
                {
                    PantryId = request.PantryId,
                    Amount = request.Amount,
                    DonorName = request.ContactInfo.Name,
                    DonorEmail = request.ContactInfo.Email,
                    DonorPhone = request.ContactInfo.Phone,
                    PaymentMethod = request.PaymentMethod,
                    IsRecurring = request.IsRecurring,
                    Notes = request.Notes,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Pending"
                };

                _context.MonetaryDonations.Add(donation);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Monetary donation submitted successfully with ID: {donation.Id}");

                // In a real application, you would integrate with a payment processor here
                // For now, we'll just mark it as processed
                donation.Status = "Processed";
                donation.TransactionId = $"TXN_{DateTime.UtcNow:yyyyMMddHHmmss}_{donation.Id}";
                await _context.SaveChangesAsync();

                return Ok(new { Id = donation.Id, TransactionId = donation.TransactionId, Message = "Monetary donation processed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting monetary donation");
                return StatusCode(500, "An error occurred while processing the donation");
            }
        }

        // GET: api/donations/food
        [HttpGet("food")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FoodDonation>>> GetFoodDonations()
        {
            try
            {
                var donations = await _context.FoodDonations
                    .Include(d => d.Pantry)
                    .Include(d => d.Items)
                    .OrderByDescending(d => d.CreatedAt)
                    .ToListAsync();

                return Ok(donations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving food donations");
                return StatusCode(500, "An error occurred while retrieving donations");
            }
        }

        // GET: api/donations/money
        [HttpGet("money")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MonetaryDonation>>> GetMonetaryDonations()
        {
            try
            {
                var donations = await _context.MonetaryDonations
                    .Include(d => d.Pantry)
                    .OrderByDescending(d => d.CreatedAt)
                    .ToListAsync();

                return Ok(donations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving monetary donations");
                return StatusCode(500, "An error occurred while retrieving donations");
            }
        }

        // GET: api/donations/pantry/{pantryId}
        [HttpGet("pantry/{pantryId}")]
        [Authorize]
        public async Task<ActionResult> GetDonationsByPantry(int pantryId)
        {
            try
            {
                var foodDonations = await _context.FoodDonations
                    .Include(d => d.Items)
                    .Where(d => d.PantryId == pantryId)
                    .OrderByDescending(d => d.CreatedAt)
                    .ToListAsync();

                var monetaryDonations = await _context.MonetaryDonations
                    .Where(d => d.PantryId == pantryId)
                    .OrderByDescending(d => d.CreatedAt)
                    .ToListAsync();

                return Ok(new { FoodDonations = foodDonations, MonetaryDonations = monetaryDonations });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving donations for pantry {PantryId}", pantryId);
                return StatusCode(500, "An error occurred while retrieving donations");
            }
        }
    }

    // DTOs for requests
    public class FoodDonationRequest
    {
        [Required]
        public int PantryId { get; set; }
        
        [Required]
        public ContactInfo ContactInfo { get; set; }
        
        [Required]
        public List<DonationItemRequest> Items { get; set; } = new List<DonationItemRequest>();
        
        public DateTime PreferredPickupTime { get; set; }
        public string Notes { get; set; }
    }

    public class DonationItemRequest
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public string Unit { get; set; }
        
        public string Category { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }

    public class MonetaryDonationRequest
    {
        [Required]
        public int PantryId { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public ContactInfo ContactInfo { get; set; }
        
        [Required]
        public string PaymentMethod { get; set; }
        
        public bool IsRecurring { get; set; } = false;
        public string Notes { get; set; }
    }

    public class ContactInfo
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Phone { get; set; }
    }
}
