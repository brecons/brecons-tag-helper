﻿//-----------------------------------------------------------------------
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
        /// <remarks>Handles the tag helper name, required indication and label / help texts.</remarks>
        public static void BindProperty(this FormTagHelperBase tagHelper, TagHelperContext context)
        {
            var modelExpression = tagHelper.GetModelExpression(context);

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
        /// Gets the model expression of a tag helper.
        /// </summary>
        /// <param name="tagHelper">The tag helper.</param>
        /// <param name="context">The tag helper context.</param>
        /// <returns>Returns the model expression of a tag helper.</returns>
        public static ModelExpression GetModelExpression(this FormTagHelperBase tagHelper, TagHelperContext context)
        {
            return context.AllAttributes.FirstOrDefault(a => a.Name == "asp-for")?.Value as ModelExpression;
        }

        /// <summary>
        /// Gets the property value of the to tag helper binded model.
        /// </summary>
        /// <param name="tagHelper">The tag helper.</param>
        /// <param name="context">The tag helper context.</param>
        /// <returns>Returns the property value of the to tag helper binded model.</returns>
        public static object GetModelValue(this FormTagHelperBase tagHelper, TagHelperContext context)
        {
            var modelExpression = GetModelExpression(tagHelper, context);
            return modelExpression?.Model;
        }

        /// <summary>
        /// Handles the name of the control from the binded model.
        /// </summary>
        /// <param name="tagHelper">The tag helper.</param>
        /// <param name="modelExpression">The tag helper model expression.</param>
        public static void HandleName(this FormTagHelperBase tagHelper, ModelExpression modelExpression)
        {
            var name = modelExpression.Name;
            tagHelper.Name = tagHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
        }

        /// <summary>
        /// Handles the required field information of the control getting from the binded model.
        /// </summary>
        /// <param name="tagHelper">The tag helper.</param>
        /// <param name="modelExpression">The tag helper model expression.</param>
        public static void HandleRequired(this FormTagHelperBase tagHelper, ModelExpression modelExpression)
        {
            if (modelExpression.Metadata != null)
            {
                var property = modelExpression.Metadata.ContainerType.GetPublicProperty(modelExpression.Metadata.PropertyName);

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
        public static void HandleInformation(this FormTagHelperBase tagHelper, ModelExpression modelExpression)
        {
            if (modelExpression.Metadata != null)
            {
                // Localized Property Info
                var pi = modelExpression.Metadata.ContainerType.GetPublicProperty(modelExpression.Metadata.PropertyName).GetLocalization();

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
