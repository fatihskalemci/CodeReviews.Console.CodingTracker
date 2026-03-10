using CodingTracker.fatihskalemci;
using CodingTracker.fatihskalemci.Models;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

var config = builder.Build();

string connectionString = config.GetConnectionString("DefaultConnection");

UserInterface userInterface = new(connectionString);
userInterface.MainMenu();