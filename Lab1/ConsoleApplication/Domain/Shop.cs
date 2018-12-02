namespace ConsoleApplication.Domain
{
    using System;
    using System.Collections.Generic;



    public sealed class Shop : IEntity
    {
        public int Id { get; set; }

        public string Address { get; set; }
        
        public string City { get; set; }

        public ISet<PartShop> PartShops { get; set; } = new HashSet<PartShop>(new PartShopComparer());


        public override bool Equals(object obj) => new ShopComparer().Equals(this, obj as Shop);
        
        public override int GetHashCode() => new ShopComparer().GetHashCode(this);
    }


    internal sealed class ShopComparer : IEqualityComparer<Shop>
    {
        public bool Equals(Shop x, Shop y) => x.Address.Equals(y.Address, StringComparison.InvariantCultureIgnoreCase) &&
                                              x.City.Equals(y.City, StringComparison.InvariantCultureIgnoreCase);

        public int GetHashCode(Shop obj) => (obj.Address.ToLower(), obj.City.ToLower()).GetHashCode();
    }
}