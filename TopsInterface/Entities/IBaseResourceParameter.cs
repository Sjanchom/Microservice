namespace TopsInterface.Entities
{
    public interface IBaseResourceParameter
    {
        int Page { get; set; }
        int PageSize { get; set; }
        string SearchText { get; set; }
    }
}