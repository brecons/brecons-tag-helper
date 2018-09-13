//-----------------------------------------------------------------------
// <copyright file="MandatoryAttribute.cs" company="Bremus Solutions">
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
    using BSolutions.Brecons.Core.Exceptions;
    using BSolutions.Brecons.Core.Extensions;
    using System;
    using System.Linq;

    /// <summary>
    /// Marks a tag helper Attribute as mandatory.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MandatoryAttribute : Attribute
    {
        /// <summary>
        /// Checks all properties of a tag helper.
        /// </summary>
        /// <param name="tagHelper">The tag helper.</param>
        /// <exception cref="MandatoryAttributeException"></exception>
        public static void CheckProperties(object tagHelper)
        {
            foreach (var propertyInfo in tagHelper.GetType().GetProperties().Where(pi => pi.HasCustomAttribute<MandatoryAttribute>()))
            {
                var value = propertyInfo.GetValue(tagHelper);
                string htmlAttributeName = propertyInfo.GetHtmlAttributeName();

                // String Type
                if (propertyInfo.PropertyType == typeof(string) && string.IsNullOrEmpty((string)value))
                {
                    throw new MandatoryAttributeException(htmlAttributeName, tagHelper.GetType());
                }

                // Default Type
                if(value == null)
                {
                    throw new MandatoryAttributeException(htmlAttributeName, tagHelper.GetType());
                }
            }
        }
    }
}
