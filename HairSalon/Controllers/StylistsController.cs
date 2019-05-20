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
    public ActionResult Create(string name, string information)
    {
        Stylist newStylist = new Stylist(name, information);    
        List<Stylist> allStylists = Stylist.GetAll();
        newStylist.Save();
        return RedirectToAction("Index");
    }


    [HttpGet("/stylists/{id}")]
    public ActionResult Show(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Stylist stylist = Stylist.Find(id);
        List<Speciality> allSpecialities = Speciality.GetAll();
        List<Client> allClients = Client.GetAll();
        List<Speciality> allStylistSpecialty = stylist.GetSpecialities();

        model.Add("allStylistSpecialty", allStylistSpecialty);
        model.Add("stylist", stylist);
        model.Add("allSpecialities", allSpecialities);  
        model.Add("allClients", allClients);
        return View(model);
    }



    [HttpGet("/stylists/{id}/clients")]
    public ActionResult ShowClients(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Stylist selectedStylist = Stylist.Find(id);
        List<Client> stylistClients = selectedStylist.GetClients();
        List<Client> allClients = Client.GetAll();
        // List<Speciality> allStylistSpecialty = selectedStylist.GetSpecialities();

        // model.Add("allStylistSpecialty", allStylistSpecialty);
        model.Add("stylist", selectedStylist);
        model.Add("stylistClients", stylistClients);  
        model.Add("allClients", allClients);
        return View(model);
    }


    [HttpPost("/stylists/{stylistId}/clients/add")]
    public ActionResult AddClient(int stylistId, int clientId)
    {
        Stylist stylist = Stylist.Find(stylistId);
        Client client = Client.Find(clientId);
        stylist.AddClient(client);
        return RedirectToAction("ShowClients",  new { id = stylistId });
    }

    [HttpPost("/stylists/{stylistId}/specialities/new")]
    public ActionResult AddSpeciality(int stylistId, int specialityId)
    {
        Speciality speciality = Speciality.Find(specialityId);
        Stylist stylist = Stylist.Find(stylistId);
        stylist.AddSpeciality(speciality);
        return RedirectToAction("Show",  new { id = stylistId });
    }


    [HttpPost("/stylists/{id}/delete")]
    public ActionResult Destroy(int id)
    {
        Stylist deleteStylist = Stylist.Find(id);
        deleteStylist.Delete();
        return RedirectToAction("Index");
    }

    [HttpPost("/stylists/deleteall")]
    public ActionResult DeleteAll()
    {
        Stylist.DeleteAll();
        return RedirectToAction("Index", "Home");
    }

    
    [HttpGet("/stylists/{id}/edit")]
    public ActionResult Edit(int id)
    {
        Stylist editStylist = Stylist.Find(id);
        return View(editStylist);
    }


    [HttpPost("/stylists/{stylistId}/edit")]
    public ActionResult Update(int stylistId, string newName)
    {
        Stylist thisStylist = Stylist.Find(stylistId);
        thisStylist.Edit(newName);
        return RedirectToAction("Show", new { id = stylistId });
    }

  }
}