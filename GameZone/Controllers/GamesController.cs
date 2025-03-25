namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesService _devicesService;
        private readonly IGamesService _gamesService;

        public GamesController(ApplicationDbContext context, ICategoriesService categoriesService, IDevicesService devicesService, IGamesService gamesService)
        {
            _context=context;
            _categoriesService=categoriesService;
            _devicesService=devicesService;
             _gamesService=gamesService;
        }

        #region Index
        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }

        #endregion

        #region Details

        public IActionResult Details(int id)
        {
            var game = _gamesService.GetById(id);
           if (game == null) 
                return NotFound();
            return View(game);
        }

        #endregion

        #region Create
        public IActionResult Create()
        {
            ViewData["Title"] = "Add Game"; // Add this line to set the ViewData

            CreateGameFormViewModel viewModel = new()
            {
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectList(),
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();
                model.Devices =_devicesService.GetSelectList();
                return View(model);
            }

            //Save Game inside Database
            //Save Cover inside Server
            await _gamesService.Create(model);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Update

        public IActionResult Edit(int id)
        {
            var game = _gamesService.GetById(id);
            if (game is null)
                return NotFound();
            EditGameFormViewModel viewModel = new()
            {//Make Initialization  for values of ViewModel Need it.
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                Categories = _categoriesService.GetSelectList(),
                SelectedDevices=game.Devices.Select(d => d.DeviceId).ToList(),
                Devices=_devicesService.GetSelectList(),
                CurrentCover = game.Cover,
            };
            return View(viewModel);
            
        }     
        #endregion

    }
}
