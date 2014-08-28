using System.Collections.Generic;

namespace Stucked.Model
{
    public class RawSignInformation
    {
        public RawSignInformation()
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
