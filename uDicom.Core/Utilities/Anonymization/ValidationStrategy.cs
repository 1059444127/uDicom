/////////////////////////////////////////////////////////////////////////
//// Copyright, (c) Shanghai United Imaging Healthcare Inc
//// All rights reserved. 
//// 
//// author: qiuyang.cao@united-imaging.com
////
//// File: ValidationStrategy.cs
////
//// Summary:
////
////
//// Date: 2014/08/19
//////////////////////////////////////////////////////////////////////////
#region License

// Copyright (c) 2011 - 2013, United-Imaging Inc.
// All rights reserved.
// http://www.united-imaging.com

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UIH.Dicom.Iod;

namespace UIH.Dicom.Utilities.Anonymization
{
	/// <summary>
	/// An enumeration of flags to control the behaviour of the <see cref="DicomAnonymizer.ValidationStrategy"/>.
	/// </summary>
	[FlagsAttribute]
	public enum ValidationOptions : uint
	{

		#region Individual Flags - Study Level

		/// <summary>
		/// Indicates that the <see cref="DicomAnonymizer.ValidationStrategy"/> should not enforce a non-empty patient ID in the anonymized data set.
		/// </summary>
		AllowEmptyPatientId = 0x01,

		/// <summary>
		/// Indicates that the <see cref="DicomAnonymizer.ValidationStrategy"/> should not enforce a non-empty patient name in the anonymized data set.
		/// </summary>
		AllowEmptyPatientName = 0x02,

		/// <summary>
		/// Indicates that the <see cref="DicomAnonymizer.ValidationStrategy"/> should not enforce a different patient's birthdate in the anonymized data set.
		/// </summary>
		AllowEqualBirthDate = 0x04,

		#endregion

		#region Group Flags

		/// <summary>
		/// Indicates that the <see cref="DicomAnonymizer.ValidationStrategy"/> should relax all optional attribute value checks in the anonymized data set.
		/// </summary>
		RelaxAllChecks = AllowEmptyPatientId | AllowEmptyPatientName | AllowEqualBirthDate,

		/// <summary>
		/// Indicates that the <see cref="DicomAnonymizer.ValidationStrategy"/>  should use its default behaviour, which is to enforce non-empty and different values in all checked attributes.
		/// </summary>
		Default = 0x0

		#endregion
	}

	/// <summary>
	/// Reason for the validation failure.
	/// </summary>
	public enum ValidationFailureReason
	{
		EmptyValue,
		ConflictingValue
	}

	/// <summary>
	/// Describes the validation failure.
	/// </summary>
	/// <remarks>
	/// For those properties, in <see cref="SeriesData"/> and <see cref="StudyData"/>, that have a 'Raw' counterpart
	/// (e.g. <see cref="StudyData.PatientsNameRaw"/>), the <see cref="PropertyName"/> will always correspond to the non-raw
	/// property (e.g. <see cref="StudyData.PatientsName"/>).
	/// </remarks>
	public class ValidationFailureDescription
	{
		internal ValidationFailureDescription(string propertyName, ValidationFailureReason reason, string description)
		{
			PropertyName = propertyName;
			Reason = reason;
			Description = description;
		}

		public readonly string PropertyName;
		public readonly ValidationFailureReason Reason;
		public readonly string Description;
	}

	public partial class DicomAnonymizer
	{
		/// <summary>
		/// The strategy used by the <see cref="DicomAnonymizer"/> to validate anonymized data.
		/// </summary>
		/// <remarks>
		/// You can use this class on its own to pre-determine if your anonymized <see cref="StudyData"/> or
		/// <see cref="SeriesData"/> will be accepted by the <see cref="DicomAnonymizer"/>.
		/// </remarks>
		public class ValidationStrategy
		{
			private ValidationOptions _options = ValidationOptions.Default;

			private List<ValidationFailureDescription> _failures;

			/// <summary>
			/// Default constructor.
			/// </summary>
			public ValidationStrategy()
			{
			}

			/// <summary>
			/// Gets or sets the validation options.
			/// </summary>
			public ValidationOptions Options
			{
				get { return _options; }
				set { _options = value; }
			}

