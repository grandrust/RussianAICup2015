using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.GameEntities;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk {
    public sealed class MyStrategy : IStrategy
    {
        public void Move(Car self, World world, Game game, Move move)
        {
            var baggy = new Baggy(self, world, game, move);
            baggy.Update();
        }
    }
}