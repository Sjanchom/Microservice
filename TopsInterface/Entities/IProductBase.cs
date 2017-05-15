namespace TopsInterface.Entities
{
    public interface IProductBase
    {
        int Id { get; set; }
        string ApoClassCode { get; set; }
        string ProductCode { get; set; }
        string ProductName { get; set; }
        int? BrandId { get; set; }
    }
}
