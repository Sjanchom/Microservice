namespace TopsInterface.Entities
{
    public interface IAttributeTypeAndValueDataTranferObject
    {
        IAttributeTypeDataTranferObject Type { get; set; }
        IAttributeValueDataTranferObject Value { get; set; }
    }
}
