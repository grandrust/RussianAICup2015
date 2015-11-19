using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.FSM;

// ReSharper disable once CheckNamespace
namespace GranDrust.GameEntities
{
    public abstract class Vehicle
    {
        public Move Move { get; private set; }
        
        public Game Game { get; private set; }

        public World World { get; private set; }
        
        public Car Self { get; private set; }

        protected Vehicle(Car self, World world, Game game, Move move)
        {
            Self = self;
            World = world;
            Game = game;
            Move = move;
        }

        public virtual IState State { get; set; }

        public virtual void Update()
        {
            State.Execute(this);
        }
    }
}