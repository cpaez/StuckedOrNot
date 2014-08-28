namespace Stucked.Model
{
    public class SignInformation
    {
        public SignInformation()
        { }

        public SignInformation(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
