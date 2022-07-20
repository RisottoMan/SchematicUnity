namespace SchematicUnity.API.Objects
{
    using System;
    using System.Linq;
    using UnityEngine;

    public sealed class SObject : IEquatable<SObject>
    {
        internal SObject(ObjectJSON json, Transform parent)
        {
            Transform = new GameObject().transform;
            Transform.parent = parent;

            if (json.Primitive != null) Primitive = new PrimitiveParams(this, json.Primitive);
            if (json.Light != null) Light = new LightParams(this, json.Light);

            Position = json.Position;
            Childrens = json.Childrens.Select(child => new SObject(child, Transform)).ToArray();
        }

        public Vector3 Position
        {
            get => Transform.position;
            set
            {
                Transform.position = value;
                if (Primitive.HasValue) Primitive.Value.Base.Position = value;
                if (Light.HasValue) Light.Value.Base.Position = value;
            }
        }

        public PrimitiveParams? Primitive { get; }
        public LightParams? Light { get; }

        public SObject[] Childrens { get; }

        public bool Equals(SObject other) => other == this;
        public override bool Equals(object obj) => obj is SObject sobj && sobj.Equals(this);
        public override int GetHashCode() => Transform.GetHashCode();

        public static bool operator ==(SObject a, SObject b) => a.Transform == b.Transform;
        public static bool operator !=(SObject a, SObject b) => !(a == b);

        internal readonly Transform Transform;
    }
}