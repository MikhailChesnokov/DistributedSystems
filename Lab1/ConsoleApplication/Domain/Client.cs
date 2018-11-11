namespace ConsoleApplication.Domain
{
    using System;
    using System.Collections.Generic;



    public sealed class Client : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        
        public ISet<Order> Orders { get; set; } = new HashSet<Order>(new OrderComparer());
        
        
        public override bool Equals(object obj) => new ClientComparer().Equals(this, obj as Client);

        public override int GetHashCode() => new ClientComparer().GetHashCode(this);
    }

    internal sealed class ClientComparer : IEqualityComparer<Client>
    {
        public bool Equals(Client x, Client y) => x.Name.Equals(y.Name, StringComparison.InvariantCultureIgnoreCase);

        public int GetHashCode(Client obj) => obj.Name.ToLower().GetHashCode();
    }
}