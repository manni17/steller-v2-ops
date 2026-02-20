// <copyright file="Item2FaceValues.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
using APIMatic.Core.Utilities.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace BambooClientIntegrationDocumentationV2.Standard.Models.Containers
{
    /// <summary>
    /// This is a container class for one-of types.
    /// </summary>
    [JsonConverter(
        typeof(UnionTypeConverter<Item2FaceValues>),
        new Type[] {
            typeof(FaceValueCase),
            typeof(MStringCase)
        },
        true
    )]
    public abstract class Item2FaceValues
    {
        /// <summary>
        /// This is FaceValue case.
        /// </summary>
        /// <returns>
        /// The Item2FaceValues instance, wrapping the provided FaceValue value.
        /// </returns>
        public static Item2FaceValues FromFaceValue(FaceValue faceValue)
        {
            return new FaceValueCase().Set(faceValue);
        }

        /// <summary>
        /// This is String case.
        /// </summary>
        /// <returns>
        /// The Item2FaceValues instance, wrapping the provided string value.
        /// </returns>
        public static Item2FaceValues FromString(string mString)
        {
            return new MStringCase().Set(mString);
        }

        /// <summary>
        /// Method to match from the provided one-of cases. Here parameters
        /// represents the callback functions for one-of type cases. All
        /// callback functions must have the same return type T. This typeparam T
        /// represents the type that will be returned after applying the selected
        /// callback function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract T Match<T>(Func<FaceValue, T> faceValue, Func<string, T> mString);

        [JsonConverter(typeof(UnionTypeCaseConverter<FaceValueCase, FaceValue>))]
        private sealed class FaceValueCase : Item2FaceValues, ICaseValue<FaceValueCase, FaceValue>
        {
            public FaceValue _value;

            public override T Match<T>(Func<FaceValue, T> faceValue, Func<string, T> mString)
            {
                return faceValue(_value);
            }

            public FaceValueCase Set(FaceValue value)
            {
                _value = value;
                return this;
            }

            public FaceValue Get()
            {
                return _value;
            }

            public override string ToString()
            {
                return _value?.ToString();
            }
        }

        [JsonConverter(typeof(UnionTypeCaseConverter<MStringCase, string>), JTokenType.String, JTokenType.Null)]
        private sealed class MStringCase : Item2FaceValues, ICaseValue<MStringCase, string>
        {
            public string _value;

            public override T Match<T>(Func<FaceValue, T> faceValue, Func<string, T> mString)
            {
                return mString(_value);
            }

            public MStringCase Set(string value)
            {
                _value = value;
                return this;
            }

            public string Get()
            {
                return _value;
            }

            public override string ToString()
            {
                return _value?.ToString();
            }
        }
    }
}