namespace Apphia_Website_API.Repository.Model.Transaction {
    public class Contact : BaseModel {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? contactNo { get; set; }
        public string? inquiry { get; set; }
        public string? message { get; set; }
        public bool cookieAccepted { get; set; }
    }
}
