using NexusCommerce.DAL.Data.Context;
using NexusCommerce.DAL.Data.Models;
using NexusCommerce.DAL.Repositories.GenericRepository;

namespace NexusCommerce.DAL.Repositories.CategoryRepository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
