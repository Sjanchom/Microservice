namespace TopsInterface.Entities
{
    public interface IAttributeTypeAndValueDomain
    {
        IAttributeTypeDomain AttributeTypeDomain { get; set; }
        IAttributeValueDomain AttributeValueDomain { get; set; }
    }
}
