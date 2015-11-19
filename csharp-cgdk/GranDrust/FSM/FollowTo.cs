﻿using System;
using GranDrust.GameEntities;
using GranDrust.Helpers;

// ReSharper disable once CheckNamespace
namespace GranDrust.FSM
{
    public class FollowTo : IState
    {
        public Point TargetPoint { get; set; }

        public void Execute(Vehicle vehicle)
        {
            double angleToWaypoint = vehicle.Self.GetAngleTo(TargetPoint.X, TargetPoint.Y);
            double speedModule = vehicle.Self.SpeedModule();

            vehicle.Move.WheelTurn = angleToWaypoint*32.0D/Math.PI;
            vehicle.Move.EnginePower = 1.0D;

            if (speedModule*speedModule*Math.Abs(angleToWaypoint) > 30.0D*Math.PI)
            {
                vehicle.Move.IsBrake = true;
            }
        }
    }
}