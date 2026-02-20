// <copyright file="Price1Max.cs" company="APIMatic">
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
        typeof(UnionTypeConverter<Price1Max>),
        new Type[] {
            typeof(NumberCase),
            typeof(PrecisionCase)
        },
        true
    )]
    public abstract class Price1Max
    {
        /// <summary>
        /// This is Number case.
        /// </summary>
        /// <returns>
        /// The Price1Max instance, wrapping the provided int value.
        /// </returns>
        public static Price1Max FromNumber(int number)
        {
            return new NumberCase().Set(number);
        }

        /// <summary>
        /// This is Precision case.
        /// </summary>
        /// <returns>
        /// The Price1Max instance, wrapping the provided double value.
        /// </returns>
        public static Price1Max FromPrecision(double precision)
        {
            return new PrecisionCase().Set(precision);
        }

        /// <summary>
        /// Method to match from the provided one-of cases. Here parameters
        /// represents the callback functions for one-of type cases. All
        /// callback functions must have the same return type T. This typeparam T
        /// represents the type that will be returned after applying the selected
        /// callback function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract T Match<T>(Func<int, T> number, Func<double, T> precision);

        [JsonConverter(typeof(UnionTypeCaseConverter<NumberCase, int>), JTokenType.Integer)]
        private sealed class NumberCase : Price1Max, ICaseValue<NumberCase, int>
        {
            public int _value;

            public override T Match<T>(Func<int, T> number, Func<double, T> precision)
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

        [JsonConverter(typeof(UnionTypeCaseConverter<PrecisionCase, double>), JTokenType.Float)]
        private sealed class PrecisionCase : Price1Max, ICaseValue<PrecisionCase, double>
        {
            public double _value;

            public override T Match<T>(Func<int, T> number, Func<double, T> precision)
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
    }
}