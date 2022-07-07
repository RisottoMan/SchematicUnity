using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchematicUnity
{
    public class Schematic
    {
        public List<PrimitiveObject> Primitives { get; set; } = new List<PrimitiveObject>();
        public List<Lights> LightSources { get; set; } = new List<Lights>();
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Workstation> WorkStations { get; set; } = new List<Workstation>();
        public List<Schematic> Childrens { get; set; } = new List<Schematic>();
    }
}
