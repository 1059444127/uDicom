/////////////////////////////////////////////////////////////////////////
//// Copyright, (c) Shanghai United Imaging Healthcare Inc
//// All rights reserved. 
//// 
//// author: qiuyang.cao@united-imaging.com
////
//// File: DicomVr.cs
////
//// Summary:
////
////
//// Date: 2014/08/18
//////////////////////////////////////////////////////////////////////////
#region License

// Copyright (c) 2011 - 2013, United-Imaging Inc.
// All rights reserved.
// http://www.united-imaging.com

#endregion

using System;
using System.Collections.Generic;
using UIH.Dicom.IO;

namespace UIH.Dicom
{
    /// <summary>
    /// Class encapsulating a DICOM Value Representation.
    /// </summary>
    public class DicomVr
    {
        private delegate DicomElement CreateAttribute(DicomTag tag, ByteBuffer bb);

        #region Private Members
        private readonly String _name;
        private readonly bool _isText = false;
        private readonly bool _specificCharSet = false;
        private readonly bool _isMultiValue = false;
        private readonly uint _maxLength = 0;
        private readonly bool _is16BitLength = false;
        private readonly char _padChar = ' ';
        private readonly int _unitSize = 1;
        private readonly CreateAttribute _createDelegate;
        private static readonly IDictionary<String, DicomVr> _vrs = new Dictionary<String, DicomVr>();
        #endregion

        #region Public Static Members
        /// <summary>
        /// Static constructor.
        /// </summary>
        static DicomVr()
        {
            _vrs.Add(AEvr.Name, AEvr);
            _vrs.Add(ASvr.Name, ASvr);
            _vrs.Add(ATvr.Name, ATvr);
            _vrs.Add(CSvr.Name, CSvr);
            _vrs.Add(DAvr.Name, DAvr);
            _vrs.Add(DSvr.Name, DSvr);
            _vrs.Add(DTvr.Name, DTvr);
            _vrs.Add(FLvr.Name, FLvr);
            _vrs.Add(FDvr.Name, FDvr);
            _vrs.Add(ISvr.Name, ISvr);
            _vrs.Add(LOvr.Name, LOvr);
            _vrs.Add(LTvr.Name, LTvr);
            _vrs.Add(OBvr.Name, OBvr);
            _vrs.Add(ODvr.Name, ODvr);
            _vrs.Add(OFvr.Name, OFvr);
            _vrs.Add(OWvr.Name, OWvr);
            _vrs.Add(PNvr.Name, PNvr);
            _vrs.Add(SHvr.Name, SHvr);
            _vrs.Add(SLvr.Name, SLvr);
            _vrs.Add(SQvr.Name, SQvr);
            _vrs.Add(SSvr.Name, SSvr);
            _vrs.Add(STvr.Name, STvr);
            _vrs.Add(TMvr.Name, TMvr);
            _vrs.Add(UIvr.Name, UIvr);
            _vrs.Add(ULvr.Name, ULvr);
            _vrs.Add(USvr.Name, USvr);
            _vrs.Add(UTvr.Name, UTvr);
        }

        /// <summary>
        /// The Application Entity VR.
        /// </summary>
        public static readonly DicomVr AEvr = new DicomVr("AE", true, false, true, 16, true, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementAE(tag);

                return new DicomElementAE(tag, bb);
            });

