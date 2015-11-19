using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.FSM;

// ReSharper disable once CheckNamespace
namespace GranDrust.GameEntities
{
    public class Baggy: Vehicle
    {
        public Baggy(Car self, World world, Game game, Move move) 
            : base(self, world, game, move)
        {
        }

        private static IState _state = Seek.Instance;
        public override IState State
        {
            get { return _state; }
            set { _state = value; }
        }
    }
}
