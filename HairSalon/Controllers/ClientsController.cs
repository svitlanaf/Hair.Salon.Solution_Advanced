using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System.Linq;
using MySql.Data.MySqlClient;

namespace HairSalon.Controllers
{
  public class ClientsController : Controller
  {

      [HttpGet("/clients")]
      public ActionResult Index()
      {
          List<Client> allClients = Client.GetAll();
          return View(allClients);
      }


      [HttpGet("/stylists/{stylistId}/clients/new")]
      public ActionResult New(int stylistId)
      {
          
          return View(stylistId);
      }


      [HttpPost("/stylists/{stylistId}/clients/new")]
      public ActionResult New(string name, string details, DateTime appointment, int stylistId)
      {
        Stylist stylist = Stylist.Find(stylistId);
        Client newClient = new Client(name, details, appointment);
        newClient.Save();
        newClient.AddStylist(stylist);
        return RedirectToAction("ShowClients", "Stylists", new {id = stylistId});
      }


      [HttpGet("/clients/{id}")]
      public ActionResult Show(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Client selectedClient = Client.Find(id);
        List<Stylist> clientStylists = selectedClient.GetStylists();
        List<Stylist> allStylists = Stylist.GetAll();
        Stylist selectedStylist = Stylist.Find(id);
        List<Speciality> specialityStylists = selectedStylist.GetSpecialities();
        model.Add("selectedClient", selectedClient);
        model.Add("clientStylists", clientStylists);
        model.Add("specialityStylists", specialityStylists);
        model.Add("allStylists", allStylists);
        return View(model);
      }


      [HttpPost("/clients/{clientsId}/stylists/new")]
      public ActionResult AddStylist(int clientId, int stylistId)
      {
        Client client = Client.Find(clientId);
        Stylist stylist = Stylist.Find(stylistId);
        client.AddStylist(stylist);
        return RedirectToAction("Show",  new { id = clientId });
      }


      [HttpPost("/clients/{id}/delete")]
      public ActionResult Destroy(int id)
      {
        Client deleteClient = Client.Find(id);
        deleteClient.Delete();
        return RedirectToAction("Index");
      }


        [HttpPost("/clients/deleteall")]
        public ActionResult DeleteAll()
        {
            Client.DeleteAll();
            return RedirectToAction("Index", "Home");
        }

   
    [HttpGet("/clients/{id}/edit")]
    public ActionResult Edit(int id)
    {
        Client editClient = Client.Find(id);
        return View(editClient);
    }


    [HttpPost("/clients/{clientId}/edit")]
    public ActionResult Update(int clientId, string newName, string newDetails, DateTime newAppointment)
    {
        Client client = Client.Find(clientId);
        client.Edit(newName, newDetails, newAppointment);
        return RedirectToAction("Show", new { id = clientId });
    }

  }
}