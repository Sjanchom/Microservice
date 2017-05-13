namespace TopsInterface.Entities
{
    public interface IProductDetail
    {
        string Id { get; set; }
        string ProductId { get; set; }
        string ApoClass { get; set; }
        string TypeId { get; set; }
        string ValueId { get; set; }
    }
}
