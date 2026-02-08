using KASHOP.DAL.DTO.Response;
using System.Diagnostics;

namespace KASHOP.PL.Middleware
{
    public class GlobalExceptionHandling
    {
        private readonly RequestDelegate next;

        public GlobalExceptionHandling(RequestDelegate next)
        {
            this.next = next;
        }
        //middleware to return errors
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                var errorDetails = new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Server Error ..",
                  //  StackTrace = e.InnerException.Message
                };
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(errorDetails);
            }

        }
    }
}
