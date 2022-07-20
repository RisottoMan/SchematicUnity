namespace SchematicUnity.API.Objects
{
    using UnityEngine;
    using Light = Qurre.API.Controllers.Light;

    public struct LightParams
    {
        private const bool DefaultIsShadowsEnabledState = true;

        internal LightParams(SObject parent, LightJSON json, bool shadows = DefaultIsShadowsEnabledState)
        {
            Parent = parent;
            if (!ColorUtility.TryParseHtmlString(json.Color, out var color)) color = Color.white;
            Base = new Light(parent.Transform.position, color, json.Intensity, json.Intensity, shadows);
            Base.Base.transform.parent = parent.Transform;
        }

        public Color Color
        {
            get => Base.Color;
            set => Base.Color = value;
        }
        public float Intensivity
        {
            get => Base.Intensivity;
            set => Base.Intensivity = value;
        }
        public float Range
        {
            get => Base.Range;
            set => Base.Range = value;
        }
        public bool Shadows
        {
            get => Base.EnableShadows;
            set => Base.EnableShadows = value;
        }

        public SObject Parent { get; }
        internal readonly Light Base;
    }
}