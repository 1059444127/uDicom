/////////////////////////////////////////////////////////////////////////
//// Copyright, (c) Shanghai United Imaging Healthcare Inc
//// All rights reserved. 
//// 
//// author: qiuyang.cao@united-imaging.com
////
//// File: ObjectWalker.cs
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
using System.Reflection;

namespace uDicom.Common.Utilities
{
    /// <summary>
    /// Defines a callback interface used by the <see cref="ObjectWalker"/> class.
    /// </summary>
    public interface IObjectMemberContext
    {
        /// <summary>
        /// Gets the object instance being walked, or null if a type is being walked.
        /// </summary>
        object Object { get;}

        /// <summary>
        /// Gets the property or field that the walker is currently at.
        /// </summary>
        MemberInfo Member { get;}

        /// <summary>
        /// Gets the type of the property or field that the walker is currently at.
        /// </summary>
        Type MemberType { get; }

        /// <summary>
        /// Gets or sets the value of the property or field that the walker is currently at,
        /// assuming an object instance is being walked.
        /// </summary>
        object MemberValue { get; set; }
    }

    /// <summary>
    /// Utility class for walking the properties and/or fields of an object.
    /// </summary>
    /// <remarks>
    /// By default, the public properties and fields of the object will be included in the walk.
    /// Set the properties of the <see cref="ObjectWalker"/> instance to optionally include
    /// private fields and/or properties, or to optionally exclude public fields/properties. 
    /// </remarks>
    public class ObjectWalker
    {
        #region IObjectWalkerContext implementations

        class PropertyContext : IObjectMemberContext
        {
            private readonly PropertyInfo _property;
            private readonly object _obj;

            public PropertyContext(object obj, PropertyInfo property)
            {
                _property = property;
                _obj = obj;
            }

            public object Object
            {
                get { return _obj; }
            }

            public Type MemberType
            {
                get { return _property.PropertyType; }
            }

            public MemberInfo Member
            {
                get { return _property;}
            }

            public object MemberValue
            {
                get { return _property.GetValue(_obj, BindingFlags.Public|BindingFlags.NonPublic, null, null, null);}
                set { _property.SetValue(_obj, value, BindingFlags.Public | BindingFlags.NonPublic, null, null, null); }
            }
        }

        class FieldContext : IObjectMemberContext
        {
            private readonly FieldInfo _member;
            private readonly object _obj;

            public FieldContext(object obj, FieldInfo member)
            {
                _member = member;
                _obj = obj;
            }

            public object Object
            {
                get { return _obj; }
            }

            public Type MemberType
            {
                get { return _member.FieldType; }
            }

            public MemberInfo Member
            {
                get { return _member;}
            }

            public object MemberValue
            {
                get { return _member.GetValue(_obj);}
                set { _member.SetValue(_obj, value);}
            }
        }

        #endregion

        #region Private fields

        private bool _includeNonPublicFields;
        private bool _includePublicFields;
        private bool _includeNonPublicProperties;
        private bool _includePublicProperties;

        private Predicate<MemberInfo> _memberFilter;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ObjectWalker()
			:this(null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
		/// <param name="memberFilter"></param>
        public ObjectWalker(Predicate<MemberInfo> memberFilter)
        {
			// includ public fields and properties by default
			_includePublicFields = true;
			_includePublicProperties = true;
			_memberFilter = memberFilter;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to include non-public fields in the walk.
        /// </summary>
        public bool IncludeNonPublicFields
        {
            get { return _includeNonPublicFields; }
            set { _includeNonPublicFields = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include public fields in the walk.
        /// </summary>
        public bool IncludePublicFields
        {
            get { return _includePublicFields; }
            set { _includePublicFields = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include non-public properties in the walk.
        /// </summary>
        public bool IncludeNonPublicProperties
        {
            get { return _includeNonPublicProperties; }
            set { _includeNonPublicProperties = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include public properties in the walk.
        /// </summary>
        public bool IncludePublicProperties
        {
            get { return _includePublicProperties; }
            set { _includePublicProperties = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Walks properties and/or fields of the specified object.
        /// </summary>
        /// <param name="obj"></param>
		public IEnumerable<IObjectMemberContext> Walk(object obj)
        {
            Platform.CheckForNullReference(obj, "obj");

            return WalkHelper(obj.GetType(), obj);
        }

        /// <summary>
        /// Walks properties and/or fields of the specified type.
        /// </summary>
        /// <param name="type"></param>
		public IEnumerable<IObjectMemberContext> Walk(Type type)
        {
            return WalkHelper(type, null);
        }

        #endregion

        private IEnumerable<IObjectMemberContext> WalkHelper(Type type, object instance)
        {
            // walk properties
            if (_includePublicProperties || _includeNonPublicProperties)
            {
                BindingFlags bindingFlags = BindingFlags.Instance;
                if (_includePublicProperties)
                    bindingFlags |= BindingFlags.Public;
                if (_includeNonPublicProperties)
                    bindingFlags |= BindingFlags.NonPublic;
                foreach (PropertyInfo property in type.GetProperties(bindingFlags))
                {
                    if (_memberFilter == null || _memberFilter(property))
                    {
                    	yield return new PropertyContext(instance, property);
                    }
                }
            }

            // walk fields
            if (_includePublicFields || _includeNonPublicFields)
            {
                BindingFlags bindingFlags = BindingFlags.Instance;
                if (_includePublicFields)
                    bindingFlags |= BindingFlags.Public;
                if (_includeNonPublicFields)
                    bindingFlags |= BindingFlags.NonPublic;
                foreach (FieldInfo field in type.GetFields(bindingFlags))
                {
                    if (_memberFilter == null || _memberFilter(field))
                    {
						yield return new FieldContext(instance, field);
					}
                }
            }
        }
    }
}
