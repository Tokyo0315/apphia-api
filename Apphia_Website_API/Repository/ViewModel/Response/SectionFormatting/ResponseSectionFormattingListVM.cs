using Apphia_Website_API.Repository.Model.SystemSetup;

namespace Apphia_Website_API.Repository.ViewModel.Response.SectionFormatting
{
    public class ResponseSectionFormattingListVM : ResponseApiViewModel
    {
        public List<Model.SystemSetup.SectionFormatting> Data { get; set; } = new List<Model.SystemSetup.SectionFormatting>();
    }
}
