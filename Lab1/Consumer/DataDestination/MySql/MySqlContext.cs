namespace Consumer.DataDestination.MySql
{
    using Microsoft.EntityFrameworkCore;

    
    
    internal sealed class MySqlContext : CommonContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Database=parts;User=root;Password= ;SslMode=none"); // TODO password
        }
    }
}