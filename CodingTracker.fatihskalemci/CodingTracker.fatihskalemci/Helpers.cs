using CodingTracker.fatihskalemci.Models;
using Spectre.Console;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Timers;

namespace CodingTracker.fatihskalemci;

class Helpers
{
    private static int TimeCount = 0;
    static public CodingSession GetSessionFromUser(bool stopWatch = false)
    {
        Console.Clear();

        DateTime start, end;

        if (stopWatch)
        {
            start = GetTimeFromUser("Pres any key to start session", stopWatch);
            end = GetTimeFromUser("Press any key to stop session", stopWatch);
        }
        else
        {
            start = GetTimeFromUser("Please Enter Start Time");
            end = GetTimeFromUser("Please Enter End Time");
        }

        TimeSpan duration = end - start;

        return new CodingSession
        {
            StartTime = start,
            EndTime = end,
            Duration = duration
        };
    }

    static private DateTime GetTimeFromUser(string message, bool stopWatch = false)
    {
        Console.Clear();

        DateTime dateInput;

        if (stopWatch)
        {
            dateInput = DateTime.Now;
        }
        else
        {
            AnsiConsole.MarkupLine($"[green4]{message}[/]");
            AnsiConsole.MarkupLine("[green4]Press [/][bold teal]Enter[/][green4] to enter [/][white]Now[/]");
            AnsiConsole.MarkupLine("[green4]Entry should be in following 24H format[/]\n[maroon]yyyy-MM-dd HH:mm[/] (e.g 2025-02-20 19:30)");

            string? userInput = Console.ReadLine();
            
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
        }
        return dateInput;
    }

    internal static void DrawTime(int time)
    {
        int hours = time / 3600;
        int minutes = (time - hours * 3600) / 60;
        int seconds = time - hours * 3600 - minutes * 60;

        string watch = $@"
                       ,--.-----.--.
                       |--|-----|--|
                       |--|     |--|
                       |  |-----|  |
                     __|--|     |--|__
                    /  |  |-----|  |  \
                   /   \__|-----|__/   \
                  /   ______---______   \/\
                 /   /               \   \/
                {{   /                 \   }}
                |  {{    HH : mm : ss   }}  |-,
                |  |    {hours:00} : {minutes:00} : {seconds:00}   |  | |
                |  {{                   }}  |-'
                {{   \                 /   }}
                 \   `------___------'   /\
                  \     __|-----|__     /\/
                   \   /  |-----|  \   /
                    \  |--|     |--|  /
                     --|  |-----|  |--
                       |--|     |--|
                       |--|-----|--|
                       `--'-----`--'";

        Console.Clear();
        Console.WriteLine(watch);
        Console.WriteLine("\nPress the Enter key to end the session\n");
    }

    internal static CodingSession StopWatchSession()
    {
        System.Timers.Timer aTimer;
        DateTime start = GetTimeFromUser("", true);

        Console.WriteLine("\nPress the Enter key to start the session\n");
        Console.ReadLine();

        aTimer = new System.Timers.Timer(1000);
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;

        Console.ReadLine();
        aTimer.Stop();
        aTimer.Dispose();

        DateTime end = GetTimeFromUser("", true);

        TimeSpan duration = end - start;

        return new CodingSession
        {
            StartTime = start,
            EndTime = end,
            Duration = duration
        };
    }
    private static void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        TimeCount++;
        DrawTime(TimeCount);
    }
}
