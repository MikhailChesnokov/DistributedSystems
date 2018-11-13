namespace ConsoleApplication
{
    using DataDestination;
    using Domain.Converter;
    using ExcelGeneration;
    using static DataSource.MsAccess.MsAccessHelper;



    class Program
    {
        static void Main()
        {
            LoadFlatTableRows()
                .ConvertToEntities()
                .SaveToMySql()
                .SaveToSqlServer()
                .GenerateExcel().SaveToFile("result.xlsx");
        }
    }
}