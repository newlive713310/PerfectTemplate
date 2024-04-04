using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfectTemplate.Application.Models.Entities;

namespace PerfectTemplate.WebApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly Route[] _routes;
        private readonly Dictionary<string, Route> _routeDictionary;
        public RoutesController() 
        {
            _routes = new Route[]
                {
                new Route { From = "London", To = "Dubai" },
                new Route { From = "Moscow", To = "Almaty" },
                new Route { From = "Almaty", To = "London" },
                new Route { From = "Paris", To = "Rio" },
                new Route { From = "Dubai", To = "Paris" }
                };

            // Создаем словарь для быстрого доступа к маршрутам по начальной точке
            _routeDictionary = _routes.ToDictionary(route => route.From);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Находим начальную точку маршрута
            string startingPoint = FindStartingPoint(_routes);

            var sortedRoutes = new List<Route>();

            if (startingPoint != null)
            {
                // Сортируем маршруты в правильной последовательности
                sortedRoutes = SortRoutes(_routes, startingPoint);

                // Выводим отсортированные маршруты
                foreach (var route in sortedRoutes)
                {
                    Console.WriteLine($"{route.From} - {route.To}");
                }
            }
            else
            {
                Console.WriteLine("Не удалось определить начальную точку маршрута.");
            }

            return Ok(sortedRoutes);
        }
        private string FindStartingPoint(Route[] routes)
        {
            // Находим все конечные точки маршрутов
            var destinations = routes.Select(route => route.To).Distinct().ToHashSet();

            // Находим начальную точку, которая не является конечной
            foreach (var route in routes)
            {
                if (!destinations.Contains(route.From))
                {
                    return route.From;
                }
            }

            return null;
        }
        private List<Route> SortRoutes(Route[] routes, string startingPoint)
        {
            List<Route> sortedRoutes = new List<Route>();
            string currentPoint = startingPoint;

            while (true)
            {
                // Находим следующий маршрут из текущей точки
                if (_routeDictionary.TryGetValue(currentPoint, out Route nextRoute))
                {
                    // Добавляем маршрут в отсортированный список и обновляем текущую точку
                    sortedRoutes.Add(nextRoute);
                    currentPoint = nextRoute.To;
                }
                else
                {
                    // Если не найден следующий маршрут, выходим из цикла
                    break;
                }
            }

            return sortedRoutes;
        }
    }
    class Route
    {
        public string From { get; set; }
        public string To { get; set; }
    }
}
