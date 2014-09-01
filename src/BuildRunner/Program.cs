using System;
using System.Collections.Generic;
using Args;
using NHibernate;
using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.Database;
using Lacjam.Framework.Utilities;

namespace Lacjam.BuildRunner
{
    class Program
    {
        /// <summary>
        /// Debug args -- 
        /// /d /b /u /c "Data Source=(local)\SQL2012;Database=Lacjam; Integrated Security=SSPI;"
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ShowHeader();

            var cleaned = new List<string>();

            if (args.Length == 0)
            {
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("No args passed to setup");
                Console.WriteLine("");
                Console.WriteLine("");
                ShowHelp();
                return;
            }

            // build script coming in from powershell was keeping the ,,,,,,,   so having to strip them
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Args received");
            Console.WriteLine("");


            foreach (string s in args)
            {
                Console.WriteLine(s);
                cleaned.Add(s.Replace(",", ""));
            }
            Console.WriteLine("");
            Console.WriteLine("");

            CommandArgs command = Configuration.Configure<CommandArgs>().CreateAndBind(cleaned);

            if (command == null)
            {
                Console.WriteLine("Error with args");
                Environment.Exit(1);
            }
            var database = string.IsNullOrEmpty(command.Connectionstring)
                ? new ApplicationDatabase()
                : new ApplicationDatabase().WithConnectionString(command.Connectionstring);

            if (command.Help)
            {
                ShowHelp();
                Pause();
            }

            if (command.DeleteTables)
            {
                FinallyGuarded.Apply(
                    () => DatabaseRunner.DeleteAllTables(database.ConnectionString),
                    ShowError,
                     Completed
                    );
            }

            if (command.BuildDatabase)
            {
                FinallyGuarded.Apply(
                    () => DatabaseRunner.CreateDatabase(database.ConnectionString),
                     ShowError,
                    Completed
                    );
            }

            if (command.UpdateDatabase)
            {
                FinallyGuarded.Apply(
                    () => DatabaseRunner.MigrateToLatest(database.ConnectionString),
                    ShowError,
                    Completed
                    );
            }

            #if DEBUG
            Pause();
            #endif
        }

        private static void ShowHeader()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Metastore Build Tool");
            Console.WriteLine("------------------------");
            Console.WriteLine("This utility performs actions such as.");
            Console.WriteLine("- Database creation/migrations");
            Console.WriteLine("- Replaying test data");
            Console.WriteLine("Be cautious when using the /d option, as it will delete all tables.");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void ShowHelp()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Usage:");
            Console.WriteLine("/help\t\t\tShow help");
            Console.WriteLine("Example:");
            Console.WriteLine(
                @"BuildRunner.exe /b /m");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void Completed()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("");
            Console.WriteLine("...completed");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void ShowError(Exception e)
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("...failed!");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("");
            Console.WriteLine(e);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void Pause()
        {
            Console.WriteLine("");
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadLine();
        }
    }
}
