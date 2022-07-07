using System;

namespace SchematicUnity
{
    public class Item
    {
        public ItemType ItemType { get; set; }
        public Vector Position { get; set; }
        public Vector Rotation { get; set; }
        public Vector Scale { get; set; }
    }
}
