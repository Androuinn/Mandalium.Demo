using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandalium.Core.Helpers
{
    public static class Utility
    {

        public static void ReportError(Exception exception)
        {
            ReportError(exception, string.Empty);
        }

        public static void ReportError(Exception exception, string message)
        {
            string errorMessage;
            if (!string.IsNullOrEmpty(message))
                errorMessage = message;
            else
                errorMessage = exception.StackTrace;

            LogManager.GetLogger("Mandalium Nlog").Error(exception, errorMessage);
        }



        public static string ReplaceCharactersWithWhiteSpace(string[] characters, string input)
        {
            if (characters == null || !characters.Any() || string.IsNullOrEmpty(input))
                return input;

            foreach (string character in characters)
            {
                input = input.Replace(character, string.Empty);
            }
            return input;
        }
    }
}
