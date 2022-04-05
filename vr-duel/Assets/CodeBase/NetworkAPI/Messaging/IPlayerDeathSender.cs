using System.Threading.Tasks;

namespace CodeBase.NetworkAPI.Messaging
{
    public interface IPlayerDeathSender
    {
        Task SendDeathState(string matchId, string userId);
    }
}