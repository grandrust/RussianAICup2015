using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.Helpers;

// ReSharper disable once CheckNamespace
namespace GranDrust.GameEntities
{
    public class Map
    {
        private class WayPointNode
        {
            private int _x;
            private int _y;

            public bool IsMe(int x, int y)
            {
                return _x == x && _y == y;
            }

            public WayPointNode(int x, int y, TileType type)
            {
                _x = x;
                _y = y;
                Type = type;
            }

            public Point Point { get; private set; }
            public WayPointNode Next { get; private set; }
            public TileType Type { get; private set; }

            public WayPointNode SetNext(WayPointNode wayPointNode, Vehicle vehicle) //TODO: create clever map
            {
                SetLastValue(vehicle, wayPointNode);
                return Next = wayPointNode;
            }

            public void SetLastValue(Vehicle vehicle, WayPointNode wayPointNode)
            {
                double nextWaypointX = (_x + 0.5D)*vehicle.Game.TrackTileSize;
                double nextWaypointY = (_y + 0.5D)*vehicle.Game.TrackTileSize;

                var cornerTileOffset = 0.3D*vehicle.Game.TrackTileSize;

                switch (Type)
                {
                    case TileType.LeftTopCorner:
                        nextWaypointX += cornerTileOffset;
                        nextWaypointY += cornerTileOffset;
                        break;
                    case TileType.RightTopCorner:
                        nextWaypointX -= cornerTileOffset;
                        nextWaypointY += cornerTileOffset;
                        break;
                    case TileType.RightBottomCorner:
                        nextWaypointX -= cornerTileOffset;
                        nextWaypointY -= cornerTileOffset;
                        break;
                    case TileType.LeftBottomCorner:
                        nextWaypointX += cornerTileOffset;
                        nextWaypointY -= cornerTileOffset;
                        break;
                }

                Point = new Point(nextWaypointX, nextWaypointY);
            }
        }

        private class MapStructure
        {
            private readonly Vehicle _vehicle;
            private WayPointNode _start;
            private WayPointNode _last;

            public MapStructure(Vehicle vehicle)
            {
                _vehicle = vehicle;
                foreach (var waypoint in vehicle.World.Waypoints)
                {
                    Add(waypoint[0], waypoint[1]);
                }

                _last.SetLastValue(_vehicle, _start);
                
            }

            private void Add(int x, int y)
            {
                var wayPointNode = new WayPointNode(x, y, _vehicle.World.TilesXY[x][y]);

                if (_start == null)
                {
                    _last = _start = wayPointNode;
                }
                else
                {
                    _last = _last.SetNext(wayPointNode, _vehicle);
                }
            }

            public Point GetNextPoint(int x, int y)
            {
                var current = _start;
                while (!current.IsMe(x, y))
                {
                    current = current.Next;
                }

                return current.Point;
            }

            public Point GetNextPoint(int index)
            {
                index = index % _vehicle.World.Waypoints.Length;
                var point = _vehicle.World.Waypoints[index];

                return GetNextPoint(point[0], point[1]);
            }
        }
        
        public void Build(Vehicle vehicle)
        {
            Structure = new MapStructure(vehicle);
        }

        private MapStructure Structure { get; set; }

        public Point GetNextPoint(int x, int y)
        {
            return Structure.GetNextPoint(x, y);
        }

        public Point GetNextPoint(int index)
        {
            return Structure.GetNextPoint(index);
        }
    }
}
