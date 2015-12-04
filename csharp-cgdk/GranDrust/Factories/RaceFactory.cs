using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.GameEntities;

// ReSharper disable once CheckNamespace
namespace GranDrust.Factories
{
    public class RaceFactory
    {
        public static Vehicle GetVehicle(Car car, World world, Game game, Move move)
        {
            switch (car.Type)
            {
                case CarType.Buggy:
                    return new Jeep(car, world, game, move);
                case CarType.Jeep:
                    return new Jeep(car, world, game, move);
                default:
                    return new Buggy(car, world, game, move);
            }
        }
    }
}
