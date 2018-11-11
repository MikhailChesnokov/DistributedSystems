namespace ConsoleApplication.DataDestination
{
    using Domain;
    using Microsoft.EntityFrameworkCore;



    public abstract class CommonContext : DbContext
    {
        protected CommonContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        
        
        public DbSet<Client> Client { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<Part> Part { get; set; }

        public DbSet<Shop> Shop { get; set; }

        public DbSet<OrderPart> OrderPart { get; set; }
    }
}