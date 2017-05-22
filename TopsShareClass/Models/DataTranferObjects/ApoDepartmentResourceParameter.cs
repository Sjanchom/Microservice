using TopsInterface.Entities;

namespace TopsShareClass.Models.DataTranferObjects
{
    public class ApoDepartmentResourceParameter : IApoDepartmentResourceParameter
    {
        public ApoDepartmentResourceParameter(int page, int pageSize, int? apoDivision, int? apoGroup, string searchText)
        {
            Page = page;
            PageSize = pageSize;
            SearchText = searchText;
            ApoDivisionId = apoDivision;
            ApoGroupId = apoGroup;

        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public int? ApoDivisionId { get; set; }
        public int? ApoGroupId { get; set; }
    }
}
