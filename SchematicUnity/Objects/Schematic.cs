using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchematicUnity
{
    public class Schematic
    {
        public List<PrimitiveObject> Primitives { get; set; }
        public List<Lights> LightSources { get; set; }
        public List<Item> Items { get; set; }
        public List<Workstation> WorkStations { get; set; }
        public List<Schematic> Childrens { get; set; }
    }
}
