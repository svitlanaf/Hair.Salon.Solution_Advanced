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


      [HttpGet("/clients/new")]
      public ActionResult New()
      {
          return View();
      }


      [HttpPost("/clients")]
      public ActionResult Create(string name, string details, DateTime appointment)
      {
        Client newClient = new Client(name, details, appointment);
        newClient.Save();
        List<Client> allPatients = Client.GetAll();
        return View("Index", allPatients);
      }

  }
}