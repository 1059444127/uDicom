﻿using System.ComponentModel.Composition;
using uDicom.WorkItemService.Interface;

namespace uDicom.WorkItem.Archive.DicomSend
{
    [Export(typeof(IWorkItemProcessorFactory)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class DicomSendFactory : IWorkItemProcessorFactory
    {
        public string GetWorkQueueType()
        {
            return "DicomSend";
        }

        public IWorkItemProcessor GetItemProcessor()
        {
            return new DicomSendItemProcessor();
        }
    }
}
