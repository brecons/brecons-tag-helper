//-----------------------------------------------------------------------
// <copyright file="ContextClassAttribute.cs" company="Bremus Solutions">
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
    using System.Reflection;

    /// <summary>
    /// Marks a tag helper as a context class. This means that the tag helper can included as a context (parent)
    /// in other tag helpers.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class ContextClassAttribute : Attribute
    {
        public ContextClassAttribute(Type type)
        {
            this.Type = type;
        }

        public ContextClassAttribute()
        {
        }

        public ContextClassAttribute(string key)
        {
            this.Key = key;
        }

        /// <summary>
        ///     Custom key which will be used to store the context in context items. If set the context will not be accessible
        ///     without the key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Type which will be used as key in context items
        /// </summary>
        public Type Type { get; set; }

        public static void SetContext(object target, TagHelperContext context)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var targetType = target.GetType();
            var attr = targetType.GetTypeInfo().GetCustomAttribute<ContextClassAttribute>();
            if (attr != null)
            {
                if (string.IsNullOrEmpty(attr.Key))
                {
                    context.SetContextItem(attr.Type ?? targetType, target);
                }
                else
                {
                    context.SetContextItem(attr.Key, target);
                }
            }
        }
    }
}
