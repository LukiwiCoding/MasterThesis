using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TeleporterUnit : NetworkBehaviour
{
    [SerializeField] private TeleporterController controller;
    private List<ulong> players = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) 
        { 
            players.Add(other.gameObject.GetComponent<NetworkObject>().OwnerClientId);
            
        }
        controller.InteractionServerRPC(0);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            players.Remove(other.gameObject.GetComponent<NetworkObject>().OwnerClientId);
        }
    }
    public int GetPlayerCount() => players.Count;
    public ulong GetOwnerClientID() => players[0];
}
