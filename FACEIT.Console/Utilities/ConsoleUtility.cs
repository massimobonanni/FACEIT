using FACEIT.Core.Entities;
using Figgle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Console.Utilities
{
    internal static class ConsoleUtility
    {
        public static void WriteLine(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
        {
            var currentForegroundColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = foregroundColor;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = currentForegroundColor;
        }

        public static void Write(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
        {
            var currentForegroundColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = foregroundColor;
            System.Console.Write(message);
            System.Console.ForegroundColor = currentForegroundColor;
        }

        public static void WriteLineWithTimestamp(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
        {
            WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] - {message}", foregroundColor);
        }

        public static void WriteWithTimestamp(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Write($"[{DateTime.Now:HH:mm:ss.fff}] - {message}", foregroundColor);
        }

        public static void WriteApplicationBanner()
        {
            WriteLine();
            WriteLine(FiggleFonts.Banner.Render("FACEIT Console"), ConsoleColor.Green);
            WriteLine();
        }

        public static void DisplayPerson(Person person)
        {
            WriteLine($"Person ID: {person.Id}");
            WriteLine($"\tName: {person.Name}");
            WriteLine($"\tProperties:");
            if (person.Properties.Any())
            {
                foreach (var property in person.Properties)
                {
                    WriteLine($"\t\t{property.Key}: {property.Value}");
                }
            }
            else
            {
                WriteLine($"\t\tNo properties found");
            }
            WriteLine($"\tPersisted faces:");
            if (person.PersistedFaceIds.Any())
            {
                foreach (var persistedFaceId in person.PersistedFaceIds)
                {
                    WriteLine($"\t\t{persistedFaceId}");
                }
            }
            else
            {
                WriteLine($"\t\tNo persisted faces found");
            }
            WriteLine();
        }

        public static void DisplayGroup(Group group)
        {
            WriteLine($"Group ID: {group.Id}");
            WriteLine($"\tName: {group.Name}");
            WriteLine($"\tProperties:");
            if (group.Properties.Any())
            {
                foreach (var property in group.Properties)
                {
                    WriteLine($"\t\t{property.Key}: {property.Value}");
                }
            }
            else
            {
                WriteLine($"\t\tNo properties found");
            }
            WriteLine();
        }
    }
}
