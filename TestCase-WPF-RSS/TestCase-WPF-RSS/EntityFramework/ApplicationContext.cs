using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System;

namespace TestCase_WPF_RSS.EntityFramework
{
    internal class ApplicationContext : DbContext
    {

        public DbSet<DatabaseConfiguration> DatabaseConfiguration { get; set; } = null!;
        public string connectionString;

        public ApplicationContext(string connectionString)
        {
            this.connectionString = connectionString;   // получаем извне строку подключения
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        static public bool TestConnection(string connectionString)
        {
            bool result = false;

            try
            {
                using (ApplicationContext db = new ApplicationContext(connectionString))
                {
                    var dbcnf = db.DatabaseConfiguration.ToList();
                    foreach (DatabaseConfiguration dbc in dbcnf)
                    {
                        result &= result;
                    }
                }
            }
            catch(Exception ex) { }


            return result;
        }
    }

    internal class DatabaseConfiguration { }
}
