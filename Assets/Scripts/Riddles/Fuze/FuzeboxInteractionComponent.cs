using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzeboxInteractionComponent : InteractableObject
{
    [SerializeField] private FuzeController controller;
    public override void Interact(ulong playerID)
    {
        controller.InteractionServerRPC((int)playerID);
    }
}
