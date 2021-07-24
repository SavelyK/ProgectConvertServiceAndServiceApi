using Microsoft.AspNetCore.Builder;


namespace RepositoryWebApi.Middlewere
{
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCastomExeptionHandler(this
           IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
