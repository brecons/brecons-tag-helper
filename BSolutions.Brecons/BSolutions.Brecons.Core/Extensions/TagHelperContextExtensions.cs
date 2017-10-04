//-----------------------------------------------------------------------
// <copyright file="TagHelperContextExtensions.cs" company="Bremus Solutions">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extension methods for the tag helper context.
    /// </summary>
    public static class TagHelperContextExtensions
    {

        /// <summary>
        /// Determines whether the tag helper context [has context item] of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the context item.</typeparam>
        /// <param name="context">The tag helper context.</param>
        /// <returns>
        /// <c>true</c> if [has context item] [the specified context]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasContextItem<T>(this TagHelperContext context)
        {
            return HasContextItem<T>(context, true);
        }

        /// <summary>
        /// Determines whether the tag helper context [has context item] of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the context item.</typeparam>
        /// <param name="context">The tag helper context.</param>
        /// <param name="useInherited">if set to <c>true</c> [use inherited].</param>
        /// <returns>
        /// <c>true</c> if [has context item] [the specified context]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasContextItem<T>(this TagHelperContext context, bool useInherited)
        {
            return context.HasContextItem(typeof(T), useInherited);
        }

        public static bool HasContextItem(this TagHelperContext context, Type type)
        {
            return HasContextItem(context, type, true);
        }

        public static bool HasContextItem(this TagHelperContext context, Type type, bool useInherited)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            var contextItem = GetContextItem(context, type, useInherited);
            return contextItem != null && type.IsInstanceOfType(contextItem);
        }

        public static bool HasContextItem<T>(this TagHelperContext context, string key)
        {
            return HasContextItem(context, typeof(T), key);
        }

        public static bool HasContextItem(this TagHelperContext context, Type type, string key)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            return context.Items.ContainsKey(key) && type.IsInstanceOfType(context.Items[key]);
        }

        public static T GetContextItem<T>(this TagHelperContext context) where T : class
        {
            return GetContextItem<T>(context, true);
        }

        public static T GetContextItem<T>(this TagHelperContext context, bool useInherited) where T : class
        {
            return GetContextItem(context, typeof(T), useInherited) as T;
        }

        public static object GetContextItem(this TagHelperContext context, Type type)
        {
            return GetContextItem(context, type, true);
        }

        public static T GetContextItem<T>(this TagHelperContext context, string key) where T : class
        {
            return GetContextItem(context, typeof(T), key) as T;
        }

        public static object GetContextItem(this TagHelperContext context, Type type, string key)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            return context.Items.ContainsKey(key) && type.IsInstanceOfType(context.Items[key]) ? context.Items[key] : null;
        }

        public static object GetContextItem(this TagHelperContext context, Type type, bool useInherit)
        {
            if (context.Items.ContainsKey(type))
                return context.Items.First(kVP => kVP.Key.Equals(type)).Value;
            if (useInherit)
                return context.Items.FirstOrDefault(kVP => kVP.Key is Type && type.IsAssignableFrom((Type)kVP.Key)).Value;
            return null;
        }

        public static void SetContextItem<T>(this TagHelperContext context, T contextItem)
        {
            SetContextItem(context, typeof(T), contextItem);
        }

        public static void SetContextItem(this TagHelperContext context, Type type, object contextItem)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (context.Items.ContainsKey(type))
                context.Items[type] = contextItem;
            else
                context.Items.Add(type, contextItem);
        }

        public static void SetContextItem(this TagHelperContext context, string key, object contextItem)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (context.Items.ContainsKey(key))
                context.Items[key] = contextItem;
            else
                context.Items.Add(key, contextItem);
        }

        public static void RemoveContextItem<T>(this TagHelperContext context)
        {
            RemoveContextItem<T>(context, true);
        }

        public static void RemoveContextItem<T>(this TagHelperContext context, bool useInherited)
        {
            RemoveContextItem(context, typeof(T), useInherited);
        }

        public static void RemoveContextItem(this TagHelperContext context, Type type)
        {
            RemoveContextItem(context, type, true);
        }

        public static void RemoveContextItem(this TagHelperContext context, Type type, bool useInherited)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (context.Items.ContainsKey(type))
                context.Items.Remove(type);
            else if (useInherited)
            {
                var key = context.Items.FirstOrDefault(kVP => kVP.Key is Type && ((Type)kVP.Key).IsAssignableFrom(type));
                if (!key.Equals(default(KeyValuePair<object, object>)))
                    context.Items.Remove(key);
            }
        }

        public static void RemoveContextItem(this TagHelperContext context, string key)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (context.Items.ContainsKey(key))
                context.Items.Remove(key);
        }
    }
}
