using System;
using GranDrust.GameEntities;

namespace GranDrust.FSM.States
{
    public interface IState
    {
        void Enter(Vehicle vehicle);
        void Execute(Vehicle vehicle);
        void Terminate(Vehicle vehicle);
        void Update(Vehicle vehicle);
    }

    public class StateBase : IState
    {
        /// <remarks>
        /// This method shouldn't change Vehicle state
        /// </remarks>
        /// <param name="vehicle"></param>
        public virtual void Enter(Vehicle vehicle)
        {
        }

        public virtual void Execute(Vehicle vehicle)
        {
            throw new System.NotImplementedException("Execute method isn't implemented");
        }

        /// <remarks>
        /// This method shouldn't change Vehicle state
        /// </remarks>
        /// <param name="vehicle"></param>
        public virtual void Terminate(Vehicle vehicle)
        {
        }

        /// <remarks>
        /// Update Vehicle state only here
        /// </remarks>
        /// <param name="vehicle"></param>
        public virtual void Update(Vehicle vehicle)
        {
        }
    }
}
