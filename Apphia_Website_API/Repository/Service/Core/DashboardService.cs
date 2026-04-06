using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Interface.Core;
using Apphia_Website_API.Repository.ViewModel.Core;

namespace Apphia_Website_API.Repository.Service.Core {
    public class DashboardService : IDashboardService {
        private readonly DatabaseContext _context;
        public DashboardService(DatabaseContext context) { _context = context; }

        public async Task<DashboardLatestGeneratedDateViewModel> ReadLatestGeneratedDate() {
            var latest = await _context.DashboardPages.OrderByDescending(d => d.GeneratedDate).FirstOrDefaultAsync();
            return new DashboardLatestGeneratedDateViewModel { GeneratedDate = latest?.GeneratedDate };
        }

        public async Task<IList<DashboardChartViewModel>> ReadPages(DateTime generatedDate, string domain) {
            return await _context.DashboardPages.Where(d => d.GeneratedDate.Date == generatedDate.Date && d.Domain == domain)
                .Select(d => new DashboardChartViewModel { Name = d.Url, Value = d.Count }).ToListAsync();
        }

        public async Task<IList<DashboardChartViewModel>> ReadDevices(DateTime generatedDate, string domain) {
            return await _context.DashboardDevices.Where(d => d.GeneratedDate.Date == generatedDate.Date && d.Domain == domain)
                .Select(d => new DashboardChartViewModel { Name = d.Device, Value = d.Count }).ToListAsync();
        }

        public async Task<IList<DashboardChartViewModel>> ReadCountries(DateTime generatedDate, string domain) {
            return await _context.DashboardCountries.Where(d => d.GeneratedDate.Date == generatedDate.Date && d.Domain == domain)
                .Select(d => new DashboardChartViewModel { Name = d.Country, Value = d.Count }).ToListAsync();
        }

        public async Task<IList<DashboardChartViewModel>> ReadEngagements(DateTime generatedDate, string domain) {
            return await _context.DashboardEngagements.Where(d => d.GeneratedDate.Date == generatedDate.Date && d.Domain == domain)
                .Select(d => new DashboardChartViewModel { Name = "Engagement", Value = d.TotalTime }).ToListAsync();
        }

        public async Task Create(DashboardCreateViewModel viewModel) {
            // TODO: Parse and store dashboard data from analytics
            await Task.CompletedTask;
        }
    }
}
