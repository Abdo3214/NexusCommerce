using Microsoft.EntityFrameworkCore;
using NexusCommerce.Common.Filtering;
using NexusCommerce.Common.Pagination;
using NexusCommerce.DAL.Data.Context;
using NexusCommerce.DAL.Data.Models;
using NexusCommerce.DAL.Repositories.GenericRepository;

namespace NexusCommerce.DAL.Repositories.ProductRepository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<Product>> GetProductsPagedAsync(ProductFilterParameters filterParams)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (filterParams.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == filterParams.CategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filterParams.Name))
            {
                var search = filterParams.Name.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(filterParams.SortBy))
            {
                var isDesc = filterParams.SortOrder.ToLower() == "desc";
                query = filterParams.SortBy.ToLower() switch
                {
                    "price" => isDesc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                    "name" => isDesc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                    _ => isDesc ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id)
                };
            }
            else
            {
                query = query.OrderBy(p => p.Id);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .ToListAsync();

            return new PagedResult<Product>(items, totalCount, filterParams.PageNumber, filterParams.PageSize);
        }

        public async Task<Product?> GetProductWithCategoryAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
