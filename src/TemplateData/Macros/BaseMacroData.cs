namespace TemplateData.Macros
{
    public abstract class BaseMacroData
    {
        public abstract string Type { get; }

        public string VariableName { get; set; }

        public string DataType { get; set; }
    }
}
