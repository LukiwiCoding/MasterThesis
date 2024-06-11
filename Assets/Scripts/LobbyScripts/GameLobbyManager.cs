using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class GameLobbyManager : Singleton<GameLobbyManager>
{
    private List<LobbyPlayerData> lobbyPlayerDatas = new();
    private LobbyPlayerData localPlayerData;
    private LobbyData lobbyData;
    private bool inGame = false;

    public bool IsHost => localPlayerData.PlayerId == LobbyManager.Instance.GetHostId();
    public async Task<bool> CreateLobby()
    {
        localPlayerData = new();
        localPlayerData.InitLobbyPlayerData(AuthenticationService.Instance.PlayerId, GetPlayerNameOS());
        lobbyData = new LobbyData();
        lobbyData.Initialize("");
        return await LobbyManager.Instance.CreateLobby(localPlayerData.SerializeLobbyPlayerData(), lobbyData.Serialize());       
    }

    public async Task<bool> JoinLobby(string lobbyCode)
    {
        localPlayerData = new();
        localPlayerData.InitLobbyPlayerData(AuthenticationService.Instance.PlayerId, GetPlayerNameOS());
        return await LobbyManager.Instance.JoinLobby(lobbyCode, localPlayerData.SerializeLobbyPlayerData());
    }

    public string GetLobbyCode() => LobbyManager.Instance.GetLobbyCode();
   
    public List<LobbyPlayerData> GetPlayers() => lobbyPlayerDatas;

    public async Task<bool> TogglePlayerReadyState()
    {
        localPlayerData.IsReady = !localPlayerData.IsReady;
        return await LobbyManager.Instance.UpdatePlayerData(localPlayerData.PlayerId, localPlayerData.SerializeLobbyPlayerData());
    }

    public async Task StartGame()
    {
        string relayServerCode = await RelayManager.Instance.CreateRelay(LobbyManager.Instance.GetMaxPlayerCount());
        inGame = true;
        LobbyManager.Instance.RelayServerCode = relayServerCode;
        string allocationId = RelayManager.Instance.AllocationId;
        string connectionData = RelayManager.Instance.ConnectionData;

        await LobbyManager.Instance.UpdatePlayerData(localPlayerData.PlayerId, localPlayerData.SerializeLobbyPlayerData(), allocationId, connectionData);
        SceneManager.LoadSceneAsync("Online");
    }

    private async void OnLobbyUpdated(Lobby lobby)
    {
        List<Dictionary<string, PlayerDataObject>> data = LobbyManager.Instance.GetPlayersData();
        lobbyPlayerDatas.Clear();

        int readyPlayers = 0;
        foreach(Dictionary<string, PlayerDataObject> playerData in data)
        {
            LobbyPlayerData lobbyPlayerData = new();
            lobbyPlayerData.InitLobbyPlayerData(playerData);

            if (lobbyPlayerData.IsReady) readyPlayers++;
            if (lobbyPlayerData.PlayerId == AuthenticationService.Instance.PlayerId) localPlayerData = lobbyPlayerData;

            lobbyPlayerDatas.Add(lobbyPlayerData);
        }

        LobbyEvents.OnClientUpdated?.Invoke();

        //if(readyPlayers == lobby.MaxPlayers) LobbyEvents.OnLobbyReady?.Invoke();
        if(readyPlayers > 0) LobbyEvents.OnLobbyReady?.Invoke();

        print($"Updated Lobby Data, RelayServerCode: {LobbyManager.Instance.RelayServerCode}, IsInGame: {inGame}\nJoin Condition: {LobbyManager.Instance.RelayServerCode != default && !inGame}");
        if(LobbyManager.Instance.RelayServerCode != default && !inGame)
        {
            print("Joining Relay");
            await JoinRelayServer(LobbyManager.Instance.RelayServerCode);
            SceneManager.LoadSceneAsync("Online");
        }
    }

    private async Task<bool> JoinRelayServer(string  relayServerCode)
    {
        inGame = true;
        await RelayManager.Instance.JoinRelay(relayServerCode);

        string allocationId = RelayManager.Instance.AllocationId;
        string connectionData = RelayManager.Instance.ConnectionData;

        await LobbyManager.Instance.UpdatePlayerData(localPlayerData.PlayerId, localPlayerData.SerializeLobbyPlayerData(), allocationId, connectionData);
        return true;
    }
    private string GetPlayerNameOS()
    {
        return Environment.UserName;
    }

    private void OnEnable()
    {
        LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
    }

   
    private void OnDisable()
    {
        LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
    }
}
