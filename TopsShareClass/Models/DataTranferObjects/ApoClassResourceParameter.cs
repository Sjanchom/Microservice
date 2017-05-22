using TopsInterface.Entities;

namespace TopsShareClass.Models.DataTranferObjects
{
    public class ApoClassResourceParameter : IApoClassResourceParameter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public int? ApoDepartmentId { get; set; }

        public ApoClassResourceParameter()
        {

        }

        public ApoClassResourceParameter(int page, int pageSize, int? apoDepartmentId, string searchText)
        {
            Page = page;
            PageSize = pageSize;
            ApoDepartmentId = apoDepartmentId;
            SearchText = searchText;
        }
    }

}
