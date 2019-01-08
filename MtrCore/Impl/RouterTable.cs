using System.Collections.Generic;

namespace MatterCore.Impl
{
    public class RouterTable : IRouterTable
    {
        public Dictionary<int, IRoute> Table { get; set; } = new Dictionary<int, IRoute>();

        public RouterTable() => Table = new Dictionary<int, IRoute>();
        public RouterTable(Dictionary<int, IRoute> table) => Table = table;

        public IRoute GetRoute(int label)
        {
            Table.TryGetValue(label, out var res);
            return res;
        }

        public void AddRoute(int label, IRoute record) => 
            Table.Add(label, record);
    }
}
