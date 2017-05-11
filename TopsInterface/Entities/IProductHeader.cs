namespace TopsInterface.Entities
{
    public interface IProductHeader
    {
        int Id { get; set; }
        int ProductId { get; set; }
        int ApoClass { get; set; }
        int TypeId { get; set; }
        int ValueId { get; set; }
    }
}
