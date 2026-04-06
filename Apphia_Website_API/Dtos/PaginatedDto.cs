namespace Apphia_Website_API.Dtos;

public record PaginatedDto<T>(IEnumerable<T> Data, int TotalCount, int PageNumber, int PageSize, int LastPage);
