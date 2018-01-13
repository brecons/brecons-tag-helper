//-----------------------------------------------------------------------
// <copyright file="BreconsTagHelperBase.cs" company="Bremus Solutions">
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
    using Extensions;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Base class for each Brecons tag helper.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public abstract class BreconsTagHelperBase : TagHelper
    {
        #region --- Attribute Names ---

        protected const string AttributePrefix = "bc-";
        private const string DisableBreconsAttributeName = "disable-brecons";

        #endregion

        #region --- Properties ---

        [HtmlAttributeName(DisableBreconsAttributeName)]
        [HtmlAttributeNotBound]
        [HtmlAttributeMinimizable]
        public bool DisableBootstrap { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the control.
        /// </summary>
        [CopyToOutput]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name for the control.
        /// </summary>
        [CopyToOutput]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the control output.
        /// </summary>
        [HtmlAttributeNotBound]
        public TagHelperOutput Output { get; set; }

        /// <summary>
        /// Gets or sets the action context accessor.
        /// </summary>
        [HtmlAttributeNotBound]
        public IActionContextAccessor ActionContextAccessor { get; set; }

        /// <summary>
        /// Gets or sets the view context.
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        #endregion

        public override void Init(TagHelperContext context)
        {
            ContextAttribute.SetContexts(this, context);
            ContextClassAttribute.SetContext(this, context);
            HtmlAttributeMinimizableAttribute.FillMinimizableAttributes(this, context);
            ConvertVirtualUrlAttribute.ConvertUrls(this, this.ActionContextAccessor);
        }

        #region --- Rendering ---

        /// <summary>
        /// Synchronously executes the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" /> with the given <paramref name="context" /> and
        /// <paramref name="output" />.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            this.Output = output;

            if (!this.DisableBootstrap)
            {
                CopyToOutputAttribute.CopyPropertiesToOutput(this, output);
                this.BasicRenderProcess(output);
                this.RenderProcess(context, output);
                this.RemoveMinimizableAttributes(output);
            }
        }

        /// <summary>
        /// Asynchronously executes the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" /> with the given <paramref name="context" /> and
        /// <paramref name="output" />.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that on completion updates the <paramref name="output" />.
        /// </returns>
        /// <remarks>
        /// By default this calls into <see cref="M:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper.Process(Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContext,Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput)" />.
        /// </remarks>
        /// .
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            this.Output = output;

            if (!this.DisableBootstrap)
            {
                CopyToOutputAttribute.CopyPropertiesToOutput(this, output);
                this.BasicRenderProcess(output);
                await RenderProcessAsync(context, output);
                this.RemoveMinimizableAttributes(output);
            }
        }

        /// <summary>
        /// Overrideable method for individual rendering processes.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        protected virtual void RenderProcess(TagHelperContext context, TagHelperOutput output)
        {
        }

        /// <summary>
        /// Overrideable asynchronous method for individual rendering processes. Renders the process.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that on completion updates the <paramref name="output" />.
        /// </returns>
        protected virtual async Task RenderProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            this.RenderProcess(context, output);
        }

        /// <summary>
        /// Rendering basics for each metronic control.
        /// </summary>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        private void BasicRenderProcess(TagHelperOutput output)
        {
            // Render control id attribute
            if (!string.IsNullOrEmpty(this.Id))
            {
                output.Attributes.Add("id", this.Id);
            }
            else
            {
                GenerateIdAttribute generaterIdAttribute = this.GetType().GetTypeInfo().GetCustomAttributes<GenerateIdAttribute>(true).FirstOrDefault();
                if (generaterIdAttribute != null)
                {
                    this.Id = generaterIdAttribute.Id;

                    if(generaterIdAttribute.RenderIdAttribute)
                    {
                        output.Attributes.Add("id", this.Id);
                    }
                    
                }
            }
        }

        private void RemoveMinimizableAttributes(TagHelperOutput output)
        {
            output.Attributes.RemoveAll(
                GetType().GetProperties()
                .Where(pI => pI.GetCustomAttribute<HtmlAttributeMinimizableAttribute>() != null)
                .Select(pI => pI.GetHtmlAttributeName())
                .ToArray());
        }

        #endregion
    }
}
