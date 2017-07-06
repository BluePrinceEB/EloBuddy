namespace Ezreal.Utils
{
    #region Imports

    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Enumerations;

    #endregion

    /// <summary>
    ///     The Logging
    /// </summary>
    internal class Logging
    {
        #region Private Static Fields

        /// <summary>
        ///     Contains Placeholders For Replacements.
        /// </summary>
        private static readonly Dictionary<string, string> Replacements = new Dictionary<string, string>
        {
            {
                "[SEPERATOR]",
                "----------------------------------------------------"
                + Environment.NewLine
            }
        };

        #endregion

        #region Methods

        /// <summary>
        ///     Replaces Placeholders In A Given <see cref="string" />
        /// </summary>
        /// <param name="message">The Message</param>
        /// <returns></returns>
        public static string ReplaceStrings(string message)
        {
            try
            {
                return Replacements.Aggregate(
                    message,
                    (current, replacement) => current.Replace(replacement.Key, replacement.Value));
            }
            catch (Exception e)
            {
                Console.WriteLine("@Logging.cs: Can't replace strings - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     The Add Entry
        /// </summary>
        /// <param name="type">The Type</param>
        /// <param name="message">The Message</param>
        /// <param name="args">The Args</param>
        public static void AddEntry(LoggingEntryType type, string message, params object[] args)
        {
            switch (type)
            {
                case LoggingEntryType.Debug:
                    AddEntryColored(GetFormattedEntry(type, message, args), ConsoleColor.Green);
                    break;
                case LoggingEntryType.Error:
                    AddEntryColored(GetFormattedEntry(type, message, args), ConsoleColor.DarkRed);
                    break;
                case LoggingEntryType.Generic:
                    AddEntryColored(GetFormattedEntry(type, message, args), ConsoleColor.White);
                    break;
                case LoggingEntryType.Info:
                    AddEntryColored(GetFormattedEntry(type, message, args), ConsoleColor.Yellow);
                    break;
                case LoggingEntryType.Warning:
                    AddEntryColored(GetFormattedEntry(type, message, args), ConsoleColor.Red);
                    break;
            }

        }

        /// <summary>
        ///     The Add Colored Entry
        /// </summary>
        /// <param name="message">The Message</param>
        /// <param name="color">The Color</param>
        private static void AddEntryColored(string message, ConsoleColor color)
        {
            try
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.WriteLine("@Logging.cs: Can't set color - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     Formats A Given <see cref="string" /> And Adds Specific Prefixes
        /// </summary>
        /// <param name="type">The Type</param>
        /// <param name="message">The Message</param>
        /// <param name="args">The Args</param>
        /// <returns></returns>
        public static string GetFormattedEntry(LoggingEntryType type, string message, params object[] args)
        {
            try
            {
                return string.Format(
                    "[{0} - Ezreal][{1}]: {2}",
                    DateTime.Now.ToString("HH:mm:ss"),
                    type,
                    string.Format(ReplaceStrings(message), args));
            }
            catch (Exception e)
            {
                Console.WriteLine("@Logging.cs: Can't format string - {0}", e);
                throw;
            }
        }
        #endregion
    }
}
