using System;


[Serializable]
public class LobbyPlayer
{
    private string playerName;
    private bool isReady;

    public string PlayerName => playerName;
    public bool IsReady => isReady;

    public void SetPlayerLobbyData(LobbyPlayerData playerData)
    {
        playerName = playerData.PlayerName;
        isReady = playerData.IsReady;
    }
}
