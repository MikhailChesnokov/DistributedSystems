namespace ConsoleApplication
{
    using DataDestination;
    using Domain.Converter;
    using static DataSource.MsAccess.MsAccessHelper;



    class Program
    {
        static void Main()
        {
            LoadFlatTableRows()
                .ConvertToEntities()
                .SaveToMySql()
                .SaveToSqlServer();
        }
    }
}