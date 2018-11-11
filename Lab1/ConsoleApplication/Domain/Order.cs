namespace ConsoleApplication.Domain
{
    using System;
    using System.Collections.Generic;



    public sealed class Order : IEntity
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public DateTime Date { get; set; }

        public Client Client { get; set; }

        public ISet<OrderPart> OrderParts { get; set; } = new HashSet<OrderPart>(new OrderPartComparer());
        
        
        public override bool Equals(object obj) => new OrderComparer().Equals(this, obj as Order);
        
        public override int GetHashCode() => new OrderComparer().GetHashCode(this);
    }

    internal sealed class OrderComparer : IEqualityComparer<Order>
    {
        public bool Equals(Order x, Order y) => x.Number == y.Number;

        public int GetHashCode(Order obj) => obj.Number;
    }
}