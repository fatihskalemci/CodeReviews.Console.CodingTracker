using CodingTracker.fatihskalemci.Models;
using Spectre.Console;
using System.Globalization;
using static CodingTracker.fatihskalemci.Enums;

namespace CodingTracker.fatihskalemci;

internal class UserInterface(string connectionString)
{
    private readonly DataBaseConnection _dataBase = new(connectionString);

    internal void MainMenu()
    {
        _dataBase.CreateTable();

        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            CodingSession session;

            var menuSelection = AnsiConsole.Prompt(new SelectionPrompt<MenuOptions>()
                .Title("Please select the action you want to perform")
                .AddChoices(Enum.GetValues<MenuOptions>()));

            switch (menuSelection)
            {
                case MenuOptions.AddSession:
                    session = Helpers.GetSessionFromUser();
                    _dataBase.AddSession(session);
                    break;
                case MenuOptions.UpdateSession:
                    _dataBase.UpdateSession();
                    break;
                case MenuOptions.DeleteSession:
                    _dataBase.DeleteSession();
                    break;
                case MenuOptions.ShowSessions:
                    _dataBase.ShowSessions();
                    break;
                case MenuOptions.Reports:
                    ReportMenu();
                    break;
                case MenuOptions.StopWatch:
                    session = Helpers.StopWatchSession();
                    _dataBase.AddSession(session);
                    break;
                case MenuOptions.SetCodingGoal:
                    break;
                case MenuOptions.Exit:
                    exit = true;
                    break;
            }
        }
    }

    internal void ReportMenu()
    {
        var reportSelection = AnsiConsole.Prompt(new SelectionPrompt<ReportOptions>()
                .Title("Please select the action you want to perform")
                .AddChoices(Enum.GetValues<ReportOptions>()));

        switch (reportSelection)
        {
            case ReportOptions.Filter:
                FilterMenu();
                break;
            case ReportOptions.FullReport:
                _dataBase.ShowReport();
                break;
        }
    }

    internal void FilterMenu()
    {
        var filterSelection = AnsiConsole.Prompt(new SelectionPrompt<FilterOptions>()
                .Title("Please select the action you want to perform")
                .AddChoices(Enum.GetValues<FilterOptions>()));

        switch (filterSelection)
        {
            case FilterOptions.Day:
                break;
            case FilterOptions.Week:
                break;
            case FilterOptions.Year:
                break;
        }

    }
}
