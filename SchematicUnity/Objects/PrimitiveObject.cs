using System;
using UnityEngine;

namespace SchematicUnity
{
    public class PrimitiveObject
    {
        public PrimitiveType PrimitiveType { get; set; }
        public Color32 Color { get; set; }
        public Vector Position { get; set; }
        public Vector Rotation { get; set; }
        public Vector Scale { get; set; }
    }
}
