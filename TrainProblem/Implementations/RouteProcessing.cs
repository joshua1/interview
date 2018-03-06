using System;
using System.Collections.Generic;
using TrainProblem.Interfaces;
using TrainProblem.Models;

namespace TrainProblem.Implementations
{
    public class RouteProcessing
    {
        private readonly ITrainRouteLogic _trainLogic;
        private Dictionary<Town, Route> _routesModel;

        public RouteProcessing(Dictionary<Town, Route> routeModel,ITrainRouteLogic trainLogic)
        {
            _trainLogic = trainLogic;
            _routesModel = routeModel;
        }

        public RouteProcessing()
        {
            if (_routesModel == null)
            {
                _routesModel = new Dictionary<Town, Route>();
            }
            if (_trainLogic == null)
            {
                _trainLogic=new TrainRouteLogic();
            }
        }

        public void ProcessRoutingDataFromTerminal()
        {
            var routes = GetRoutesData();
            _routesModel = _trainLogic.ConstructRouteNodes(routes);
            if (_routesModel != null)
            {
                Question1();
            }
            else
            {
                ConsoleColour(ConsoleColor.Red);
                Console.WriteLine("Error Creating Routes Graph Model");
            }
        }
        
        public int ComputeNumberOfTripsBetweenStations(Dictionary<Town, Route> routesModel, Town startTown,
            Town endTown, int maxStops, bool isExact)
        {
            return _trainLogic.ComputeNumberOfTripsBetweenStations(routesModel, startTown, endTown, maxStops, isExact);
        }
        
        public int ComputesPathDistance(Dictionary<Town, Route> routesModel, string paths)
        {
            return _trainLogic.ComputesPathDistance(routesModel, paths);
        }
        public int ComputeLengthOfShortestRoute(Dictionary<Town, Route> routesModel, Town startTown, Town endTown)
        {
            return _trainLogic.ComputeLengthOfShortestRoute(routesModel, startTown, endTown);
        }
        public int ComputeNumberOfRoutesWithMaxDistancesBewteen(Dictionary<Town, Route> routesModel, Town startTown, Town endTown, int maxDistance)
        {
            return _trainLogic.ComputeNumberOfRoutesWithMaxDistancesBewteen(routesModel, startTown, endTown, maxDistance);
        }

        private void Question10()
        {
            Console.WriteLine("Question 10");
            var startTown = new Town("C");
            var endTown = new Town("C");
            var maxDistance = 30;
            DifferentRoutesWithMaxDistance(startTown, endTown, maxDistance);
            ConsoleColour(ConsoleColor.White);
            Console.WriteLine("End of Solution");
            Console.ReadLine();
        }


        private void Question9()
        {
            Console.WriteLine("Question 9");
            var startTown = new Town("B");
            var endTown = new Town("B");
            LengthOfShortestRouteQuestion(startTown, endTown);
            Question10();
        }

        private void Question8()
        {
            Console.WriteLine("Question 8");
            var startTown = new Town("A");
            var endTown = new Town("C");
            LengthOfShortestRouteQuestion(startTown, endTown);
            Question9();
        }


        private void Question7()
        {
            Console.WriteLine("Question 7");
            var startTown = new Town("A");
            var endTown = new Town("C");
            var maxStops = 4;
            NumberOfTripsQuestion(startTown, endTown, maxStops, true);
            Question8();
        }

        private void Question6()
        {
            Console.WriteLine("Question 6");
            var startTown = new Town("C");
            var endTown = new Town("C");
            var maxStops = 3;
            NumberOfTripsQuestion(startTown, endTown, maxStops);
            Question7();
        }

        private void Question5()
        {
            Console.WriteLine("Question 5");
            string paths = "A-E-D";
            DistanceQuestion(paths);
            Question6();
        }

        private void Question4()
        {
            Console.WriteLine("Question 4");
            string paths = "A-E-B-C-D";
            DistanceQuestion(paths);
            Question5();
        }

