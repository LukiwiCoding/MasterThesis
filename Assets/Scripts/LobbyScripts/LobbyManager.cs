using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
    private string relayServerCode;
    private Lobby lobby;
    private Coroutine lobbyCoroutine;
    private Coroutine refreshLobbyCoroutine;

    public string RelayServerCode
    {
        get => relayServerCode; set => relayServerCode = value;
    }
    public async Task<bool> CreateLobby(Dictionary<string, string> data)
    {
        Player player = new (AuthenticationService.Instance.PlayerId, null, SerializePlayerData(data));
        CreateLobbyOptions options = new()
        {
            IsPrivate = true,
            Player = player,
            Password = "1234567890"
        };

        try
        {
            lobby = await LobbyService.Instance.CreateLobbyAsync("DefaultLobbyName", 2, options);
        }
        catch(Exception)
        {
            return false;
        }

        lobbyCoroutine = StartCoroutine(LobbyCoroutine(lobby.Id, 6f));
        refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 1f));

        return true;
    }

    public async Task<bool> JoinLobby(string lobbyCode, Dictionary<string, string> data)
    {
        Player player = new(AuthenticationService.Instance.PlayerId, null, SerializePlayerData(data));
        JoinLobbyByCodeOptions options = new()
        {
            Password = "1234567890",
            Player = player
        };
        print("Joining Lobby");
        try
        {
            lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, options);
        }
        catch(Exception e) 
        {
            Debug.LogError(e.Message);
            return false; 
        }

        refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 1f));
        return true;

    }

    public List<Dictionary<string, PlayerDataObject>> GetPlayersData()
    {
        List<Dictionary<string, PlayerDataObject>> data = new();
        foreach(Player player in lobby.Players)
        {
            data.Add(player.Data);
        }
        return data;
    }

    public void OnApplicationQuit()
    {
        DeleteLobby();
    }

    public void DeleteLobby()
    {
        if (lobby != null && lobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
        }
    }
    public string GetLobbyCode() => lobby?.LobbyCode;

    public int GetMaxPlayerCount() => (int)(lobby?.MaxPlayers);

    public string GetHostId() => lobby.HostId;

    public async Task<bool> UpdatePlayerData(string id, Dictionary<string, string> data, string allocationId = default, string connectionData = default, string relayServerCode = default)
    {
        Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);
        UpdatePlayerOptions options = new()
        {
            Data = playerData,
            AllocationId = allocationId,
            ConnectionInfo = connectionData,
        };
        try
        {
            lobby = await LobbyService.Instance.UpdatePlayerAsync(lobby.Id, id, options);
        }
        catch
        {
            return false;
        }

        LobbyEvents.OnLobbyUpdated(lobby, relayServerCode);
        return true;
    }
    private IEnumerator RefreshLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        while(true)
        {
            Task<Lobby> task = LobbyService.Instance.GetLobbyAsync(lobbyId);
            yield return new WaitUntil(() => task.IsCompleted);
            Lobby newLobby = task.Result;
            if (newLobby.LastUpdated > lobby.LastUpdated)
            {
                lobby = newLobby;
                LobbyEvents.OnLobbyUpdated?.Invoke(newLobby);
            }
            yield return new WaitForSecondsRealtime(waitTimeSeconds);
        }
    }
    private IEnumerator LobbyCoroutine(string lobbyId, float waitTiemSeconds)
    {
        while(true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return new WaitForSecondsRealtime(waitTiemSeconds);
        }
    }
    private Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
    {
        Dictionary<string, PlayerDataObject> playerData = new();
        foreach(var(key, value) in data)
        {
            playerData.Add(key, new PlayerDataObject(
                visibility: PlayerDataObject.VisibilityOptions.Member,
                value: value )) ;
        }

        return playerData;
    }
}
