using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Contracts;
using ProductCatalog.Api.Middlewares;

namespace ProductCatalog.Api.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected string TraceId => RequestCorrelationMiddleware.GetRequestId(HttpContext);

    protected ActionResult<ApiResponse<T>> OkResponse<T>(T data)
        => Ok(ApiResponse<T>.Ok(data, TraceId));
}




