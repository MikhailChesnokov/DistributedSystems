namespace ConsoleApplication
{
    using static Producer.DataSource.MsAccess.MsAccessHelper;
    using System.Collections.Generic;
    using AutoMapper;
    using Consumer.DataDestination;
    using Consumer.Domain.Converter;
    using Consumer.ExcelGeneration;
    using Producer.DataSource;
    
    
    public static class Program
    {
        static Program()
        {
            Mapper.Initialize(x => x.CreateMap<FlatTableRow, Consumer.FlatTableRow>());
        }
        
        public static void Main()
        {
            Mapper.Map<IEnumerable<Consumer.FlatTableRow>>(LoadFlatTableRows())
                .ConvertToEntities()
                .SaveToMySql()
                .GenerateExcel()
                .SaveToFile("result.xlsx");
        }
    }
}