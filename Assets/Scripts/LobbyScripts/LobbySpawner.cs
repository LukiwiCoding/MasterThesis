using System.Collections.Generic;
using UnityEngine;

public class LobbySpawner : MonoBehaviour
{
    [SerializeField] private List<LobbyPlayer> players;

    private void Awake()
    {
        players.Add(new());
        players.Add(new());
    }
    private void OnEnable()
    {
        LobbyEvents.OnClientUpdated += ConnectionsUpdated;
    }

    private void OnDisable()
    {
        LobbyEvents.OnClientUpdated -= ConnectionsUpdated;
    }
    private void ConnectionsUpdated()
    {

        List<LobbyPlayerData> playerData = GameLobbyManager.Instance.GetPlayers();

        
        for(int i = 0; i < playerData.Count; i++)
        {
            players[i].SetPlayerLobbyData(playerData[i]);
        }

        LobbyEvents.OnUIUpdated?.Invoke(players);
    }
}
