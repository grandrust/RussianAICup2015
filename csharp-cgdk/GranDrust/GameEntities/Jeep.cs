using System;
using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.FSM.States;
using GranDrust.Helpers;
using GranDrust.Search;

// ReSharper disable once CheckNamespace
namespace GranDrust.GameEntities
{
    public class Jeep : Vehicle
    {
        private static Map _map;
        private static Map GetMap(Jeep vehicle)
        {
            if (_map == null)
            {
                _map = new Map();
                _map.Build(vehicle);
            }

            return _map;
        }

        public Jeep(Car self, World world, Game game, Move move)
            : base(self, world, game, move)
        {
            Map = GetMap(this);
        }

        public override void Strike()
        {
            foreach (var car in World.Cars)
            {
                if (car.IsTeammate) continue;

                var angle = Self.GetAngleTo(car.NextPoint());
                var distanceTo = Self.GetDistanceTo(car);

                if (Math.Abs(angle) < 0.08D && distanceTo / Game.TrackTileSize < 5.0D)
                    Move.IsThrowProjectile = ThereIsNoObstaclesForThrow(angle + Self.Angle, distanceTo, car.CurrentPoint());
            }
        }

        private bool ThereIsNoObstaclesForThrow(double angle, double distanceTo, Point carPoint)
        {
            bool result = true;
            var stepLength = Game.TrackTileMargin*1.5D;
            var steps = Convert.ToInt32(Math.Floor(distanceTo /stepLength));
            var startCell = this.GetCurrentCell(Self.CurrentPoint());

            var route = new BFSearch(this, startCell, this.GetCurrentCell(carPoint)).Search();
            var index = 0;

            for (var i = 0; i < steps; i ++)
            {
                var nx = Self.X + (i + 1) * stepLength * Math.Cos(angle);
                var ny = Self.Y + (i + 1) * stepLength * Math.Sin(angle);

                var cell = this.GetCurrentCell(new Point(nx, ny));

                var currentIndex = route.IndexOf(cell); 

                if (currentIndex == -1 && !startCell.Equals(cell))
                {
                    result = false;
                    break;
                }

                if (currentIndex - index > 1)
                {
                    result = false;
                    break;
                }

                index = currentIndex;
            }

            return result;
        }


        private static IState _previousState = InitialState.Instance;
        public override IState PreviousState
        {
            get { return _previousState; }
            protected set { _previousState = value; }
        }

        private static IState _currentState = InitialState.Instance;
        public override IState CurrentState
        {
            get { return _currentState; }
            protected set { _currentState = value; }
        }
    }
}
