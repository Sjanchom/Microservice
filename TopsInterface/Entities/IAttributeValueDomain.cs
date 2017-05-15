namespace TopsInterface.Entities
{
    public interface IAttributeValueDomain : IAttributeBaseDomain
    {
        string TypeId { get; set; }
        string ApoClass { get; set; }
    }
}
