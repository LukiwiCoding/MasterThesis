using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class InteractableObject : NetworkBehaviour, IInteractable
{
    public int ObjectID;
    public virtual void Interact() { }  
}
