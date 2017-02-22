﻿#region License

// 
// Copyright (c) 2011 - 2012, United-Imaging Inc.
// All rights reserved.
// http://www.united-imaging.com

#endregion

using UIH.Dicom.Network;
using UIH.Dicom.PACS.Service.Model;

namespace UIH.Dicom.PACS.Service.Interface
{
    public interface IDeviceManager
    {
        Device LookupDevice(AssociationParameters association, out bool isNew);
    }
}