namespace Consumer.DataDestination
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MySql;
    using Domain;
    
    
    public static class DataDestinationExtensions
    {
        public static IDictionary<Type, IEnumerable<IEntity>> SaveToMySql(this IDictionary<Type, IEnumerable<IEntity>> entities)
        {
            using (var context = new MySqlContext())
            {
                //DropIds(entities);
                
                SaveAll(entities, context);
            }

            return entities;
        }

        private static void SaveAll(IDictionary<Type, IEnumerable<IEntity>> entities, CommonContext context)
        {
            entities[typeof(OrderPart)].Cast<OrderPart>().ToList().ForEach(x => context.OrderPart.Add(x));
            
            context.SaveChanges();
        }

        private static void DropIds(IDictionary<Type, IEnumerable<IEntity>> entities)
        {
            entities.Values.ToList().ForEach(x => x.ToList().ForEach(y => y.Id = default));
        }
    }
}