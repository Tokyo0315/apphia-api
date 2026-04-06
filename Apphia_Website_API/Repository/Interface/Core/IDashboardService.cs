using Apphia_Website_API.Repository.ViewModel.Core;

namespace Apphia_Website_API.Repository.Interface.Core {
    public interface IDashboardService {
        Task<DashboardLatestGeneratedDateViewModel> ReadLatestGeneratedDate();
        Task<IList<DashboardChartViewModel>> ReadPages(DateTime generatedDate, string domain);
        Task<IList<DashboardChartViewModel>> ReadDevices(DateTime generatedDate, string domain);
        Task<IList<DashboardChartViewModel>> ReadCountries(DateTime generatedDate, string domain);
        Task<IList<DashboardChartViewModel>> ReadEngagements(DateTime generatedDate, string domain);
        Task Create(DashboardCreateViewModel viewModel);
    }
}
