using App5.Data;

namespace App5
{
    public class App
    {
        ProductService _productService;

        public App(ProductService productService)
        {
            _productService = productService;
        }

        public void Run()
        {
            _ = _productService.GetProductList();
        }
    }
}
