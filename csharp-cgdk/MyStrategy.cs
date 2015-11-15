using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.Factories;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk {
    public sealed class MyStrategy : IStrategy {
        public void Move(Car self, World world, Game game, Move move) {

            var stratagy = RaceFactory.GetRaceStratagy(self);

            stratagy.Move(self, world, game, move);
        }
    }
}