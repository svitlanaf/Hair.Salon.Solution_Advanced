using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Speciality
  {
    private int _id;
    private string _name;
    

    public Speciality(string specialityName, int id = 0)
    {
      _name = specialityName;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }


    public int GetId()
    {
      return _id;
    }

    public override int GetHashCode()
    {
    return this.GetId().GetHashCode();
    }


    public override bool Equals(System.Object otherSpeciality)
    {
    if (!(otherSpeciality is Speciality))
    {
        return false;
    }
    else
        {
            Speciality newSpeciality = (Speciality) otherSpeciality;
            bool idEquality = this.GetId() == newSpeciality.GetId();
            bool nameEquality = this.GetName() == newSpeciality.GetName();
            return (idEquality && nameEquality);
        }
    }


    public void Dispose()
    {
      Stylist.ClearAll();
      Client.ClearAll();
      Speciality.ClearAll();
    }


    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM specialities;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }


    public void Save()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO specialities (name) VALUES (@name);";

        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@name";
        name.Value = this._name;
        cmd.Parameters.Add(name);

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }


    public static List<Speciality> GetAll()
    {
      List<Speciality> allSpecialities = new List<Speciality> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT id, name FROM specialities;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int SpecialityId = rdr.GetInt32(0);
        string SpecialityName = rdr.GetString(1);
        Speciality newSpeciality = new Speciality(SpecialityName, SpecialityId);
        allSpecialities.Add(newSpeciality);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allSpecialities;
    }


    public static Speciality Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT id, name FROM specialities WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int SpecialityId = 0;
      string SpecialityName = "";
      while(rdr.Read())
      {
        SpecialityId = rdr.GetInt32(0);

        Console.WriteLine(SpecialityId);

        SpecialityName = rdr.GetString(1);

        Console.WriteLine(SpecialityName);
      }
      Speciality newSpeciality = new Speciality(SpecialityName, SpecialityId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newSpeciality;
    }


    public void Delete()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = new MySqlCommand( "DELETE FROM specialities WHERE id = @SpecialityId;", conn);
        MySqlParameter specialityIdParameter = new MySqlParameter();
        specialityIdParameter.ParameterName = "@SpecialityId";
        specialityIdParameter.Value = this.GetId();
        cmd.Parameters.Add(specialityIdParameter);
        cmd.ExecuteNonQuery();

        if (conn != null)
        {
            conn.Close();
        }
    }


    public List<Stylist> GetStylists()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT stylists.* FROM 
            specialities JOIN specialities_stylists ON (specialities.id = specialities_stylists.speciality_id)
                    JOIN stylists ON (specialities_stylists.stylist_id = stylists.id)
                    WHERE specialities.id = @SpecialityId;";
        MySqlParameter specialityIdParameter = new MySqlParameter();
        specialityIdParameter.ParameterName = "@SpecialityId";
        specialityIdParameter.Value = _id;
        cmd.Parameters.Add(specialityIdParameter);
        MySqlDataReader stylistQueryRdr = cmd.ExecuteReader() as MySqlDataReader;
        List<Stylist> stylists = new List<Stylist> {
        };

        while(stylistQueryRdr.Read())
        {
            int thisStylistId = stylistQueryRdr.GetInt32(0);
            string stylistName = stylistQueryRdr.GetString(1);
            string stylistInformation = stylistQueryRdr.GetString(2);
            Stylist newStylist = new Stylist (stylistName, stylistInformation, thisStylistId);
            stylists.Add (newStylist);
        }

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return stylists;
    }


    public void AddStylist (Stylist newStylist)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO specialities_stylists (speciality_id, stylist_id) VALUES (@SpecialityId, @StylistId);";
        MySqlParameter speciality_id = new MySqlParameter();
        speciality_id.ParameterName = "@SpecialityId";
        speciality_id.Value = _id;
        cmd.Parameters.Add(speciality_id);

        MySqlParameter stylist_id = new MySqlParameter();
        stylist_id.ParameterName = "@StylistId";
        stylist_id.Value = newStylist.GetId();
        cmd.Parameters.Add(stylist_id);
        cmd.ExecuteNonQuery();
        conn.Close();
        if(conn != null)
        {
            conn.Dispose();
        }
    }


    public void Edit(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE specialities SET name = @newName WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@newName";
      name.Value = newName;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      _name = newName;
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

  }
}