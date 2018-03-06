using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TrainProblem.Interfaces;
using TrainProblem.Models;

namespace TrainProblem.Implementations
{
    public class TrainRouteLogic : ITrainRouteLogic
    {
        public Dictionary<Town, Route> ConstructRouteNodes(string routes)
        {
            Dictionary<Town, Route> routesModel = new Dictionary<Town, Route>();
            string[] routeInfos = routes.Split(',');
            foreach (var routeInfo in routeInfos)
            {
                var currentRouteInfo = routeInfo.Trim();
                if (currentRouteInfo.Length == 3)
                {
                    var originStation = new Town(currentRouteInfo[0].ToString());
                    var destinationStation = new Town(currentRouteInfo[1].ToString());
                    var stationPath = new Route(originStation, destinationStation, int.Parse(currentRouteInfo[2].ToString()));
                    var node = routesModel.Where(k =>AreThesameTown( k.Key,originStation));
                    var nodeWithRoute =
                        routesModel.Where(k => AreThesameTown(k.Key,originStation) &&
                        AreThesameTown(k.Value.OriginTown,stationPath.OriginTown) &&
                        AreThesameTown(k.Value.DestinationTown, stationPath.DestinationTown));
                    var keyValuePairs = node as IList<KeyValuePair<Town, Route>> ?? node.ToList();
                    if (!keyValuePairs.Any())
                    {
                        routesModel.Add(originStation, stationPath);
                    }
                    else
                    {
                        if (nodeWithRoute.Any())
                        {
                            throw new DuplicateNameException(
                                $"the Path {originStation.TownName}-{destinationStation.TownName} already exist for the station {originStation.TownName}");
                        }
                        else
                        {
                            var routeForNode = keyValuePairs.FirstOrDefault().Value;
                            while (routeForNode.NextRoute != null)
                            {
                                routeForNode = routeForNode.NextRoute;
                                
                            }
                            routeForNode.Next(stationPath);
                        }
                    }
                }
                else
                {
                    throw new ArgumentException($"The station information: {routeInfo} is incorrect");
                }
            }
            return routesModel;
        }

        public int ComputesPathDistance(Dictionary<Town, Route> routesModel, string paths)
        {
            var townsList = new List<Town>();

            var pathsInfo = paths.Trim().Split('-');
            foreach (var path in pathsInfo)
            {
                var newTown = new Town(path);
                townsList.Add(newTown);
            }
            if (townsList.Count < 2)
            {
                return 0;
            }
            var k = 0;
            var finalValue = 0;
            var depth = 0;
            while (k < townsList.Count - 1)
            {
                Town currentTown = townsList[k];
                Town nextTown = townsList[k + 1];
                if (ContainsTown(routesModel,currentTown))
                {
                    var route =  routesModel.FirstOrDefault(p => AreThesameTown(p.Key, currentTown)).Value;
                    while (route != null)
                    {
                        if (AreThesameTown(route.DestinationTown,nextTown))
                        {
                            finalValue += route.Distance;
                            depth++;
                            break;
                        }
                        route = route.NextRoute;
                    }
                }
                k++;
            }

            if (depth != townsList.Count - 1)
            {
                
                return -1; //NO SUCH ROUTE";
            }
            return finalValue;
        }

        public int ComputeNumberOfTripsBetweenStations(Dictionary<Town, Route> routesModel, Town startTown, Town endTown, int maxStops, bool isExact)
        {
            if (isExact)
            {
                
               return NumTrips(routesModel, startTown, endTown, 0, maxStops,true);
            }
               
            return  NumTrips(routesModel, startTown, endTown,0, maxStops,false);
        }


        private int NumTrips(Dictionary<Town, Route> routesModel, Town startTown,
            Town endTown,int level, int maxStops,bool isExact)
        {
            var routes = 0;
            if (ContainsTown(routesModel, startTown) && ContainsTown(routesModel, endTown))
            {
                level++;
                if (isExact)
                {
                    if (level >= maxStops-1 )
                        return 0;
                }
                else
                {
                    if (level > maxStops)
                        return 0;
                }
                
                startTown.TownVisited = true;
                var currentRoute = routesModel.FirstOrDefault(k => AreThesameTown(k.Key, startTown)).Value;
                while (currentRoute != null)
                {
                    if (AreThesameTown(currentRoute.DestinationTown,endTown))
                    {
                        routes++;
                        currentRoute = currentRoute.NextRoute;
                        continue;
                    }
                    if (!currentRoute.DestinationTown.TownVisited)
                    {
                        routes += NumTrips(routesModel, currentRoute.DestinationTown, endTown,level, maxStops,isExact);
                        level--;
                    }
                    currentRoute = currentRoute.NextRoute;
                }
            }
            else
            {
                return -1; //NO SUCH ROUTE";
            }
            startTown.TownVisited = false;
            return routes;
        }

