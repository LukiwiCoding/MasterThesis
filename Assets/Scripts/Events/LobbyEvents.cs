using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

public static class LobbyEvents 
{
    public delegate void LobbyUpdated(Lobby lobby);
    public static LobbyUpdated OnLobbyUpdated;

    public delegate void ClientUpdated();
    public static ClientUpdated OnClientUpdated;

    public delegate void UIUpdated(List<LobbyPlayer> players);
    public static UIUpdated OnUIUpdated;

    public delegate void LobbyReady();
    public static LobbyReady OnLobbyReady;
}
