using CodeBase.NetworkAPI.Messaging;
using CodeBase.ServiceLocator;

namespace CodeBase.NakamaClient.Messaging
{
    public interface INetworkMessaging : IService, IPlayerDeathReceiver, IPlayerDeathSender, IPlayerInputReceiver, IPlayerInputSender, IPlayerPositionReceiver, IPlayerPositionSender, IPlayerPresencesReceiver
    {
        
    }
}