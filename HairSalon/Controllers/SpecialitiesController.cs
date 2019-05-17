using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System.Linq;
using MySql.Data.MySqlClient;

namespace HairSalon.Controllers
{
  public class SpecialitiesController : Controller
  {

    [HttpGet("/specialities")]
    public ActionResult Index()
    {
        List<Speciality> allSpecialities = Speciality.GetAll();
        return View(allSpecialities);
    }


    [HttpGet("/specialities/new")]
    public ActionResult New()
    {
        return View();
    }


    [HttpPost("/specialities")]
    public ActionResult Create(string specialityName)
    {
        Speciality newSpeciality = new Speciality(specialityName);
        newSpeciality.Save();
        List<Speciality> allSpecialities = Speciality.GetAll();
        return View("Index", allSpecialities);
    }

  }
}