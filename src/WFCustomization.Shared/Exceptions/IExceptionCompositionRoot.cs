using System;

namespace WFCustomization.Shared.Exceptions
{
    public interface IExceptionCompositionRoot
    {
        ExceptionResponse Map(Exception exception);
    }
}
