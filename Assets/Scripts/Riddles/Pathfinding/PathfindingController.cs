using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PathfindingController : RiddleController
{
    [SerializeField] private Collider resetTrigger;
    [ServerRpc(RequireOwnership = false)]
    public override void InteractionServerRPC(int objectId)
    {
        if (solved.Value) return;
        
        GridInteractionComponent interactionComponent = interactables[objectId] as GridInteractionComponent;
        
        if(interactionComponent == null) return;

        if (!interactionComponent.IsWalkable) ResetPlayerToSpawnServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ResetPlayerToSpawnServerRPC()
    {
        resetTrigger.GetComponent<ResetZoneTrigger>().GetPlayer().transform.position = resetTrigger.transform.position;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetRiddleSolvedServerRPC() => solved.Value = true;

}
