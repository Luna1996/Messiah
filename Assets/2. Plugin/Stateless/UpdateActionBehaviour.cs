using System;
using System.Threading.Tasks;

namespace Stateless
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal abstract class UpdateActionBehavior
        {
            public abstract void Execute(float deltaTimeInSecond);

            protected UpdateActionBehavior(Reflection.InvocationInfo actionDescription)
            {
                Description = actionDescription ?? throw new ArgumentNullException(nameof(actionDescription));
            }

            internal Reflection.InvocationInfo Description { get; }

            public class Sync : UpdateActionBehavior
            {
                readonly Action<float> _action;

                public Sync(Action<float> action, Reflection.InvocationInfo actionDescription) : base(actionDescription)
                {
                    _action = action;
                }

                public override void Execute(float deltaTimeInSecond)
                {
                    _action(deltaTimeInSecond);
                }
            }
        }
    }
}