        /// <summary>
        /// The Age String VR.
        /// </summary>
        public static readonly DicomVr ASvr = new DicomVr("AS", true, false, true, 4, true, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementAs(tag);

                return new DicomElementAs(tag, bb);
            });
        /// <summary>
        /// The Attribute Tag VR.
        /// </summary>
        public static readonly DicomVr ATvr = new DicomVr("AT", false, false, true, 4, true, '\0', 4,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementAt(tag);

                return new DicomElementAt(tag, bb);
            });
        /// <summary>
        /// The Code String VR.
        /// </summary>
        public static readonly DicomVr CSvr = new DicomVr("CS", true, false, true, 16, true, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementCs(tag);

                return new DicomElementCs(tag, bb);
            });
        /// <summary>
        /// The Date VR.
        /// </summary>
        public static readonly DicomVr DAvr = new DicomVr("DA", true, false, true, 8, true, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementDa(tag);

                return new DicomElementDa(tag, bb);
            });
        /// <summary>
        /// The Decimal String VR.
        /// </summary>
        public static readonly DicomVr DSvr = new DicomVr("DS", true, false, true, 16, true, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementDs(tag);

                return new DicomElementDs(tag, bb);
            });
        /// <summary>
        /// The Date Time VR.
        /// </summary>
        public static readonly DicomVr DTvr = new DicomVr("DT", true, false, true, 26, true, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementDt(tag);

                return new DicomElementDt(tag, bb);
            });
        /// <summary>
        /// The Floating Point Single VR.
        /// </summary>
        public static readonly DicomVr FLvr = new DicomVr("FL", false, false, true, 4, true, '\0', 4,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementFl(tag);

                return new DicomElementFl(tag, bb);
            });
        /// <summary>
        /// The Floating Point Double VR.
        /// </summary>
        public static readonly DicomVr FDvr = new DicomVr("FD", false, false, true, 8, true, '\0', 8,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementFd(tag);

                return new DicomElementFd(tag, bb);
            });
        /// <summary>
        /// The Integer String VR.
        /// </summary>
        public static readonly DicomVr ISvr = new DicomVr("IS", true, false, true, 12, true, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementIs(tag);

                return new DicomElementIs(tag, bb);
            });
        /// <summary>
        /// The Long String VR.
        /// </summary>
        public static readonly DicomVr LOvr = new DicomVr("LO", true, true, true, 64, true, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementLo(tag);

                return new DicomElementLo(tag, bb);
            });
        /// <summary>
        /// The Long Text VR.
        /// </summary>
        public static readonly DicomVr LTvr = new DicomVr("LT", true, true, false, 10240, true, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementLt(tag);

                return new DicomElementLt(tag, bb);
            });
        /// <summary>
        /// The Other Byte String VR.
        /// </summary>
        public static readonly DicomVr OBvr = new DicomVr("OB", false, false, false, 1, false, '\0', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementOb(tag);

                return new DicomElementOb(tag, bb);
            });
        /// <summary>
        /// The Other Double String VR.
        /// </summary>
        public static readonly DicomVr ODvr = new DicomVr("OD", false, false, false, 8, false, '\0', 8,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementOD(tag);

                return new DicomElementOD(tag, bb);
            });
        /// <summary>
        /// The Other Float String VR.
        /// </summary>
        public static readonly DicomVr OFvr = new DicomVr("OF", false, false, false, 4, false, '\0', 4,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementOf(tag);

                return new DicomElementOf(tag, bb);
            });
        /// <summary>
        /// The Other Word String VR.
        /// </summary>
        public static readonly DicomVr OWvr = new DicomVr("OW", false, false, false, 2, false, '\0', 2,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementOw(tag);

                return new DicomElementOw(tag, bb);
            });
        /// <summary>
        /// The Person Name VR.
        /// </summary>
        public static readonly DicomVr PNvr = new DicomVr("PN", true, true, true, 64 * 5, true, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementPn(tag);

                return new DicomElementPn(tag, bb);
            });
        /// <summary>
        /// The Short String VR.
        /// </summary>
        public static readonly DicomVr SHvr = new DicomVr("SH", true, true, true, 16, true, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementSh(tag);

                return new DicomElementSh(tag, bb);
            });
        /// <summary>
        /// The Signed Long VR.
        /// </summary>
        public static readonly DicomVr SLvr = new DicomVr("SL", false, false, true, 4, true, '\0', 4,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementSl(tag);

                return new DicomElementSl(tag, bb);
            });
        /// <summary>
        /// The Sequence of Items VR.
        /// </summary>
        public static readonly DicomVr SQvr = new DicomVr("SQ", false, false, false, 0, false, '\0', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementSq(tag);

                return new DicomElementSq(tag, bb);
            });
        /// <summary>
        /// The Signed Short VR.
        /// </summary>
        public static readonly DicomVr SSvr = new DicomVr("SS", false, false, true, 2, true, '\0', 2,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementSs(tag);

                return new DicomElementSs(tag, bb);
            });
        /// <summary>
        /// The Short Text VR.
        /// </summary>
        public static readonly DicomVr STvr = new DicomVr("ST", true, true, false, 1024, true, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementSt(tag);

                return new DicomElementSt(tag, bb);
            });
        /// <summary>
        /// The Time VR.
        /// </summary>
        public static readonly DicomVr TMvr = new DicomVr("TM", true, false, true, 16, true, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementTm(tag);

                return new DicomElementTm(tag, bb);
            });
        /// <summary>
        /// The Unique Identifer (UID) VR.
        /// </summary>
        public static readonly DicomVr UIvr = new DicomVr("UI", true, false, true, 64, true, '\0', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementUi(tag);

                return new DicomElementUi(tag, bb);
            });
        /// <summary>
        /// The Unsigned Long VR.
        /// </summary>
        public static readonly DicomVr ULvr = new DicomVr("UL", false, false, true, 4, true, '\0', 4,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementUl(tag);

                return new DicomElementUl(tag, bb);
            });
        /// <summary>
        /// The Unknown VR.
        /// </summary>
        public static readonly DicomVr UNvr = new DicomVr("UN", false, false, false, 0, false, '\0', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementUn(tag);

                return new DicomElementUn(tag, bb);
            });
        /// <summary>
        /// The Unsigned Short VR.
        /// </summary>
        public static readonly DicomVr USvr = new DicomVr("US", false, false, true, 2, true, '\0', 2,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementUs(tag);

                return new DicomElementUs(tag, bb);
            });
        /// <summary>
        /// The Unlimited Text VR.
        /// </summary>
        public static readonly DicomVr UTvr = new DicomVr("UT", true, true, false, 0, false, ' ', 1,
            delegate(DicomTag tag, ByteBuffer bb)
            {
                if (bb == null) return new DicomElementUt(tag);

                return new DicomElementUt(tag, bb);
            });

        internal static readonly DicomVr NONE = new DicomVr("NONE", false, false, false, 1, false, '\0', 1,
            delegate
            {
                return null;
            });

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets a DicomVR instance based on an input VR string.
        /// </summary>
        /// <param name="name">The string representation of the VR.</param>
        /// <returns>A DicomVr instance for <paramref name="name"/>.</returns>
        public static DicomVr GetVR(String name)
        {
            DicomVr theVr;
            if (!_vrs.TryGetValue(name, out theVr))
                return UNvr;
            return theVr;
        }

        public static IList<DicomVr> GetDicomVrList()
        {
            return new List<DicomVr>(_vrs.Values);
        }
        #endregion

        /// <summary>
        /// Private constructor for DicomVr.
        /// </summary>
        /// <param name="name">The two digit text name of the VR.</param>
        /// <param name="isText">Boolean telling if the VR is text based.</param>
        /// <param name="isSpecificCharacterSet">Boolean telling if the value for Specific Character Set impacts the VR.</param>
        /// <param name="isMultiValue">Boolean telling if the VR supports multiple values.</param>
        /// <param name="maxLength">The maximum length of the tag, 0 if the tag is unlimited in length (max value of 2^32).</param>
        /// <param name="is16bitLength">The VR is encoded with a 16 bit length for Explict VRs</param>
        /// <param name="padChar">The character used for padding with the VR.</param>
        /// <param name="unitSize">The size in bytes for binary VRs of each value encoded</param>
        /// <param name="createDelegate">A delegate to create <see cref="DicomElement"/> instances for the specific Vr.</param>
        private DicomVr(String name,
                        bool isText,
                        bool isSpecificCharacterSet,
                        bool isMultiValue,
                        uint maxLength,
                        bool is16bitLength,
                        char padChar,
                        int unitSize,
                        CreateAttribute createDelegate)
        {
            _name = name;
            _isText = isText;
            _specificCharSet = isSpecificCharacterSet;
            _isMultiValue = isMultiValue;
            _maxLength = maxLength;
            _is16BitLength = is16bitLength;
            _padChar = padChar;
            _unitSize = unitSize;
            _createDelegate = createDelegate;
        }

        /// <summary>
        /// The two digit string representation of the VR.
        /// </summary>
        /// <returns>A Value Representation string.</returns>
        public override string ToString()
        {
            return _name;
        }

        /// <summary>
        /// Implicit cast to a String object, for ease of use.
        /// </summary>
        public static implicit operator String(DicomVr myVr)
        {
            return myVr.ToString();
        }

        /// <summary>
        /// Internal method for creating a new <see cref="DicomElement"/> derived class for the VR.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        internal DicomElement CreateDicomAttribute(DicomTag tag)
        {
            return _createDelegate(tag, null);
        }

        /// <summary>
        /// Internal method for creating a new <see cref="DicomElement"/> derived class for the VR.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="bb"></param>
        /// <returns></returns>
        internal DicomElement CreateDicomAttribute(DicomTag tag, ByteBuffer bb)
        {
            return _createDelegate(tag, bb);
        }

        #region Public Properties

        /// <summary>
        /// Is the VR text based?
        /// </summary>
        public bool IsTextVR
        {
            get { return _isText; }
        }

        /// <summary>
        /// Does the VR support multiple values?
        /// </summary>
        public bool IsMultiValue
        {
            get { return _isMultiValue; }
        }

        /// <summary>
        /// Does the value of the tag Specific Character Set impact the encoding of the VR?
        /// </summary>
        public bool SpecificCharacterSet
        {
            get { return _specificCharSet; }
        }

        /// <summary>
        /// What is the maximum length of the a tag encoded with the VR?  (A value of 0 means the maximum length is 2^32.)
        /// </summary>
        public uint MaximumLength
        {
            get { return _maxLength; }
        }

        /// <summary>
        /// The name of the VR.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Does the VR require 16 bit length fields for Explicit VR transfer syntaxes? 
        /// </summary>
        public bool Is16BitLengthField
        {
            get { return _is16BitLength; }
        }

        /// <summary>
        /// What is the padding character for the VR?
        /// </summary>
        public char PadChar
        {
            get { return _padChar; }
        }

        /// <summary>
        /// For binary VRs, what is the size of each individual value?
        /// </summary>
        public int UnitSize
        {
            get { return _unitSize; }
        }
        #endregion

    }
}
