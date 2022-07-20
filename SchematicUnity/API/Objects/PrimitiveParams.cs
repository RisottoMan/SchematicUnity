namespace SchematicUnity.API.Objects
{
    using UnityEngine;
    using Primitive = Qurre.API.Controllers.Primitive;

    public struct PrimitiveParams
    {
        internal PrimitiveParams(SObject parent, PrimitiveJSON json)
        {
            Parent = parent;
            if (!ColorUtility.TryParseHtmlString(json.Color, out var color)) color = Color.white;
            Base = new Primitive(json.PrimitiveType, parent.Transform.position, color, Quaternion.Euler(json.Rotation), json.Scale, true, false);
            Base.Base.transform.parent = parent.Transform;
        }

        public Vector3 Position
        {
            get => Base.Position;
            set => Parent.Position = value;
        }
        public Quaternion Rotation
        {
            get => Base.Rotation;
            set => Base.Rotation = value;
        }
        public Vector3 Scale
        {
            get => Base.Scale;
            set => Base.Scale = value;
        }
        public PrimitiveType Type
        {
            get => Base.Type;
            set => Base.Type = value;
        }
        public Color Color
        {
            get => Base.Color;
            set => Base.Color = value;
        }

        public SObject Parent { get; }
        internal readonly Primitive Base;
    }
}