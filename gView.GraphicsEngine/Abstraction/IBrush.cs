﻿using System;
using System.Collections.Generic;
using System.Text;

namespace gView.GraphicsEngine.Abstraction
{
    public interface IBrush : IDisposable
    {
        object EngineElement { get; }
    }
}
