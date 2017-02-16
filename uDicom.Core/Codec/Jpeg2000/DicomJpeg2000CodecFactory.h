////////////////////////////////////////////////////////////////////////
/// Copyright, (c) Shanghai United Imaging Healthcare Inc
/// All rights reserved. 
/// 
/// *@author: qiuyang.cao@united-imaging.com
///
/// @file: DicomJpeg2000CodecFactory.h
///
/// @brief:
///
///
/// @date: 2014/08/19
/////////////////////////////////////////////////////////////////////////
#pragma region License

// Copyright (c) 2012, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This file is part of the ClearCanvas RIS/PACS open source project.
//
// The ClearCanvas RIS/PACS open source project is free software: you can
// redistribute it and/or modify it under the terms of the GNU Lesser Public
// License as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version.
//
// The ClearCanvas RIS/PACS open source project is distributed in the hope that
// it will be useful, but WITHOUT ANY WARRANTY; without even the implied
// warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser Public License for more details.
//
// You should have received a copy of the GNU Lesser Public License along with
// the ClearCanvas RIS/PACS open source project.  If not, see
// <http://www.gnu.org/licenses/>.

#pragma endregion

#pragma region Inline Attributions
// The source code contained in this file is based on an original work
// from
//
// mDCM: A C# DICOM library
//
// Copyright (c) 2008  Colby Dillion
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Author:
//    Colby Dillion (colby.dillion@gmail.com)
#pragma endregion

#pragma once

using namespace System;
using namespace System::IO;
using namespace System::Xml;
using namespace System::ComponentModel::Composition;

using namespace UIH::Dicom;
using namespace UIH::Dicom::Codec;

#include "DicomJpeg2000Codec.h"

namespace UIH {
namespace Dicom {
namespace Codec {
namespace Jpeg2000 {

[Export(IDicomCodecFactory::typeid)]
public ref class DicomJpeg2000LosslessCodecFactory : public IDicomCodecFactory {
public:
    virtual property String^ Name { String^ get() ;}
    virtual property bool Enabled { bool get() ;}
    virtual property UIH::Dicom::TransferSyntax^ CodecTransferSyntax {  UIH::Dicom::TransferSyntax^ get(); };

    virtual DicomCodecParameters^ GetCodecParameters(DicomDataset^ dataSet);
    virtual DicomCodecParameters^ GetCodecParameters(XmlDocument^ parms);
	virtual IDicomCodec^ GetDicomCodec();
};

[Export(IDicomCodecFactory::typeid)]
public ref class DicomJpeg2000LossyCodecFactory : public IDicomCodecFactory {
public:
    virtual property String^ Name { String^ get();}
    virtual property bool Enabled { bool get() ;}
    virtual property UIH::Dicom::TransferSyntax^ CodecTransferSyntax { UIH::Dicom::TransferSyntax^ get(); };

    virtual DicomCodecParameters^ GetCodecParameters(DicomDataset^ dataSet);
    virtual DicomCodecParameters^ GetCodecParameters(XmlDocument^ parms);
	virtual IDicomCodec^ GetDicomCodec();
};

} // Jpeg2000
} // Codec
} // Dicom
} // UIH

