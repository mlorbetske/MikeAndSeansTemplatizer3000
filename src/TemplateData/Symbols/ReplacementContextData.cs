namespace TemplateData.Symbols
{
    public class ReplacementContextData
    {
        public ReplacementContextData(string before, string after)
        {
            OnlyIfBefore = before;
            OnlyIfAfter = after;
        }

        public string OnlyIfBefore { get; set; }

        public string OnlyIfAfter { get; set; }
    }
}
