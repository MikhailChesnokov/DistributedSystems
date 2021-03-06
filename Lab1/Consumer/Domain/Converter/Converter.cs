using System;
using System.Collections.Generic;
using System.Linq;

namespace Consumer.Domain.Converter
{
    public static class Converter
    {
        private static readonly ISet<Client> Clients = new HashSet<Client>(new ClientComparer());
        private static readonly ISet<Order> Orders = new HashSet<Order>(new OrderComparer());
        private static readonly ISet<Part> Parts = new HashSet<Part>(new PartComparer());
        private static readonly ISet<Shop> Shops = new HashSet<Shop>(new ShopComparer());
        private static readonly ISet<OrderPart> OrderParts = new HashSet<OrderPart>();
        private static readonly ISet<PartShop> PartShops = new HashSet<PartShop>();


        
        public static IDictionary<Type, IEnumerable<IEntity>> ConvertToEntities(this IEnumerable<FlatTableRow> flatTableRows)
        {
            foreach (var row in flatTableRows)
            {
                var client = new Client {Name = row.ClientName, Email = row.ClientEmail, PhoneNumber = row.ClientPhoneNumber};
                var order = new Order {Number = row.OrderNumber, Date = row.OrderDate};
                var part = new Part {Name = row.PartName, Price = row.PartPrice, Presence = row.PartPresence};
                var shop = new Shop {Address = row.ShopAddress, City = row.ShopCity};
                var orderPart = new OrderPart {Order = order, Part = part};
                var partShop = new PartShop {Part = part, Shop = shop};
                
                Clients.Add(client);
                Orders.Add(order);
                Parts.Add(part);
                Shops.Add(shop);
                
                client = Clients.Single(x => x.Equals(client));
                order = Orders.Single(x => x.Equals(order));
                part = Parts.Single(x => x.Equals(part));
                shop = Shops.Single(x => x.Equals(shop));

                if (!client.Orders.Contains(order, new OrderComparer()))
                {
                    client.Orders.Add(order);
                    order.Client = client;
                }

                if (!order.OrderParts.Contains(orderPart, new OrderPartComparer()))
                {
                    OrderParts.Add(orderPart);
                    order.OrderParts.Add(orderPart);
                    part.OrderParts.Add(orderPart);
                    orderPart.Part = part;
                    orderPart.Order = order;
                }
                
                if (!shop.PartShops.Contains(partShop, new PartShopComparer()))
                {
                    PartShops.Add(partShop);
                    shop.PartShops.Add(partShop);
                    part.PartShops.Add(partShop);
                    partShop.Part = part;
                    partShop.Shop = shop;
                }
            }

            return new Dictionary<Type, IEnumerable<IEntity>>
            {
                [typeof(Client)] = Clients,
                [typeof(Order)] = Orders,
                [typeof(Part)] = Parts,
                [typeof(Shop)] = Shops,
                [typeof(OrderPart)] = OrderParts,
                [typeof(PartShop)] = PartShops
            };
        }
    }
}