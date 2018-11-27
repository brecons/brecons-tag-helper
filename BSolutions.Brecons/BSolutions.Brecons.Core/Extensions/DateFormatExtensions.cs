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
    /// <summary>
    /// Extensions for DateTime Handling.
    /// </summary>
    public static class DateFormatExtensions
    {
        /// <summary>
        /// Converts a .NET date format string into a jQuery format string.
        /// </summary>
        /// <param name="format">The .NET date format string.</param>
        /// <returns>Returns a jQuery date format string.</returns>
        public static string ConvertNetFormatTojQuery(this string format)
        {
            /*
             *  .NET    JQueryUI        Output
             *  ---------------------------------
             *  d       d               1 ... 31
             *  dd      dd              01 ... 31
             *  ddd     D               Thu
             *  dddd    DD              Thursday
             *  M       m               1 ... 12
             *  MM      mm              01 ... 12
             *  MMM     M               Nov
             *  MMMM    MM              November
             *  yy      yy              09
             *  yyyy    yyyy            2009
             */

            // Day
            format = format.Replace("dddd", "DD");
            format = format.Replace("ddd", "D");

            // Month
            if (format.Contains("MMMM"))
            {
                format = format.Replace("MMMM", "MM");
            }
            else if (format.Contains("MMM"))
            {
                format = format.Replace("MMM", "M");
            }
            else if (format.Contains("MM"))
            {
                format = format.Replace("MM", "mm");
            }
            else
            {
                format = format.Replace("M", "m");
            }

            return format;
        }

        /// <summary>
        /// Converts a .NET date format string into a MomentJs format string.
        /// </summary>
        /// <param name="format">The .NET date format string.</param>
        /// <returns>Returns a MomentJs date format string.</returns>
        public static string ConvertNetFormatToMomentJs(this string format)
        {
            /*
             *  .NET    MomentJS        Output
             *  ---------------------------------
             *  d       D               1 ... 31
             *  dd      DD              01 ... 31
             *  ddd     ddd             Thu
             *  dddd    dddd            Thursday
             *  M       M               1 ... 12
             *  MM      MM              01 ... 11
             *  MMM     MMM             Nov
             *  MMMM    MMMM            November
             *  yy      YY              09
             *  yyyy    YYYY            2009
             *  h       h               1 ... 12
             *  hh      hh              01 ... 12
             *  H       H               1 ... 24
             *  HH      HH              01 ... 24
             *  m       m               0 ... 59
             *  mm      mm              00 ... 59
             */

            // Day
            format = format.Replace("dd", "DD");
            format = format.Replace("d", "D");


            // Years
            format = format.Replace("yyyy", "YYYY");
            format = format.Replace("yy", "YY");

            return format;
        }

        /// <summary>
        /// Converts a MomentJs date format string into a .NET date format string.
        /// </summary>
        /// <param name="format">The MomentJs date format string.</param>
        /// <returns>Returns a .NET date format string.</returns>
        public static string ConvertMomentJsFormatToNet(this string format)
        {
            /*
             *  .NET    MomentJS        Output
             *  ----------------------------------
             *  d       D               1 ... 31
             *  dd      DD              01 ... 31
             *  ddd     ddd             Thu
             *  dddd    dddd            Thursday
             *  M       M               1 ... 12
             *  MM      MM              01 ... 11
             *  MMM     MMM             Nov
             *  MMMM    MMMM            November
             *  yy      YY              09
             *  yyyy    YYYY            2009
             *  h       h               1 ... 12
             *  hh      hh              01 ... 12
             *  H       H               1 ... 24
             *  HH      HH              01 ... 24
             *  m       m               0 ... 59
             *  mm      mm              00 ... 59
             */

            // Day
            format = format.Replace("DD", "dd");
            format = format.Replace("D", "d");

            // Year
            format = format.Replace("YYYY", "yyyy");
            format = format.Replace("YY", "yy");

            return format;
        }
    }
}
