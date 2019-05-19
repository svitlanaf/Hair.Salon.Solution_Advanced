# _**Hair Salon**_

## _An application which allows salon employees to manage sylists and clients._

#### _**By Svitlana Filatova**_

## Description

_This application will allow salon employees to manage the salon's stylists and clients._

## Hair Salon Website Specifications
#### _User Stories_

* _As a salon employee, I need to be able to see a list of all our stylists._
* _As an employee, I need to be able to select a stylist, see their details, and see a list of all clients that belong to that stylist._
* _As an employee, I need to add new stylists to our system when they are hired._
* _As an employee, I need to be able to add new clients to a specific stylist. I should not be able to add a client if no stylists have been added._
* _As an employee, I need to be able to delete stylists (all and single)._
* _As an employee, I need to be able to delete clients (all and single)._
* _As an employee, I need to be able to view clients (all and single)._
* _As an employee, I need to be able to edit just the name of a stylist._
* _As an employee, I need to be able to edit all of the information for a client._
* _As an employee, I need to be able to add a specialty and view all specialties that have been added._
* _As an employee, I need to be able to add a specialty to a stylist._
* _As an employee, I need to be able to click on a specialty and see all of the stylists that have that specialty._
* _As an employee, I need to see the stylist's specialties on the stylist's details page._
* _As an employee, I need to be able to add a stylist to a specialty._


## Setup/Installation Requirements

* _To use this application you need to have .NET (ver. 2.2) Framework and Mono tool installed on your computer (https://dotnet.microsoft.com/download/dotnet-core/2.2)._
* _Clone this repository on your desktop._
* _This application database based. To be able to add information and manage the database you need to install and configure MAMP (see instructions here: https://www.learnhowtoprogram.com/c/database-basics-ee7c9fd3-fcd9-4fff-8b1d-5ff7bfcbf8f0/introducing-and-installing-mamp). After starting Servers you need to connect to the server by using the following command - /Applications/MAMP/Library/bin/mysql --host=localhost -uroot -proot. If all steps were correct you should see this prompt - 'mysql>'._
* _Setup instructions to re-create the database(semicolons are important!):_
  _1. CREATE DATABASE hair_salon;_
  _2. USE hair_salon;_
  _3. CREATE TABLE stylists (id serial PRIMARY KEY, name VARCHAR(255), information TEXT);_
  _4. CREATE TABLE clients (id serial PRIMARY KEY, stylist_id INT, name VARCHAR(255), details TEXT, appointment DATE);_
  _5. SHOW TABLES;_
* _Open Terminal (for Mac users) or PowerShell (for Windows users), navigate to HairSalon folder(cd .../Desktop/HairSalon.Solution/HairSalon) and run the following command: dotnet add package MySqlConnector && dotnet restore && dotnet build && dotnet run._
* _Copy http://localhost:5000 link and paste in the browser of your choise_.


## Known Bugs
_No bugs were found during testing._


## Support and contact details

_If you find any issue regarding this application please contact Svitlana Filatova via email: svitlana.filatova@gmail.com._


## Technologies Used

_C#/.NET/ASP.NET Core MVC/phpMyAdmin/MySQL_


### License

*This software is licensed under the MIT license*

Copyright (c) 2019 **Svitlana Filatova**