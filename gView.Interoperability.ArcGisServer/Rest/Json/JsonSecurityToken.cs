﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gView.Interoperability.ArcGisServer.Rest.Json
{
    public class JsonSecurityToken
    {
        public string token { get; set; }
        public long expires { get; set; }
    }
}
