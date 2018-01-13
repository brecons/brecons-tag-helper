//-----------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Bremus Solutions">
//     Copyright (c) Bremus Solutions. All rights reserved.
// </copyright>
// <author>Timm Bremus</author>
// <license>
//      Licensed to the Apache Software Foundation(ASF) under one
//      or more contributor license agreements.See the NOTICE file
//      distributed with this work for additional information
//      regarding copyright ownership.The ASF licenses this file
//      to you under the Apache License, Version 2.0 (the
//      "License"); you may not use this file except in compliance
//      with the License.  You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//      Unless required by applicable law or agreed to in writing,
//      software distributed under the License is distributed on an
//      "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
//      KIND, either express or implied.  See the License for the
//      specific language governing permissions and limitations
//      under the License.
// </license>
//-----------------------------------------------------------------------
namespace BSolutions.Brecons.Core.Extensions
{
    using System.Text.RegularExpressions;

    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts a bootstrap date picker format to c# date time format.
        /// </summary>
        /// <param name="format">The bootstrap date picker format.</param>
        /// <returns>Returns a c# date time format.</returns>
        public static string ConvertDateTojQuery(this string format)
        {
            /*
             *  Date used in this comment : 5th - Nov - 2009 (Thursday)
             *
             *  .NET    JQueryUI        Output      Comment
             *  --------------------------------------------------------------
             *  d       d               5           day of month(No leading zero)
             *  dd      dd              05          day of month(two digit)
             *  ddd     D               Thu         day short name
             *  dddd    DD              Thursday    day long name
             *  M       m               11          month of year(No leading zero)
             *  MM      mm              11          month of year(two digit)
             *  MMM     M               Nov         month name short
             *  MMMM    MM              November    month name long.
             *  yy      y               09          Year(two digit)
             *  yyyy    yy              2009        Year(four digit)             *
             */

            string currentFormat = format;

            // Convert the date
            currentFormat = currentFormat.Replace("dddd", "DD");
            currentFormat = currentFormat.Replace("ddd", "D");

            // Convert month
            if (currentFormat.Contains("MMMM"))
            {
                currentFormat = currentFormat.Replace("MMMM", "MM");
            }
            else if (currentFormat.Contains("MMM"))
            {
                currentFormat = currentFormat.Replace("MMM", "M");
            }
            else if (currentFormat.Contains("MM"))
            {
                currentFormat = currentFormat.Replace("MM", "mm");
            }
            else
            {
                currentFormat = currentFormat.Replace("M", "m");
            }

            return currentFormat;
        }

        /// <summary>
        /// Converts the day format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>Returns a bootstrap date picker day format.</returns>
        private static string ConvertDayFormat(string format)
        {
            // DD = Monday
            if (Regex.Matches(format, "D{2,}").Count > 0)
            {
                return Regex.Replace(format, "D{2,}", "dddd");
            }

            // D = Mon
            if (Regex.Matches(format, "D{1}").Count > 0)
            {
                return Regex.Replace(format, "D{1}", "ddd");
            }

            return format;
        }

        /// <summary>
        /// Converts the month format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>Returns a bootstrap date picker month format.</returns>
        private static string ConvertMonthFormat(string format)
        {
            // MM = December
            if (Regex.Matches(format, "M{2,}").Count > 0)
            {
                return Regex.Replace(format, "M{2,}", "MMMM");
            }

            // M = Dec
            if (Regex.Matches(format, "M{1}").Count > 0)
            {
                return Regex.Replace(format, "M{1}", "MMM");
            }

            // mm = 07
            if (Regex.Matches(format, "m{2,}").Count > 0)
            {
                return Regex.Replace(format, "m{2,}", "MM");
            }

            // m = 7
            if (Regex.Matches(format, "m{1}").Count > 0)
            {
                return Regex.Replace(format, "m{1}", "M");
            }

            return format;
        }
    }
}
