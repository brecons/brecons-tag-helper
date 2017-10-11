﻿//-----------------------------------------------------------------------
// <copyright file="TagHelperOutputExtensions.cs" company="Bremus Solutions">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class TagHelperOutputExtensions
    {
        /// <summary>
        /// Adds an css class if not already added
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="cssClass">The CSS class.</param>
        public static void AddCssClass(this TagHelperOutput output, string cssClass)
        {
            AddCssClass(output, new[] { cssClass });
        }

        /// <summary>
        /// Adds css classes if not already existing.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="cssClasses">The CSS classes.</param>
        public static void AddCssClass(this TagHelperOutput output, IEnumerable<string> cssClasses)
        {
            if (output.Attributes.ContainsName("class") && output.Attributes["class"] != null)
            {
                List<string> classes = output.Attributes["class"].Value.ToString().Split(' ').ToList();
                foreach (string cssClass in cssClasses.Where(cssClass => !classes.Contains(cssClass)))
                    classes.Add(cssClass);
                output.Attributes.SetAttribute("class", classes.Aggregate((s, s1) => s + " " + s1));
            }
            else if (output.Attributes.ContainsName("class"))
                output.Attributes.SetAttribute("class", cssClasses.Aggregate((s, s1) => s + " " + s1));
            else
                output.Attributes.Add("class", cssClasses.Aggregate((s, s1) => s + " " + s1));
        }

        /// <summary>
        /// Adds css classes if already existing.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="cssClass">The CSS class.</param>
        public static void RemoveCssClass(this TagHelperOutput output, string cssClass)
        {
            if (output.Attributes.ContainsName("class"))
            {
                List<string> classes = output.Attributes["class"].Value.ToString().Split(' ').ToList();
                classes.Remove(cssClass);
                if (classes.Count == 0)
                    output.Attributes.RemoveAll("class");
                else
                    output.Attributes.SetAttribute("class", classes.Aggregate((s, s1) => s + " " + s1));
            }
        }

        /// <summary>
        ///     Adds an style entry
        /// </summary>
        public static void AddCssStyle(this TagHelperOutput output, string name, string value)
        {
            if (output.Attributes.ContainsName("style"))
            {
                if (string.IsNullOrEmpty(output.Attributes["style"].Value.ToString()))
                {
                    output.Attributes.SetAttribute("style", name + ": " + value + ";");
                }
                else
                {
                    output.Attributes.SetAttribute("style", (output.Attributes["style"].Value.ToString().EndsWith(";")
                                                             ? " "
                                                             : "; ") + name + ": " + value + ";");
                }
            }
            else
            {
                output.Attributes.Add("style", name + ": " + value + ";");
            }
        }

        /// <summary>
        /// Loads the child content asynchronous.
        /// </summary>
        /// <param name="output">The output.</param>
        public static async Task LoadChildContentAsync(this TagHelperOutput output)
        {
            output.Content.SetHtmlContent(await output.GetChildContentAsync() ?? new DefaultTagHelperContent());
        }

        /// <summary>
        /// Converts a <see cref="output" /> into a <see cref="TagHelperContent" />
        /// </summary>
        public static TagHelperContent ToTagHelperContent(this TagHelperOutput output)
        {
            var content = new DefaultTagHelperContent();
            content.AppendHtml(output.PreElement);
            var builder = new TagBuilder(output.TagName);
            foreach (TagHelperAttribute attribute in output.Attributes)
            {
                builder.Attributes.Add(attribute.Name, attribute.Value?.ToString());
            }

            if (output.TagMode == TagMode.SelfClosing)
            {
                builder.TagRenderMode = TagRenderMode.SelfClosing;
                content.AppendHtml(builder);
            }
            else
            {
                builder.TagRenderMode = TagRenderMode.StartTag;
                content.AppendHtml(builder);
                content.AppendHtml(output.PreContent);
                content.AppendHtml(output.Content);
                content.AppendHtml(output.PostContent);

                if (output.TagMode == TagMode.StartTagAndEndTag)
                {
                    content.AppendHtml($"</{output.TagName}>");
                }
            }
            content.AppendHtml(output.PostElement);
            return content;
        }

        /// <summary>
        /// Adds an aria attribute
        /// </summary>
        /// <param name="name">Name of the attribute. "aria-" is prepended.</param>
        /// <param name="value"></param>
        public static void AddAriaAttribute(this TagHelperOutput output, string name, object value)
        {
            output.MergeAttribute("aria-" + name, value);
        }

        /// <summary>
        /// Adds an aria attribute
        /// </summary>
        /// <param name="name">Name of the attribute. "aria-" is prepended.</param>
        /// <param name="value"></param>
        public static void AddDataAttribute(this TagHelperOutput output, string name, object value)
        {
            output.MergeAttribute("data-" + name, value);
        }

        /// <summary>
        /// Adds an attribute to the attributes collection. Existing attributes are overwritten.
        /// </summary>
        public static void MergeAttribute(this TagHelperOutput output, string key, object value)
        {
            output.Attributes.SetAttribute(key, value);
        }

        /// <summary>
        ///     Wraps a <see cref="builder" /> around the content of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreContent" /> and <see cref="TagHelperOutput.PostContent" />. All content that is
        ///     inside the <see cref="output" /> will be inside of the <see cref="builder" />.
        ///     <see cref="TagBuilder.InnerHtml" /> will not be included.
        /// </summary>
        public static void WrapContentOutside(this TagHelperOutput output, TagBuilder builder)
        {
            builder.TagRenderMode = TagRenderMode.StartTag;
            WrapContentOutside(output, builder, new TagBuilder(builder.TagName) { TagRenderMode = TagRenderMode.EndTag });
        }

        /// <summary>
        ///     Wraps <see cref="startTag" /> and <see cref="endTag" /> around the content of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreContent" /> and <see cref="TagHelperOutput.PostContent" />. All content that is
        ///     inside the <see cref="output" /> will be inside of the <see cref="Microsoft.AspNetCore.Html.IHtmlContent" />s.
        /// </summary>
        public static void WrapContentOutside(this TagHelperOutput output, IHtmlContent startTag, IHtmlContent endTag)
        {
            output.PreContent.Prepend(startTag);
            output.PostContent.AppendHtml(endTag);
        }

        /// <summary>
        ///     Wraps <see cref="startTag" /> and <see cref="endTag" /> around the content of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreContent" /> and <see cref="TagHelperOutput.PostContent" />. All content that is
        ///     inside the <see cref="output" /> will be inside of the <see cref="string" />s.
        /// </summary>
        public static void WrapContentOutside(this TagHelperOutput output, string startTag, string endTag)
        {
            output.PreContent.Prepend(startTag);
            output.PostContent.Append(endTag);
        }

        /// <summary>
        ///     Wraps <see cref="startTag" /> and <see cref="endTag" /> around the content of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreContent" /> and <see cref="TagHelperOutput.PostContent" />. All content that is
        ///     inside the <see cref="output" /> will be inside of the <see cref="string" />s. <see cref="startTag" /> and
        ///     <see cref="endTag" /> will not be encoded.
        /// </summary>
        public static void WrapHtmlContentOutside(this TagHelperOutput output, string startTag, string endTag)
        {
            output.PreContent.PrependHtml(startTag);
            output.PostContent.AppendHtml(endTag);
        }

        /// <summary>
        ///     Wraps a <see cref="builder" /> around the content of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreContent" /> and <see cref="TagHelperOutput.PostContent" />. The current contents of
        ///     <see cref="TagHelperOutput.PreContent" /> and <see cref="TagHelperOutput.PostContent" /> will be outside.
        /// </summary>
        public static void WrapContentInside(this TagHelperOutput output, TagBuilder builder)
        {
            builder.TagRenderMode = TagRenderMode.StartTag;
            WrapContentInside(output, builder, new TagBuilder(builder.TagName) { TagRenderMode = TagRenderMode.EndTag });
        }

        /// <summary>
        ///     Wraps <see cref="startTag" /> and <see cref="endTag" /> around the content of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreContent" /> and <see cref="TagHelperOutput.PostContent" />.
        ///     <see cref="TagBuilder.InnerHtml" /> will not be included. The current contents of
        ///     <see cref="TagHelperOutput.PreContent" /> and <see cref="TagHelperOutput.PostContent" /> will be outside.
        /// </summary>
        public static void WrapContentInside(this TagHelperOutput output, IHtmlContent startTag, IHtmlContent endTag)
        {
            output.PreContent.AppendHtml(startTag);
            output.PostContent.Prepend(endTag);
        }

        /// <summary>
        ///     Wraps <see cref="startTag" /> and <see cref="endTag" /> around the content of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreContent" /> and <see cref="TagHelperOutput.PostContent" />.
        ///     <see cref="TagBuilder.InnerHtml" /> will not be included. The current contents of
        ///     <see cref="TagHelperOutput.PreContent" /> and <see cref="TagHelperOutput.PostContent" /> will be outside.
        /// </summary>
        public static void WrapContentInside(this TagHelperOutput output, string startTag, string endTag)
        {
            output.PreContent.Append(startTag);
            output.PostContent.Prepend(endTag);
        }

        /// <summary>
        ///     Wraps <see cref="startTag" /> and <see cref="endTag" /> around the content of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreContent" /> and <see cref="TagHelperOutput.PostContent" />.
        ///     <see cref="TagBuilder.InnerHtml" /> will not be included. The current contents of
        ///     <see cref="TagHelperOutput.PreContent" /> and <see cref="TagHelperOutput.PostContent" /> will be outside.
        ///     <see cref="startTag" /> and <see cref="endTag" /> will not be encoded.
        /// </summary>
        public static void WrapHtmlContentInside(this TagHelperOutput output, string startTag, string endTag)
        {
            output.PreContent.AppendHtml(startTag);
            output.PostContent.PrependHtml(endTag);
        }

        /// <summary>
        ///     Wraps a <see cref="builder" /> around the element of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" />. The current contents of
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" /> will be inside.
        ///     <see cref="TagBuilder.InnerHtml" /> will not be included.
        /// </summary>
        public static void WrapOutside(this TagHelperOutput output, TagBuilder builder)
        {
            builder.TagRenderMode = TagRenderMode.StartTag;
            WrapOutside(output, builder, new TagBuilder(builder.TagName) { TagRenderMode = TagRenderMode.EndTag });
        }

        /// <summary>
        ///     Wraps <see cref="startTag" /> and <see cref="endTag" /> around the element of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" />. The current contents of
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" /> will be inside.
        /// </summary>
        public static void WrapOutside(this TagHelperOutput output, IHtmlContent startTag, IHtmlContent endTag)
        {
            output.PreElement.Prepend(startTag);
            output.PostElement.AppendHtml(endTag);
        }

        /// <summary>
        ///     Wraps <see cref="startTag" /> and <see cref="endTag" /> around the element of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" />. The current contents of
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" /> will be inside.
        /// </summary>
        public static void WrapOutside(this TagHelperOutput output, string startTag, string endTag)
        {
            output.PreElement.Prepend(startTag);
            output.PostElement.Append(endTag);
        }

        /// <summary>
        ///     Wraps <see cref="startTag" /> and <see cref="endTag" /> around the element of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" />. The current contents of
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" /> will be inside.
        ///     <see cref="startTag" /> and <see cref="endTag" /> will not be encoded.
        /// </summary>
        public static void WrapHtmlOutside(this TagHelperOutput output, string startTag, string endTag)
        {
            output.PreElement.PrependHtml(startTag);
            output.PostElement.AppendHtml(endTag);
        }

        /// <summary>
        ///     Wraps a <see cref="builder" /> around the element of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" />. The current contents of
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" /> will be outside.
        ///     <see cref="TagBuilder.InnerHtml" /> will not be included.
        /// </summary>
        public static void WrapInside(this TagHelperOutput output, TagBuilder builder)
        {
            builder.TagRenderMode = TagRenderMode.StartTag;
            WrapInside(output, builder, new TagBuilder(builder.TagName) { TagRenderMode = TagRenderMode.EndTag });
        }

        /// <summary>
        ///     Wraps <see cref="startTag" /> and <see cref="endTag" /> around the element of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" />. The current contents of
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" /> will be Outside.
        /// </summary>
        public static void WrapInside(this TagHelperOutput output, IHtmlContent startTag, IHtmlContent endTag)
        {
            output.PreElement.AppendHtml(startTag);
            output.PostElement.Prepend(endTag);
        }

        /// <summary>
        ///     Wraps <see cref="startTag" /> and <see cref="endTag" /> around the element of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" />. The current contents of
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" /> will be outside.
        /// </summary>
        public static void WrapInside(this TagHelperOutput output, string startTag, string endTag)
        {
            output.PreElement.Append(startTag);
            output.PostElement.Prepend(endTag);
        }

        /// <summary>
        ///     Wraps <see cref="startTag" /> and <see cref="endTag" /> around the element of the <see cref="output" /> using
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" />. The current contents of
        ///     <see cref="TagHelperOutput.PreElement" /> and <see cref="TagHelperOutput.PostElement" /> will be outside.
        ///     <see cref="startTag" /> and <see cref="endTag" /> will not be encoded.
        /// </summary>
        public static void WrapHtmlInside(this TagHelperOutput output, string startTag, string endTag)
        {
            output.PreElement.AppendHtml(startTag);
            output.PostElement.PrependHtml(endTag);
        }
    }
}
