using Business.Services.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Petshop.Controllers
{
    public class HomeController : Controller
    {
        ITeamService _teamService;

        public HomeController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public IActionResult Index()
        {
            List<Team> teams = _teamService.GetAllTeams();
            return View(teams);
        }
    }
}
