using GranDrust.GameEntities;

// ReSharper disable once CheckNamespace
namespace GranDrust.FSM
{
    public interface IState
    {
        void Execute(Vehicle vehicle);
    }
}
