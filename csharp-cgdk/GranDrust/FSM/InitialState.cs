using GranDrust.GameEntities;

// ReSharper disable once CheckNamespace
namespace GranDrust.FSM
{
    public class InitialState : StateBase
    {
        private InitialState()
        {
        }

        private static InitialState _instantce;
        public static InitialState Instance
        {
            get { return _instantce ?? (_instantce = new InitialState()); }
        }

        public override void Update(Vehicle vehicle)
        {
            vehicle.ChangeState(Seek.Instance);
        }
    }
}