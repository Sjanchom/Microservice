using TopsInterface.Entities;

namespace TopsShareClass.Models.DataTranferObjects
{
    public class ApoGroupResourceParameter : IApoGroupResourceParameter
    {
        public ApoGroupResourceParameter(int page, int pageSize, int? apoDivisionId, string searchText)
        {
            Page = page;
            PageSize = pageSize;
            ApoDivsionId = apoDivisionId;
            SearchText = searchText;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public int? ApoDivsionId { get; set; }
    }

}
