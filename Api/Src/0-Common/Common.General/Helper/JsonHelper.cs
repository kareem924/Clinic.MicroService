using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.General.Helper
{
    public static class JsonHelper
    {
        private const string Empty = "";
        public static JsonSerializer JsonSerializer => JsonSerializer.CreateDefault(GetSettings(false));

        public static string Serialize(object value, bool camelCase = false, bool indented = false, int? maxDepth = null)
        {
            var formatting = indented
                ? Formatting.Indented
                : Formatting.None;
            var settings = GetSettings(camelCase);
            if (maxDepth.HasValue)
            {
                var initialJson = JsonConvert.SerializeObject(value, formatting, settings);
                var intermediateValue = (JObject)JsonConvert.DeserializeObject(initialJson, settings);
                NullObjectsAtDepth(intermediateValue, maxDepth.Value);
                return JsonConvert.SerializeObject(intermediateValue, formatting, settings);
            }
            return JsonConvert.SerializeObject(value, formatting, settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static dynamic Deserialize(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        public static dynamic Deserialize(string json)
        {
            return JObject.Parse(json);
        }

        public static IEnumerable<KeyValuePair<string, string>> FlattenObject(object value, string prefix = Empty)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>();
            var token = JToken.FromObject(value);
            FillDictionaryFromJToken(keyValuePairs, token, prefix);
            return keyValuePairs;
        }

        private static void FillDictionaryFromJToken(List<KeyValuePair<string, string>> keyValuePairs, JToken token, string prefix)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (JProperty property in token.Children<JProperty>())
                    {
                        FillDictionaryFromJToken(keyValuePairs, property.Value, Join(prefix, property.Name));
                    }
                    break;
                case JTokenType.Array:
                    // this will add array items with same key and different values
                    foreach (JToken value in token.Children())
                    {
                        FillDictionaryFromJToken(keyValuePairs, value, prefix);
                    }
                    break;
                default:
                    keyValuePairs.Add(KeyValuePair.Create(prefix, ((JValue)token).Value?.ToString()));
                    break;
            }
        }

        private static string Join(string prefix, string name)
        {
            return string.IsNullOrEmpty(prefix)
                ? name
                : $"{prefix}.{name}";
        }

        private static JsonSerializerSettings GetSettings(bool camelCase)
        {
            var result = new JsonSerializerSettings
            {
                Converters = new JsonConverter[]
                {
                    new StringEnumConverter(),
                },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                NullValueHandling = NullValueHandling.Ignore
            };
            if (camelCase)
            {
                result.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            return result;
        }

        private static void NullObjectsAtDepth(JObject value, int depth)
        {
            if (value == null)
            {
                return;
            }
            if (depth < 1)
            {
                throw new InvalidOperationException();
            }
            var objectJsonTypes = new[]
            {
                JTokenType.Object,
                //JTokenType.Array,
                //JTokenType.Bytes
            };
            var properties = value.Properties()
                .Where(property => objectJsonTypes.Contains(property.Value.Type))
                .ToList();
            if (depth == 1)
            {
                properties.ForEach(property => value.Remove(property.Name));
                return;
            }
            properties.ForEach(property => NullObjectsAtDepth((JObject)property.Value, depth - 1));

           
        }
    }
}
