﻿using CodingTracker.fatihskalemci.Models;
using Dapper;
using Spectre.Console;
using System.Configuration;
using System.Data.SQLite;
using System.Globalization;

namespace CodingTracker.fatihskalemci;

internal class DataBaseConnection
{
    private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBSQLite"].ConnectionString;

    public void CreateTable()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            var sql = @"CREATE TABLE IF NOT EXISTS coding_sessions(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Start TEXT,
                        End TEXT,
                        Duration INTEGER)";

            connection.Open();
            connection.Execute(sql);
            connection.Close();
        }
    }

    internal void AddSession(CodingSession session)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            var sql = @"INSERT INTO coding_sessions (Start, End, Duration)
                        VALUES (@Start, @End, @Duration)";

            var anonymousSession = new
            {
                Start = session.StartTime.ToString("yyyy-MM-dd HH:mm"),
                End = session.EndTime.ToString("yyyy-MM-dd HH:mm"),
                Duration = ((int)Math.Floor(session.Duration.TotalMinutes))
            };

            connection.Open();
            connection.Execute(sql, anonymousSession);
            connection.Close();
        }
    }

    internal void DeleteSession()
    {
        List<CodingSession> sessions = GetSessions();
        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No session to delete[/]");
            Console.WriteLine("Press any key to return to main menu");
            Console.ReadKey();
            return;
        }

        var sessionToDelete = AnsiConsole.Prompt(new SelectionPrompt<CodingSession>()
                                .Title("Please select the Coding Session you want to update")
                                .UseConverter(s => $"{s.StartTime:yyyy-MM-dd HH:mm} | {s.EndTime:yyyy-MM-dd HH:mm} | {s.Duration.Hours} Hours {s.Duration.Minutes} Minutes")
                                .AddChoices(sessions));

        using (var connection = new SQLiteConnection(connectionString))
        {
            var sql = @"DELETE FROM coding_sessions
                        WHERE Id = @Id";

            var parameters = new
            {
                Id = sessionToDelete.Id,
            };

            connection.Open();
            connection.Execute(sql, parameters);
            connection.Close();
        }
    }

    internal void ShowSessions()
    {
        List<CodingSession> sessions = GetSessions("start");

        var table = new Table();

        table.AddColumn("Session Start");
        table.AddColumn("Session Duration");
        table.AddColumn("Session Finish");

        foreach (var session in sessions)
        {
            table.AddRow(session.StartTime.ToString("yyyy-MM-dd HH:mm"), $"{session.Duration.Hours} Hours {session.Duration.Minutes} Minutes", session.EndTime.ToString("yyyy-MM-dd HH:mm"));
        }
        AnsiConsole.Write(table);
        Console.ReadKey();
    }

    internal void UpdateSession()
    {
        List<CodingSession> sessions = GetSessions();

        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No session to update[/]");
            Console.WriteLine("Press any key to return to main menu");
            Console.ReadKey();
            return;
        }

        var sessionToUpdate = AnsiConsole.Prompt(new SelectionPrompt<CodingSession>()
                                .Title("Please select the Coding Session you want to update")
                                .UseConverter(s => $"{s.StartTime:yyyy-MM-dd HH:mm} | {s.EndTime:yyyy-MM-dd HH:mm} | {s.Duration.Hours} Hours {s.Duration.Minutes} Minutes")
                                .AddChoices(sessions));

        var newSession = Helpers.GetSessionFromUser();

        using (var connection = new SQLiteConnection(connectionString))
        {
            var sql = @"UPDATE coding_sessions
                        SET Start = @Start, End = @End, Duration = @Duration
                        WHERE Id = @Id";

            var parameters = new
            {
                Id = sessionToUpdate.Id,
                Start = newSession.StartTime.ToString("yyyy-MM-dd HH:mm"),
                End = newSession.EndTime.ToString("yyyy-MM-dd HH:mm"),
                Duration = ((int)Math.Floor(newSession.Duration.TotalMinutes))
            };

            connection.Open();
            connection.Execute(sql, parameters);
            connection.Close();
        }
    }

    internal List<CodingSession> GetSessions(string sortedBy = "")
    {
        List<CodingSession> sessions = [];

        using (var connection = new SQLiteConnection(connectionString))
        {
            string sql;

            switch (sortedBy)
            {
                case "start":
                    sql = @"SELECT * FROM coding_sessions
                            ORDER BY
                            Start ASC;";
                    break;
                case "end":
                    sql = @"SELECT * FROM coding_sessions
                            ORDER BY
                            End ASC;";
                    break;
                case "duration":
                    sql = @"SELECT * FROM coding_sessions
                            ORDER BY
                            Duration DESC;";
                    break;
                default:
                    sql = "SELECT * FROM coding_sessions";
                    break;
            }

            connection.Open();
            var reader = connection.ExecuteReader(sql);
            while (reader.Read())
            {
                sessions.Add(new CodingSession
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    StartTime = DateTime.ParseExact(reader.GetString(reader.GetOrdinal("Start")), "yyyy-MM-dd HH:mm", new CultureInfo("tr-TR"), DateTimeStyles.None),
                    EndTime = DateTime.ParseExact(reader.GetString(reader.GetOrdinal("End")), "yyyy-MM-dd HH:mm", new CultureInfo("tr-TR"), DateTimeStyles.None),
                    Duration = TimeSpan.FromMinutes(reader.GetInt32(reader.GetOrdinal("Duration")))
                });
            }
            connection.Close();
        }

        return sessions;
    }
}