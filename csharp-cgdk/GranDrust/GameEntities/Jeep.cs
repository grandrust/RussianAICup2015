using System;
using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.FSM.States;
using GranDrust.Helpers;

// ReSharper disable once CheckNamespace
namespace GranDrust.GameEntities
{
    public class Jeep: Vehicle
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

                var angel = Self.GetAngleTo(car.NextPoint());

                Move.IsThrowProjectile = Math.Abs(angel) < 0.08D;
            }
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
