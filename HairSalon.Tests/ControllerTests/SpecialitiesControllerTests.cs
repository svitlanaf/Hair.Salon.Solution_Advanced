using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon.Controllers;
using HairSalon.Models;

namespace HairSalon.Tests
{
    [TestClass]
    public class SpecialitiesControllerTests : IDisposable
  {
    public void Dispose()
    {
      Stylist.ClearAll();
      Client.ClearAll();
      Speciality.ClearAll();
    }
    public SpecialitiesControllerTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=svitlana_filatova_test;";
    }


    [TestMethod]
      public void Index_HasCorrectModelType_SpecialityList()
      {
        ViewResult indexView = new SpecialitiesController().Index() as ViewResult;
        var result = indexView.ViewData.Model;
        Assert.IsInstanceOfType(result, typeof(List<Speciality>));
      }

      [TestMethod]
      public void Index_ReturnsCorrectView_True()
      {
        SpecialitiesController controller = new SpecialitiesController();
        ActionResult indexView = controller.Index();
        Assert.IsInstanceOfType(indexView, typeof(ViewResult));
      }

      [TestMethod]
      public void New_ReturnsCorrectView_True()
      {
        SpecialitiesController controller = new SpecialitiesController();
        ActionResult newSpecialityView = controller.New();
        Assert.IsInstanceOfType(newSpecialityView, typeof(ViewResult));
      }


    }
}