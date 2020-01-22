﻿using JsonCryption.Encrypters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption.Converters
{
    internal sealed class EncryptedEnumerableConverter<T> : JsonConverter<IEnumerable<T>>
    {
        private readonly Encrypter _encrypter;
        private readonly JsonSerializerOptions _options;
        private readonly EncryptedArrayConverter<T> _arrayConverter;
        private readonly Type _elementType;
        private readonly Type _arrayType;

        public EncryptedEnumerableConverter(Encrypter encrypter, JsonSerializerOptions options, EncryptedArrayConverter<T> arrayConverter)
        {
            _encrypter = encrypter;
            _options = options;
            _arrayConverter = arrayConverter;

            _elementType = typeof(T);
            _arrayType = _elementType.MakeArrayType();
        }

        public override IEnumerable<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => _arrayConverter.Read(ref reader, _arrayType, options ?? _options);

        public override void Write(Utf8JsonWriter writer, IEnumerable<T> value, JsonSerializerOptions options)
            => _arrayConverter.Write(writer, value.ToArray(), options ?? _options);
    }
}
