namespace Consumer.ExcelGeneration
{
    using System.Drawing;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    
    public sealed class WriteContext
    {
        public ExcelPackage Package { get; set; }

        public Color Color { get; set; } = Color.White;

        public object Value { get; set; }

        public (string column, int? row) From { get; set; }

        public int Sheet { get; set; }
    }
    
    
    
    public static class ExcelGenerationExtensions
    {
        internal static WriteContext Write(this ExcelPackage package, object value)
        {
            return new WriteContext
            {
                Package = package,
                Value = value
            };
        }

        internal static WriteContext Fill(this WriteContext context, Color color)
        {
            context.Color = color;

            return context;
        }

        internal static WriteContext OnSheet(this WriteContext context, int sheet)
        {
            context.Sheet = sheet;

            return context;
        }

        public static WriteContext From(this WriteContext context, string column, int row)
        {
            context.From = (column, row);

            return context;
        }
        
        public static void To(this WriteContext context, string column, int row)
        {
            var range =
                context.Package
                       .Workbook
                       .Worksheets[context.Sheet]
                       .Cells[$"{context.From.column ?? column}{context.From.row ?? row}:{column}{row}"];
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            if (context.Color != Color.White)
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(context.Color);
            }
            range.Merge = true;
            range.Value = context.Value;
        }
    }
}