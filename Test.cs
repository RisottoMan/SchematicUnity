using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class Test : MonoBehaviour
{
    private string PathToFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SchematicUnity.json");

    private void Start()
    {
        File.WriteAllText(PathToFile, JsonConvert.SerializeObject(SerializeSchematic(), Formatting.Indented));
        Debug.Log($"В файл была записана информация!");
    }

    // Сериализация схематики
    public SchematicJSON SerializeSchematic()
    {
        var result = new SchematicJSON();
        var objs = SceneManager.GetActiveScene().GetRootGameObjects();

        result.Objects = new ObjectJSON[objs.Length];
        for (int i = 0; i < objs.Length; i++)
            result.Objects[i] = SerializeObject(objs[i].transform);

        return result;
    }

    // Сериализация игрового объекта
    private ObjectJSON SerializeObject(Transform go)
    {
        var result = new ObjectJSON()
        {
            Position = go.localPosition
        };

        // Примитив
        if (go.TryGetComponent(out MeshFilter filter))
        {
            result.Primitive = new PrimitiveJSON()
            {
                Rotation = go.localRotation.eulerAngles,
                Scale = go.localScale,
                Color = go.TryGetComponent(out MeshRenderer render) ? render.material.color : Color.white,
                PrimitiveType = filter.sharedMesh.name switch
                {
                    "Capsule" => PrimitiveType.Capsule,
                    "Cylinder" => PrimitiveType.Cylinder,
                    "Cube" => PrimitiveType.Cube,
                    "Plane" => PrimitiveType.Plane,
                    "Quad" => PrimitiveType.Quad,
                    _ => PrimitiveType.Sphere
                }
            };
        }

        // Свет
        if (go.TryGetComponent(out Light light))
        {
            result.Light = new LightJSON()
            {
                Color = light.color,
                Intensity = light.intensity,
                Range = light.range
            };
        }

        // Дочерние объекты
        result.Childrens = new ObjectJSON[go.childCount];
        for (int i = 0; i < go.childCount; i++)
            result.Childrens[i] = SerializeObject(go.GetChild(i));

        return result;
    }

    #region JSONs
    public class SchematicJSON
    {
        [JsonProperty("objects")]
        public ObjectJSON[] Objects { get; set; }
    }

    public class Vector3JSON
    {
        [JsonProperty("x")]
        public float X { get; set; }

        [JsonProperty("y")]
        public float Y { get; set; }

        [JsonProperty("z")]
        public float Z { get; set; }

        public static implicit operator Vector3JSON(Vector3 value) => new Vector3JSON()
        {
            X = value.x,
            Y = value.y,
            Z = value.z
        };
    }

    public class ColorJSON
    {
        [JsonProperty("r")]
        public float R { get; set; }

        [JsonProperty("g")]
        public float G { get; set; }

        [JsonProperty("b")]
        public float B { get; set; }

        [JsonProperty("a")]
        public float A { get; set; }

        public static implicit operator ColorJSON(Color value) => new ColorJSON()
        {
            R = value.r,
            G = value.g,
            B = value.b,
            A = value.a
        };
    }

    public class ObjectJSON
    {
        [JsonProperty("position")]
        public Vector3JSON Position { get; set; }

        [JsonProperty("primitive")]
        public PrimitiveJSON Primitive { get; set; }

        [JsonProperty("light")]
        public LightJSON Light { get; set; }

        [JsonProperty("item")]
        public ItemJSON Item { get; set; }

        [JsonProperty("workstation")]
        public WorkstationJSON Workstation { get; set; }

        [JsonProperty("childrens")]
        public ObjectJSON[] Childrens { get; set; }
    }

    public class PrimitiveJSON
    {
        [JsonProperty("rotation")]
        public Vector3JSON Rotation { get; set; }

        [JsonProperty("scale")]
        public Vector3JSON Scale { get; set; }

        [JsonProperty("primitiveType")]
        public PrimitiveType PrimitiveType { get; set; }

        [JsonProperty("color")]
        public ColorJSON Color { get; set; }
    }

    public class LightJSON
    {
        [JsonProperty("color")]
        public ColorJSON Color { get; set; }

        [JsonProperty("intensivity")]
        public float Intensity { get; set; }

        [JsonProperty("range")]
        public float Range { get; set; }
    }

    public class ItemJSON { }
    public class WorkstationJSON { }
    #endregion
}