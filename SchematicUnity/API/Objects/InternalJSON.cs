namespace SchematicUnity.API.Objects
{
    using UnityEngine;
    using Newtonsoft.Json;

    internal class SchematicJSON
    {
        [JsonProperty("objects")]
        public ObjectJSON[] Objects { get; set; }
    }

    internal class Vector3JSON
    {
        [JsonProperty("x")]
        public float X { get; set; }

        [JsonProperty("y")]
        public float Y { get; set; }

        [JsonProperty("z")]
        public float Z { get; set; }

        public static implicit operator Vector3(Vector3JSON value) => new Vector3()
        {
            x = value.X,
            y = value.Y,
            z = value.Z
        };
    }

    internal class ObjectJSON
    {
        [JsonProperty("position")]
        public Vector3JSON Position { get; set; }

        [JsonProperty("primitive")]
        public PrimitiveJSON Primitive { get; set; }

        [JsonProperty("light")]
        public LightJSON Light { get; set; }

        [JsonProperty("childrens")]
        public ObjectJSON[] Childrens { get; set; }
    }

    internal class PrimitiveJSON
    {
        [JsonProperty("rotation")]
        public Vector3JSON Rotation { get; set; }

        [JsonProperty("scale")]
        public Vector3JSON Scale { get; set; }

        [JsonProperty("primitiveType")]
        public PrimitiveType PrimitiveType { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }

    internal class LightJSON
    {
        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("intensivity")]
        public float Intensity { get; set; }

        [JsonProperty("range")]
        public float Range { get; set; }
    }
}