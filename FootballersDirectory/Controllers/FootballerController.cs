using FootballersDirectory.Data;
using FootballersDirectory.Data.Models;
using FootballersDirectory.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballersDirectory.Controllers
{
    public class FootballerController : Controller
    {
        private AppDBContext database;

        public FootballerController(AppDBContext context)
        {
            database = context;
        }

        [HttpGet]
        public async Task<IActionResult> AllFootballers()
        {
            var footballers = await database.Footballers.ToListAsync();
            var model = from footballer in database.Footballers
                        join team in database.Teams
                          on footballer.TeamId equals team.Id
                        select new FootballersWithTeamViewModel
                        {
                            Id = footballer.Id,
                            Birthday = footballer.Birthday,
                            FirstName = footballer.FirstName,
                            LastName = footballer.LastName,
                            Country = footballer.Country,
                            Gender = footballer.Gender,
                            TeamName = team.Name
                        };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddFootballer()
        {
            var model = new AddFootballerViewModel();

            model.Teams = await database.Teams.Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() }).Distinct().ToListAsync();

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> AddFootballer(AddFootballerViewModel model)
        {
            model.Teams = await database.Teams.
                Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() }).Distinct().ToListAsync();

            if (ModelState.IsValid)
            {

                var footballer = new Footballer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Birthday = model.Birthday,
                    Country = model.Country,
                    Gender = model.Gender
                };

                if (String.IsNullOrEmpty(model.TeamName))
                {
                    if (!String.IsNullOrEmpty(model.SelectTeam) &&
                        !model.Teams.Contains(new SelectListItem { Text = model.SelectTeam, Value = model.SelectTeam }))
                        footballer.TeamId = Int32.Parse(model.SelectTeam);
                    else
                    {
                        ModelState.AddModelError("", "Ошибка в выборе команды");
                        return View(model);
                    }
                }
                else
                {
                    var team = new Team { Name = model.TeamName };
                    await database.AddAsync(team);
                    await database.SaveChangesAsync();

                    footballer.TeamId = team.Id;
                }

                await database.AddAsync(footballer);
                await database.SaveChangesAsync();

                return RedirectToAction("AllFootballers");
            }


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditFootballer(int footballerId)
        {
            var footballer = await database.Footballers.FirstOrDefaultAsync(r => r.Id == footballerId);
            var model = new EditFootballerViewModel
            {
                Id = footballer.Id,
                FirstName = footballer.FirstName,
                LastName = footballer.LastName,
                Gender = footballer.Gender,
                Birthday = footballer.Birthday,
                SelectTeam = footballer.TeamId.ToString(),
                Country = footballer.Country
            };

            model.Teams = await database.Teams.
                Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() }).Distinct().ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditFootballer(EditFootballerViewModel model)
        {
            model.Teams = await database.Teams.
                    Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() }).Distinct().ToListAsync();

            if (ModelState.IsValid)
            {
                var footballer = await database.Footballers.FirstOrDefaultAsync(r => r.Id == model.Id);
                if (footballer != null)
                {
                    footballer.FirstName = model.FirstName;
                    footballer.LastName = model.LastName;
                    footballer.Gender = model.Gender;
                    footballer.Country = model.Country;
                    footballer.Birthday = model.Birthday;

                    if (String.IsNullOrEmpty(model.TeamName))
                    {
                        if (!String.IsNullOrEmpty(model.SelectTeam) &&
                            !model.Teams.Contains(new SelectListItem { Text = model.SelectTeam, Value = model.SelectTeam }))
                            footballer.TeamId = Int32.Parse(model.SelectTeam);
                        else
                        {
                            ModelState.AddModelError("", "Ошибка в выборе команды");
                            return View(model);
                        }
                    }
                    else
                    {

                        var team = new Team { Name = model.TeamName };

                        database.AddTeam(team);

                        footballer.TeamId = team.Id;
                    }

                    database.Update(footballer);
                    await database.SaveChangesAsync();

                    return RedirectToAction("AllFootballers");

                }
                else ModelState.AddModelError("", "Пользователь не найден");
            }

            return View(model);
        }
    }
}
