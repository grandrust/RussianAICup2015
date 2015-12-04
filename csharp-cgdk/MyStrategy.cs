using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.Factories;
using GranDrust.GameEntities;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk {
    public sealed class MyStrategy : IStrategy
    {
        public void Move(Car self, World world, Game game, Move move)
        {
            RaceFactory.GetVehicle(self, world, game, move).Update();
        }
    }
}