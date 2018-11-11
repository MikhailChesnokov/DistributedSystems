namespace ConsoleApplication.DataSource
{
    using System;



    internal sealed class FlatTableRow
    {
        public int Id { get; set; }

        
        public string ClientName { get; set; }

        public string ClientEmail { get; set; }

        public string ClientPhoneNumber { get; set; }


        public string PartName { get; set; }

        public double PartPrice { get; set; }

        public bool PartPresence { get; set; }


        public int OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }


        public string ShopAddress { get; set; }

        public string ShopCity { get; set; }
    }
}