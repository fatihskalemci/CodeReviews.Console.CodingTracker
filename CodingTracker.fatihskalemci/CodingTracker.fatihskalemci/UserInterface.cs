﻿using CodingTracker.fatihskalemci.Models;
using Spectre.Console;
using System.Globalization;
using static CodingTracker.fatihskalemci.Enums;

namespace CodingTracker.fatihskalemci;

internal class UserInterface
{
    private readonly DataBaseConnection dataBase = new();

    internal void MainMenu()
    {
        dataBase.CreateTable();

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
                    dataBase.AddSession(session);
                    break;
                case MenuOptions.UpdateSession:
                    dataBase.UpdateSession();
                    break;
                case MenuOptions.DeleteSession:
                    dataBase.DeleteSession();
                    break;
                case MenuOptions.ShowSessions:
                    dataBase.ShowSessions();
                    break;
                case MenuOptions.ShowReport:
                    break;
                case MenuOptions.StopWatch:
                    break;
                case MenuOptions.Exit:
                    exit = true;
                    break;
            }
        }
    }
}
