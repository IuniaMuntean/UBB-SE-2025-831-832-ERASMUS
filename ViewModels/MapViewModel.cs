using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;

namespace UBB_SE_2025_EUROTRUCKERS.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly IMapService _mapService;
        private readonly INavigationService _navigationService;

        
        public ObservableCollection<(float X, float Y, string Name)> CityCoordinates { get; } = new();

        public ObservableCollection<((float x, float y) start, (float x, float y) end)> RoadCoordinates { get; } = new();  

        public IAsyncRelayCommand LoadMapCommand { get; }
        public IAsyncRelayCommand GetCitiesCommand { get; }

        public MapViewModel(
            IMapService mapService,
            INavigationService navigationService)
        {
            _mapService = mapService;
            _navigationService = navigationService;
            Title = "Map";

            _ = LoadCitiesAsync();
            _ = LoadRoadsAsync();

            //LoadMapCommand = new AsyncRelayCommand(LoadMapAsync);
        }

        public async Task LoadCitiesAsync()
        {
            List<City> cities = await _mapService.GetCitiesAsync();

            List<(float x, float y, string name)> coordinates = cities
                .Select(city => (city.x, city.y, city.name))
                .ToList();

            CityCoordinates.Clear();
            foreach (var coord in coordinates)
                CityCoordinates.Add(coord);
        }

        public async Task LoadRoadsAsync()
        {
            List<Road> roads = await _mapService.GetRoadsAsync();

            var lines = await GetRoadLinesSequentiallyAsync(roads);

            RoadCoordinates.Clear();
            foreach (var coord in lines)
                RoadCoordinates.Add(coord);
        }

        private async Task<List<((float x, float y) start, (float x, float y) end)>> GetRoadLinesSequentiallyAsync(List<Road> roads)
        {
            var lines = new List<((float, float) start, (float, float) end)>();

            foreach (var road in roads)
            {
                // Await each call sequentially.
                var startCity = await _mapService.GetCityByIdAsync(road.startCityID);
                var endCity = await _mapService.GetCityByIdAsync(road.endCityID);
                lines.Add(((startCity.x, startCity.y), (endCity.x, endCity.y)));
            }

            return lines;
        }


        private async Task LoadMapAsync()
        {
            //setSize();

            Canvas canvas = new Canvas
            {
                Width = 600,
                Height = 600,
                Background = new SolidColorBrush(Microsoft.UI.Colors.LightGray)
            };

            

            //DrawCircles(canvas, coordinates);

            //var path = Fabi__Path_Finding.Path(graphService.Graph, startCityID, endCityID);
            
            

            //DrawLines(canvas, new SolidColorBrush(Microsoft.UI.Colors.Black), lines);
            
            //DrawLine(canvas, new SolidColorBrush(Microsoft.UI.Colors.Yellow), path.Select(id => (graphService.Graph.City(id).x, graphService.Graph.City(id).y)).ToList());
        }

        }
}
