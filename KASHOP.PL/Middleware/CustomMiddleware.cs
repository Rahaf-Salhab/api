namespace KASHOP.PL.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate next;

        public CustomMiddleware(RequestDelegate next) 
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("processing request");
            await next(context);
            Console.WriteLine("processing response");
        }
    }

}
