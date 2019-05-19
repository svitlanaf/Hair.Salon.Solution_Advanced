using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Client
  {
      private int _id;
      private string _name;
      private string _details;
      private DateTime _appointment;

      public Client(string name, string details, DateTime appointment, int id = 0)
      {
          _name = name;
          _details = details;
          _appointment = appointment;
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

    public string GetDetails()
    {
      return _details;
    }

    public DateTime GetAppointment()
    {
        return _appointment;
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


    public override bool Equals(System.Object otherClient)
    {
      if (!(otherClient is Client))
      {
          return false;
      }
      else
      {
        Client newClient = (Client) otherClient;
        bool idEquality = this.GetId() == newClient.GetId();
        bool nameEquality = this.GetName() == newClient.GetName();
        bool detailsEquality = this.GetDetails() == newClient.GetDetails();
        bool appointmentEquality = this.GetAppointment() == newClient.GetAppointment();
        return (idEquality && nameEquality && detailsEquality && appointmentEquality);
      }
    }


    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM clients;";
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
    cmd.CommandText = @"INSERT INTO clients (name, details, appointment) VALUES (@name, @details, @appointment);";
    MySqlParameter name = new MySqlParameter();
    name.ParameterName = "@name";
    name.Value = this._name;
    cmd.Parameters.Add(name);

    MySqlParameter details = new MySqlParameter();
    details.ParameterName = "@details";
    details.Value = this._details;
    cmd.Parameters.Add(details);

    MySqlParameter appointment = new MySqlParameter();
    appointment.ParameterName = "@appointment";
    appointment.Value = this._appointment;
    cmd.Parameters.Add(appointment);

    cmd.ExecuteNonQuery();
    _id = (int) cmd.LastInsertedId;
    conn.Close();
    if (conn != null)
        {
            conn.Dispose();
        }
    }


    public static List<Client> GetAll()
    {
    List<Client> allClients = new List<Client> { };
    MySqlConnection conn = DB.Connection();
    conn.Open();
    MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"SELECT id, name, details, appointment FROM clients;";
    MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
    while(rdr.Read())
    {
        int clientId = rdr.GetInt32(0);
        string clientName = rdr.GetString(1);
        string clientDetails = rdr.GetString(2);
        DateTime clientAppointment = rdr.GetDateTime(3);
        
        Client newClient = new Client(clientName, clientDetails, clientAppointment, clientId);
        allClients.Add(newClient);
    }
    conn.Close();
    if (conn != null)
        {
            conn.Dispose();
        }
    return allClients;
    }



    public static Client Find(int id)
    {
    MySqlConnection conn = DB.Connection();
    conn.Open();
    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"SELECT id, name, details, appointment FROM clients WHERE id = (@searchId);";
    MySqlParameter searchId = new MySqlParameter();
    searchId.ParameterName = "@searchId";
    searchId.Value = id;
    cmd.Parameters.Add(searchId);
    var rdr = cmd.ExecuteReader() as MySqlDataReader;
    int clientId = 0;
    string clientName = "";
    string clientDetails = "";
    DateTime clientAppointment = new DateTime();
    while(rdr.Read())
    {
        clientId = rdr.GetInt32(0);
        clientName = rdr.GetString(1);
        clientDetails = rdr.GetString(2);
        clientAppointment = rdr.GetDateTime(3);
    }
    Client foundClient = new Client(clientName, clientDetails, clientAppointment, clientId);
    conn.Close();
    if (conn != null)
        {
            conn.Dispose();
        }
    return foundClient;
    }


    public void Delete()
    {
    MySqlConnection conn = DB.Connection();
    conn.Open();
    MySqlCommand cmd = new MySqlCommand("DELETE FROM clients WHERE id = @ClientId; DELETE FROM stylists_clients WHERE client_id = @ClientId;", conn);
    MySqlParameter clientParameter = new MySqlParameter();
    clientParameter.ParameterName = "@ClientId";
    clientParameter.Value = _id;
    cmd.Parameters.Add(clientParameter);
    cmd.ExecuteNonQuery();
    conn.Close();
    if (conn != null)
        {
        conn.Dispose();
        }
    }



    public List<Stylist> GetStylists()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT stylist_id FROM stylists_clients WHERE client_id = @ClientId;";
        MySqlParameter clientIdParameter = new MySqlParameter();
        clientIdParameter.ParameterName = "@ClientId";
        clientIdParameter.Value = _id;
        cmd.Parameters.Add(clientIdParameter);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        List<int> stylistIds = new List <int> {
        };
        while (rdr.Read())
        {
            int stylistId = rdr.GetInt32(0);
            stylistIds.Add(stylistId);
        }
        rdr.Dispose();
        List<Stylist> stylists = new List<Stylist> {
        };
        foreach (int stylistId in stylistIds)
        {
            var stylistQuery = conn.CreateCommand() as MySqlCommand;
            stylistQuery.CommandText = @"SELECT * FROM stylists WHERE id = @StylistId;";
            MySqlParameter stylistIdParameter = new MySqlParameter();
            stylistIdParameter.ParameterName = "@StylistId";
            stylistIdParameter.Value = stylistId;
            stylistQuery.Parameters.Add(stylistIdParameter);
            var stylistQueryRdr = stylistQuery.ExecuteReader() as MySqlDataReader;
            while(stylistQueryRdr.Read())
            {
                int thisStylistId = stylistQueryRdr.GetInt32(0);
                string stylistName = stylistQueryRdr.GetString(1);
                string stylistInformailon = stylistQueryRdr.GetString(2);
                Stylist foundStylist = new Stylist(stylistName, stylistInformailon, thisStylistId);
                stylists.Add(foundStylist);
            }
            stylistQueryRdr.Dispose();
        }
        conn.Close();
        if(conn != null)
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
        cmd.CommandText = @"INSERT INTO stylists_clients (stylist_id, client_id) VALUES (@StylistId, @ClientId);";
        MySqlParameter stylist_id = new MySqlParameter();
        stylist_id.ParameterName = "@StylistId";
        stylist_id.Value = newStylist.GetId();
        cmd.Parameters.Add(stylist_id);
        MySqlParameter client_id = new MySqlParameter();
        client_id.ParameterName = "@ClientId";
        client_id.Value = _id;
        cmd.Parameters.Add(client_id);
        cmd.ExecuteNonQuery();


        conn.Close();
        if(conn != null)
        {
        conn.Dispose();
        }
    }


    public void Edit(string newName, string newDetails, DateTime newAppointment)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE clients SET name = @newName, details = @newDetails, appointment = @NewAppointment WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@newName";
      name.Value = newName;
      cmd.Parameters.Add(name);

      MySqlParameter details = new MySqlParameter();
      details.ParameterName = "@newDetails";
      details.Value = newDetails;
      cmd.Parameters.Add(details);

      MySqlParameter appointment = new MySqlParameter();
      appointment.ParameterName = "@newAppointment";
      appointment.Value = newAppointment;
      cmd.Parameters.Add(appointment);
      cmd.ExecuteNonQuery();
      
      _name = newName;
      _details = newDetails;
      _appointment = newAppointment;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
        }
    }

  }
}
