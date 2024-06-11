using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyPlayerData
{
    private string playerId;
    private string playerName;
    private string relayCode;
    private bool isReady;

    public string PlayerId => playerId;
    public string PlayerName => playerName;

    public string RelayCode => relayCode;
    public bool IsReady { 
        get => isReady; 
        set => isReady = value; 
    }

    public void InitLobbyPlayerData(string playerId, string playerName)
    {
        this.playerId = playerId;
        this.playerName = playerName;
        isReady = false;
    }

    public void InitLobbyPlayerData(Dictionary<string, PlayerDataObject> data)
    {
        UpdateLobbyPlayerData(data);
    }

    public void UpdateLobbyPlayerData(Dictionary<string, PlayerDataObject> data)
    {
        if (data.ContainsKey("Id")) playerId = data["Id"].Value;
        if (data.ContainsKey("PlayerName")) playerName = data["PlayerName"].Value;
        if (data.ContainsKey("IsReady")) isReady = data["IsReady"].Value == "True";        
    }

    public Dictionary<string, string> SerializeLobbyPlayerData()
    {
        return new()
        {
            { "Id", playerId },
            { "PlayerName", playerName },
            { "IsReady", isReady.ToString() }
        };
    }
}
