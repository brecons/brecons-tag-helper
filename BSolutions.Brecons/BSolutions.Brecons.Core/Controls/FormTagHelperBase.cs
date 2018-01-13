//-----------------------------------------------------------------------
// <copyright file="FormTagHelperBase.cs" company="Bremus Solutions">
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
namespace BSolutions.Brecons.Core.Controls
{
    using BSolutions.Brecons.Core.Attributes.Controls;
    using BSolutions.Brecons.Core.Enumerations;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    /// <summary>
    /// Base class for each Brecons form control tag helper.
    /// </summary>
    /// <seealso cref="BSolutions.Brecons.TagHelper.Controls.BreconsTagHelperBase" />
    public abstract class FormTagHelperBase : BreconsTagHelperBase
    {
        #region --- Attribute Names ---

        private const string ValidationAttributeName = AttributePrefix + "validation";
        private const string AspForAttributeName = "asp-for";
        private const string HelpAttributeName = AttributePrefix + "help";
        private const string LabelAttributeName = AttributePrefix + "label";
        private const string SizeAttributeName = AttributePrefix + "size";
        private const string RequiredAttributeName = AttributePrefix + "required";

        #endregion

        #region --- Properties ---

        /// <summary>
        /// Gets or sets the form wide validation handling
        /// </summary>
        [HtmlAttributeName(ValidationAttributeName)]
        public bool Validation { get; set; }

        /// <summary>
        /// Gets or sets the model binding.
        /// </summary>
        [HtmlAttributeName(AspForAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Gets or sets the help.
        /// </summary>
        /// <value>
        /// The help.
        /// </value>
        [HtmlAttributeName(HelpAttributeName)]
        public string Help { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        [HtmlAttributeName(LabelAttributeName)]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        [HtmlAttributeName(SizeAttributeName)]
        public Size Size { get; set; } = Size.Default;

        /// <summary>
        /// Gets or sets a value indicating whether this control value is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        [HtmlAttributeName(RequiredAttributeName)]
        public bool IsRequired { get; set; }

        #endregion
    }
}
