using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace TestCase_WPF_RSS.EntityFramework
{
    internal class ApplicationContext : DbContext
    {

        public DbSet<DatabaseConfiguration> DatabaseConfiguration { get; set; } = null!;
        public DbSet<Shipments> Shipments { get; set; } = null!;
        public DbSet<ShipmentStatus> ShipmentStatuses { get; set; } = null!;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shipments>()
                .Property(b => b.Created)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");
            modelBuilder.Entity<Shipments>()
                .Property(b => b.Modified)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");
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

                    var dbsts = db.ShipmentStatuses.ToList();
                    count = dbcnf.Count;

                    if (count == 0)
                    {
                        db.ShipmentStatuses.Add(new ShipmentStatus() { StatusId = ShipmentStatusEnum.Received, StatusText = "Принят" });
                        db.ShipmentStatuses.Add(new ShipmentStatus() { StatusId = ShipmentStatusEnum.ToWarehouse, StatusText = "На склад" });
                        db.ShipmentStatuses.Add(new ShipmentStatus() { StatusId = ShipmentStatusEnum.Sold, StatusText = "Продан" });
                        db.SaveChanges();
                    } 

                    result = true;
                }
            }
            catch (Exception) { }


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

        public ShipmentStatusEnum Status { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Modified { get; set; }
    }

    public enum ShipmentStatusEnum
    {
        // Принят
        Received = 0,

        // На склад
        ToWarehouse = 1,

        // Продан
        Sold = 2
    }

    public class ShipmentStatus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ShipmentStatusEnum StatusId { get; set; }

        public string StatusText { get; set; }
    }
}
