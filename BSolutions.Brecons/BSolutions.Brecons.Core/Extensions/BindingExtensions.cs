//-----------------------------------------------------------------------
// <copyright file="BindingExtensions.cs" company="Bremus Solutions">
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
    using BSolutions.Brecons.Core.Controls;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    public static class BindingExtensions
    {
        /// <summary>
        /// Binds the property to the tag helper.
        /// </summary>
        /// <param name="tagHelper">The tag helper.</param>
        public static void BindProperty(this FormTagHelperBase tagHelper, TagHelperContext context)
        {
            var modelExpression = context.AllAttributes.FirstOrDefault(a => a.Name == "asp-for")?.Value as ModelExpression;

            if (modelExpression != null)
            {
                // Name
                tagHelper.HandleName(modelExpression);

                // Required
                tagHelper.HandleRequired(modelExpression);

                // Label and Help
                tagHelper.HandleInformation(modelExpression);
            }
        }

        /// <summary>
        /// Handles the name of the control from the binded model.
        /// </summary>
        /// <param name="tagHelper">The tag helper.</param>
        /// <param name="modelExpression">The tag helper model expression.</param>
        private static void HandleName(this FormTagHelperBase tagHelper, ModelExpression modelExpression)
        {
            var name = modelExpression.Name;
            tagHelper.Name = tagHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
        }

        /// <summary>
        /// Handles the required field information of the control getting from the binded model.
        /// </summary>
        /// <param name="tagHelper">The tag helper.</param>
        /// <param name="modelExpression">The tag helper model expression.</param>
        private static void HandleRequired(this FormTagHelperBase tagHelper, ModelExpression modelExpression)
        {
            if (modelExpression.Metadata != null)
            {
                var property = modelExpression.Metadata.ContainerType.GetProperty(modelExpression.Metadata.PropertyName);

                // RequiredAttribute
                if (property.IsDefined(typeof(RequiredAttribute)))
                {
                    tagHelper.IsRequired = true;
                }
            }
        }

        /// <summary>
        /// Handles the information of the control getting from the binded model (e.g. label und help).
        /// </summary>
        /// <param name="tagHelper">The tag helper.</param>
        /// <param name="modelExpression">The tag helper model expression.</param>
        private static void HandleInformation(this FormTagHelperBase tagHelper, ModelExpression modelExpression)
        {
            if (modelExpression.Metadata != null)
            {
                // Localized Property Info
                var pi = modelExpression.Metadata.ContainerType.GetProperty(modelExpression.Metadata.PropertyName).GetLocalization();

                if (pi != null)
                {
                    if (string.IsNullOrEmpty(tagHelper.Label) && !string.IsNullOrEmpty(pi.DisplayName))
                    {
                        tagHelper.Label = pi.DisplayName;
                    }

                    if (string.IsNullOrEmpty(tagHelper.Help) && !string.IsNullOrEmpty(pi.Description))
                    {
                        tagHelper.Help = pi.Description;
                    }
                }
            }
        }
    }
}
