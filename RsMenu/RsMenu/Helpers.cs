using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RsMenu
{
    public static class StringHelpers
    {
        /// <summary>
        /// Left of the first occurance of c
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">Return everything to the left of this character.</param>
        /// <returns>String to the left of c, or the entire string.</returns>
        public static string LeftOf(this string src, char c)
        {
            string ret = src;
            int idx = src.IndexOf(c);
            if (idx != -1)
            {
                ret = src.Substring(0, idx);
            }
            return ret;
        }

        /// <summary>
        /// Left of the n'th occurance of c.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">Return everything to the left n'th occurance of this character.</param>
        /// <param name="n">The occurance.</param>
        /// <returns>String to the left of c, or the entire string if not found or n is 0.</returns>
        public static string LeftOf(this string src, char c, int n)
        {
            string ret = src;
            int idx = -1;
            while (n > 0)
            {
                idx = src.IndexOf(c, idx + 1);
                if (idx == -1)
                {
                    break; // : might not be correct. Was : Exit While
                }
                n -= 1;
            }
            if (idx != -1)
            {
                ret = src.Substring(0, idx);
            }
            return ret;
        }

        /// <summary>
        /// Right of the first occurance of c
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">The search char.</param>
        /// <returns>Returns everything to the right of c, or an empty string if c is not found.</returns>
        public static string RightOf(this string src, char c)
        {
            string ret = src;
            int idx = src.IndexOf(c);
            if (idx != -1)
            {
                ret = src.Substring(idx + 1);
            }
            return ret;
        }

        /// <summary>
        /// Right of the n'th occurance of c
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">The search char.</param>
        /// <param name="n">The occurance.</param>
        /// <returns>Returns everything to the right of c, or an empty string if c is not found.</returns>
        public static string RightOf(this string src, char c, int n)
        {
            string ret = src;
            int idx = -1;
            while (n > 0)
            {
                idx = src.IndexOf(c, idx + 1);
                if (idx == -1)
                {
                    break; //: might not be correct. Was : Exit While
                }
                n -= 1;
            }

            if (idx != -1)
            {
                ret = src.Substring(idx + 1);
            }

            return ret;
        }

        /// <summary>
        /// Returns everything to the left of the righmost char c.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">The search char.</param>
        /// <returns>Everything to the left of the rightmost char c, or the entire string.</returns>
        public static string LeftOfRightmostOf(this string src, char c)
        {
            string ret = src;
            int idx = src.LastIndexOf(c);
            if (idx != -1)
            {
                ret = src.Substring(0, idx);
            }
            return ret;
        }

        /// <summary>
        /// Returns everything to the right of the rightmost char c.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">The seach char.</param>
        /// <returns>Returns everything to the right of the rightmost search char, or an empty string.</returns>
        public static string RightOfRightmostOf(this string src, char c)
        {
            string ret = src;
            int idx = src.LastIndexOf(c);
            if (idx != -1)
            {
                ret = src.Substring(idx + 1);
            }
            return ret;
        }

        /// <summary>
        /// Returns everything between the start and end chars, exclusive.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="start">The first char to find.</param>
        /// <param name="end">The end char to find.</param>
        /// <returns>The string between the start and stop chars, or an empty string if not found.</returns>
        public static string Between(this string src, char start, char end)
        {
            string ret = src;
            int idxStart = src.IndexOf(start);
            if (idxStart != -1)
            {
                idxStart += 1;
                int idxEnd = src.IndexOf(end, idxStart);
                if (idxEnd != -1)
                {
                    ret = src.Substring(idxStart, idxEnd - idxStart);
                }
            }
            return ret;
        }

        /// <summary>
        /// Returns the number of occurances of "find".
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="find">The search char.</param>
        /// <returns>The # of times the char occurs in the search string.</returns>
        public static int Count(this string src, char find)
        {
            return src.Count(s => s == find);
        }

        /// <summary>
        /// Returns the rightmost char in src.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <returns>The rightmost char, or '\0' if the source has zero length.</returns>
        public static char Rightmost(this string src)
        {
            char c = ' ';
            if (src.Length > 0)
            {
                c = src[src.Length - 1];
            }
            return c;
        }

        public static bool BeginsWith(this string src, char c)
        {
            bool ret = false;

            if (src.Length > 0)
            {
                ret = src[0] == c;
            }

            return ret;
        }

        public static bool EndsWith(this string src, char c)
        {
            bool ret = false;

            if (src.Length > 0)
            {
                ret = src[src.Length - 1] == c;
            }

            return ret;
        }
        /// <summary>
        /// Toes the proper case.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <returns></returns>
        /// 13/04/2010 by gpilar
        public static string ToProperCase(this string original)
        {
            if (string.IsNullOrEmpty(original))
                return original;

            string result = ProperNameRx.Replace(original.ToLower(CultureInfo.CurrentCulture), HandleWord);
            return result;
        }

        /// <summary>
        /// Words to proper case.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        /// 13/04/2010 by gpilar
        public static string WordToProperCase(this string word)
        {
            if (string.IsNullOrEmpty(word))
                return word;

            if (word.Length > 1)
                return Char.ToUpper(word[0], CultureInfo.CurrentCulture) + word.Substring(1);

            return word.ToUpper(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly Regex ProperNameRx = new Regex(@"\b(\w+)\b");

        /// <summary>
        /// 
        /// </summary>
        private static readonly string[] Prefixes = { "mc" };

        /// <summary>
        /// Handles the word.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <returns></returns>
        /// 13/04/2010 by gpilar
        private static string HandleWord(Match m)
        {
            string word = m.Groups[1].Value;

            foreach (string prefix in Prefixes)
            {
                if (word.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase))
                    return prefix.WordToProperCase() + word.Substring(prefix.Length).WordToProperCase();
            }

            return word.WordToProperCase();
        }
    }
}