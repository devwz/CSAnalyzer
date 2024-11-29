using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App6.Infra.Data
{
    public interface IProductService
    {
        public int Add(Product product);
        public List<Product> All();
        public void Delete(int id);
        public Product Find(int id);
    }

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public int Add(Product product)
        {
            string cmd = ProductSql.AddCmd;
            return product.Id = _context.SqlConnection.ExecuteScalar<int>(
                cmd,
                new { product.Title, product.Price });
        }

        public List<Product> All()
        {
            string cmd = ProductSql.AllCmd;
            return _context.SqlConnection.Query<Product>(cmd)
                .ToList();
        }

        public void Delete(int id)
        {
            string cmd = ProductSql.DeleteCmd;
            _context.SqlConnection.Execute(cmd, new { Id = id });
        }

        public Product Find(int id)
        {
            string cmd = ProductSql.FindCmd;
            return _context.SqlConnection.Query<Product>(cmd, new { Id = id })
                .FirstOrDefault();
        }
    }
}
