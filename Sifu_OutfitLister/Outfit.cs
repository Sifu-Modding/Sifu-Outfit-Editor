using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sifu_OutfitLister
{
    class Outfit
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }

        public Outfit()
        {
            Name = "Undefined Name";
            Description = "Undefined Description";
            Path = "Undefined Path";
        }

        public Outfit(string Name, string Description, string Path)
        {
            this.Name = Name;
            this.Description = Description;
            this.Path = Path;
        }
    }
}
