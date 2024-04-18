using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [Header("MainMenuComponents")]
    [SerializeField] private GameObject mainmenu;
    [SerializeField] private Button host;
    [SerializeField] private Button openjoinmenu;
    [SerializeField] private Button quit;

    [Header("JoinMenuComponents")]
    [SerializeField] private GameObject joinmenu;
    [SerializeField] private Button join;
    [SerializeField] private Button back;
    [SerializeField] private TMP_InputField lobbyCodeInput;

    private void OnEnable()
    {
        host.onClick.AddListener(async() => {
            if(await GameLobbyManager.Instance.CreateLobby())
            {
                SceneManager.LoadSceneAsync("Lobby");
            }
        });

        openjoinmenu.onClick.AddListener(() => {
            mainmenu.SetActive(false);
            joinmenu.SetActive(true);
        });

        quit.onClick.AddListener(() => Application.Quit());

        join.onClick.AddListener(async() => 
        {
            string code = lobbyCodeInput.text/*[..^1]*/;
            if(await GameLobbyManager.Instance.JoinLobby(code))
            {
                SceneManager.LoadSceneAsync("Lobby");
            }
        });

        back.onClick.AddListener(() =>
        {
            mainmenu.SetActive(true);
            joinmenu.SetActive(false);
            lobbyCodeInput.text = "";
        });

        lobbyCodeInput.onValueChanged.AddListener((val) =>
        {
            join.enabled = !val.IsNullOrEmpty();
        });
    }

    private void OnDisable()
    {
        host.onClick.RemoveAllListeners();
        openjoinmenu.onClick.RemoveAllListeners();
        quit.onClick.RemoveAllListeners();
        join.onClick.RemoveAllListeners();
        back.onClick.RemoveAllListeners();
        lobbyCodeInput.onValueChanged.RemoveAllListeners();
    }

    private void Awake()
    {
        host.enabled = false;
        openjoinmenu.enabled = false;
        join.enabled = false;
    }

    public void EnableButtons()
    {
        host.enabled = true;
        openjoinmenu.enabled = true;
    }
}
