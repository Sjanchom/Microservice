using TopsInterface.Entities;

namespace TopsService.Models.DataTranferObjects
{
    public class ResourceParamater : IBaseResourceParameter
    {
        public ResourceParamater(int page, int pageSize, string searchText)
        {
            Page = page;
            PageSize = pageSize;
            SearchText = searchText;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
    }
}
