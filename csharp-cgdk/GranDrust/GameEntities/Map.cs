namespace GranDrust.GameEntities
{
    public class WayPointNode
    {
        private readonly int _x;
        private readonly int _y;

        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        public bool IsMe(int x, int y)
        {
            return _x == x && _y == y;
        }

        public WayPointNode(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public WayPointNode Next { get; private set; }

        public WayPointNode SetNext(WayPointNode wayPointNode)
        {
            return Next = wayPointNode;
        }
    }

    public class Map
    {
        private int[][] _waypoints;
        private WayPointNode _start;
        private WayPointNode _last;

            private void Add(int x, int y)
            {
                var wayPointNode = new WayPointNode(x, y);

                if (_start == null)
                {
                    _last = _start = wayPointNode;
                }
                else
                {
                    _last = _last.SetNext(wayPointNode);
                }
            }

            public WayPointNode GetNextPoint(int x, int y)
            {
                var current = _start;
                while (!current.IsMe(x, y))
                {
                    current = current.Next;
                }

                return current;
            }

            public WayPointNode GetNextPoint(int index)
            {
                index = index % _waypoints.Length;
                var point = _waypoints[index];

                return GetNextPoint(point[0], point[1]);
            }
        
        public void Build(Vehicle vehicle)
        {
            _waypoints = vehicle.World.Waypoints;
            foreach (var waypoint in _waypoints)
            {
                Add(waypoint[0], waypoint[1]);
            }
        }
    }
}
