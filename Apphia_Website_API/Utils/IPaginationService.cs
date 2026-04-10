using Apphia_Website_API.Dtos;

namespace Apphia_Website_API.Utils;

public interface IPaginationService
{
    PaginatedDto<T> ToPagination<T>(IEnumerable<T> logs, int totalCount, int pageNumber, int pageSize);
}
