using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PathfindingController : RiddleController
{
    [SerializeField] private Collider resetTrigger;
    [ServerRpc]
    public override void InteractionServerRPC(int objectId)
    {
        if (solved) return;
        
        GridInteractionComponent interactionComponent = interactables[objectId] as GridInteractionComponent;
        
        if(interactionComponent == null) return;

        if (!interactionComponent.IsWalkable) ResetPlayerToSpawnServerRPC();
    }

    [ServerRpc]
    private void ResetPlayerToSpawnServerRPC()
    {
        resetTrigger.GetComponent<ResetZoneTrigger>().GetPlayer().transform.position = resetTrigger.transform.position;
    }

    [ServerRpc]
    public void SetRiddleSolvedServerRPC() => solved = true;

}
