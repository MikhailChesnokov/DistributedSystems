namespace Producer.DataSource.MsAccess
{
    using System.Collections.Generic;
    using System.Linq;


    public static class MsAccessHelper
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