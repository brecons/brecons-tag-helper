//-----------------------------------------------------------------------
// <copyright file="MemberInfoExtensions.cs" company="Bremus Solutions">
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
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public static class MemberInfoExtensions
    {
        public static bool HasCustomAttribute<T>(this MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttributes(typeof(T), true).Any();
        }

        public static bool HasCustomAttribute(this MemberInfo memberInfo, Type attributeType)
        {
            return memberInfo.GetCustomAttributes(attributeType, true).Any();
        }

        public static string GetHtmlAttributeName(this MemberInfo property)
        {
            var htmlAttributeNameAttribute = property.GetCustomAttribute<HtmlAttributeNameAttribute>();
            if (htmlAttributeNameAttribute != null)
                return htmlAttributeNameAttribute.DictionaryAttributePrefix + htmlAttributeNameAttribute.Name;
            return Regex.Replace(property.Name, "([A-Z])", "-$1").ToLower().Trim('-');
        }
    }
}
