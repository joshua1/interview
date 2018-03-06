namespace TrainProblem.Models
{
    public class Route 
    {
        public Route(Town origin,Town destination ,int distance)
        {
            OriginTown = origin;
            DestinationTown = destination;
            Distance = distance;
            NextRoute = null;
        }

        public Route Next(Route nextRoute)
        {
            NextRoute = nextRoute;
            return this;
        }

        
        public Town OriginTown { get; set; }
        public Town DestinationTown { get; set; }
        public int Distance { get; set; }
        
        public Route NextRoute { get; set; }
    }
}