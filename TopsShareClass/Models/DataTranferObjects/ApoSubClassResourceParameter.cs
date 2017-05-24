using TopsInterface.Entities;

namespace TopsShareClass.Models.DataTranferObjects
{
    public class ApoSubClassResourceParameter : IApoSubClassResourceParameter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public int? ApoClassId { get; set; }

        public ApoSubClassResourceParameter()
        {

        }

        public ApoSubClassResourceParameter(int page, int pageSize, int? apoClassId, string searchText)
        {
            Page = page;
            PageSize = pageSize;
            ApoClassId = apoClassId;
            SearchText = searchText;
        }
    }

}
