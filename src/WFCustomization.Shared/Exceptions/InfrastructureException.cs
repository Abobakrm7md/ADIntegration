using System;

namespace WFCustomization.Shared.Exceptions
{
    public abstract class InfrastructureException : Exception
    {
        public virtual string Code { get; }

        protected InfrastructureException(string message)
            : base(message) { }
    }
}
