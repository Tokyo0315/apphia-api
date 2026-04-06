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
    public class ProductCategoryController : BaseController {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IAuditGenericService<ProductCategoryAudit> _auditService;

        public ProductCategoryController(
            IProductCategoryService productCategoryService,
            IAuditGenericService<ProductCategoryAudit> auditService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _productCategoryService = productCategoryService;
            _auditService = auditService;
        }

        [Authorize]
        [Control(AccessType.Create, Policies.ProductCategory)]
        [HttpPost("Create/{userId}")]
        public async Task<IActionResult> Create([FromBody] ProductCategoryCreateViewModel model, int userId) {
            try {
                var result = await _productCategoryService.Create(model, userId);
                await _auditService.CreateLog(new ProductCategoryAudit {
                    Action = "Create",
                    Details = "Created product category: " + model.Name
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Product Category created successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ProductCategoryController.Create", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.ProductCategory)]
        [HttpGet("Read")]
        public async Task<IActionResult> Read([FromQuery] int isActive, [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? filter, [FromQuery] string? sort) {
            try {
                var result = await _productCategoryService.Read(isActive, pageNumber, pageSize, filter, sort);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ProductCategoryController.Read", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.ProductCategory)]
        [HttpGet("ReadIndividual/{id}")]
        public async Task<IActionResult> ReadIndividual(int id) {
            try {
                var result = await _productCategoryService.ReadById(id);
                if (result == null)
                    return StatusCode(404, _requestStatusHelper.response(404, false, "Product Category not found", null, null));
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ProductCategoryController.ReadIndividual", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Update, Policies.ProductCategory)]
        [HttpPost("Update/{id}/{userId}")]
        public async Task<IActionResult> Update([FromBody] ProductCategoryUpdateViewModel model, int id, int userId) {
            try {
                await _productCategoryService.Update(model, id, userId);
                await _auditService.CreateLog(new ProductCategoryAudit {
                    Action = "Update",
                    Details = "Updated product category ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Product Category updated successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ProductCategoryController.Update", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Delete, Policies.ProductCategory)]
        [HttpPost("Delete/{id}/{userId}")]
        public async Task<IActionResult> Delete(int id, int userId) {
            try {
                await _productCategoryService.Delete(id, userId);
                await _auditService.CreateLog(new ProductCategoryAudit {
                    Action = "Delete",
                    Details = "Deleted product category ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Product Category deleted successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ProductCategoryController.Delete", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Restore, Policies.ProductCategory)]
        [HttpPost("Restore/{id}/{userId}")]
        public async Task<IActionResult> Restore(int id, int userId) {
            try {
                await _productCategoryService.Restore(id, userId);
                await _auditService.CreateLog(new ProductCategoryAudit {
                    Action = "Restore",
                    Details = "Restored product category ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Product Category restored successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ProductCategoryController.Restore", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll() {
            try {
                var result = await _productCategoryService.GetAll();
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ProductCategoryController.GetAll", 500, "Internal Server Error");
            }
        }
    }
}
