using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Background;

namespace UBB_SE_2025_EUROTRUCKERS.Models
{
    internal class GraphOld
    {
        protected Dictionary<int, City> cities;
        protected Dictionary<City, int> ids;

        protected Dictionary<int, HashSet<int>> outbound;
        protected Dictionary<int, HashSet<int>> inbound;

        protected HashSet<Road> edges;

        public GraphOld() 
        {
            cities = new Dictionary<int, City>();
            ids = new Dictionary<City, int>();
            outbound = new Dictionary<int, HashSet<int>>();
            inbound = new Dictionary<int, HashSet<int>>();
            edges = new HashSet<Road>();
        }

        public bool add(City city)
        {
            int count = cities.Count;
            cities.Add(city.id, city);
            if (cities.Count == count) 
                return false;
            ids.Add(city, city.id);
            outbound.Add(city.id, new HashSet<int>());
            inbound.Add(city.id, new HashSet<int>());
            return true;
        }
        public bool add(Road road)
        {
            bool added = outbound[road.start].Add(road.end);
            if (added == false)
                return false;
            inbound[road.end].Add(road.start);
            edges.Add(road);
            return true;
        }

        public List<City> Cities()
        {
            return new List<City>(cities.Values);
        }
        public City City(int id)
        {
            return new City(cities[id]);
        }
        public List<Road> Roads()
        {
            return new List<Road>(edges.ToList());
        }

        public Dictionary<int, HashSet<int>> Outbound()
        {
            return outbound;
        }

        public Dictionary<int, HashSet<int>> Inbound()
        {
            return inbound;
        }

        public HashSet<Road> Edges()
        {
            return edges;
        }

        //private Dictionary<int, int> bfs(int start)
        //{
        //    Dictionary<int, int> parents = new Dictionary<int, int>();
        //    Queue<int> q = new Queue<int>();
        //    HashSet<int> visited = new HashSet<int>();

        //    q.Enqueue(start);
        //    visited.Add(start);
        //    parents.Add(start, 0);

        //    while (q.Count() != 0)
        //    {
        //        int city = q.Dequeue();
        //        foreach (int neighbour in outbound[city])
        //            if (!visited.Contains(neighbour))
        //            {
        //                q.Enqueue(neighbour);
        //                visited.Add(neighbour);
        //                parents.Add(neighbour, city);
        //            }
        //    }

        //    return parents;
        //}


        //public List<int> path(int start, int end)
        //{
        //    List<int> path = new List<int>();
        //    Dictionary<int, int> parents = bfs(start);

        //    int city = end;
        //    while (city != 0)
        //    {
        //        path.Add(cities[city].id);
        //        city = parents[city];
        //    }

        //    path.Reverse();
        //    if (path[0] != start)
        //        return new List<int>();
        //    return path;
        //}
    }
}
