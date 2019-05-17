using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System.Linq;
using MySql.Data.MySqlClient;

namespace HairSalon.Controllers
{
  public class StylistsController : Controller
  {

    [HttpGet("/stylists")]
    public ActionResult Index()
    {
        List<Stylist> allStylists = Stylist.GetAll();
        return View(allStylists);
    }

    [HttpGet("/stylists/new")]
    public ActionResult New()
    {
        List<Speciality> allSpecialities = Speciality.GetAll();
        return View(allSpecialities);
    }


    [HttpPost("/stylists")]
    public ActionResult Create(string stylistName, string stylistInformation)
    {
        Stylist newStylist = new Stylist(stylistName, stylistInformation);    
        List<Stylist> allStylists = Stylist.GetAll();
        newStylist.Save();
        return View("Index", allStylists);
    }

  }
}