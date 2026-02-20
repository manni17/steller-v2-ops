// <copyright file="Item1Categories.cs" company="APIMatic">
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
        typeof(UnionTypeConverter<Item1Categories>),
        new Type[] {
            typeof(CategoryCase),
            typeof(MStringCase)
        },
        true
    )]
    public abstract class Item1Categories
    {
        /// <summary>
        /// This is Category case.
        /// </summary>
        /// <returns>
        /// The Item1Categories instance, wrapping the provided Category value.
        /// </returns>
        public static Item1Categories FromCategory(Category category)
        {
            return new CategoryCase().Set(category);
        }

        /// <summary>
        /// This is String case.
        /// </summary>
        /// <returns>
        /// The Item1Categories instance, wrapping the provided string value.
        /// </returns>
        public static Item1Categories FromString(string mString)
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
        public abstract T Match<T>(Func<Category, T> category, Func<string, T> mString);

        [JsonConverter(typeof(UnionTypeCaseConverter<CategoryCase, Category>))]
        private sealed class CategoryCase : Item1Categories, ICaseValue<CategoryCase, Category>
        {
            public Category _value;

            public override T Match<T>(Func<Category, T> category, Func<string, T> mString)
            {
                return category(_value);
            }

            public CategoryCase Set(Category value)
            {
                _value = value;
                return this;
            }

            public Category Get()
            {
                return _value;
            }

            public override string ToString()
            {
                return _value?.ToString();
            }
        }

        [JsonConverter(typeof(UnionTypeCaseConverter<MStringCase, string>), JTokenType.String, JTokenType.Null)]
        private sealed class MStringCase : Item1Categories, ICaseValue<MStringCase, string>
        {
            public string _value;

            public override T Match<T>(Func<Category, T> category, Func<string, T> mString)
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