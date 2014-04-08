using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stucked.Model
{
    public class Sign
    {
        public Sign()
        {
            this.Screens = new List<Screen>();
        }

        public string Name { get; set; }
        public IList<Screen> Screens { get; set; }
    }

    public class Screen
    {
        public Screen()
        {
            this.Messages = new List<string>();
        }

        public int Number { get; set; }
        public int Icon { get; set; }
        public List<string> Messages { get; set; }
    }
}
