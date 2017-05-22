namespace TopsInterface.Entities
{
    public interface IApoSubClassForCreateOrEdit : IApoBaseForCreateOrEdit
    {
        int ApoClassId { get; set; }
        string Name { get; set; }
    }
}
