using System;
using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.GameEntities;
using GranDrust.Helpers;

namespace GranDrust.FSM.States
{
    public class Arrive: TargetState
    {
        private const double Deceleration = 0.03D;

        private Arrive()
        {
        }

        private static Arrive _instantce;
        public static Arrive Instance
        {
            get { return _instantce ?? (_instantce = new Arrive()); }
        }

        public override void Execute(Vehicle vehicle)
        {
            var distance = vehicle.Self.GetDistanceTo(TargetPoint);

            var speedModule = vehicle.Self.SpeedModule();

            var speed = distance / (Deceleration * (vehicle.Self.AngularSpeed + vehicle.Game.TrackTileSize));

            var angleToWaypoint = AngleToNextPoint(TargetPoint, vehicle);
            
            vehicle.Move.WheelTurn = angleToWaypoint * 32.0D / Math.PI;
            vehicle.Move.EnginePower = speed / Math.Abs(Math.Cos(angleToWaypoint)) * speedModule;

            vehicle.Move.EnginePower = Math.Abs(speedModule) < 0.1 ? 1.0D : speed / speedModule;


            if (speedModule * speedModule * Math.Abs(angleToWaypoint) /** vehicle.Self.AngularSpeed */> 42.5D * Math.PI)
            {
                vehicle.Move.IsBrake = true;
            }


            if (Math.Abs(Math.Cos(angleToWaypoint)) * speedModule > speed)  //TODO: consistent brake
            {
                vehicle.Move.IsBrake = true;
            }


            //STARIKE ACTION
            vehicle.Move.IsSpillOil = vehicle.CurrentTile() != TileType.Horizontal &&
                                      vehicle.CurrentTile() != TileType.Vertical;

            vehicle.Move.IsUseNitro = vehicle.CanUseNitro
                                        && vehicle.Game.InitialFreezeDurationTicks < vehicle.World.Tick - 100
                                        && Math.Abs(vehicle.Self.GetAngleTo(TargetPoint)) < 0.9D;

            vehicle.Strike();
        }

        private double AngleToNextPoint(Point target, Vehicle vehicle) //TODO: Find another way
        {
            var nextPoint = vehicle.Self.NextPoint();
            double absoluteAngleTo = Math.Atan2(target.Y - nextPoint.Y, target.X - nextPoint.X);
            double relativeAngleTo = absoluteAngleTo - vehicle.Self.Angle;

            while (relativeAngleTo > Math.PI)
            {
                relativeAngleTo -= 2.0D * Math.PI;
            }

            while (relativeAngleTo < -Math.PI)
            {
                relativeAngleTo += 2.0D * Math.PI;
            }

            return relativeAngleTo;
        }
    }
}
