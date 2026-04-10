using Apphia_Website_API.Repository.ViewModel.Transaction;

namespace Apphia_Website_API.Repository.Interface.Validation.Transactions
{
    public interface IProductValidationService
    {
        bool ProductCreateValidation(ProductCreateViewModel product);
        bool ProductUpdateValidation(ProductUpdateViewModel product, int productId);
    }
}
