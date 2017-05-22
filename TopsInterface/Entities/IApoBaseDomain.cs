namespace TopsInterface.Entities
{
    public interface IApoBaseDomain:IBaseDomain
    {
        int Id { get; set; }
        string Name { get; set; }
        string Code { get; set; }
    }
}