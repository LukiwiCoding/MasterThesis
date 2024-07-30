using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FuzeController : RiddleController
{
    [SerializeField] Transform levler;
    [SerializeField] TeleporterController teleporter;
    private ulong carryingPlayer = 100;
    NetworkVariable<bool> fuzeInserted = new(false);

    [ServerRpc(RequireOwnership = false)]
    public override void InteractionServerRPC(int playerID)
    {
        if (solved.Value) return;

        if (!fuzeInserted.Value)
        {
            if (playerID <= 100 && carryingPlayer >= 2)
            {
                carryingPlayer = (ulong)playerID;
                Transform carryingPlayerTransform = NetworkManager.Singleton.ConnectedClients[(ulong)playerID].PlayerObject.transform;
                Transform child = transform.GetChild(0);
                child.SetParent(carryingPlayerTransform);
                child.localPosition = new(0, 0, 1.1f);
                child.forward = carryingPlayerTransform.forward;
            }
            else if (playerID <= 100 && carryingPlayer < 2)
            {
                Transform carryingPlayerTransform = NetworkManager.Singleton.ConnectedClients[(ulong)playerID].PlayerObject.transform;
                Transform fuze = carryingPlayerTransform.GetComponentInChildren<InteractableObject>().transform;
                fuze.SetParent(transform.GetChild(0));
                fuze.localPosition = Vector3.up * -.165f;
                fuze.forward = fuze.parent.forward;
                fuzeInserted.Value = true;
            }
        }
        else
        {
            levler.Rotate(Vector3.right * -82);
            solved.Value = true;
            teleporter.ActivateTeleportersServerRPC();
        }

    }
}
