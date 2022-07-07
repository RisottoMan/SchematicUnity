using MEC;
using Mirror;
using Qurre;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Qurre.API.Addons.Models;

namespace SchematicUnity
{
    public class SchematicUnity : Plugin
    {
        public override string Developer => "KoT0XleB#4663";
        public override string Name => "SchematicUnity";
        public override void Enable()
        {
            Qurre.Events.Round.Waiting += OnWaiting;
            Qurre.Events.Server.SendingRA += SendRA;
        }
        public override void Disable()
        {
            Qurre.Events.Round.Waiting -= OnWaiting;
            Qurre.Events.Server.SendingRA -= SendRA;
        }
        public void SendRA(SendingRAEvent ev)
        {
            if (ev.Name == "schematic") // sc Json.json
            {
                if (ev.Args[1] != string.Empty)
                {
                    Load(ev.Args[1], ev.Player.Position, out Model Model);
                    ev.ReplyMessage = "<color=green>Успешно</color>";
                    ev.Success = true;
                }
                else
                {
                    ev.ReplyMessage = "<color=green>Ошибка: Введите аргументы!</color>";
                    ev.Success = false;
                }
            }
        }
        public static void Load(string path, Vector3 pos, out Model model)
        {
            string fileName = path;
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), path);

            var Model = new Model(fileName, pos);
            model = Model;

            if (File.Exists(path))
            {
                Log.Info($"Файл {fileName} найден!");
                var json = JsonConvert.DeserializeObject<Schematic>(File.ReadAllText(path));

                // Загрузка из Json всех примитивов и света
                foreach (var prim in json.Primitives)
                {
                    Model.AddPart(new ModelPrimitive(Model, prim.PrimitiveType, prim.Color, GetJsonVector(prim.Position), GetJsonVector(prim.Rotation), GetJsonVector(prim.Scale)));
                }
                foreach (var light in json.LightSources)
                {
                    Model.AddPart(new ModelLight(Model, light.Color, GetJsonVector(light.Position), light.Intensity, light.Range, false));
                }

                // убирает сеть, чтобы не нагружало
                // Timing.CallDelayed(1f, () => Model.Primitives.ForEach(prim => { try { prim.Primitive.Break(); } catch { } }));
            }
            else Log.Info($"Файл {fileName} не найден!");
        }
        public void OnWaiting()
        {
            //var path = $@"SchematicUnity.json";
            //Load(path, new Vector3(145.18f, 945.26f, -122.97f), out Model model);
        }
        public static Vector3 GetJsonVector(Vector vector)
        {
            return new Vector3() { x = vector.x, y = vector.y, z = vector.z };
        }
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
}
