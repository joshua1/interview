using System.Collections.Generic;
using TrainProblem.Models;

namespace TrainProblem.Interfaces
{
    public interface ITrainRouteLogic
    {
        Dictionary<Town, Route> ConstructRouteNodes(string routes);
        int ComputesPathDistance(Dictionary<Town, Route> routesModel, string paths);
        int ComputeNumberOfTripsBetweenStations(Dictionary<Town, Route> routesModel, Town startStation, Town endStation, int maxStops, bool isExact);
        int ComputeLengthOfShortestRoute(Dictionary<Town, Route> routesModel, Town startStation, Town endStation);
        int ComputeNumberOfRoutesWithMaxDistancesBewteen(Dictionary<Town, Route> routesModel, Town startTown,
            Town endTown, int maxDistance);
    }
}