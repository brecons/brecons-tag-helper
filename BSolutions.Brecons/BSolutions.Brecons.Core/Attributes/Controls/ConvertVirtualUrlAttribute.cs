//-----------------------------------------------------------------------
// <copyright file="ConvertVirtualUrlAttribute.cs" company="Bremus Solutions">
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
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Indicates that a property might contain a virtual Url that has to be converted into a absolute path
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConvertVirtualUrlAttribute : Attribute
    {
        public static void ConvertUrls(object target, IActionContextAccessor accessor)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var decoratedProperties = target.GetType().GetProperties().Where(pI => pI.HasCustomAttribute<ConvertVirtualUrlAttribute>()).ToList();

            if (!decoratedProperties.Any())
                return;

            if (accessor == null)
                throw new ArgumentNullException(nameof(accessor));

            var urlHelper = new UrlHelper(accessor.ActionContext);

            foreach (var property in decoratedProperties)
            {
                if (property.PropertyType != typeof(string))
                    throw new Exception("Decorated property must be a string");
                property.SetValue(target, urlHelper.Content((string)property.GetValue(target)));
            }
        }
    }
}
