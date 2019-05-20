using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace HairSalon.Tests
{
  [TestClass]
  public class SpecialityTest : IDisposable
  {

    public SpecialityTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=svitlana_filatova_test;";
    }

    public void Dispose()
    {
      Stylist.ClearAll();
      Client.ClearAll();
      Speciality.ClearAll();
    }


    [TestMethod]
    public void SpecialityConstructor_CreatesInstanceOfSpeciality_Speciality()
    {
    string name = "Cut";
    Speciality newSpeciality = new Speciality(name);
    Assert.AreEqual(typeof(Speciality), newSpeciality.GetType());
    }


    [TestMethod]
    public void GetName_ReturnsName_String()
    {
    string name = "Cut";
    Speciality newSpeciality = new Speciality(name);
    string result = newSpeciality.GetName();
    Assert.AreEqual(name, result);
    }


    [TestMethod]
    public void Equals_ReturnsTrueIfNamesAreTheSame_Speciality()
    {
    Speciality firstSpeciality = new Speciality("Cut");
    Speciality secondSpeciality = new Speciality("Cut");
    Assert.AreEqual(firstSpeciality, secondSpeciality);
    }


    [TestMethod]
    public void Find_ReturnsSpecialityInDatabase_Speciality()
    {
    Speciality testSpeciality = new Speciality("Cut");
    testSpeciality.Save();
    Speciality foundSpeciality = Speciality.Find(testSpeciality.GetId());
    Assert.AreEqual(testSpeciality, foundSpeciality);
    }


    [TestMethod]
    public void GetAll_ReturnsEmptyListFromDatabase_SpecialityList()
    {
    List<Speciality> newList = new List<Speciality> { };
    List<Speciality> result = Speciality.GetAll();
    CollectionAssert.AreEqual(newList, result);
    }


    [TestMethod]
    public void GetAll_SpecialityEmptyAtFirst_List()
    {
    int result = Speciality.GetAll().Count;
    Assert.AreEqual(0, result);
    }


    [TestMethod]
    public void GetAll_TakeAllSpecialties_List()
    {
        List<Speciality> newList = new List<Speciality> {};
        List<Speciality> result = Speciality.GetAll();
        CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void GetAll_ReturnsSpecialties_SpecialtiesList()
    {
        Speciality newSpecialty = new Speciality("Cut");
        newSpecialty.Save();
        List<Speciality> newList = new List<Speciality> {newSpecialty};
        List<Speciality> result = Speciality.GetAll();
        CollectionAssert.AreEqual(newList, result);
    }


    [TestMethod]
    public void Delete_DeletesSpecialityFromDatabase_SpecialityList()
    {
      Speciality testSpeciality = new Speciality("Cut");
      testSpeciality.Save();
      Stylist testStylist = new Stylist("Emmaline", "Has an experience in the beauty industry.");
      testStylist.Save();
      testSpeciality.AddStylist(testStylist);
      testSpeciality.Delete();
      List<Speciality> resultStylistSpecialities = testStylist.GetSpecialities();
      List<Speciality> testStylistSpecialities = new List<Speciality> {};
      CollectionAssert.AreEqual(resultStylistSpecialities, testStylistSpecialities);
    }


    [TestMethod]
    public void DeleteAll_DeletesAllSpecialitiesFromDatabase_EmptySpecialityList()
    {
      Speciality testSpeciality1 = new Speciality("Cut");
      testSpeciality1.Save();
      Speciality testSpeciality2 = new Speciality("Color");
      testSpeciality2.Save();
      Stylist testStylist = new Stylist("Emmaline", "Has an experience in the beauty industry.");
      testStylist.Save();
      testSpeciality1.AddStylist(testStylist);
      testSpeciality2.AddStylist(testStylist);
      Speciality.DeleteAll();
      List<Speciality> resultStylistSpecialities = testStylist.GetSpecialities();
      List<Speciality> testStylistSpecialities = new List<Speciality> {};

      CollectionAssert.AreEqual(resultStylistSpecialities, testStylistSpecialities);
    }


    [TestMethod]
    public void GetStylists_ReturnsAllSpecialityStylists_SpecialityList()
    {
    Speciality testSpeciality = new Speciality("Cut");
    testSpeciality.Save();
    Stylist testStylist = new Stylist("Emmaline", "Has an experience in the beauty industry.");
    testStylist.Save();
    Stylist testStylist2 = new Stylist("Emma", "Has an experience in the beauty industry.");
    testStylist2.Save();
    testSpeciality.AddStylist(testStylist);
    List<Stylist> savedStylists = testSpeciality.GetStylists();
    List<Stylist> testList = new List<Stylist> {testStylist};
    CollectionAssert.AreEqual(testList, savedStylists);
    }


    [TestMethod]
    public void Test_AddStylist_AddsStylistToSpeciality()
    {
    Speciality testSpeciality = new Speciality("Cut");
    testSpeciality.Save();
    Stylist testStylist = new Stylist("Emmaline", "Has an experience in the beauty industry.");
    testStylist.Save();
    Stylist testStylist2 = new Stylist("Emma", "Has an experience in the beauty industry.");
    testStylist2.Save();
    testSpeciality.AddStylist(testStylist);
    testSpeciality.AddStylist(testStylist2);
    List<Stylist> result = testSpeciality.GetStylists();
    List<Stylist> testList = new List<Stylist>{testStylist, testStylist2};
    CollectionAssert.AreEqual(testList, result);
    }


    [TestMethod]
    public void Save_SavesSpecialityToDatabase_SpecialityList()
    {
    Speciality testSpeciality = new Speciality("Cut");
    testSpeciality.Save();
    List<Speciality> result = Speciality.GetAll();
    List<Speciality> testList = new List<Speciality>{testSpeciality};
    CollectionAssert.AreEqual(testList, result);
    }


    [TestMethod]
    public void Save_DatabaseAssignsIdToStylist_Id()
    {
    Speciality testSpeciality = new Speciality("Cut");
    testSpeciality.Save();
    Speciality savedSpeciality = Speciality.GetAll()[0];
    int result = savedSpeciality.GetId();
    int testId = testSpeciality.GetId();
    Assert.AreEqual(testId, result);
    }


    [TestMethod]
    public void Edit_UpdatesSpecialityNameInDatabase_String()
    {
    Speciality testSpeciality = new Speciality("Cut");
    testSpeciality.Save();
    string secondName = "Color";
    testSpeciality.Edit(secondName);
    string result = Speciality.Find(testSpeciality.GetId()).GetName();
    Assert.AreEqual(secondName, result);
    }

  }
}
