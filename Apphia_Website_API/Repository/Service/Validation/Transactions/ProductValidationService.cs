using Apphia_Website_API.Repository.Interface.Validation.Transactions;
using Apphia_Website_API.Repository.ViewModel.Transaction;

namespace Apphia_Website_API.Repository.Service.Validation.Transactions
{
    public class ProductValidationService : IProductValidationService
    {
        private readonly DatabaseContext _context;

        public ProductValidationService(DatabaseContext context)
        {
            _context = context;
        }

        public bool ProductCreateValidation(ProductCreateViewModel viewModel)
        {
            if (viewModel == null) throw new Exception("Product object is empty");
            if (string.IsNullOrWhiteSpace(viewModel.Name)) throw new Exception("Name is required");
            if (viewModel.ProductCategoryId <= 0) throw new Exception("Product Category is required");
            return true;
        }

        public bool ProductUpdateValidation(ProductUpdateViewModel viewModel, int productId)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Name)) throw new Exception("Name is required");
            if (viewModel.ProductCategoryId <= 0) throw new Exception("Product Category is required");
            return true;
        }
    }
}
