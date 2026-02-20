// <copyright file="PriceMin.cs" company="APIMatic">
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
        typeof(UnionTypeConverter<PriceMin>),
        new Type[] {
            typeof(PrecisionCase),
            typeof(NumberCase)
        },
        true
    )]
    public abstract class PriceMin
    {
        /// <summary>
        /// This is Precision case.
        /// </summary>
        /// <returns>
        /// The PriceMin instance, wrapping the provided double value.
        /// </returns>
        public static PriceMin FromPrecision(double precision)
        {
            return new PrecisionCase().Set(precision);
        }

        /// <summary>
        /// This is Number case.
        /// </summary>
        /// <returns>
        /// The PriceMin instance, wrapping the provided int value.
        /// </returns>
        public static PriceMin FromNumber(int number)
        {
            return new NumberCase().Set(number);
        }

        /// <summary>
        /// Method to match from the provided one-of cases. Here parameters
        /// represents the callback functions for one-of type cases. All
        /// callback functions must have the same return type T. This typeparam T
        /// represents the type that will be returned after applying the selected
        /// callback function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract T Match<T>(Func<double, T> precision, Func<int, T> number);

        [JsonConverter(typeof(UnionTypeCaseConverter<PrecisionCase, double>), JTokenType.Float)]
        private sealed class PrecisionCase : PriceMin, ICaseValue<PrecisionCase, double>
        {
            public double _value;

            public override T Match<T>(Func<double, T> precision, Func<int, T> number)
            {
                return precision(_value);
            }

            public PrecisionCase Set(double value)
            {
                _value = value;
                return this;
            }

            public double Get()
            {
                return _value;
            }

            public override string ToString()
            {
                return _value.ToString();
            }
        }

        [JsonConverter(typeof(UnionTypeCaseConverter<NumberCase, int>), JTokenType.Integer)]
        private sealed class NumberCase : PriceMin, ICaseValue<NumberCase, int>
        {
            public int _value;

            public override T Match<T>(Func<double, T> precision, Func<int, T> number)
            {
                return number(_value);
            }

            public NumberCase Set(int value)
            {
                _value = value;
                return this;
            }

            public int Get()
            {
                return _value;
            }

            public override string ToString()
            {
                return _value.ToString();
            }
        }
    }
}