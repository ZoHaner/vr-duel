using CodeBase.Entities;

namespace CodeBase.Services
{
    public class PlayerDataService : IPlayerDataService
    {
        public string Username { get; set; }
        public UserAccount User { get; set; }
    }
}