using System.Net;

namespace WFCustomization.Shared.Exceptions
{
    public record ExceptionResponse(object Response, HttpStatusCode StatusCode);
}
