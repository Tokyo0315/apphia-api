using Apphia_Website_API.Repository.Model.SystemSetup;
using Apphia_Website_API.Repository.ViewModel.SectionFormatting;

namespace Apphia_Website_API.Repository.Interface.SystemSetup {
    public interface ISectionFormattingService {
        Task<SectionFormatting> Create(SectionFormattingCreateViewModel model, int userId);
        Task<List<SectionFormattingReadViewModel>> Read(int isActive, int pagenumber, int pagesize, string? filter, string? sort);
        Task<SectionFormatting> ReadSF(int id, bool isActive);
        Task<SectionFormattingGrapesJSViewModel> ReadGrapesJS(int id);
        Task<List<SectionFormattingTabs>> ReadTabs();
        Task<SectionFormattingGrapesJSViewModel> ReadWebsite(string page);
        Task<string> Update(SectionFormattingUpdateViewModel model, int id, int userId);
        Task<bool> UpdateGrapesJS(SectionFormattingGrapesJSViewModel model, int id, int userId);
        Task<bool> Delete(int id, int userId);
        Task<bool> Restore(int id, int userId);
        Task<int> Route(int id, int workflowId, int userId);
        Task<string> Remind(int id, int userId, bool remind);
        Task<string> Recall(int id, int userId);
        Task<string> Publish(int id, int userId);
        Task<object> ReadApprovalStatus(int id);
    }
}
