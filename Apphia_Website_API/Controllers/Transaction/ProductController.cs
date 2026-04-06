using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.Transaction;
using Apphia_Website_API.Repository.ViewModel.Transaction;
using Apphia_Website_API.Repository.Model.Audit;
using Apphia_Website_API.Repository.Configuration.Attribute_Extender;
using Apphia_Website_API.Repository.Configuration.Enum;

namespace Apphia_Website_API.Controllers.Transaction {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController {
        private readonly IProductService _productService;
        private readonly IAuditGenericService<ProductAudit> _auditService;

        public ProductController(
            IProductService productService,
            IAuditGenericService<ProductAudit> auditService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _productService = productService;
            _auditService = auditService;
        }

        [Authorize]
        [Control(AccessType.Create, Policies.Product)]
        [HttpPost("Create/{userId}")]
        public async Task<IActionResult> Create([FromForm] ProductCreateViewModel model, IFormFile? image, int userId) {
            try {
                var result = await _productService.Create(model, userId, image);
                await _auditService.CreateLog(new ProductAudit {
                    Action = "Create",
                    Details = "Created product: " + model.Name
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Product created successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ProductController.Create", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.Product)]
        [HttpGet("Read")]
        public async Task<IActionResult> Read([FromQuery] int isActive, [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? filter, [FromQuery] string? sort) {
            try {
                var result = await _productService.Read(isActive, pageNumber, pageSize, filter, sort);
                int? totalCount = result.Count > 0 ? result[0].TotalCount : 0;
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, totalCount));
            } catch (Exception ex) {
                return await HandleException(ex, "ProductController.Read", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.Product)]
        [HttpGet("ReadIndividual/{id}")]
        public async Task<IActionResult> ReadIndividual(int id) {
            try {
                var result = await _productService.ReadById(id);
                if (result == null)
                    return StatusCode(404, _requestStatusHelper.response(404, false, "Product not found", null, null));
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ProductController.ReadIndividual", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Update, Policies.Product)]
        [HttpPost("Update/{id}/{userId}")]
        public async Task<IActionResult> Update([FromForm] ProductUpdateViewModel model, IFormFile? image, int id, int userId) {
            try {
                await _productService.Update(model, id, userId, image);
                await _auditService.CreateLog(new ProductAudit {
                    Action = "Update",
                    Details = "Updated product ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Product updated successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ProductController.Update", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Delete, Policies.Product)]
        [HttpPost("Delete/{id}/{userId}")]
        public async Task<IActionResult> Delete(int id, int userId) {
            try {
                await _productService.Delete(id, userId);
                await _auditService.CreateLog(new ProductAudit {
                    Action = "Delete",
                    Details = "Deleted product ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Product deleted successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ProductController.Delete", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Restore, Policies.Product)]
        [HttpPost("Restore/{id}/{userId}")]
        public async Task<IActionResult> Restore(int id, int userId) {
            try {
                await _productService.Restore(id, userId);
                await _auditService.CreateLog(new ProductAudit {
                    Action = "Restore",
                    Details = "Restored product ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Product restored successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ProductController.Restore", 500, "Internal Server Error");
            }
        }
    }
}
