using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrainProblem.Implementations;
using TrainProblem.Interfaces;
using TrainProblem.Models;
using NSubstitute;

namespace TrainTest
{
    [TestClass]
    public class UnitTest1
    {
        private static Dictionary<Town, Route> routesModel;
        private static Town _townA;
        private static Town _townB;
        private static Town _townC;
        private static Town _townD;
        private static Town _townE;
        private static RouteProcessing routeProcessor;
        private ITrainRouteLogic trainLogic;

        [TestInitialize]
        public  void InitTest()
        { 
            //init routeModel for test with nodes AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7
            routesModel = new Dictionary<Town, Route>();
            _townA=new Town("A");
            _townB=new Town("B");
            _townC=new Town("C");
            _townD=new Town("D");
            _townE=new Town("E");
            
            var aRoute=new Route(_townA,_townB,5);
            var a2Route = new Route(_townA, _townD, 5);
            var a3Route=new Route(_townA,_townE,7);
            a2Route.Next(a3Route);
            aRoute.Next(a2Route);
            
            routesModel.Add(_townA,aRoute);
            var bRoute=new Route(_townB,_townC,4);
            routesModel.Add(_townB,bRoute);
            
            var cRoute=new Route(_townC,_townD,8);
            var c2Route=new Route(_townC,_townE,2);
            cRoute.Next(c2Route);
            routesModel.Add(_townC,cRoute);
            
            var dRoute=new Route(_townD,_townC,8);
            var d2Route=new Route(_townD,_townE,6);
            dRoute.Next(d2Route);
            routesModel.Add(_townD,dRoute);
            
            var eRoute=new Route(_townE,_townB,3);
            routesModel.Add(_townE,eRoute);
            trainLogic = new TrainRouteLogic();
            routeProcessor = new RouteProcessing(routesModel,trainLogic);
        }
        [TestMethod]
        public void Test_Distance_Of_Route_ABC()
        {
            var path = "A-B-C";
            var returnedValue = routeProcessor.ComputesPathDistance(routesModel, path);
            Assert.AreEqual(9,returnedValue);
        }

        [TestMethod]
        public void Test_Distance_Of_Route_AD()
        {
            var path = "A-D";
            Assert.AreEqual(5,routeProcessor.ComputesPathDistance(routesModel,path));
        }
        [TestMethod]
        public void Test_Distance_Of_Route_ADC()
        {
            var path = "A-D-C";
            Assert.AreEqual(13,routeProcessor.ComputesPathDistance(routesModel,path));
        }
        [TestMethod]
        public void Test_Distance_Of_Route_AEBCD()
        {
            var path = "A-E-B-C-D";
            Assert.AreEqual(22,routeProcessor.ComputesPathDistance(routesModel,path));
        }
        [TestMethod]
        public void Test_Distance_Of_Route_AED()
        {
            var path = "A-E-D";
           Assert.AreEqual(-1,routeProcessor.ComputesPathDistance(routesModel,path));
        }

        [TestMethod]
        public void Number_Of_Trips_Between_Routes_With_MaxStop()
        {
            Assert.AreEqual(2, routeProcessor.ComputeNumberOfTripsBetweenStations(routesModel, _townC, _townC, 3,false));
        }
        [TestMethod]
        public void Number_Of_Trips_Between_Routes_With_Exact_Stops()
        {
            Assert.AreEqual(3, routeProcessor.ComputeNumberOfTripsBetweenStations(routesModel, _townA, _townC, 4,true));
        }

        [TestMethod]
        public void Length_Of_Shortest_route_from_A_to_C()
        {
            Assert.AreEqual(9,routeProcessor.ComputeLengthOfShortestRoute(routesModel,_townA,_townC));
        }
        [TestMethod]
        public void Length_Of_Shortest_route_from_B_to_B()
        {
            Assert.AreEqual(9,routeProcessor.ComputeLengthOfShortestRoute(routesModel,_townB,_townB));
        }

        [TestMethod]
        public void Number_Of_Different_Routes_From_C_To_C_With_Distance_Less_Than_30()
        {
            Assert.AreEqual(7,routeProcessor.ComputeNumberOfRoutesWithMaxDistancesBewteen(routesModel,_townC,_townC,30));
        }
    }
}