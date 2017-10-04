//-----------------------------------------------------------------------
// <copyright file="TagHelperExtensions.cs" company="Bremus Solutions">
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
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public static class TagHelperExtensions
    {
        public static async Task RunTagHelperAsync(this ITagHelper tagHelper, Options options)
        {
            if (options.Context == null)
            {
                options.Context = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), Guid.NewGuid().ToString("N"));
            }

            if (options.Attributes == null)
            {
                options.Attributes = new List<TagHelperAttribute>();
            }

            if (options.TagName == null)
            {
                options.TagName = tagHelper.GetTagName();
            }

            var output = new TagHelperOutput(options.TagName, new TagHelperAttributeList(options.Attributes), (b, e) => Task.Factory.StartNew(() => options.Content)) { TagMode = options.TagMode };
            if (options.InitTagHelper)
            {
                tagHelper.Init(options.Context);
            }

            await tagHelper.ProcessAsync(options.Context, output);
        }

        public static async Task<TagHelperContent> ToTagHelperContentAsync(this IEnumerable<ITagHelper> tagHelpers, Options options)
        {
            var tagHelperList = tagHelpers as List<ITagHelper> ?? tagHelpers.ToList();
            if (options.Context == null)
                options.Context = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), Guid.NewGuid().ToString("N"));
            if (options.Attributes == null)
                options.Attributes = new List<TagHelperAttribute>();
            if (options.TagName == null)
                options.TagName = tagHelpers.GetTagName();
            var output = new TagHelperOutput(options.TagName, new TagHelperAttributeList(options.Attributes), (b, e) => Task.Factory.StartNew(() => options.Content)) { TagMode = options.TagMode };
            if (options.InitTagHelper)
                tagHelperList.ForEach(tH => tH.Init(options.Context));
            foreach (var tagHelper in tagHelperList)
            {
                await tagHelper.ProcessAsync(options.Context, output);
            }
            if (options.Content != null && !output.IsContentModified)
                output.Content.SetHtmlContent(options.Content);
            return output.ToTagHelperContent();
        }

        public static string GetTagName(this ITagHelper tagHelper)
        {
            return tagHelper.GetType().GetTypeInfo().GetCustomAttributes<HtmlTargetElementAttribute>().FirstOrDefault(a => a.Tag != "*")?.Tag
                   ?? Regex.Replace(tagHelper.GetType().Name.Replace("TagHelper", ""), "([A-Z])", "-$1").Trim('-').ToLower();
        }

        public static string GetTagName(this IEnumerable<ITagHelper> tagHelpers)
        {
            var tagHelperList = tagHelpers as IList<ITagHelper> ?? tagHelpers.ToList();
            return
                tagHelperList.OrderBy(tH => tH.Order)
                             .FirstOrDefault(tH => tH.GetType().GetTypeInfo().HasCustomAttribute<HtmlTargetElementAttribute>())?
                             .GetType().GetTypeInfo()
                             .GetCustomAttributes<HtmlTargetElementAttribute>()
                             .FirstOrDefault(a => a.Tag != "*")?.Tag
                ?? Regex.Replace(tagHelperList.Min(tH => tH.Order).GetType().Name.Replace("TagHelper", ""), "([A-Z])", "-$1").Trim('-').ToLower();
        }

        public class Options
        {
            public string TagName { get; set; }

            public TagMode TagMode { get; set; }

            public TagHelperContent Content { get; set; }

            public TagHelperContext Context { get; set; }

            public IEnumerable<TagHelperAttribute> Attributes { get; set; }
            public bool InitTagHelper { get; set; } = true;
        }
    }
}
