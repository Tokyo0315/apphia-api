using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Dtos;
using Apphia_Website_API.Utils;

namespace Apphia_Website_API.Controllers.Audit {
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase {
        private readonly IPaginationService _paginationService;
        private readonly IAuditServiceFactory _serviceFactory;

        public AuditController(IPaginationService paginationService, IAuditServiceFactory serviceFactory) {
            _paginationService = paginationService;
            _serviceFactory = serviceFactory;
        }

        [HttpGet("{type}")]
        public async Task<IActionResult> Get(string type, [FromQuery] AuditQueryParams param) {
            if (!System.Enum.TryParse<AuditType>(type, true, out var auditType))
                return BadRequest($"Invalid audit type: {type}");
            var service = _serviceFactory.GetService(auditType);
            var (logs, totalCount) = await service.GetLogs(param.From, param.To, param.Filter, param.PageSize, param.PageNumber);
            var result = _paginationService.ToPagination(logs, totalCount, param.PageNumber, param.PageSize);
            return Ok(result);
        }

        [HttpGet("All/{type}")]
        public async Task<IActionResult> GetAll(string type, [FromQuery] AuditAllQueryParams param) {
            if (!System.Enum.TryParse<AuditType>(type, true, out var auditType))
                return BadRequest($"Invalid audit type: {type}");
            var service = _serviceFactory.GetService(auditType);
            var logs = await service.GetAllLogs(param.From, param.To);
            return Ok(logs);
        }
    }
}
