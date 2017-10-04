//-----------------------------------------------------------------------
// <copyright file="AttributeExtensions.cs" company="Bremus Solutions">
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
    using Attributes;
    using BSolutions.Brecons.Core.Models;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    public static class AttributeExtensions
    {
        public static LocalizedPropertyInfo GetLocalization(this PropertyInfo property)
        {
            if (property != null)
            {
                // MetronicDisplayAttribute
                if (property.IsDefined(typeof(BreconsDisplayAttribute)))
                {
                    BreconsDisplayAttribute attribute = property.GetCustomAttribute<BreconsDisplayAttribute>();
                    return new LocalizedPropertyInfo { DisplayName = attribute.DisplayName, Description = attribute.Description };
                }

                // DisplayAttribute
                if (property.IsDefined(typeof(DisplayAttribute)))
                {
                    DisplayAttribute attribute = property.GetCustomAttribute<DisplayAttribute>();
                    return new LocalizedPropertyInfo { DisplayName = attribute.Name, Description = attribute.Description };
                }

                // DisplayNameAttribute
                if (property.IsDefined(typeof(DisplayNameAttribute)))
                {
                    DisplayNameAttribute attribute = property.GetCustomAttribute<DisplayNameAttribute>();
                    return new LocalizedPropertyInfo { DisplayName = attribute.DisplayName };
                }
            }

            return null;
        }
    }
}
