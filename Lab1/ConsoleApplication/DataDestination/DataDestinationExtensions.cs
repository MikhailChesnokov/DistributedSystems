namespace ConsoleApplication.DataDestination
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using MySql;
    using SqlServer;



    internal static class DataDestinationExtensions
    {
        internal static IDictionary<Type, IEnumerable<IEntity>> SaveToMySql(this IDictionary<Type, IEnumerable<IEntity>> entities)
        {
            using (var context = new MySqlContext())
            {
                DropIds(entities);
                
                SaveAll(entities, context);
            }

            return entities;
        }

        internal static IDictionary<Type, IEnumerable<IEntity>> SaveToSqlServer(this IDictionary<Type, IEnumerable<IEntity>> entities)
        {
            using (var context = new SqlServerContext())
            {
                DropIds(entities);
                
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