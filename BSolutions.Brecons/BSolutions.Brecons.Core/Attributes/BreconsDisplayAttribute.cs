//-----------------------------------------------------------------------
// <copyright file="BreconsDisplayAttribute.cs" company="Bremus Solutions">
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
namespace BSolutions.Brecons.Core.Attributes
{
    using System;

    /// <summary>
    /// Specifies the display name and an optinally description for a property.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="BSolutions.Brecons.Core.Attributes.IBreconsDisplayAttribute" />
    /// <remarks>
    /// This attribute is used when an own localization logic is used in an application
    /// and the DisplayAttribute of .NET it is usable.
    /// </remarks>
    public class BreconsDisplayAttribute : Attribute, IBreconsDisplayAttribute
    {
        protected string _displayName;
        protected string _description;

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public virtual string DisplayName { get { return this._displayName; } }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public virtual string Description { get { return this._description; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreconsDisplayAttribute"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        public BreconsDisplayAttribute(string displayName)
            : this(displayName, string.Empty)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreconsDisplayAttribute"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="description">The description.</param>
        public BreconsDisplayAttribute(string displayName, string description)
        {
            this._displayName = displayName;
            this._description = description;
        }
    }
}
