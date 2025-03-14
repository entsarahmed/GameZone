
namespace GameZone.Services
{
    public class DevicesService : IDevicesService
    {
        private readonly ApplicationDbContext _dbContext;

        public DevicesService(ApplicationDbContext dbContext)
        {
            _dbContext=dbContext;
        }
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _dbContext.Devices.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name,
            })
               .OrderBy(d => d.Text)
               .AsNoTracking() //improve the Performance because EF No Tracking for Categories & Devices after selection with Database
               .ToList();
        }
    }
}
