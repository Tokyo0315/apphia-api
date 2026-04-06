namespace Apphia_Website_API.Repository.Model.UserManagement {
    public class Control : BaseModel {
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Restore { get; set; }
    }
}
