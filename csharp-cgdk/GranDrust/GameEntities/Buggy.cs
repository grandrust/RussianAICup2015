﻿using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.FSM.States;

// ReSharper disable once CheckNamespace
namespace GranDrust.GameEntities
{
    public class Buggy: Vehicle
    {
        private static Map _map;
        private static Map GetMap(Buggy vehicle)
        {
            if (_map == null)
            {
                _map = new Map();
                _map.Build(vehicle);
            }

            return _map;
        }

        public Buggy(Car self, World world, Game game, Move move) 
            : base(self, world, game, move)
        {
            Map = GetMap(this);
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
