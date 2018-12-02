namespace ConsoleApplication.Domain
{
    using System;
    using System.Collections.Generic;



    public sealed class Part : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public bool Presence { get; set; }

        public ISet<PartShop> PartShops { get; set; } = new HashSet<PartShop>(new PartShopComparer());

        public ISet<OrderPart> OrderParts { get; set; } = new HashSet<OrderPart>(new OrderPartComparer());
        
        
        public override bool Equals(object obj) => new PartComparer().Equals(this, obj as Part);
        
        public override int GetHashCode() => new PartComparer().GetHashCode(this);
    }

    internal sealed class PartComparer : IEqualityComparer<Part>
    {
        public bool Equals(Part x, Part y) => x.Name.Equals(y.Name, StringComparison.InvariantCultureIgnoreCase);

        public int GetHashCode(Part obj) => obj.Name.ToLower().GetHashCode();
    }
}