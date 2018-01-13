//-----------------------------------------------------------------------
// <copyright file="FormTagHelperExtensions.cs" company="Bremus Solutions">
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

    public static class FormTagHelperExtensions
    {
        /// <summary>
        /// Checks if the bounded property of the tag helper is valid.
        /// </summary>
        /// <param name="tagHelper">The tag helper.</param>
        /// <returns>Returns true if the bounded property is valid,</returns>
        /// <remarks>If no property is bound to the tag helper, the result is true.</remarks>
        public static bool IsValid(this FormTagHelperBase tagHelper)
        {
            if (tagHelper.For != null)
            {
                return tagHelper.ViewContext.ViewData.ModelState.GetFieldValidationState(tagHelper.For.Metadata.PropertyName) == ModelValidationState.Valid;
            }

            return true;
        }

        /// <summary>
        /// Checks if the bounded property of the tag helper is valid on a POST request.
        /// </summary>
        /// <param name="tagHelper">The tag helper.</param>
        /// <returns>Returns true if the bounded property is valid,</returns>
        /// <remarks>If no property is bound to the tag helper or the request is not a POST, the result is true.</remarks>
        public static bool IsPostValid(this FormTagHelperBase tagHelper)
        {
            if (tagHelper.For != null && tagHelper.ViewContext.HttpContext.Request.Method == "POST")
            {
                return tagHelper.ViewContext.ViewData.ModelState.GetFieldValidationState(tagHelper.For.Metadata.PropertyName) == ModelValidationState.Valid;
            }

            return true;
        }
    }
}
