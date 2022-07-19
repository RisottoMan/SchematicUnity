namespace SchematicUnity.API
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Qurre;
    using API.Objects;
    using UnityEngine;

    public static class SchematicManager
    {
        static SchematicManager() => 
            _schemes = new Dictionary<uint, Scheme>();

        public static Scheme LoadSchematic(string filePath, Vector3 position = default, Quaternion rotation = default)
        {
            try
            {
                if (!File.Exists(filePath)) return null;
                Scheme scheme = new(Counter, JsonConvert.DeserializeObject<SchematicJSON>(File.ReadAllText(filePath)), filePath, position, rotation);

                if (_schemes.ContainsKey(Counter))
                    _schemes[Counter] = scheme;
                else
                    _schemes.Add(Counter, scheme);

                Counter++;
                return scheme;
            }
            catch (Exception ex)
            {
                Log.Error($"An unexpected exception occurred while loading schematic file at path \"{filePath}\":\n{ex}");
                return null;
            }
        }
        public static Scheme LoadSchematic(string filePath) => LoadSchematic(filePath, default, default);

        public static void UnloadSchematic(Scheme scheme) => UnloadSchematic(scheme.ID);
        public static void UnloadSchematic(uint identity)
        {
            static void ProcessObject(SObject obj)
            {
                if (obj.Primitive.HasValue) obj.Primitive.Value.Base.Destroy();
                if (obj.Light.HasValue) obj.Light.Value.Base.Destroy();

                UnityEngine.Object.DestroyImmediate(obj.Transform.gameObject);
                obj.Childrens.ToList().ForEach(child => ProcessObject(child));
            }

            if (_schemes is null || !_schemes.ContainsKey(identity)) return;
            var scheme = _schemes[identity];

            _schemes.Remove(identity);
            scheme.Objects.ToList().ForEach(child => ProcessObject(child));
        }

        public static IReadOnlyDictionary<uint, Scheme> LoadedSchemes => _schemes;
        private static readonly Dictionary<uint, Scheme> _schemes;
        private static uint Counter;
    }
}