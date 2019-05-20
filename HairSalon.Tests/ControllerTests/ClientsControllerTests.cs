using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon.Controllers;
using HairSalon.Models;

namespace HairSalon.Tests
{
    [TestClass]
    public class ClientsControllerTests : IDisposable
  {
    public void Dispose()
    {
      Stylist.ClearAll();
      Client.ClearAll();
      Speciality.ClearAll();
    }
    public ClientsControllerTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=svitlana_filatova_test;";
    }


    [TestMethod]
      public void Index_HasCorrectModelType_ClientList()
      {
        ViewResult indexView = new ClientsController().Index() as ViewResult;
        var result = indexView.ViewData.Model;
        Assert.IsInstanceOfType(result, typeof(List<Client>));
      }

      [TestMethod]
      public void Index_ReturnsCorrectView_True()
      {
        ClientsController controller = new ClientsController();
        ActionResult indexView = controller.Index();
        Assert.IsInstanceOfType(indexView, typeof(ViewResult));
      }

      [TestMethod]
      public void New_ReturnsCorrectView_True()
      {
        int stylistId = 1;
        ClientsController controller = new ClientsController();
        ActionResult newStylistView = controller.New(stylistId);
        Assert.IsInstanceOfType(newStylistView, typeof(ViewResult));
      }


    }
}