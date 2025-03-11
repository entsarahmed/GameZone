namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesService _devicesService;

        public GamesController(ApplicationDbContext context, ICategoriesService categoriesService, IDevicesService devicesService)
        {
            _context=context;
            _categoriesService=categoriesService;
            _devicesService=devicesService;
        }

        public IActionResult Index()
        {
            return View();
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
        public IActionResult Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();
                model.Devices =_devicesService.GetSelectList();
                return View(model);
            }

            //Save Game inside Database
            //Save Cover inside Server
            return RedirectToAction(nameof(Index));
        }
    }
}
