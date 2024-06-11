using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzeLevlerInteractionObject : InteractableObject
{
    [SerializeField] private FuzeController controller;
    public override void Interact(ulong playerID)
    {
        controller.InteractionServerRPC((int)playerID);
    }
}
