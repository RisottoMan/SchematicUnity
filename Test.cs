using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Object = UnityEngine.Object;

public class Test : MonoBehaviour
{
    public static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $@"\SchematicUnity.json";
    void Start()
    {
        if (!File.Exists(path))
        {
            FileStream file = File.Create(path);
            file.Close();
            Debug.Log($"Файл {file.Name} был создан!");
        }
        CheckObjects();
        CreateNewInfoJson();
    }
    public void CreateNewInfoJson()
    {
        string json = JsonConvert.SerializeObject(Schema, Formatting.Indented);
        StreamWriter jsonFile = File.CreateText(path);
        jsonFile.WriteLine(json);
        jsonFile.Close();
        Debug.Log($"В файл была записана информация!");
    }
    public void CheckObjects()
    {
        //var log = GameObject.Find("DontDeleteMe").transform.GetChild(0).GetChild(0).name;
        //Debug.Log(log);
        foreach (GameObject objects in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            PrimitiveType primitiveType = 0;
            if (objects.TryGetComponent(out Light light))
            {
                Schema.LightSources.Add(new Lights()
                {
                    Position = new Vector
                    {
                        x = objects.transform.localPosition.x,
                        y = objects.transform.localPosition.y,
                        z = objects.transform.localPosition.z
                    },
                    Color = light.color,
                    Intensity = (int)light.intensity,
                    Range = (int)light.range
                });
            }
            if (objects.TryGetComponent(out MeshFilter component))
            {
                switch (component.sharedMesh.name)
                {
                    case "Sphere": primitiveType = (PrimitiveType)0; break;
                    case "Capsule": primitiveType = (PrimitiveType)1; break;
                    case "Cylinder": primitiveType = (PrimitiveType)2; break;
                    case "Cube": primitiveType = (PrimitiveType)3; break;
                    case "Plane": primitiveType = (PrimitiveType)4; break;
                    case "Quad": primitiveType = (PrimitiveType)5; break;
                }
                objects.TryGetComponent(out Renderer render);
                Schema.Primitives.Add(new PrimitiveObject()
                {
                    PrimitiveType = primitiveType,
                    Color = render.material.color,
                    Position = new Vector
                    {
                        x = objects.transform.localPosition.x,
                        y = objects.transform.localPosition.y,
                        z = objects.transform.localPosition.z
                    },
                    Rotation = new Vector
                    {
                        x = objects.transform.localRotation.eulerAngles.x,
                        y = objects.transform.localRotation.eulerAngles.y,
                        z = objects.transform.localRotation.eulerAngles.z
                    },
                    Scale = new Vector
                    {
                        x = objects.transform.localScale.x,
                        y = objects.transform.localScale.y,
                        z = objects.transform.localScale.z
                    }
                });
                /*
                Schema.Add(new Schematic()
                {
                    Primitives = new List<PrimitiveObject>()
                    {
                        new PrimitiveObject()
                        {
                            PrimitiveType = primitiveType,
                            Color = render.material.color,
                            Position = new Vector
                            {
                                x = objects.transform.position.x,
                                y = objects.transform.position.y,
                                z = objects.transform.position.z
                            },
                            Rotation = new Vector
                            {
                                x = objects.transform.rotation.x,
                                y = objects.transform.rotation.y,
                                z = objects.transform.rotation.z
                            },
                            Scale = new Vector
                            {
                                x = objects.transform.localScale.x,
                                y = objects.transform.localScale.y,
                                z = objects.transform.localScale.z
                            },
                        }
                    }
                });
                */
            }
        }
    }
    //public List<Schematic> Schema { get; set; } = new List<Schematic>() { };
    public Schematic Schema { get; set; } = new Schematic() { };
    public class Schematic
    {
        public List<PrimitiveObject> Primitives { get; set; } = new List<PrimitiveObject>();
        public List<Lights> LightSources { get; set; } = new List<Lights>();
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Workstation> WorkStations { get; set; } = new List<Workstation>();
    }
    public class PrimitiveObject
    {
        public PrimitiveType PrimitiveType { get; set; }
        public Color32 Color { get; set; }
        public Vector Position { get; set; }
        public Vector Rotation { get; set; }
        public Vector Scale { get; set; }
    }
    public class Vector
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }
    public class Lights
    {
        public Vector Position { get; set; }
        public Color32 Color { get; set; }
        public int Intensity { get; set; }
        public int Range { get; set; }
    }
    public class Item { }
    public class Workstation { }
}

