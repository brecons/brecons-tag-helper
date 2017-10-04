//-----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Bremus Solutions">
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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods for string operations.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the illegal characters for variable declaration.
        /// </summary>
        /// <value>
        /// The illegal characters.
        /// </value>
        public static List<char> IllegalCharacters { get; private set; } = new List<char>
        {
            '.', '-', ' ', '@', '+'
        };

        /// <summary>
        /// Set the first letter of a string to upper.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns></returns>
        public static string FirstLetterToUpper(this string value)
        {
            if (value == null) return string.Empty;

            if (value.Length > 1) return char.ToUpper(value[0]) + value.Substring(1);

            return value.ToUpper();
        }

        /// <summary>
        /// Transforms a string to a valid class name.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns></returns>
        public static string TransformClassname(this string value)
        {
            if (value == null) return string.Empty;

            value = IllegalCharacters.Aggregate(value, (current, c) => current.Replace(c, '_'));

            value = string.Join(string.Empty, value.Split('_').Select(s => s.FirstLetterToUpper()));

            int output = 0;
            if (int.TryParse(value.Substring(0, 1), out output))
            {
                value = "_" + value;
            }

            return value;
        }

        /// <summary>
        /// Merge to collections to a dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="keys">The keys.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
        {
            var dic = new Dictionary<TKey, TValue>();

            keys.Each<TKey>((x, i) =>
            {
                dic.Add(x, values.ElementAt(i));
            });

            return dic;
        }

        /// <summary>
        /// Proceed an action for a enumerable collection.
        /// </summary>
        /// <typeparam name="T">The type of the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="a">The action.</param>
        public static void Each<T>(this IEnumerable collection, Action<T, int> a)
        {
            int i = 0;
            foreach (T e in collection)
            {
                a(e, i++);
            }
        }
    }
}
