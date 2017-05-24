namespace TopsInterface.Entities
{
    public interface IApoBaseForCreateOrEdit
    {
        string Name { get; set; }
        int UserId { get; set; }
        int Id { get; set; }
    }
}