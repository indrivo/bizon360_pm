namespace Gear.Localizer
{
    public class Language
    {
        /// <summary>
        /// The identifier of the language that is
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Friendly name of the language used.
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Identifier ?? "Null identifier" } - {Name ?? "Null name"}";
        }

    }
}