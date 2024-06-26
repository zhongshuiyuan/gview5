﻿using System;

namespace gView.Framework.OGC.Exceptions
{
    public class ParseParametersException : Exception
    {
        public ParseParametersException(byte[] data, string contentType)
        {
            MessageData = data;
            ContentType = contentType;
        }

        public byte[] MessageData { get; }
        public string ContentType { get; }
    }
}