        public int ComputeLengthOfShortestRoute(Dictionary<Town, Route> routesModel, Town startTown, Town endTown)
        {
            return ShortestRoute(routesModel, startTown, endTown,0,0);
        }

        public int ComputeNumberOfRoutesWithMaxDistancesBewteen(Dictionary<Town, Route> routesModel, Town startTown, Town endTown, int maxDistance)
        {
            return NumberOfRoutesWithinDistance(routesModel, startTown, endTown, 0, maxDistance);
        }

       


        private int NumberOfRoutesWithinDistance(Dictionary<Town, Route> routesModel, Town startTown, Town endTown, int currentDistance, int maxDistance)
        {
            int routes = 0;
            if (ContainsTown(routesModel, startTown) && ContainsTown(routesModel, endTown))
            {
                var currentRoute = routesModel.FirstOrDefault(k => AreThesameTown(k.Key, startTown)).Value;
                while (currentRoute != null)
                {
                    currentDistance += currentRoute.Distance;


                    if (currentDistance <= maxDistance)
                    {
                        if (AreThesameTown(currentRoute.DestinationTown,endTown))
                        {
                            routes++;
                            routes += NumberOfRoutesWithinDistance(routesModel, currentRoute.DestinationTown, endTown,
                                currentDistance, maxDistance);
                            currentRoute = currentRoute.NextRoute;
                            continue;
                        }
                        else
                        {
                            routes += NumberOfRoutesWithinDistance(routesModel, currentRoute.DestinationTown, endTown,
                                currentDistance, maxDistance);
                            currentDistance -= currentRoute.Distance;
                        }
                    }
                    else
                    {
                        currentDistance -= currentRoute.Distance;
                    }
                    currentRoute = currentRoute.NextRoute;
                }
            }
            else
                return -1; //NO SUCH ROUTE";
            return routes;
        }

        private int ShortestRoute(Dictionary<Town, Route> routesModel, Town startTown, Town endTown,int currentDistance,int shortestRoute)
        {

            if (ContainsTown(routesModel, startTown) && ContainsTown(routesModel,endTown))
            {
                startTown.TownVisited = true;
                var currentRoute = routesModel.FirstOrDefault(k => AreThesameTown(k.Key,startTown)).Value;
                while (currentRoute != null)
                {
                    //if destination is same as end town or has not been visited, add the distance to destination 
                    if (AreThesameTown(currentRoute.DestinationTown, endTown) || !currentRoute.DestinationTown.TownVisited)
                    {
                        currentDistance += currentRoute.Distance;
                    }
                    
                    //if destination is same as end we compare currentDistance to shortest route and return the lower of the two
                    if (AreThesameTown(currentRoute.DestinationTown,endTown))
                    {
                        if (currentDistance < shortestRoute || shortestRoute == 0)
                        {
                            shortestRoute = currentDistance;
                        }
                        startTown.TownVisited = false; //enable new Transversal or start route
                        return shortestRoute;
                    }
                    
                    //transverse destination Town if it is not the same as end and has not been visited
                    else if (!currentRoute.DestinationTown.TownVisited)
                    {
                        shortestRoute = ShortestRoute(routesModel, currentRoute.DestinationTown, endTown,
                            currentDistance, shortestRoute);
                        currentDistance -= currentRoute.Distance;
                    }
                    currentRoute = currentRoute.NextRoute;
                }
            }
            else
                return -1; //NO SUCH ROUTE";

            startTown.TownVisited = false;
            return shortestRoute;
        }

        private static bool ContainsTown(Dictionary<Town, Route> routesModel, Town startTown)
        {
            return routesModel.Any(p => p.Key.TownName == startTown.TownName);
        }

        private static bool AreThesameTown(Town currentTown, Town endTown)
        {
            return currentTown.TownName == endTown.TownName;
        }
    }
}