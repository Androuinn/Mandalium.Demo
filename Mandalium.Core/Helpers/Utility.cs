using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
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

        public static string CleanXss(string input)
        {
            return !string.IsNullOrEmpty(input) ? HtmlEncoder.Default.Encode(input) : string.Empty;
        }

        public static void CleanXss<T>(T entity)
        {
            PropertyInfo[] properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.PropertyType != typeof(string))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty((string) propertyInfo.GetValue(entity, null)))
                {
                    propertyInfo.SetValue(entity, Utility.CleanXss((string)propertyInfo.GetValue(entity, null)));
                }
            }
        }


    }
}
