// -----------------------------------------------------------------------
// <copyright file="ExpiryDateInTenYearsAttribute.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Storefront.Models.Validators
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// The Credit Card Expiry date range validation attribute.
    /// </summary>    
    [AttributeUsage(AttributeTargets.Property)]
    public class ExpiryDateInTenYearsAttribute : RangeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpiryDateInTenYearsAttribute" /> class.
        /// </summary>
        public ExpiryDateInTenYearsAttribute() : base(DateTime.Now.Year, DateTime.Now.Year + 10)
        {            
        }
    }
}