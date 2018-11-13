namespace ConsoleApplication.ExcelGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using Domain;
    using OfficeOpenXml;



    internal static class ExcelGenerator
    {
        public static Stream GenerateExcel(this IDictionary<Type, IEnumerable<IEntity>> entities)
        {
            string tempFileName = Path.GetTempFileName();
            
            File.Copy("ExcelGeneration/Templates/Default.xlsx", tempFileName, overwrite: true);
            
            using (var package = new ExcelPackage(new FileInfo(tempFileName)))
            {
                FillContent(package, entities);
                
                package.Save();
            }

            return File.Open(tempFileName, FileMode.Open, FileAccess.Read);
        }

        private static void FillContent(ExcelPackage excel, IDictionary<Type, IEnumerable<IEntity>> entities)
        {
            int sheet = 1;
            
            entities.Values.ToList().ForEach(entitiesSet =>
            {
                var column = 'A';
                var row = 1;
                
                var properties = entitiesSet
                    .First()
                    .GetType()
                    .GetProperties()
                    .Where(p => p.PropertyType.IsPrimitive || new[] {typeof(string), typeof(decimal)}.Contains(p.PropertyType))
                    .ToList();
                    
                properties.ForEach(p => excel.Write(p.Name).OnSheet(sheet).Fill(Color.PowderBlue).To(column++.ToString(), 1));

                column = 'A';
                row++;
                
                entitiesSet.ToList().ForEach(e =>
                {
                    properties.ForEach(p => excel.Write(e.GetType().GetProperties().First(y => y.Name == p.Name).GetValue(e)).OnSheet(sheet).To(column++.ToString(), row));
                    
                    column = 'A';
                    row++;
                });

                sheet++;
            });
        }
        
        public static void SaveToFile(this Stream stream, string name)
        {
            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, (int)stream.Length);
            
            File.WriteAllBytes(name, bytes);
        }
    }
}