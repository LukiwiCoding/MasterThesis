using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private NetworkManager manager;
    void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        root.Q<Button> ("Start").clickable.clicked += 
            () => manager.StartHost();
        root.Q<Button> ("FindServer").clickable.clicked += 
            () => manager.StartClient();
        root.Q<TextField>("Address").RegisterValueChangedCallback(
            (e) => manager.networkAddress = e.newValue); ; 
        //root.Q<Button>("Options").clickable.clicked
        root.Q<Button>("Exit").clickable.clicked +=
            () => Application.Quit();
    }
}
