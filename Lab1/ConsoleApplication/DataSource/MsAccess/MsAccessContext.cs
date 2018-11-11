namespace ConsoleApplication.DataSource.MsAccess
{
    using EntityFrameworkCore.Jet;
    using Microsoft.EntityFrameworkCore;



    internal sealed class MsAccessContext : DbContext
    {
        internal DbSet<FlatTableRow> MainTable { get; set; }

        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseJet("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=DataSource/MsAccess/Database1.accdb; Persist Security Info=False;");
        }
    }
}