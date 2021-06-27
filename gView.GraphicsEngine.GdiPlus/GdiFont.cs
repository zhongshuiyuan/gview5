﻿using gView.GraphicsEngine.Abstraction;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace gView.GraphicsEngine.GdiPlus
{
    class GdiFont : IFont
    {
        private Font _font;

        public GdiFont(string fontFamily, float size, FontStyle fontStyle)
        {
            _font = new Font(fontFamily, size);
        }

        public object EngineElement => _font;

        public void Dispose()
        {
            if (_font != null)
            {
                _font.Dispose();
                _font = null;
            }
        }
    }
}
