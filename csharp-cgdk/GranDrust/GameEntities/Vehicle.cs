using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.FSM.States;

// ReSharper disable once CheckNamespace
namespace GranDrust.GameEntities
{
    public abstract class Vehicle
    {
        public Move Move { get; private set; }
        public Map Map { get; set; }

        public Game Game { get; private set; }

        public World World { get; private set; }
        
        public Car Self { get; private set; }

        protected Vehicle(Car self, World world, Game game, Move move, Map map = null)
        {
            Self = self;
            World = world;
            Game = game;
            Move = move;
            Map = map;
        }

        public virtual IState CurrentState { get; protected set; }
        public virtual IState PreviousState { get; protected set; }

        public void ChangeState(IState state)
        {
            PreviousState.Terminate(this);
            PreviousState = CurrentState;
            CurrentState = state;
            CurrentState.Enter(this);
        }

        public virtual void Update()
        {
            CurrentState.Update(this);
            CurrentState.Execute(this);
        }
    }
}