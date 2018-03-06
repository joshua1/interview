using System;
using System.Collections.Generic;

namespace TrainProblem.Models
{
    //node
    public class Town 
    {
        public Town(string name)
        {
            TownName = name;
            TownVisited = false;
        }
        public string TownName { get; }
        public bool TownVisited { get; set; }

        protected bool Equals(Town toCompare)
        {
            return toCompare != null && this.Equals(toCompare) && string.Equals(TownName, toCompare.TownName);
        }


        public override int GetHashCode()
        {
            return (TownName != null ? TownName.GetHashCode() : 0);
        }
    }
}