using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : Singleton<LobbyUIManager>
{
    private Color RED = Color.red;
    private Color GREEN = new(0, 1, 0.02592254f);

    [Serializable]
    private struct PlayerLobbyUI
    {
        public TextMeshProUGUI PlayerName;
        public TextMeshProUGUI IsReadyState;
    }

    [SerializeField] private GameObject playerContainer;
    [SerializeField] private TextMeshProUGUI lobbyCode;
    [SerializeField] private PlayerLobbyUI[] playerLobbyUIs = new PlayerLobbyUI[2];
    [SerializeField] private Button leave;
    [SerializeField] private Button readyUp;
    [SerializeField] private Button startGame;

    private void Awake()
    {
        List<RectTransform> playerRectTransforms = new();
        foreach(RectTransform t in playerContainer.transform)
        {
            playerRectTransforms.Add(t);
        }

        playerLobbyUIs[0].PlayerName = playerRectTransforms[0].GetChild(0).GetComponent<TextMeshProUGUI>();
        playerLobbyUIs[0].IsReadyState = playerRectTransforms[0].GetChild(1).GetComponent<TextMeshProUGUI>();

        playerLobbyUIs[1].PlayerName = playerRectTransforms[1].GetChild(0).GetComponent<TextMeshProUGUI>();
        playerLobbyUIs[1].IsReadyState = playerRectTransforms[1].GetChild(1).GetComponent<TextMeshProUGUI>();

        lobbyCode.text = $"Lobby Code: {LobbyManager.Instance.GetLobbyCode()}";

        if (!GameLobbyManager.Instance.IsHost) startGame.gameObject.SetActive(false);
    }

    public void UpdatePlayerUI(List<LobbyPlayer> playerData)
    {
        bool startGame = true;
        for(int i =  0; i < playerData.Count; i++)
        {
            
            playerLobbyUIs[i].PlayerName.text = playerData[i].PlayerName != null ? playerData[i].PlayerName : "";
            playerLobbyUIs[i].IsReadyState.text = ConvertPlayerStateToString(playerData[i].IsReady);

            if (playerData[i] != null)
            {
                startGame &= playerData[i].IsReady;
            }
            else 
            { 
                startGame = false; 
            }  
        }

        this.startGame.enabled = startGame;

        UpdatePlayerReadyState();
    }

    private string ConvertPlayerStateToString(bool state)
    {
        string readyState = "Not Ready";
        if (state) readyState = "Ready";

        return readyState;
    }
    
    private void UpdatePlayerReadyState()
    {
        foreach(PlayerLobbyUI pui in playerLobbyUIs)
        {
            pui.IsReadyState.color = pui.IsReadyState.text.Equals("Ready") ? GREEN : RED;
        }
    }

    private async void OnReadyUp()
    {
        await GameLobbyManager.Instance.TogglePlayerReadyState();
    }

    private void OnLeave() => LobbyManager.Instance.DeleteLobby();

    private void LobbyReady() => startGame.enabled = true;

    private async void OnStartGame()
    {
        await GameLobbyManager.Instance.StartGame();
    }

    private void OnEnable()
    {
        if (GameLobbyManager.Instance.IsHost)
        {
            LobbyEvents.OnLobbyReady += LobbyReady;
            startGame.onClick.AddListener(OnStartGame);
        }

        startGame.enabled = false;
        LobbyEvents.OnUIUpdated += UpdatePlayerUI;
        leave.onClick.AddListener(OnLeave);
        readyUp.onClick.AddListener(OnReadyUp);

    }

    private void OnDisable()
    {
        LobbyEvents.OnUIUpdated -= UpdatePlayerUI;
        LobbyEvents.OnLobbyReady -= LobbyReady;
        leave.onClick.RemoveListener(OnLeave);
        readyUp.onClick.RemoveListener(OnReadyUp);
        startGame.onClick.RemoveListener(OnStartGame);
    }
}
