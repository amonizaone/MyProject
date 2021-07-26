using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Core.Helpers
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Deep copy (requires Json.Net component)
        /// </summary>
        /// <typeparam name="T">Copy object category</typeparam>
        /// <param name="source">copy object</param>
        /// <returns>replica</returns>
        public static T DeepCloneViaJson<T>(this T source)
        {

            if (source != null)
            {
                // avoid self reference loop issue
                // track object references when serializing and deserializing JSON
                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    TypeNameHandling = TypeNameHandling.Auto
                };

                var serializedObj = JsonConvert.SerializeObject(source, Formatting.Indented, jsonSerializerSettings);
                return JsonConvert.DeserializeObject<T>(serializedObj, jsonSerializerSettings);
            }
            else
            { return default(T); }

        }

        // <summary>
        /// Deep copy (the copied object must be serializable)
        /// </summary>
        /// <typeparam name="T">Copy object category</typeparam>
        /// <param name="source">copy object</param>
        /// <returns>replica</returns>
        public static T DeepClone<T>(this T source)
        {

            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (source != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, source);
                    stream.Seek(0, SeekOrigin.Begin);
                    T clonedSource = (T)formatter.Deserialize(stream);
                    return clonedSource;
                }
            }
            else
            { return default(T); }

        }

    }
}
