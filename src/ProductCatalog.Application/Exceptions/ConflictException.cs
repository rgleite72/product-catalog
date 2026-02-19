using System.Net;

namespace ProductCatalog.Application.Exceptions;

public sealed class ConflictException : AppException
{
    public ConflictException(string message, string code = "CONFLICT")
        : base(code, message, HttpStatusCode.Conflict) { }
}
