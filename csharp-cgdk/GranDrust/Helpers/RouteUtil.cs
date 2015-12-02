using System.Collections.Generic;
using GranDrust.GameEntities;
using GranDrust.Search;

namespace GranDrust.Helpers
{
    public static class RouteUtil
    {
        public static IList<BFSearch.Cell> CreateRouteOf(this Vehicle vehicle, int pointCount, Point startPoint)
        {
            var startCell = vehicle.GetCurrentCell(startPoint);

            var terminateIndex = vehicle.Self.NextWaypointIndex;
            var terminateCell = vehicle.GetCellByIndex(terminateIndex);

            var route = (List<BFSearch.Cell>)new BFSearch(vehicle, startCell, terminateCell).Search();

            while (route.Count < pointCount)
            {
                startCell = terminateCell;
                terminateIndex++;

                terminateCell = vehicle.GetCellByIndex(terminateIndex);

                var nextRoute = new BFSearch(vehicle, startCell, terminateCell).Search();

                route.AddRange(nextRoute);
            }

            return route;
        }
    }
}
