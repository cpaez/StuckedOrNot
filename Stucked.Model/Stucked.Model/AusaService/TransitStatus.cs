using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace Stucked.Model
{
    public class TransitStatus
    {
        public TransitStatus()
        { }

        public TransitStatus(string key, string value)
        {
            this.Key = key;
            this.Value = value;

            this.SetType();
        }

        public string Key { get; set; }
        public string Value { get; set; }

        private TransitStatusTypes type;

        [JsonConverter(typeof(StringEnumConverter))]
        public TransitStatusTypes Type
        {
            get { return type; }
            set { type = value; }
        }

        public void SetType()
        {
            if (this.Key != null)
            {
                if (this.Key.StartsWith("VPM"))
                {
                    type = TransitStatusTypes.MeasurePoints;
                }
                else if (this.Key.StartsWith("CPM"))
                {
                    type = TransitStatusTypes.SignInformation;
                }
                else
                {
                    type = TransitStatusTypes.None;
                }
            }
            else
            {
                type = TransitStatusTypes.None;
            }
        }
    }

    public enum TransitStatusTypes
    {
        [Description("Measure Points")]
        MeasurePoints = 0,
        [Description("Sign Information")]
        SignInformation,
        [Description("None")]
        None
    }
}
