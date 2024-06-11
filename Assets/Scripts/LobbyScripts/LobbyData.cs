using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyData
{
    public string RelayServerCode { get; set; }

    public void Initialize(string relayServerCode)
    {
        RelayServerCode = relayServerCode;
    }

    public void Initialize(Dictionary<string, DataObject> lobbyData)
    {
        UpdateState(lobbyData);
    }

    public void UpdateState(Dictionary<string, DataObject> lobbyData)
    {
        if (lobbyData.ContainsKey("RelayServerCode"))
        {
            RelayServerCode = lobbyData["RelayServerCode"].Value;
        }
    }
    public Dictionary<string, string> Serialize()
    {
        return new Dictionary<string, string>
        {
            {"RelayServerCode", RelayServerCode}
        };
    }
}
