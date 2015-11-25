using GranDrust.Helpers;

namespace GranDrust.FSM.States
{
    public interface ITargetState : IState
    {
        Point TargetPoint { get; set; }
    }
}
