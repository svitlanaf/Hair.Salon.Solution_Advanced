using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon.Controllers;
using HairSalon.Models;

namespace HairSalon.Tests
{
    [TestClass]
    public class StylistsControllerTests : IDisposable
  {
    public void Dispose()
    {
      Stylist.ClearAll();
      Client.ClearAll();
      Speciality.ClearAll();
    }
    public StylistsControllerTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=svitlana_filatova_test;";
    }


    [TestMethod]
      public void Index_HasCorrectModelType_StylistList()
      {
        ViewResult indexView = new StylistsController().Index() as ViewResult;
        var result = indexView.ViewData.Model;
        Assert.IsInstanceOfType(result, typeof(List<Stylist>));
      }

      [TestMethod]
      public void Index_ReturnsCorrectView_True()
      {
        StylistsController controller = new StylistsController();
        ActionResult indexView = controller.Index();
        Assert.IsInstanceOfType(indexView, typeof(ViewResult));
      }

      [TestMethod]
      public void New_ReturnsCorrectView_True()
      {
        StylistsController controller = new StylistsController();
        ActionResult newStylistView = controller.New();
        Assert.IsInstanceOfType(newStylistView, typeof(ViewResult));
      }

    }
}