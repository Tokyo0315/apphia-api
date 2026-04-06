using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Dtos;

public record AuditQueryParams(DateTime? From = null, DateTime? To = null, string Filter = "", [Range(0, int.MaxValue)] int PageSize = 10, [Range(1, int.MaxValue)] int PageNumber = 1);
public record AuditAllQueryParams(DateTime? From = null, DateTime? To = null);
