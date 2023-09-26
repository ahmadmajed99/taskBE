using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace task5.Models
{
	public class User
	{
		[Key]
		public Guid Id { get; set; }

		[StringLength(255)]
		public string Username { get; set; }	

        [Required]
		public string Password { get; set; }

	}
}

