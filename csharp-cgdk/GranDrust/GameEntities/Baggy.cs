using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.FSM;

// ReSharper disable once CheckNamespace
namespace GranDrust.GameEntities
{
    public class Baggy: Vehicle
    {
        private static Map _map;
        private static Map GetMap(Baggy vehicle)
        {
            if (_map == null)
            {
                _map = new Map();
                _map.Build(vehicle);
            }

            return _map;
        }

        public Baggy(Car self, World world, Game game, Move move) 
            : base(self, world, game, move)
        {
            Map = GetMap(this);
        }

        private static IState _state = Seek.Instance;
        public override IState State
        {
            get { return _state; }
            set { _state = value; }
        }
    }
}
