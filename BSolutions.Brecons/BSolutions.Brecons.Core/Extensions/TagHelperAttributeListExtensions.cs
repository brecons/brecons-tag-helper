//-----------------------------------------------------------------------
// <copyright file="TagHelperAttributeListExtensions.cs" company="Bremus Solutions">
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
    using System.Linq;

    public static class TagHelperAttributeListExtensions
    {
        public static bool RemoveAll(this TagHelperAttributeList attributeList, params string[] attributeNames)
        {
            return attributeNames.Aggregate(false, (current, name) => attributeList.RemoveAll(name) || current);
        }

        public static void AddAriaAttribute(this TagHelperAttributeList attributeList, string attributeName, object value)
        {
            attributeList.Add("aria-" + attributeName, value);
        }

        public static void AddDataAttribute(this TagHelperAttributeList attributeList, string attributeName, object value)
        {
            attributeList.Add("data-" + attributeName, value);
        }
    }
}
