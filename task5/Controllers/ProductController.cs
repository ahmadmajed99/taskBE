using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Channels;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using task5.DatabaseConnections;
using task5.Models;

namespace task5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProductController(DatabaseContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest(new { status = "failed", message = "Invalid product data." });
            }

            try
            {
                // Add the product to the database
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return Ok(new { status = "success", message = "Product added successfully" });
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during database operations
                return StatusCode(500, new { status = "error", message = "An error occurred while adding the product." });
            }
        }


        [HttpGet]
        public IActionResult GetData()
        {
            var products = _context.Products.ToList();

            if (products.Any())
            {
                return Ok(new { status = "success", message = "Data found", data = products });
            }
            else
            {
                return Ok(new { status = "failed", message = "No data found", data = new List<Product>() });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null)
            {
                return BadRequest(new { status = "failed", message = "Invalid product data." });
            }

            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
            {
                return NotFound(new { status = "failed", message = "Product not found." });
            }

            // Update the product properties here, e.g., updatedProduct.Name and updatedProduct.Date
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Date = updatedProduct.Date;

            try
            {
                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(new { status = "success", message = "Product updated successfully" });
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the update
                return StatusCode(500, new { status = "failed", message = "An error occurred while updating the product." });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
            {
                return NotFound(new { status = "failed", message = "Product not found." });
            }

            try
            {
                _context.Products.Remove(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(new { status = "success", message = "Product deleted successfully" });
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the delete operation
                return StatusCode(500, new { status = "failed", message = "An error occurred while deleting the product." });
            }
        }
   
    }
}
