using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FuzeController : RiddleController
{
    private Transform carryingPlayerTransform = null;

    [ServerRpc(RequireOwnership = false)]
    public override void InteractionServerRPC(int playerID)
    {
        if (solved.Value) return;

        print($"ServerRPC: Id={playerID}");
        if (playerID <= 100 && carryingPlayerTransform == null)
        {
            carryingPlayerTransform = NetworkManager.Singleton.ConnectedClients[(ulong)playerID].PlayerObject.transform;
            Transform child = transform.GetChild(0);
            child.SetParent(carryingPlayerTransform);
            child.localPosition = new (0, 0, 1.1f);
        }

    }
}
