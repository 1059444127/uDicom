/////////////////////////////////////////////////////////////////////////
//// Copyright, (c) Shanghai United Imaging Healthcare Inc
//// All rights reserved. 
//// 
//// author: qiuyang.cao@united-imaging.com
////
//// File: DicomTagDictionary.cs
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
using System.IO;

// This file is auto-generated by the UIH.Dicom.DataDictionaryGenerator project.

namespace UIH.Dicom
{
    /// <summary>
    /// This class contains a dictionary of all DICOM tags.
    /// </summary>
    /// <remarks>
    /// <para>This class is the Flyweight Factor for the DicomTag Flyweight class as defined in the Flyweight pattern.</para>
    /// </remarks>
    public class DicomTagDictionary
    {
        // Internal members
        private static readonly Dictionary<uint,DicomTag> _tags = new Dictionary<uint,DicomTag>();
        private static readonly Dictionary<string,DicomTag> _tagNames = new Dictionary<string,DicomTag>();

        // Static constructor
        static DicomTagDictionary()
        {
            InitStandardTags();
        }

        /// <summary>
        ///Get the dictionary of tags
        /// </summary>
        /// <remarks>
        /// Currently this method is only used for the regression tests.
        /// </remarks>
        internal static Dictionary<uint,DicomTag> TagDictionary
        {
            get { return _tags; }
        }

        /// <summary>
        /// Retrieve a strongly typed list containing all DICOM tags.
        /// </summary>
        /// <returns>A <see cref="System.Collections.Generic.List"/>.</returns>
        public static IList<DicomTag> GetDicomTagList()
        {
            return new List<DicomTag>(_tags.Values);
        }

        /// <summary>
        /// Method used to retrieve DicomTag instances for specific DICOM attributes.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="element"></param>
        /// <returns>A DicomTag instance, if the tag exists, or null if it doesn't.</returns>
        public static DicomTag GetDicomTag(ushort group, ushort element)
        {
            return GetDicomTag((uint)group << 16 | (uint)element);
        }

        /// <summary>
        /// Method used to retrieve DicomTag instances for specific DICOM attributes.
        /// </summary>
        /// <param name="tag">The DICOM tag to retrieve.</param>
        /// <returns>A DicomTag instance, if the tag exists, or null if it doesn't.</returns>
        public static DicomTag GetDicomTag(uint tag)
        {
            DicomTag theTag;
            if (!_tags.TryGetValue(tag, out theTag))
            {
            	if (((tag & 0xFFE10000) == 0x60000000) && ((tag & 0xFFFF0000) != 0x60000000))
            	{
            		theTag = GetDicomTag(tag & 0xFF00FFFF);
            		if (theTag == null) return null;

            		return new DicomTag(tag, theTag.Name, theTag.VariableName, theTag.VR, theTag.MultiVR, theTag.VMLow, theTag.VMHigh, theTag.Retired);
            	}
            	return null;
            }

            return theTag; 
        }

        /// <summary>
        /// Method used to retrieve DicomTag instances for specific DICOM attributes.
        /// </summary>
        /// <param name="name">The name of the DICOM tag to retrieve.</param>
        /// <returns>A DicomTag instance, if the tag exists, or null if it doesn't.</returns>
        public static DicomTag GetDicomTag(string name)
        {
            DicomTag theTag;
            if (!_tagNames.TryGetValue(name, out theTag))
                return null;
            return theTag; 
        }

        /// <summary>
        /// Initialize dictionary with standard tags.
        /// </summary>
        private static void InitStandardTags()
        {
			using (var stream = typeof(DicomTagDictionary).Assembly.GetManifestResourceStream("UIH.Dicom.DicomTagDictionary.data"))
			{
				if(stream == null)
					throw new InvalidOperationException("DICOM data dictionary embedded resource not found.");

				using (var reader = new StreamReader(stream))
				{
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						if (line.StartsWith("#"))
							continue;

						var items = line.Split(';');
						var tag = new DicomTag(
							uint.Parse(items[0]),
							items[1],
							items[2],
							DicomVr.GetVR(items[3]),
							bool.Parse(items[4]),
							uint.Parse(items[5]),
							uint.Parse(items[6]),
							bool.Parse(items[7])
							);
						_tags.Add(tag.TagValue, tag);
						_tagNames.Add(tag.VariableName, tag);
					}
				}
			}
		}
    }
}
