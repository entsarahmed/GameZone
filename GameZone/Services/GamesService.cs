
namespace GameZone.Services
{
    public class GamesService : IGamesService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        public GamesService(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext=dbContext;
            _webHostEnvironment=webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagePath}";
        }
        public IEnumerable<Game> GetAll()
        {
            var games = _dbContext.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .ToList();
            return games;
        }
        public async Task Create(CreateGameFormViewModel model)
        {   // Save Cover inside Server
            //Save Game inside Database
            // Save the file inside the Application
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(model.Cover.FileName)}";
            //// Take Only Extension
            //Save File Inside Selected Position 
            var path = Path.Combine(_imagesPath, coverName);
            using var stream = File.Create(path);
            await model.Cover.CopyToAsync(stream);
            Game game = new()
            {
                Name = model.Name,   
                Description = model.Description,
                CategoryId = model.CategoryId,
                Cover = coverName,
                Devices =model.SelectedDevices.Select( d => new GameDevice { DeviceId = d}).ToList()   
            };
            _dbContext.Add(game);
            _dbContext.SaveChanges();

        }

       
    }
}
