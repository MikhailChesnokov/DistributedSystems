namespace ConsoleApplication.ExcelGeneration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
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
                    .Where(p => !typeof(IEnumerable).IsAssignableFrom(p.PropertyType) || p.PropertyType == typeof(string))
                    .ToList();

                // sheet title
                excel.Workbook.Worksheets[sheet].Name = entitiesSet.GetType().GenericTypeArguments.First().Name;
                
                // column headers
                properties.ForEach(p => excel.Write(p.Name).OnSheet(sheet).Fill(Color.PowderBlue).To(column++.ToString(), 1));

                column = 'A';
                row++;
                
                // values
                entitiesSet.ToList().ForEach(e =>
                {
                    properties.ForEach(p => excel.Write(GetPropertyValue(e.GetType().GetProperties().First(y => y.Name == p.Name),e)).OnSheet(sheet).To(column++.ToString(), row));
                    
                    column = 'A';
                    row++;
                });

                sheet++;
            });
        }

        private static object GetPropertyValue(PropertyInfo propertyInfo, IEntity entity)
        {
            if (propertyInfo.PropertyType.IsPrimitive ||
                propertyInfo.PropertyType == typeof(string) ||
                propertyInfo.PropertyType == typeof(decimal))
            {
                return propertyInfo.GetValue(entity);
            }

            if (propertyInfo.PropertyType == typeof(DateTime))
            {
                return ((DateTime)propertyInfo.GetValue(entity)).ToLongDateString();
            }

            if (propertyInfo.PropertyType.IsClass &&
                typeof(IEntity).IsAssignableFrom(propertyInfo.PropertyType))
            {
                var relatedObject = propertyInfo.GetValue(entity);

                var relatedObjectId = relatedObject.GetType().GetProperties().First(x => x.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase)).GetValue(relatedObject);

                return relatedObjectId;
            }
            
            throw new NotImplementedException($"Unexpected property type {propertyInfo.PropertyType.Name}.");
        }
        
        public static void SaveToFile(this Stream stream, string name)
        {
            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, (int)stream.Length);
            
            File.WriteAllBytes(name, bytes);
        }
    }
}