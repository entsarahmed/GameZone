
namespace GameZone.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoriesService(ApplicationDbContext dbContext)
        {
            _dbContext=dbContext;
        }
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _dbContext.Categories.Select
                (c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name})
                .OrderBy(c => c.Text)
                .ToList();
        }
    }
}
