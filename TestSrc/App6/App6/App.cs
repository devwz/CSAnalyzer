using App6.Infra.Data;

namespace App6
{
    public class App
    {
        ApplicationDbContext _context;

        public App(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Run()
        {
            _ = _context.Product.ToList();
        }
    }
}
