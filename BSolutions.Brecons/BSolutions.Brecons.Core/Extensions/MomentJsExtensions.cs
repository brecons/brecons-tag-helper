//-----------------------------------------------------------------------
// <copyright file="MomentJsExtensions.cs" company="Bremus Solutions">
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
    public static class MomentJsExtensions
    {
        /// <summary>
        /// Converts the DateTime format of C# to the format of MomentJs.
        /// </summary>
        /// <param name="format">The C# DateTime format.</param>
        /// <returns>Returns the MomentJs date time format.</returns>
        public static string ConvertFormatToMomentJs(this string format)
        {
            /*
             *  .NET    MomentJS        Output
             *  -----------------------------------
             *  d       D                1 ... 31
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

            string currentFormat = format;

            // Days
            currentFormat = currentFormat.Replace("d", "D");
            currentFormat = currentFormat.Replace("dd", "DD");

            // Years
            currentFormat = currentFormat.Replace("yy", "YY");
            currentFormat = currentFormat.Replace("yyyy", "YYYY");

            return currentFormat;
        }

        /// <summary>
        /// Converts the date time format of MomentJs to the DateTime format of C#.
        /// </summary>
        /// <param name="format">The C# MomentJs date time format.</param>
        /// <returns>Returns the C# DateTime format.</returns>
        public static string ConvertFormatToCSharp(this string format)
        {
            /*
             *  .NET    MomentJS        Output
             *  -----------------------------------
             *  d       D                1 ... 31
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

            string currentFormat = format;

            // Days
            currentFormat = currentFormat.Replace("D", "d");
            currentFormat = currentFormat.Replace("DD", "dd");

            // Years
            currentFormat = currentFormat.Replace("YY", "yy");
            currentFormat = currentFormat.Replace("YYYY", "yyyy");

            return currentFormat;
        }
    }
}
