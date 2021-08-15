using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            // The name of the league that Penguins currently belong to 
            ViewBag.PenguinsLeague = _context.Teams
                .Include(team => team.CurrLeague)
                .FirstOrDefault(team => team.TeamName == "Penguins");

            // all teams in the Atlantic Soccer Conference 
            ViewBag.AtlanticTeams = _context.Leagues
                .Include(league => league.Teams)
                .FirstOrDefault(league => league.Name == "Atlantic Soccer Conference");
            // all (current) players on the Boston Penguins (Hint: Boston is the Location, Penguins is the TeamName)
            ViewBag.BostonPlayers = _context.Players
                .Include(player => player.CurrentTeam)
                .Where(player => player.CurrentTeam.TeamName == "Penguins")
                .ToList();

            // all (current) players in the International Collegiate Baseball Conference
            // goes to Players table
            ViewBag.PlayersFromCol = _context.Players
           //  include the current teams field in players
           .Include(players => players.CurrentTeam)
           // Team has the League in it we can include in it
           .ThenInclude(currentTeam => currentTeam.CurrLeague)
           // the player table has current team and the team table has a current league that has a league name
           .Where(
               player => player.CurrentTeam.CurrLeague.Name == "International Collegiate Baseball Conference"
               );

            // Teams with Sophia
            ViewBag.TeamsWithSophia = _context.Teams
              .Include(team => team.CurrentPlayers)
            //   team.currentPlayers gets us in players and it wants any player with the name sophia
              .Where(team => team.CurrentPlayers.Any(player => player.FirstName == "Sophia"));

            // all leagues with a (current) player named "Sophia"
            ViewBag.LeaguesWithSophia = _context.Leagues
                .Include(leg => leg.Teams)
                .ThenInclude(teams => teams.CurrentPlayers)
                // any gets us in the teams table and then the players table to find sophia
                .Where(leg => leg.Teams.Any(team => team.CurrentPlayers.Any(player => player.FirstName == "Sophia")))
                .ToList();

            // all (current) players in the American Conference of Amateur Football with last name "Lopez"
            // Same as icb but we need the last name to be lopez as well
            ViewBag.LopezPlayers = _context.Players
           //  include the current teams field in players
           .Include(players => players.CurrentTeam)
           // Team has the League in it we can include in it
           .ThenInclude(currentTeam => currentTeam.CurrLeague)
           // the player table has current team and the team table has a current league that has a league name
           .Where(
               player => player.CurrentTeam.CurrLeague.Name == "American Conference of Amateur Football" && player.LastName == "Lopez"
               );

            // all football players
            // start in players go to teams from teams to league to grab the sport 
            ViewBag.AllFootballPlayers = _context.Players
                .Include(player => player.CurrentTeam)
                .ThenInclude(currentTeam => currentTeam.CurrLeague)
                 .Where(
                player => player.CurrentTeam.CurrLeague.Sport == "Football"
                );

            // everyone with the last name "Flores" who DOESN'T (currently) play for the Washington Roughriders
            ViewBag.AllFlores = _context.Players
                .Include(player => player.CurrentTeam)
                .Where(player => player.LastName == "Flores" && player.CurrentTeam.TeamName != "Washington Roughriders")
                .ToList();


            return View();
        }

        [HttpGet("level_3")]
        public IActionResult Level3()
        {
            return View();
        }

    }
}