using System.ComponentModel.Composition;
using System.Text;
using Newtonsoft.Json;

namespace Framework.Utilities.Helpers
{
    /// <summary>
    /// Serializes/deserializes an object of specified type to/from JSON string.
    /// </summary>
    [Export(typeof(IJsonHelper))]
    public class JsonHelper : IJsonHelper
    {
        /// <summary>Converts this object to a JSON string</summary>
        /// <typeparam name="T">The type of data to convert.</typeparam>
        /// <param name="data">The data to convert.</param>
        /// <returns>The JSON representation of this object</returns>
        public string ToJson<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>Returns the DomainSerializationObject that the JSON string represents</summary>
        /// <typeparam name="T">The type of data to convert.</typeparam>
        /// <param name="json">The JSON string representing a DomainSerializationObject</param>
        /// <returns>The DomainSerializationObject represented in the JSON string</returns>
        public T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public T FromJson<T>(string json, params JsonConverter[] converters)
        {
            return JsonConvert.DeserializeObject<T>(json, converters);
        }

        /// <summary>Converts this object to a byte array</summary>
        /// <typeparam name="T">The type of data to convert.</typeparam>
        /// <param name="data">The data to convert.</param>
        /// <returns>The byte array representation of this object</returns>
        public byte[] ToByteArray<T>(T data)
        {
            return Encoding.UTF8.GetBytes(ToJson(data));
        }

        /// <summary>Returns the DomainSerializationObject that the byte array represents</summary>
        /// <typeparam name="T">The type of data to convert.</typeparam>
        /// <param name="input">The byte array representing a DomainSerializationObject</param>
        /// <returns>The DomainSerializationObject represented in the byte array</returns>
        public T FromByteArray<T>(byte[] input)
        {
            return FromJson<T>(Encoding.UTF8.GetString(input));
        }
    }

    public interface IJsonHelper
    {
        /// <summary>Converts this object to a JSON string</summary>
        /// <typeparam name="T">The type of data to convert.</typeparam>
        /// <param name="data">The data to convert.</param>
        /// <returns>The JSON representation of this object</returns>
        string ToJson<T>(T data);

        /// <summary>Returns the DomainSerializationObject that the JSON string represents</summary>
        /// <typeparam name="T">The type of data to convert.</typeparam>
        /// <param name="json">The JSON string representing a DomainSerializationObject</param>
        /// <returns>The DomainSerializationObject represented in the JSON string</returns>
        T FromJson<T>(string json);

        /// <summary>Converts this object to a byte array</summary>
        /// <typeparam name="T">The type of data to convert.</typeparam>
        /// <param name="data">The data to convert.</param>
        /// <returns>The byte array representation of this object</returns>
        byte[] ToByteArray<T>(T data);

        /// <summary>Returns the DomainSerializationObject that the byte array represents</summary>
        /// <typeparam name="T">The type of data to convert.</typeparam>
        /// <param name="input">The byte array representing a DomainSerializationObject</param>
        /// <returns>The DomainSerializationObject represented in the byte array</returns>
        T FromByteArray<T>(byte[] input);

        T FromJson<T>(string json, params JsonConverter[] converters);
    }
}
