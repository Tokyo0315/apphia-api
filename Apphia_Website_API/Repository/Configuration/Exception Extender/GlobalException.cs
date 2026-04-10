namespace Apphia_Website_API.Repository.Configuration.Exception_Extender {
    public class GlobalException {
        private readonly RequestDelegate _next;

        public GlobalException(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            } catch (Exception ex) {
                context.Response.StatusCode = ex is UnauthorizedAccessException ? 403 : 500;
                context.Response.ContentType = "application/json";
                var error = new { message = ex.Message };
                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }

}
