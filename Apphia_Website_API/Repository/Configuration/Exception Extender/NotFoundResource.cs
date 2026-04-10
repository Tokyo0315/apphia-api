namespace Apphia_Website_API.Repository.Configuration.Exception_Extender
{
    public class NotFoundResource : Exception
    {
        public NotFoundResource(string message) : base(message) { }
        public NotFoundResource(string message, Exception innerException) : base(message, innerException) { }
        public NotFoundResource() : base("The requested resource was not found.") { }
    }
}
