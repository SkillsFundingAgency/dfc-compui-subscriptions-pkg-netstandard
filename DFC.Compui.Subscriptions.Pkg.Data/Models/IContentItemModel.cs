﻿using System;
using System.Collections.Generic;

namespace DFC.Compui.Subscriptions.Pkg.Data.Models
{
    public interface IContentItemModel
    {
        Guid? ItemId { get; set; }
        IContentLinks? ContentLinks { get; set; }
        public IList<IContentItemModel> ContentItems { get; set; }

    }
}
