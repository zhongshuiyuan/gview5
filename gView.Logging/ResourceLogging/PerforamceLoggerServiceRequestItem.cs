﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace gView.Framework.Logging.ResourceLogging
{
    public class PerforamceLoggerServiceRequestItem : TableEntity, IPerformanceLoggerItem
    {
        #region IResourcesLoggerItem

        public int Milliseconds { get; set; }
        public int ContentSize { get; set; }
        public string ServiceId { get; set; }
        public string TypeName { get; set; }
        public int Count { get; set; }
        public DateTime Created { get; set; }

        public bool CanAggregate(IPerformanceLoggerItem item)
        {
            if (item is PerforamceLoggerServiceRequestItem)
            {
                return this.PartitionKey == item.PartitionKey &&
                       this.TypeName == item.TypeName &&
                       this.ServiceId == item.ServiceId;
            }

            return false;
        }

        public void Aggregate(IPerformanceLoggerItem item)
        {
            if (item is PerforamceLoggerServiceRequestItem)
            {
                this.Count += item.Count;
                this.Milliseconds += item.Milliseconds;
                this.ContentSize += item.ContentSize;
            }
        }

        #endregion
    }
}
