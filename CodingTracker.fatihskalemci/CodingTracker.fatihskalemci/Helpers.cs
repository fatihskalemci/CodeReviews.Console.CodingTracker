using CodingTracker.fatihskalemci.Models;
using Spectre.Console;
using System.Globalization;

namespace CodingTracker.fatihskalemci;

class Helpers
{
    static public CodingSession GetSessionFromUser()
    {
        Console.Clear();

        DateTime start = GetTimeFromUser("Please Enter Start Time");
        DateTime end = GetTimeFromUser("Please Enter End Time");
        TimeSpan duration = end - start;

        return new CodingSession
        {
            StartTime = start,
            EndTime = end,
            Duration = duration
        };
    }

    static private DateTime GetTimeFromUser(string message)
    {
        Console.Clear();
        AnsiConsole.MarkupLine($"[green4]{message}[/]");
        AnsiConsole.MarkupLine("[green4]Press [/][bold teal]Enter[/][green4] to enter [/][white]Now[/]");
        AnsiConsole.MarkupLine("[green4]Entry should be in following 24H format[/]\n[maroon]yyyy-MM-dd HH:mm[/] (e.g 2025-02-20 19:30)");
        string? userInput = Console.ReadLine();
        DateTime dateInput;

        if (userInput == "")
        {
            dateInput = DateTime.Now;
        }
        else
        {
            while (!DateTime.TryParseExact(userInput, "yyyy-MM-dd HH:mm", new CultureInfo("tr-TR"), DateTimeStyles.None, out dateInput))
            {
                AnsiConsole.MarkupLine("[red]Check your entry format[/]");
                AnsiConsole.MarkupLine("[green4]Please be sure to type in following 24H format[/]\n[maroon]yyyy-MM-dd HH:mm[/] (e.g 2025-02-20 19:30)");
                userInput = Console.ReadLine();
            }
        }
        return dateInput;
    }
}
