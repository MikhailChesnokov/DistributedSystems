namespace ConsoleApplication.DataSource.MsAccess
{
    using System.Collections.Generic;
    using System.Linq;



    internal static class MsAccessHelper
    {
        public static IEnumerable<FlatTableRow> LoadFlatTableRows()
        {
            using (var context = new MsAccessContext())
            {
                return context.MainTable.ToList();
            }
        }
    }
}