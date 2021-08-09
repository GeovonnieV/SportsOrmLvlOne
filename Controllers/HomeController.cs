using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsORM.Models;


namespace SportsORM.Controllers
{
    public class HomeController : Controller
    {

        private static Context _context;

        public HomeController(Context DBContext)
        {
            _context = DBContext;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.BaseballLeagues = _context.Leagues
                .Where(l => l.Sport.Contains("Baseball"))
                .ToList();
            return View();
        }

        [HttpGet("level_1")]
        public IActionResult Level1()
        {
            // ViewBag.<Name of bag> = context.<Name of table>.<Your query goes here> 
            // Womans leagues
            // going into the Leagues table and getting info out of there and putting it in the viewbag 
            ViewBag.WomansLeagues = _context.Leagues
                .Where(l => l.Name.Contains("Womens"))
                .ToList();
            // any hockey
            ViewBag.HockeyLeagues = _context.Leagues
                .Where(l => l.Sport.Contains("Hockey"))
                .ToList();

            // order by location
            ViewBag.TeamAlphaOrder = _context.Teams
                .OrderBy(t => t.Location);

            // order by location
            ViewBag.TeamDescOrder = _context.Teams
                .OrderByDescending(t => t.Location);

            // Teams in dallas
            ViewBag.DallasTeams = _context.Teams
                .Where(t => t.Location.Contains("Dallas"))
                .ToList();
            // all leagues that call themselves "conferences"
            ViewBag.Conferences = _context.Leagues
                .Where(l => l.Name.Contains("Conference"));

            // Player lastName cooper 
            ViewBag.LastNameCooper = _context.Players
                .Where(p => p.LastName == "Cooper");

            // Player FirstName Joshua 
            ViewBag.FirstNameJoshua = _context.Players
                .Where(p => p.FirstName == "Joshua");
            // LastName is Cooper but FirstName is not Joshua
            ViewBag.CooperPlayers = _context.Players
                .Where(player => player.LastName == "Cooper" && player.FirstName != "Joshua");
            // all players with first name "Alexander" OR first name "Wyatt"
            ViewBag.AlexOrWyatt = _context.Players
                .Where(p => p.FirstName == "Alexander" || p.FirstName == "Wyatt");
            // all leagues in the Atlantic region
            ViewBag.AtlanticTeams = _context.Leagues
               .Where(t => t.Name.Contains("Atlantic"))
               .ToList();
            // all teams named the Raptors
            ViewBag.RaptorTeams = _context.Teams
                .Where(t => t.TeamName.Contains("Raptors"));
            // all teams whose location includes "City"
            ViewBag.CityTeams = _context.Teams
                .Where(t => t.Location.Contains("City"));
            // Starts with T
            ViewBag.StartWithT = _context.Teams
               .Where(team => team.TeamName.StartsWith("T"));
            // No football
            ViewBag.NonFootballLeague = _context.Leagues
               .Where(league => league.Sport != "Football");

            return View("Level1");
        }

        [HttpGet("level_2")]
        public IActionResult Level2()
        {
            return View();
        }

        [HttpGet("level_3")]
        public IActionResult Level3()
        {
            return View();
        }

    }
}