using WFCustomization.Shared.Exceptions;

namespace WFCustomization.Infrastructure.Exceptions
{
    public class EmptyOrderException : InfrastructureException
    {
        public EmptyOrderException()
            : base($"Empty order defined.") { }
    }
}
