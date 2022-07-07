using MEC;
using Qurre;
using Qurre.API.Events;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Qurre.API.Addons.Models;

namespace SchematicUnity
{
    public class SchematicUnity : Plugin
    {
        public override string Developer => "KoT0XleB#4663";
        public override Version Version => new Version(1, 1, 0); 
        public override string Name => "SchematicUnity";
        public override void Enable()
        {
            Qurre.Events.Server.SendingRA += SendRA;
        }
        public override void Disable()
        {
            Qurre.Events.Server.SendingRA -= SendRA;
        }
        public void SendRA(SendingRAEvent ev)
        {
            if (ev.Name == "schematic")
            {
                if (ev.Args[1] != string.Empty)
                {
                    Load(ev.Args[1], ev.Player.Position, out Model Model);
                    ev.ReplyMessage = "<color=green>Success</color>";
                    ev.Success = true;
                }
                else
                {
                    ev.ReplyMessage = "<color=red>Error: Enter arguments!</color>";
                    ev.Success = false;
                }
            }
        }
        public void Load(string path, Vector3 pos, out Model model)
        {
            var Model = new Model(path, pos);
            model = Model;

            if (File.Exists(path))
            {
                var json = JsonConvert.DeserializeObject<Schematic>(File.ReadAllText(path));
                LoadFromJson(json, Model, Model.GameObject.transform);
                Log.Info($"File {path} found!");
            }
            else Log.Info($"File {path} not found!");
        }
        public void LoadFromJson(Schematic json, Model Model, Transform transform)
        {
            foreach (var prim in json.Primitives)
            {
                Model.AddPart(new ModelPrimitive(Model, prim.PrimitiveType, prim.Color, JsonVector(prim.Position), JsonVector(prim.Rotation), JsonVector(prim.Scale)));
            }
            foreach (var light in json.LightSources)
            {
                Model.AddPart(new ModelLight(Model, light.Color, JsonVector(light.Position), light.Intensity, light.Range, false));
            }
            foreach (var item in json.Items)
            {
                Model.AddPart(new ModelPickup(Model, item.ItemType, JsonVector(item.Position), JsonVector(item.Rotation), JsonVector(item.Scale), true));
            }
            foreach (var work in json.WorkStations)
            {
                Model.AddPart(new ModelWorkStation(Model, JsonVector(work.Position), JsonVector(work.Rotation), JsonVector(work.Scale)));
            }
        }
        public Vector3 JsonVector(Vector vector) => new Vector3() { x = vector.x, y = vector.y, z = vector.z };
    }
}
