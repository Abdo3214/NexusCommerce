namespace NexusCommerce.Common.GeneralResult
{
    public class Errors
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public Errors() { }

        public Errors(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public static Dictionary<string, List<Errors>> CreateSingle(string key, string code, string message)
        {
            return new Dictionary<string, List<Errors>>
            {
                { key, new List<Errors> { new Errors(code, message) } }
            };
        }
    }
}
