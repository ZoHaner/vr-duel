namespace CodeBase.Entities.Network
{
    public interface IClient // Connection Service
    {
        void Connect();
        void Disconnect();
    }
}

/*
 
 GameCycle , countdown, start, on death,

Round , Start end, on deeath, on round ended, on round

 
 *Client как работа с сетью в целом, соединение и разрыв

Matchmaking на уровне лобби, после окончания передается i matchmaking match в load game state

Match как начало и конец матча в целом на уровне игры. Получение изменения списка игроков и принятия решения
 * 
 */