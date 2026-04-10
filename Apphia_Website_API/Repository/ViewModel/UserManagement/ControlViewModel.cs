namespace Apphia_Website_API.Repository.ViewModel.UserManagement
{
    public class ControlCreateViewModel
    {
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Restore { get; set; }
    }

    public class ControlReadViewModel
    {
        public int Id { get; set; }
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Restore { get; set; }
    }

    public class ControlUpdateViewModel
    {
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Restore { get; set; }
    }

    public class ControlAuthenticationViewModel
    {
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Restore { get; set; }
    }
}
