using GranDrust.Helpers;

// ReSharper disable once CheckNamespace
namespace GranDrust.FSM
{
    interface ITargetState : IState
    {
        Point TargetPoint { get; set; }
    }
}
