
namespace Server.Helpers
{
    public class TpMailConfigOptions
    {
        public TpMailConfigOptions()
        {
            // Set default value.
        }
        public string Option1 { get; set; }
        public string Host {get;set;}
        public string Port {get;set;}
        public string User {get;set;}
        public string Password {get;set;}
        public string From {get;set;}
    }
}