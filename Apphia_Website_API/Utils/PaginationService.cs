using Apphia_Website_API.Dtos;

namespace Apphia_Website_API.Utils;

// IPaginationService → Utils/IPaginationService.cs

public class PaginationService : IPaginationService {
    public PaginatedDto<T> ToPagination<T>(IEnumerable<T> logs, int totalCount, int pageNumber, int pageSize) {
        var lastPage = 1;
        if (pageSize > 0) lastPage = (int)Math.Ceiling((double)totalCount / pageSize);
        return new PaginatedDto<T>(logs, totalCount, pageNumber, pageSize, lastPage);
    }
}
