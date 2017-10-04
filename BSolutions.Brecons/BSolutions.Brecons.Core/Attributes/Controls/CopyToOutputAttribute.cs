//-----------------------------------------------------------------------
// <copyright file="CopyToOutputAttribute.cs" company="Bremus Solutions">
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
namespace BSolutions.Brecons.Core.Attributes.Controls
{
    using BSolutions.Brecons.Core.Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Properties marked with this Attribute will be automatically copied to the output of the tag helper.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class CopyToOutputAttribute : Attribute
    {
        public CopyToOutputAttribute()
        {
        }

        public CopyToOutputAttribute(bool copyIfValueIsNull)
        {
            this.CopyIfValueIsNull = copyIfValueIsNull;
        }

        public CopyToOutputAttribute(string outputAttributeName)
        {
            this.OutputAttributeName = outputAttributeName;
        }

        public CopyToOutputAttribute(bool copyIfValueIsNull, string outputAttributeName)
        {
            this.OutputAttributeName = outputAttributeName;
            this.CopyIfValueIsNull = copyIfValueIsNull;
        }

        public CopyToOutputAttribute(string prefix, string suffix)
        {
            this.Prefix = prefix;
            this.Suffix = suffix;
        }

        public CopyToOutputAttribute(bool copyIfValueIsNull, string prefix, string suffix)
        {
            this.Prefix = prefix;
            this.Suffix = suffix;
            this.CopyIfValueIsNull = copyIfValueIsNull;
        }

        public CopyToOutputAttribute(bool copyIfValueIsNull, string outputAttributeName, string prefix, string suffix)
        {
            this.Prefix = prefix;
            this.Suffix = suffix;
            this.OutputAttributeName = outputAttributeName;
            this.CopyIfValueIsNull = copyIfValueIsNull;
        }

        public CopyToOutputAttribute(string outputAttributeName, string prefix, string suffix)
        {
            this.Prefix = prefix;
            this.Suffix = suffix;
            this.OutputAttributeName = outputAttributeName;
        }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public string OutputAttributeName { get; set; }

        public bool CopyIfValueIsNull { get; set; }

        public static void CopyPropertiesToOutput(object target, TagHelperOutput output)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            foreach (var propertyInfo in target.GetType().GetProperties().Where(pI => pI.HasCustomAttribute<CopyToOutputAttribute>()))
            {
                var value = propertyInfo.GetValue(target);
                if (propertyInfo.PropertyType.IsAssignableFrom(typeof(bool)))
                    value = value?.ToString().ToLower();
                foreach (var attr in propertyInfo.GetCustomAttributes<CopyToOutputAttribute>())
                {
                    if (value != null || attr.CopyIfValueIsNull)
                        output.Attributes.Add(attr.Prefix + (attr.OutputAttributeName ?? propertyInfo.GetHtmlAttributeName()) + attr.Suffix, value);
                }
            }
        }
    }
}
