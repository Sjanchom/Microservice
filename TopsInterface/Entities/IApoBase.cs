namespace TopsInterface.Entities
{
    public interface IApoBase
    {
        int Id { get; set; }
        string Name { get; set; }
        string Code { get; set; }
        int IsActive { get; set; }
    }
}
