//-----------------------------------------------------------------------
// <copyright file="TagHelperContentExtensions.cs" company="Bremus Solutions">
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
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class TagHelperContentExtensions
    {
        /// <summary>
        /// Prepends <see cref="value"/> to the current contents of <see cref="content"/>
        /// </summary>
        public static void Prepend(this TagHelperContent content, string value)
        {
            if (content.IsEmptyOrWhiteSpace)
                content.SetContent(value);
            else
                content.SetContent(value + content.GetContent());
        }

        /// <summary>
        /// Prepends <see cref="value"/> to the current contents of <see cref="content"/> without encoding it
        /// </summary>
        public static void PrependHtml(this TagHelperContent content, string value)
        {
            if (content.IsEmptyOrWhiteSpace)
                content.SetHtmlContent(value);
            else
                content.SetHtmlContent(value + content.GetContent());
        }

        /// <summary>
        /// Prepends <see cref="value"/> to the current contents of <see cref="content"/>
        /// </summary>
        public static void Prepend(this TagHelperContent content, IHtmlContent value)
        {
            if (content.IsEmptyOrWhiteSpace)
            {
                content.SetHtmlContent(value);
                content.AppendLine();
            }
            else
            {
                string currentContent = content.GetContent();
                content.SetHtmlContent(value);
                content.AppendHtml(currentContent);
            }
        }

        /// <summary>
        /// Prepends <see cref="output"/> to the current contents of <see cref="content"/>
        /// </summary>
        public static void Prepend(this TagHelperContent content, TagHelperOutput output)
        {
            content.Prepend(output.ToTagHelperContent());
        }

        /// <summary>
        /// Wraps <see cref="builder"/> around <see cref="content"/>. <see cref="TagBuilder.InnerHtml"/> will be ignored.
        /// </summary>
        public static void Wrap(this TagHelperContent content, TagBuilder builder)
        {
            builder.TagRenderMode = TagRenderMode.StartTag;
            Wrap(content, builder, new TagBuilder(builder.TagName) { TagRenderMode = TagRenderMode.EndTag });
        }

        /// <summary>
        /// Wraps <see cref="contentStart"/> and <see cref="contentEnd"/> around <see cref="content"/>
        /// </summary>
        public static void Wrap(TagHelperContent content, IHtmlContent contentStart, IHtmlContent contentEnd)
        {
            content.Prepend(contentStart);
            content.AppendHtml(contentEnd);
        }

        /// <summary>
        /// Wraps <see cref="contentStart"/> and <see cref="contentEnd"/> around <see cref="content"/>
        /// </summary>
        public static void Wrap(TagHelperContent content, string contentStart, string contentEnd)
        {
            content.Prepend(contentStart);
            content.Append(contentEnd);
        }

        /// <summary>
        /// Wraps <see cref="contentStart"/> and <see cref="contentEnd"/> around <see cref="content"/> without encoding it
        /// </summary>
        public static void WrapHtml(TagHelperContent content, string contentStart, string contentEnd)
        {
            content.PrependHtml(contentStart);
            content.AppendHtml(contentEnd);
        }
    }
}
