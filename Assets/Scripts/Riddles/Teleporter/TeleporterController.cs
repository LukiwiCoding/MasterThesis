using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class TeleporterController : RiddleController
{
    [SerializeField] private List<TeleporterUnit> teleporters;
    [SerializeField] private List<Transform> teleportPositions;
    [ServerRpc(RequireOwnership = false)]
    public override void InteractionServerRPC(int objectId)
    {
        if (!solved.Value) return;
        if (CheckTeleporterReadyState())
        {
            TeleportPlayersServerRPC();
        }
    }

    private bool CheckTeleporterReadyState()
    {
        for (int i = 0; i < teleporters.Count; i++) 
        {
            if (teleporters[i].GetPlayerCount() != 1) return false;
        }
        return true;
    }

    [ServerRpc]
    private void TeleportPlayersServerRPC()
    {
        for(int i = 0;i < teleporters.Count;i++)
        {
            ulong playerId = teleporters[i].GetOwnerClientID();
            
            MoveClientRPC(playerId, i);
        }
    }

    [ClientRpc]
    private void MoveClientRPC(ulong id, int index)
    {
        if (!IsOwner) return;
        NetworkClient client = NetworkManager.Singleton.ConnectedClients[id];
        CharacterController controller = client.PlayerObject.GetComponent<CharacterController>();
        ClientNetworkTransform obj = client.PlayerObject.GetComponent<ClientNetworkTransform>();
        controller.enabled = false;
        obj.Interpolate = false;
        client.PlayerObject.GetComponent<ClientNetworkTransform>().Teleport(teleportPositions[index].position, Quaternion.identity, transform.localScale);
        //client.PlayerObject.transform.position = teleportPositions[index].position;
        print(client.PlayerObject.transform.position);
        controller.enabled = true;
        obj.Interpolate = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ActivateTeleportersServerRPC() => solved.Value = true;
}
