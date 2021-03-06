using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Stylist
  {
      private int _id;
      private string _name;
      private string _information;

      public Stylist(string name, string information, int id = 0)
      {
          _name = name;
          _information = information;
          _id = id;
      }

    public int GetId()
    {
    return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public string GetInformation()
    {
      return _information;
    }


    public override int GetHashCode()
    {
    return this.GetId().GetHashCode();
    }

    public void Dispose()
    {
      Stylist.ClearAll();
      Client.ClearAll();
      Speciality.ClearAll();
    }


    public override bool Equals(System.Object otherStylist)
    {
      if (!(otherStylist is Stylist))
      {
          return false;
      }
      else
      {
        Stylist newStylist = (Stylist) otherStylist;
        bool idEquality = this.GetId().Equals(newStylist.GetId());
        bool nameEquality = this.GetName().Equals(newStylist.GetName());
        bool informationEquality = this.GetInformation().Equals(newStylist.GetInformation());
        return (idEquality && nameEquality && informationEquality);
      }
    }


    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM stylists;";
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
      cmd.CommandText = @"INSERT INTO stylists (name, information) VALUES (@name, @information);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      MySqlParameter information = new MySqlParameter();
      information.ParameterName = "@information";
      information.Value = this._information;
      cmd.Parameters.Add(information);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
          {
              conn.Dispose();
          }
    }


    public static List<Stylist> GetAll()
    {
      List<Stylist> allStylists = new List<Stylist> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT id, name, information FROM stylists;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int stylistId = rdr.GetInt32(0);
        string stylistName = rdr.GetString(1);
        string stylistInformation = rdr.GetString(2);
        
        Stylist newStylist = new Stylist(stylistName, stylistInformation, stylistId);
        allStylists.Add(newStylist);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStylists;
    }


    public static Stylist Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT id, name, information FROM stylists WHERE id = @thisId;";
      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int stylistId = 0;
      string stylistName = "";
      string stylistInformation = "";
      while (rdr.Read())
      {
        stylistId = rdr.GetInt32(0);
        stylistName = rdr.GetString(1);
        stylistInformation = rdr.GetString(2);
      }
      Stylist foundStylist= new Stylist(stylistName, stylistInformation, stylistId);
       conn.Close();
       if (conn != null)
       {
         conn.Dispose();
       }
      return foundStylist;
    }


    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand( "DELETE FROM stylists WHERE id = @StylistId; DELETE FROM specialities_stylists WHERE stylist_id = @StylistId; DELETE FROM stylists_clients WHERE stylist_id = @StylistId;", conn);
      MySqlParameter stylistIdParameter = new MySqlParameter();
      stylistIdParameter.ParameterName = "@StylistId";
      stylistIdParameter.Value = this.GetId();
      cmd.Parameters.Add(stylistIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
          conn.Close();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM stylists; DELETE FROM specialities_stylists; DELETE FROM stylists_clients;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }



    public List<Client> GetClients()
    {

      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT clients.* FROM
          stylists JOIN stylists_clients ON (stylists.id = stylists_clients.stylist_id)
                  JOIN clients ON (stylists_clients.client_id = clients.id)
                  WHERE stylists.id = @StylistId;";
      MySqlParameter stylistIdParameter = new MySqlParameter();
      stylistIdParameter.ParameterName = "@StylistId";
      stylistIdParameter.Value = _id;
      cmd.Parameters.Add(stylistIdParameter);
      MySqlDataReader clientQueryRdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Client> clients = new List<Client> {
      };

      while(clientQueryRdr.Read())
      {
          int thisClientId = clientQueryRdr.GetInt32(0);
          string clientName = clientQueryRdr.GetString(1);
          string clientDetails = clientQueryRdr.GetString(2);
          DateTime clientAppointment = clientQueryRdr.GetDateTime(3);

          Client newClient = new Client (clientName, clientDetails, clientAppointment, thisClientId);
          clients.Add (newClient);
      }

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return clients;
    }


    public void AddClient (Client newClient)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO stylists_clients (stylist_id, client_id) VALUES (@StylistId, @ClientId);";
      MySqlParameter stylist_id = new MySqlParameter();
      stylist_id.ParameterName = "@StylistId";
      stylist_id.Value = _id;
      cmd.Parameters.Add(stylist_id);
      MySqlParameter client_id = new MySqlParameter();
      client_id.ParameterName = "@ClientId";
      client_id.Value = newClient.GetId();
      cmd.Parameters.Add(client_id);
      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
          conn.Dispose();
      }
    }


    public List<Speciality> GetSpecialities()
    {

      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT specialities.* FROM
          stylists JOIN specialities_stylists ON (stylists.id = specialities_stylists.stylist_id)
                  JOIN specialities ON (specialities_stylists.speciality_id = specialities.id)
                  WHERE stylists.id = @StylistId;";
      MySqlParameter stylistIdParameter = new MySqlParameter();
      stylistIdParameter.ParameterName = "@StylistId";
      stylistIdParameter.Value = _id;
      cmd.Parameters.Add(stylistIdParameter);
      MySqlDataReader specialityQueryRdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Speciality> specialities = new List<Speciality> {
      };

      while(specialityQueryRdr.Read())
      {
          int specialityId = specialityQueryRdr.GetInt32(0);
          string specialityName = specialityQueryRdr.GetString(1);
          Speciality newSpeciality = new Speciality (specialityName, specialityId);
          specialities.Add (newSpeciality);
      }

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return specialities;
    }


    public void AddSpeciality (Speciality newSpeciality)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO specialities_stylists (speciality_id, stylist_id) VALUES (@SpecialityId, @StylistId);";
      MySqlParameter speciality_id = new MySqlParameter();
      speciality_id.ParameterName = "@SpecialityId";
      speciality_id.Value = newSpeciality.GetId();
      cmd.Parameters.Add(speciality_id);

      MySqlParameter stylist_id = new MySqlParameter();
      stylist_id.ParameterName = "@StylistId";
      stylist_id.Value = _id;
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
      cmd.CommandText = @"UPDATE stylists SET name = @newName WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@newName";
      name.Value = newName;
      cmd.Parameters.Add(name);

    //   MySqlParameter information = new MySqlParameter();
    //   information.ParameterName = "@newInformation";
    //   information.Value = newInformation;
    //   cmd.Parameters.Add(information);

      cmd.ExecuteNonQuery();
      _name = newName;
    //   _information = newInformation;
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }



  }
}