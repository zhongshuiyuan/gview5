﻿using System.Collections.Generic;
using gView.Framework.system;
using gView.Framework.UI;

namespace gView.Framework.GeoProcessing
{
    public interface IActivity : IErrorMessage ,IProgressReporter
    {
        List<IActivityData> Sources { get; }
        List<IActivityData> Targets { get; }
        string DisplayName { get; }
        string CategoryName { get; }

        List<IActivityData> Process();
    }
}
