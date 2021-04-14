namespace BlogLab.Service
{
    public class Jwt
    {
        public const string TokenOptions = "TokenOptions";
        public string Key { get; set; }
        public string Issuer { get; set; }
    }
}