			public ReadOnlyCollection<ValidationFailureDescription> GetValidationFailures(StudyData originalData, StudyData anonymizedData)
			{
				_failures = new List<ValidationFailureDescription>();

				ValidatePatientNamesNotEqual(originalData.PatientsName, anonymizedData.PatientsName);
				ValidateNotEqual(originalData.PatientId, anonymizedData.PatientId, "PatientId");
				ValidateNotEqual(originalData.AccessionNumber, anonymizedData.AccessionNumber, "AccessionNumber");
				ValidateNotEqual(originalData.StudyId, anonymizedData.StudyId, "StudyId");

				if (!IsOptionSet(_options, ValidationOptions.AllowEqualBirthDate))
					ValidateNotEqual(originalData.PatientsBirthDateRaw, anonymizedData.PatientsBirthDateRaw, "PatientsBirthDate");
				if (!IsOptionSet(_options, ValidationOptions.AllowEmptyPatientId))
					ValidateNotEmpty(anonymizedData.PatientId, "PatientId");
				if (!IsOptionSet(_options, ValidationOptions.AllowEmptyPatientName))
					ValidateNotEmpty(anonymizedData.PatientsNameRaw, "PatientsName");

				ReadOnlyCollection<ValidationFailureDescription> failures = _failures.AsReadOnly();
				_failures = null;
				return failures;
			}

			/// <summary>
			/// Gets a list of <see cref="ValidationFailureDescription"/>s describing all validation failures.
			/// </summary>
			/// <remarks>
			/// When an empty list is returned, it means there were no validation failures.
			/// </remarks>
			internal ReadOnlyCollection<ValidationFailureDescription> GetValidationFailures(SeriesData originalData, SeriesData anonymizedData)
			{
				_failures = new List<ValidationFailureDescription>();
				ReadOnlyCollection<ValidationFailureDescription> failures = _failures.AsReadOnly();

				//nothing to validate for now.

				_failures = null;
				return failures;
			}

			/// <summary>
			/// Gets a list of <see cref="ValidationFailureDescription"/>s describing all validation failures.
			/// </summary>
			/// <remarks>
			/// When an empty list is returned, it means there were no validation failures.
			/// </remarks>
			private void ValidatePatientNamesNotEqual(PersonName original, PersonName anonymized)
			{
				ValidatePatientNamesNotEqual(original.SingleByte, anonymized.SingleByte, "SingleByte");
				ValidatePatientNamesNotEqual(original.Ideographic, anonymized.Ideographic, "Ideographic");
				ValidatePatientNamesNotEqual(original.Phonetic, anonymized.Phonetic, "Phonetic");
			}

			private void ValidatePatientNamesNotEqual(ComponentGroup original, ComponentGroup anonymized, string componentGroup)
			{
				string format = "The anonymized name component ({0}:{1}) cannot be the same as the original.";

				//may not have the same family, given, or middle name
				ValidateNotEqual(original.FamilyName, anonymized.FamilyName, "PatientsName", String.Format(format, componentGroup, "Family"));
				ValidateNotEqual(original.GivenName, anonymized.GivenName, "PatientsName", String.Format(format, componentGroup, "Given"));
				ValidateNotEqual(original.MiddleName, anonymized.MiddleName, "PatientsName", String.Format(format, componentGroup, "Middle"));
			}

			private void ValidateNotEmpty(string value, string property)
			{
				ValidateNotEmpty(value, property, null);
			}

			private void ValidateNotEmpty(string value, string property, string description)
			{
				if (String.IsNullOrEmpty(value))
					_failures.Add(new ValidationFailureDescription(property, ValidationFailureReason.EmptyValue, description ?? "The value cannot be empty."));
			}

			private void ValidateNotEqual(string original, string anonymized, string property)
			{
				ValidateNotEqual(original, anonymized, property, null);
			}

			private void ValidateNotEqual(string original, string anonymized, string property, string description)
			{
				if (String.IsNullOrEmpty(original) && String.IsNullOrEmpty(anonymized))
					return;

				if (String.Compare(original ?? "", anonymized ?? "", true) == 0)
					_failures.Add(new ValidationFailureDescription(property, ValidationFailureReason.ConflictingValue, description ?? "The anonymized value cannot be unchanged from the original."));
			}

			private static bool IsOptionSet(ValidationOptions options, ValidationOptions flag)
			{
				return (options & flag) == flag;
			}
		}
	}
}
