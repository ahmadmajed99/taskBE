using System;
using Microsoft.EntityFrameworkCore;
using task5.Models;

namespace task5.DatabaseConnections
{
	public class DatabaseContext : DbContext
	{
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base (options)
		{
		}
	}
}

