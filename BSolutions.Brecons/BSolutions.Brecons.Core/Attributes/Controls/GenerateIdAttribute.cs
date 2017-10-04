//-----------------------------------------------------------------------
// <copyright file="GenerateIdAttribute.cs" company="Bremus Solutions">
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
    using System;

    /// <summary>
    /// Can generate a random Id (Random Guid) if the decoraded string property is null or empty
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateIdAttribute : Attribute
    {
        private string _id;
        public string Prefix { get; set; }

        public bool RenderIdAttribute { get; set; }

        public string Id => this._id ?? (this._id = this.Prefix + Guid.NewGuid().ToString("N"));

        public GenerateIdAttribute(string prefix, bool renderIdAttribute = true)
        {
            this.Prefix = prefix;
            this.RenderIdAttribute = renderIdAttribute;
        }
    }
}
