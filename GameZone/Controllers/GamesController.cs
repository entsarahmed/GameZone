using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context)
        {
            _context=context;
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
                Categories = _context.Categories
                .Select(C => new SelectListItem{
                    Value = C.Id.ToString(),
                    Text = C.Name
                })
                .OrderBy( c => c.Text)
                .ToList(),
                Devices = _context.Devices
                .Select(D => new SelectListItem
                {
                    Value= D.Id.ToString(),
                    Text= D.Name
                })
                .OrderBy(D => D.Text)
                .ToList(),
            }; 
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateGameFormViewModel model)
        {
            return View();
        }
    }
}
