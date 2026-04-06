using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.UserManagement;
using Apphia_Website_API.Repository.ViewModel.UserManagement;
using Apphia_Website_API.Repository.Model.Audit;
using Apphia_Website_API.Repository.Configuration.Attribute_Extender;
using Apphia_Website_API.Repository.Configuration.Enum;

namespace Apphia_Website_API.Controllers.UserManagement {
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController {
        private readonly IRoleService _roleService;
        private readonly IRolePolicyControlService _rolePolicyControlService;
        private readonly IAuditGenericService<RoleAudit> _auditService;

        public RoleController(
            IRoleService roleService,
            IRolePolicyControlService rolePolicyControlService,
            IAuditGenericService<RoleAudit> auditService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _roleService = roleService;
            _rolePolicyControlService = rolePolicyControlService;
            _auditService = auditService;
        }

        [Authorize]
        [Control(AccessType.Create, Policies.Role)]
        [HttpPost("Create/{userId}")]
        public async Task<IActionResult> Create([FromBody] RoleCreateViewModel model, int userId) {
            try {
                var result = await _roleService.CreateRole(model, userId);
                await _auditService.CreateLog(new RoleAudit {
                    Action = "Create",
                    Details = "Created role: " + model.Name
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Role created successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "RoleController.Create", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.Role)]
        [HttpGet("Read")]
        public async Task<IActionResult> Read([FromQuery] int isActive, [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? filter, [FromQuery] string? sort) {
            try {
                var result = await _roleService.ReadRole(isActive, pageNumber, pageSize, filter, sort);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "RoleController.Read", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.Role)]
        [HttpGet("ReadIndividual/{roleId}")]
        public async Task<IActionResult> ReadIndividual(int roleId) {
            try {
                var result = await _roleService.GetRoleById(roleId);
                if (result == null)
                    return StatusCode(404, _requestStatusHelper.response(404, false, "Role not found", null, null));
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "RoleController.ReadIndividual", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Update, Policies.Role)]
        [HttpPost("Update/{roleId}/{userId}")]
        public async Task<IActionResult> Update([FromBody] RoleUpdateViewModel model, int roleId, int userId) {
            try {
                var result = await _roleService.UpdateRole(model, roleId, userId);
                await _auditService.CreateLog(new RoleAudit {
                    Action = "Update",
                    Details = "Updated role ID: " + roleId
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Role updated successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "RoleController.Update", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Delete, Policies.Role)]
        [HttpPost("Delete/{roleId}/{userId}")]
        public async Task<IActionResult> Delete(int roleId, int userId) {
            try {
                await _roleService.DeleteRole(roleId, userId);
                await _auditService.CreateLog(new RoleAudit {
                    Action = "Delete",
                    Details = "Deleted role ID: " + roleId
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Role deleted successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "RoleController.Delete", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Restore, Policies.Role)]
        [HttpPost("Restore/{roleId}/{userId}")]
        public async Task<IActionResult> Restore(int roleId, int userId) {
            try {
                await _roleService.RestoreRole(roleId, userId);
                await _auditService.CreateLog(new RoleAudit {
                    Action = "Restore",
                    Details = "Restored role ID: " + roleId
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Role restored successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "RoleController.Restore", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll() {
            try {
                var result = await _roleService.GetAll();
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "RoleController.GetAll", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.Role)]
        [HttpGet("ReadRolePolicyControl/{roleId}")]
        public async Task<IActionResult> ReadRolePolicyControl(int roleId) {
            try {
                var result = await _rolePolicyControlService.GetRolePolicyControl(roleId);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "RoleController.ReadRolePolicyControl", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.Role)]
        [HttpGet("ReadRolePolicyControlNotActive/{roleId}")]
        public async Task<IActionResult> ReadRolePolicyControlNotActive(int roleId) {
            try {
                var result = await _rolePolicyControlService.GetRolePolicyControlNotActive(roleId);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "RoleController.ReadRolePolicyControlNotActive", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Create, Policies.Role)]
        [HttpPost("CreateRolePolicyControl/{userId}")]
        public async Task<IActionResult> CreateRolePolicyControl([FromBody] RolePolicyControlCreateViewModel model, int userId) {
            try {
                var result = await _rolePolicyControlService.CreateRolePolicyControl(model, userId);
                await _auditService.CreateLog(new RoleAudit {
                    Action = "Create RolePolicyControl",
                    Details = "Created RPC for RoleId: " + model.RoleId + ", PolicyId: " + model.PolicyId + ", ControlId: " + model.ControlId
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "RolePolicyControl created successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "RoleController.CreateRolePolicyControl", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Delete, Policies.Role)]
        [HttpPost("DeleteRolePolicyControl/{rpcId}/{userId}")]
        public async Task<IActionResult> DeleteRolePolicyControl(int rpcId, int userId) {
            try {
                var result = await _rolePolicyControlService.DeleteRPC(rpcId, userId);
                await _auditService.CreateLog(new RoleAudit {
                    Action = "Delete RolePolicyControl",
                    Details = "Deleted RPC ID: " + rpcId
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "RolePolicyControl deleted successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "RoleController.DeleteRolePolicyControl", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Restore, Policies.Role)]
        [HttpPost("RestoreRolePolicyControl/{rpcId}/{userId}")]
        public async Task<IActionResult> RestoreRolePolicyControl(int rpcId, int userId) {
            try {
                var result = await _rolePolicyControlService.RestorePRC(rpcId, userId);
                await _auditService.CreateLog(new RoleAudit {
                    Action = "Restore RolePolicyControl",
                    Details = "Restored RPC ID: " + rpcId
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "RolePolicyControl restored successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "RoleController.RestoreRolePolicyControl", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.Role)]
        [HttpGet("GetPolicyControlById/{policyId}/{roleId}")]
        public async Task<IActionResult> GetPolicyControlById(int policyId, int roleId) {
            try {
                var result = await _rolePolicyControlService.GetPolicyControlById(policyId, roleId);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "RoleController.GetPolicyControlById", 500, "Internal Server Error");
            }
        }
    }
}
