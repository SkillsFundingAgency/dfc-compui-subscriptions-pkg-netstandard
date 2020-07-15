﻿using Newtonsoft.Json;
using System;

namespace DFC.Compui.Subscriptions.Pkg.Data.Models
{
    public class LinkDetails
    {
        [JsonIgnore]
        public Uri? Uri { get; set; }

        public string? Href { get; set; }

        public string? Title { get; set; }

        public string? ContentType { get; set; }

        public string? Alignment { get; set; }

        public int Ordinal { get; set; }

        public int Size { get; set; }
    }
}
