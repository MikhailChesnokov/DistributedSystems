namespace ConsoleApplication.DataDestination.SqlServer
{
    using Microsoft.EntityFrameworkCore;



    internal sealed class SqlServerContext : CommonContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\NOTEBOOK;Database=parts;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}