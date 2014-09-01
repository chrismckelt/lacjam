namespace Lacjam.Core.Infrastructure.Settings
{
    public class Setting : TrackingBase
    {
        public Setting() { }

        public Setting(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public virtual string Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}