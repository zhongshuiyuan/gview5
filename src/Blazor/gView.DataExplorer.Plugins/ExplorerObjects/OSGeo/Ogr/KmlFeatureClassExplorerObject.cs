﻿using gView.DataExplorer.Plugins.ExplorerObjects.Base;
using gView.Framework.Core.Data;
using gView.Framework.Core.Geometry;
using gView.Framework.DataExplorer.Abstraction;
using System.Threading.Tasks;

namespace gView.DataExplorer.Plugins.ExplorerObjects.OSGeo.Ogr;

public class KmlFeatureClassExplorerObject : ExplorerObjectCls<KmlExplorerObject, IFeatureClass>,
                                             IExplorerSimpleObject,
                                             ISerializableExplorerObject
{
    private string _filename = "", _fcname = "", _type = "", _icon = "";
    private IFeatureClass? _fc = null;

    public KmlFeatureClassExplorerObject() : base() { }
    public KmlFeatureClassExplorerObject(KmlExplorerObject parent, string filename, IDatasetElement element)
        : base(parent, 1)
    {
        if (element == null)
        {
            return;
        }

        _filename = filename;
        _fcname = element.Title;

        if (element.Class is IFeatureClass)
        {
            _fc = (IFeatureClass)element.Class;
            switch (_fc.GeometryType)
            {
                case GeometryType.Envelope:
                case GeometryType.Polygon:
                    _icon = "webgis:shape-polygon";
                    _type = "Polygon Featureclass";
                    break;
                case GeometryType.Multipoint:
                case GeometryType.Point:
                    _icon = "basic:dot-filled";
                    _type = "Point Featureclass";
                    break;
                case GeometryType.Polyline:
                    _icon = "webgis:shape-polyline";
                    _type = "Polyline Featureclass";
                    break;
            }
        }
    }

    #region IExplorerObject Members

    public string Name => _fcname;

    public string FullName => _filename + ((_filename != "") ? @"\" : "") + _fcname;

    public string Type => _type;

    public string Icon => _icon;

    public void Dispose()
    {
        if (_fc != null)
        {
            _fc = null;
        }
    }
    public Task<object?> GetInstanceAsync() => Task.FromResult<object?>(_fc);

    #endregion

    #region ISerializableExplorerObject Member

    public Task<IExplorerObject?> CreateInstanceByFullName(string FullName, ISerializableExplorerObjectCache? cache)
    {
        return Task.FromResult<IExplorerObject?>(null);
    }

    #endregion
}
