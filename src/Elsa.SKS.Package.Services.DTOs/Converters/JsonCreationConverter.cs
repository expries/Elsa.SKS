﻿using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Elsa.SKS.Package.Services.DTOs.Converters
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        public override bool CanWrite => false;

        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (serializer is null)
            {
                throw new ArgumentException(null, nameof(serializer));
            }

            if (reader.TokenType is JsonToken.Null)
            {
                return null;
            }

            var jObject = JObject.Load(reader);
            var target = Create(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}