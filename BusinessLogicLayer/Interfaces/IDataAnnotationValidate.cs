using DataTransferObjects;

namespace BusinessLogicLayer.Interfaces
{
    public interface IDataAnnotationsValidator
    {
        void ValidateModel(object model, ValidatorResponse validate);
    }
}
