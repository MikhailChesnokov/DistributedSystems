namespace Consumer.Domain
{
    using System.Collections.Generic;
    
    
    public sealed class PartShop : IEntity
    {
        public int Id { get; set; }

        public Part Part { get; set; }

        public Shop Shop { get; set; }
        
        
        public override bool Equals(object obj) => new PartShopComparer().Equals(this, obj as PartShop);
        
        public override int GetHashCode() => new PartShopComparer().GetHashCode(this);
    }
    
    internal sealed class PartShopComparer : IEqualityComparer<PartShop>
    {
        public bool Equals(PartShop x, PartShop y) =>
            x.Shop.Equals(y.Shop) &&
            x.Part.Equals(y.Part);

        public int GetHashCode(PartShop obj) => (obj.Part.GetHashCode(), obj.Shop.GetHashCode()).GetHashCode();
    }
}