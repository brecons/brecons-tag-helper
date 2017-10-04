//-----------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="Bremus Solutions">
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
    using BSolutions.Brecons.Core.Attributes.Enumerations;
    using System;
    using System.Linq;
    using System.Reflection;

    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the enum information.
        /// </summary>
        /// <param name="e">The enum.</param>
        /// <returns>Returns the enum information.</returns>
        /// <exception cref="System.InvalidOperationException">It is not possible to read EnumInfoAttribute from enumeration.</exception>
        public static EnumInfoAttribute GetEnumInfo(this Enum e)
        {
            MemberInfo memberInfo = e.GetType().GetMember(e.ToString()).FirstOrDefault();

            if (memberInfo != null)
            {
                return memberInfo.GetCustomAttribute<EnumInfoAttribute>();
            }

            throw new InvalidOperationException("It is not possible to read EnumInfoAttribute from enumeration.");
        }
    }
}
