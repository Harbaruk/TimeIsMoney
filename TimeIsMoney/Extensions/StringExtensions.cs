using System;
using System.Diagnostics;

namespace TimeIsMoney.Api.Extensions
{
    public static class StringExtensions
    {
        public static string FirstLetterToLower(this string str)
        {
            Debug.Assert(str != null);
            if (str.Length == 0)
            {
                return String.Empty;
            }
            return Char.ToLowerInvariant(str[0]) + str.Substring(1);
        }
    }
}