        private void Question3()
        {
            Console.WriteLine("Question 3");
            string paths = "A-D-C";
            DistanceQuestion(paths);

            Question4();
        }

        private void Question2()
        {
            Console.WriteLine("Question 2");
            string paths = "A-D";
            DistanceQuestion(paths);

            Question3();
        }

        private void Question1()
        {
            Console.WriteLine("Question 1");
            string paths = "A-B-C";
            DistanceQuestion(paths);

            Question2();
        }

        private void DistanceQuestion(string paths)
        {
            ConsoleColour(ConsoleColor.White);
            Console.WriteLine($"Computing the distance of route {paths} ....");

            var routeDistance = ComputesPathDistance(_routesModel, paths);

            if (routeDistance > 0)
            {
                ConsoleColour(ConsoleColor.Yellow);
                Console.WriteLine($"The Distance of Route {paths} is {routeDistance}");
            }
            else
            {
                InvalidInput();
            }
        }

      


        private void NumberOfTripsQuestion(Town startTown, Town endTown, int maxStops, bool isExact = false)
        {
            ConsoleColour(ConsoleColor.White);
            var exactString = isExact != true ? "exactly":"";
            
            Console.WriteLine(
                $"Computing the number of trips starting at {startTown.TownName} and ending at {endTown.TownName} with {exactString} {maxStops} stops ...");

            var numberOfTrips =
                ComputeNumberOfTripsBetweenStations(_routesModel, startTown, endTown, maxStops, isExact);
            if (numberOfTrips > 0)
            {
                ConsoleColour(ConsoleColor.Yellow);
                if (isExact==true)
                    Console.WriteLine(
                        $"The Number of trips starting at {startTown.TownName} and ending at {endTown.TownName} with exactly {maxStops} Stops is {numberOfTrips}");
                else
                {
                    Console.WriteLine(
                        $"The Number of trips starting at {startTown.TownName} and ending at {endTown.TownName} with no more than {maxStops} Stops is {numberOfTrips}");
                }
            }
            else
            {
                InvalidInput();
            }
        }

      


        private void LengthOfShortestRouteQuestion(Town startTown, Town endTown)
        {
            ConsoleColour(ConsoleColor.White);
            Console.WriteLine(
                $"Computing the length of the shortes Route from {startTown.TownName} to {endTown.TownName} .....");
            var shortest =
                ComputeLengthOfShortestRoute(_routesModel, startTown, endTown);
            if (shortest > 0)
            {
                ConsoleColour(ConsoleColor.Yellow);
                Console.WriteLine(
                    $"The length of the shortes Route from {startTown.TownName} to {endTown.TownName} is {shortest}");
            }
            else
            {
                InvalidInput();
            }
        }

 

        private void DifferentRoutesWithMaxDistance(Town startTown, Town endTown, int maxDistance)
        {
            ConsoleColour(ConsoleColor.White);
            Console.WriteLine(
                $"Computing the number of different routes from {startTown.TownName} to {endTown.TownName} with a maximum distance of {maxDistance}");
            int numberOfRoutes =
                ComputeNumberOfRoutesWithMaxDistancesBewteen(_routesModel, startTown, endTown, maxDistance);
            if (numberOfRoutes > 0)
            {
                ConsoleColour(ConsoleColor.Yellow);
                Console.WriteLine(
                    $"The number of different routes from {startTown.TownName} to {endTown.TownName} with a maximum distance of {maxDistance} is {numberOfRoutes}");
            }
            else
            {
                InvalidInput();
            }
        }

       

        private string GetRoutesData()
        {
            ConsoleColour(ConsoleColor.Blue);
            Console.WriteLine(
                "Please enter a comma separated list of route nodes in the form start stop distance i.e AB7");
            var routes = Console.ReadLine();
            if (routes != null)
            {
                return routes.Trim();
            }
            return GetRoutesData();
        }

        private static void ConsoleColour(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        private static void InvalidInput()
        {
            ConsoleColour(ConsoleColor.Red);
            Console.WriteLine("NO SUCH ROUTE");
        }
    }
}