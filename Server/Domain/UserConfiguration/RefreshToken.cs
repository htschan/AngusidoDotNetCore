using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.UserConfiguration
{
    public class RefreshToken
    {
        public string Id { get; set; }

        public string ClientId { get; set; } // This is the userId

        [NotMapped]
        public string ClientName { get; set; }

        public string Token { get; set; }
        
        public int IsStop { get; set; }

        public System.DateTime Created { get; set; }
    }
}