using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DebuggerNetwork : NetworkBehaviour
{
    [ServerRpc]
    public void PrintPlayerIDServerRPC(ulong playerID)
    {
        print(playerID);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerNetwork player))
            PrintPlayerIDServerRPC(player.GetComponent<NetworkObject>().NetworkObjectId);
    }
}
