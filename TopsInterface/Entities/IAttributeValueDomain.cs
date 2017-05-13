namespace TopsInterface.Entities
{
    public interface IAttributeValueDomain : IAttributeBase
    {
        string TypeId { get; set; }
        string ApoClass { get; set; }
    }
}
