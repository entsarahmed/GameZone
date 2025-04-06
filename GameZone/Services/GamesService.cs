
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

        public Game? GetById(int id)
        {
            return _dbContext.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .SingleOrDefault(g => g.Id == id);
        }
        public async Task Create(CreateGameFormViewModel model)
        {   // Save Cover inside Server
            //Save Game inside Database
            // Save the file inside the Application
            //var coverName = $"{Guid.NewGuid()}{Path.GetExtension(model.Cover.FileName)}";
            ////// Take Only Extension
            ////Save File Inside Selected Position 
            //var path = Path.Combine(_imagesPath, coverName);
            //using var stream = File.Create(path);
            //await model.Cover.CopyToAsync(stream);

            var coverName = await SaveCover(model.Cover);
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

        public async Task<Game?> Update(EditGameFormViewModel model)
        {
            var game = _dbContext.Games
                .Include(g => g.Devices)
                .SingleOrDefault(g => g.Id == model.Id);
            if(game is null)
                return null;

            var hasNewCover = model.Cover is not null;
            var oldCover =  game.Cover;

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d}).ToList();
            
            if(hasNewCover)
            {
                game.Cover = await SaveCover(model.Cover!);
            }
            //SaveChanges => Return Number of Rows because its affected inside Database.
            var effectedRows = _dbContext.SaveChanges();
            if(effectedRows > 0)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }
                return game;
            }
            else
            {
                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);

                return null;
            }
        }

        public bool Delete(int id)
        {
            var isDeleted = false;
            // Make Selection with Game from Database for PrimaryKey with it
            var game = _dbContext.Games.Find(id);

            if(game is null)
                return isDeleted;
            _dbContext.Remove(game);
            var effectedRows = _dbContext.SaveChanges();
            if (effectedRows > 0)
            {
                isDeleted = true;

                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);

            }
            return isDeleted;

        }

        private async Task<string> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
            var path = Path.Combine(_imagesPath, coverName);
            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);
            return coverName;
        }

       
    }
}
