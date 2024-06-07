using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SandboxManager : MonoBehaviour
{
    private void Start()
    {
        GetComponent<NetworkManager>().StartHost();
    }
}
