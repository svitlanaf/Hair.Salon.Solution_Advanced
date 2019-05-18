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


    [HttpGet("/specialities/{id}")]
    public ActionResult Show(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Speciality selectedSpeciality = Speciality.Find(id);
        List<Stylist> specialityStylists = selectedSpeciality.GetStylists();
        List<Stylist> allStylists = Stylist.GetAll();
        model.Add("speciality", selectedSpeciality);
        model.Add("specialityStylists", specialityStylists);  
        model.Add("allStylists", allStylists);
        return View(model);
    }


    [HttpPost("/specialities/{specialityId}/stylists/new")]
    public ActionResult AddStylist(int specialityId, int stylistId)
    {
        Speciality speciality = Speciality.Find(specialityId);
        Stylist stylist = Stylist.Find(stylistId);
        speciality.AddStylist(stylist);
        return RedirectToAction("Show",  new { id = specialityId });
    }


    [HttpPost("/specialities/{id}/delete")]
    public ActionResult Destroy(int id)
    {
        Speciality deleteSpeciality = Speciality.Find(id);
        List<Stylist> deleteStylists = deleteSpeciality.GetStylists();
        foreach(Stylist stylist in deleteStylists)
        {
            stylist.Delete();
        }
        deleteSpeciality.Delete();
        return RedirectToAction("Index");
    }

  }
}