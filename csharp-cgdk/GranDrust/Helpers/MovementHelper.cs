using System;
using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;

// ReSharper disable once CheckNamespace
namespace GranDrust.Helpers
{
    public static class MovementHelper
    {
        public static double SpeedModule(this Car car)
        {
            return Math.Sqrt(car.SpeedX*car.SpeedX + car.SpeedY*car.SpeedY);
        }
    }
}
