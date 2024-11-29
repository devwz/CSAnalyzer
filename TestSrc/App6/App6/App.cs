using App6.Infra.Data;

namespace App6
{
    public class App
    {
        private readonly IProductService _productService;

        public App(IProductService productService)
        {
            _productService = productService;
        }

        public void Run()
        {
            Product product = new()
            {
                Title = "Product 1",
                Price = 10
            };

            _productService.Add(product);

            Product product1 = _productService.Find(product.Id) ?? throw new NotImplementedException();
            _productService.Delete(product1.Id);
        }
    }
}
