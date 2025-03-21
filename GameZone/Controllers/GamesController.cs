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

        public IActionResult Index()
        {
          var games = _gamesService.GetAll();
            return View(games);
        }
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
    }
}
