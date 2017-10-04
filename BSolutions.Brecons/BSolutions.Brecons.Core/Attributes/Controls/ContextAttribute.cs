//-----------------------------------------------------------------------
// <copyright file="ContextAttribute.cs" company="Bremus Solutions">
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
    using Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System;
    using System.Linq;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Property)]
    public class ContextAttribute : Attribute
    {
        /// <summary>
        /// If true, context items which type inherits from the actual property type will be used if no context item with
        /// matching type is found
        /// </summary>
        public bool UseInherited { get; set; } = true;

        /// <summary>
        /// If true, context item will be removed once after it is assigned to the decorated property
        /// </summary>
        public bool RemoveContext { get; set; }

        /// <summary>
        /// Loads context using this key instead of default one
        /// </summary>
        public string Key { get; set; }

        public ContextAttribute()
        {
        }

        public ContextAttribute(string key)
        {
            this.Key = key;
        }

        public static void SetContexts(object target, TagHelperContext context)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            foreach (var propertyInfo in target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(pi => pi.HasCustomAttribute<ContextAttribute>()))
            {
                var attr = propertyInfo.GetCustomAttribute<ContextAttribute>();
                if (string.IsNullOrEmpty(attr.Key))
                {
                    var contextItem = context.GetContextItem(propertyInfo.PropertyType, attr.UseInherited);
                    if (contextItem != null)
                        propertyInfo.SetValue(target, contextItem);
                    if (attr.RemoveContext)
                        context.RemoveContextItem(propertyInfo.PropertyType, attr.UseInherited);
                }
                else
                {
                    propertyInfo.SetValue(target, context.GetContextItem(propertyInfo.PropertyType, attr.Key));
                    if (attr.RemoveContext)
                        context.RemoveContextItem(attr.Key);
                }
            }
        }
    }
}
