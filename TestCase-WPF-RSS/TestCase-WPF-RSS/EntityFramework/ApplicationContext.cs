using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestCase_WPF_RSS.EntityFramework
{
    internal class ApplicationContext : DbContext
    {

        public DbSet<DatabaseConfiguration> DatabaseConfiguration { get; set; } = null!;
        public DbSet<Shipments> Shipments { get; set; } = null!;

        public string СonnectionString;

        public ApplicationContext(string connectionString)
        {
            this.СonnectionString = connectionString;   // получаем извне строку подключения

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(СonnectionString);
        }

        public bool TestConnection(string connectionString)
        {
            bool result = false;

            try
            {
                using (ApplicationContext db = new ApplicationContext(connectionString))
                {
                    var dbcnf = db.DatabaseConfiguration.ToList();
                    int count = dbcnf.Count;

                    if (count == 0)
                    {
                        db.DatabaseConfiguration.Add(new EntityFramework.DatabaseConfiguration() { Created = DateTimeOffset.Now });
                        db.SaveChanges();
                    }

                    result = true;
                }
            }
            catch (Exception ex) { }


            return result;
        }
    }

    internal class DatabaseConfiguration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTimeOffset Created { get; set; }

    }

    /// <summary>
    /// Товары
    /// </summary>
    public class Shipments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ShipmentStatus Status { get; set; }
    }

    public enum ShipmentStatus
    {
        // Принят
        Received = 0,

        // На склад
        ToWarehouse = 1,

        // Продан
        Sold = 2
    }
}
