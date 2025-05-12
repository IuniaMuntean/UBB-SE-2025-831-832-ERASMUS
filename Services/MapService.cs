using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Data;
using UBB_SE_2025_EUROTRUCKERS.Models;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
    public interface IMapService
    {
        public Task<List<City>> GetCitiesAsync();
        public Task<List<Road>> GetRoadsAsync();

        public Task<City> GetCityByIdAsync(int id);
        public Task<Road> GetRoadByIdAsync(int id);
    }

    public class MapService: IMapService
    {
        private readonly TransportDbContext _transportDbContext;
        public Graph? Graph;

        public MapService(TransportDbContext transportDbContext)
        {
            _transportDbContext = transportDbContext;
            Graph = new Graph(transportDbContext);
        }

        public async Task<List<City>> GetCitiesAsync()
        {
            return await _transportDbContext.cities
                //.Include(c => c.City)
                .ToListAsync();
        }

        public async Task<City> GetCityByIdAsync(int id)
        {
            return await _transportDbContext.cities
                //.Include(c => c.City)
                .FirstOrDefaultAsync(c => c.id == id);
        }

        public async Task<Road> GetRoadByIdAsync(int id)
        {
            return await _transportDbContext.roads
                //.Include(r => r.Road)
                .FirstOrDefaultAsync(r => r.id == id);
        }

        public async Task<List<Road>> GetRoadsAsync()
        {
            return await _transportDbContext.roads
                //.Include(c => c.Road)
                .ToListAsync();
        }

    }

    public class Graph
    {

        protected Dictionary<int, City> cities;
        protected Dictionary<City, int> ids;

        protected Dictionary<int, HashSet<int>> outbound;
        protected Dictionary<int, HashSet<int>> inbound;

        protected HashSet<Road> edges;

        private readonly TransportDbContext _transportDbContext;

        public Graph(TransportDbContext tr)
        {
            _transportDbContext = tr;

            outbound = new Dictionary<int, HashSet<int>>();
            inbound = new Dictionary<int, HashSet<int>>();

            cities = _transportDbContext.cities.ToDictionary(c => c.id, c => c);
            foreach (var city in cities.Values)
            {
                outbound.Add(city.id, new HashSet<int>());
                inbound.Add(city.id, new HashSet<int>());
            }
            ids = _transportDbContext.cities.ToDictionary(c => c, c => c.id);

            edges = _transportDbContext.roads.ToHashSet<Road>();
            foreach (Road road in edges) { 
                outbound[road.startCityID].Add(road.endCityID);
                inbound[road.endCityID].Add(road.startCityID);
            }
        }


        private Dictionary<int, int> bfs(int start)
        {
            Dictionary<int, int> parents = new Dictionary<int, int>();
            Queue<int> q = new Queue<int>();
            HashSet<int> visited = new HashSet<int>();

            q.Enqueue(start);
            visited.Add(start);
            parents.Add(start, 0);

            while (q.Count() != 0)
            {
                int city = q.Dequeue();
                foreach (int neighbour in outbound[city])
                    if (!visited.Contains(neighbour))
                    {
                        q.Enqueue(neighbour);
                        visited.Add(neighbour);
                        parents.Add(neighbour, city);
                    }
            }

            return parents;
        }


        public List<int> path(int start, int end)
        {
            List<int> path = new List<int>();
            Dictionary<int, int> parents = bfs(start);

            int city = end;
            while (city != 0)
            {
                path.Add(cities[city].id);
                city = parents[city];
            }

            path.Reverse();
            if (path[0] != start)
                return new List<int>();
            return path;
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
    }
}

