namespace Apphia_Website_API.Repository.ViewModel.UserManagement
{
    public class PolicyCreateViewModel
    {
        public string Name { get; set; } = string.Empty;
        public ControlCreateViewModel Controls { get; set; } = new();
    }

    public class PolicyReadViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ControlReadViewModel Controls { get; set; } = new();
    }

    public class PolicyUpdateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ControlUpdateViewModel Controls { get; set; } = new();
    }
}
