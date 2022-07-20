namespace SchematicUnity.API.Objects
{
    using System;
    using System.Linq;
    using UnityEngine;

    public sealed class Scheme : IEquatable<Scheme>
    {
        internal Scheme(uint id, SchematicJSON json, string path = null, Vector3 postion = default, Quaternion rotation = default)
        {
            Path = path;
            Guid = Guid.NewGuid();
            ID = id;
            Transform = new GameObject($"Scheme \"{Guid}\" ({ID})").transform;
            Objects = json.Objects.Select(obj => new SObject(obj, Transform)).ToArray();

            Transform.position = postion;
            Transform.rotation = rotation;
        }

        public void Unload()
        {
            SchematicManager.UnloadSchematic(ID);
        }

        public bool Equals(Scheme other) => other == this;
        public override bool Equals(object obj) => obj is SObject sobj && sobj.Equals(this);
        public override int GetHashCode() => Guid.GetHashCode();

        public static bool operator ==(Scheme a, Scheme b) => (a as object is not null) && a?.Guid == b?.Guid;
        public static bool operator !=(Scheme a, Scheme b) => !(a == b);

        public uint ID { get; private set; }
        public SObject[] Objects { get; }
        internal Transform Transform { get; }

        public readonly string Path;
        public readonly Guid Guid;
    }
}