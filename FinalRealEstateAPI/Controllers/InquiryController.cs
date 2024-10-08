﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.Dto.Inquiry;

namespace FinalRealEstateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquiryController : ControllerBase
    {
        private readonly IInquiryService _inquiryService;

        public InquiryController(IInquiryService inquiryService)
        {
            _inquiryService = inquiryService;
        }

        // Get all inquiries
        [HttpGet]
        public IActionResult GetAllInquiries()
        {
            var inquiries = _inquiryService.GetAllInquiries();
            return Ok(inquiries);
        }

        // Get inquiry by ID
        [HttpGet("{id}")]
        public IActionResult GetInquiryById(int id)
        {
            var inquiry = _inquiryService.GetInquiryById(id);
            if (inquiry == null)
                return NotFound("Inquiry not found.");

            return Ok(inquiry);
        }

        // Get inquiries by user ID
        [HttpGet("user/{userId}")]
        public IActionResult GetInquiriesByUserId(int userId)
        {
            try
            {
                var inquiries = _inquiryService.GetInquiriesByUserId(userId);
                return Ok(inquiries);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Get inquiries by property ID
        [HttpGet("property/{propertyId}")]
        public IActionResult GetInquiriesByPropertyId(int propertyId)
        {
            try
            {
                var inquiries = _inquiryService.GetInquiriesByPropertyId(propertyId);
                return Ok(inquiries);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Create a new inquiry
        [HttpPost]
        public IActionResult CreateInquiry([FromBody] InquiryInsertDto inquiryInsertDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _inquiryService.CreateInquiry(inquiryInsertDto);
            return Ok("Inquiry created successfully.");
        }

        // Update an existing inquiry
        [HttpPut]
        public IActionResult UpdateInquiry([FromBody] InquiryUpdateDto inquiryUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _inquiryService.UpdateInquiry(inquiryUpdateDto);
            return Ok("Inquiry updated successfully.");
        }

        // Delete an inquiry by ID
        [HttpDelete("{id}")]
        public IActionResult DeleteInquiry(int id)
        {
            try
            {
                _inquiryService.DeleteInquiry(id);
                return Ok("Inquiry deleted successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Get inquiries by date range
        [HttpGet("dateRange")]
        public IActionResult GetInquiriesByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var inquiries = _inquiryService.GetInquiriesByDateRange(startDate, endDate);
                return Ok(inquiries);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
