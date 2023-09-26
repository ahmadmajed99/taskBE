using System;
using System.ComponentModel.DataAnnotations;

namespace task5.Models
{
	public class Product
	{
        [Key]
        public Guid Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public string Date { get; set; }
    }
}

