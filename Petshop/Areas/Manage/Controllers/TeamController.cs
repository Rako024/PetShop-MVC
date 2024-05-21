using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Petshop.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin")]
    public class TeamController : Controller
    {
        ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public IActionResult Index()
        {
            List<Team> teams = _teamService.GetAllTeams();
            return View(teams);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Team team)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            

            try
            {
                _teamService.CreateTeam(team);
            }catch(NotFoundTeamException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (NotFoundPhotoFileException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (DataTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _teamService.DeleteTeam(id);
            }
            catch (NotFoundTeamException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View("Error");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Salam()
        {
            return View();
        }

        public IActionResult Update(int id)
        {
            Team team = _teamService.GetTeam(x => x.Id == id);
            if(team == null)
            {
                ModelState.AddModelError("", "Team is null!");
                return RedirectToAction (nameof(Index));
            }
            return View(team);  
        }

        [HttpPost]
        public IActionResult Update(Team newTeam)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _teamService.UpdateTeam(newTeam.Id, newTeam);
            }
            catch (NotFoundTeamException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View("Error");
            }
            catch (DataTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
