namespace TopsInterface.Entities
{
    public interface IApoSubClassDataTranferObject : IApoBase
    {
        int ApoClassId { get; set; }
        string ApoClassName { get; set; }
    }
}
