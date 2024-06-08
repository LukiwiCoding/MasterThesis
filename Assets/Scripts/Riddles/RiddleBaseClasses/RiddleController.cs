using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class RiddleController : NetworkBehaviour
{
    protected List<IInteractable> interactables = new();
    protected NetworkVariable<bool> solved = new(false);
    protected virtual void InitializeInteractibles() {
        GetInteractableComponentsRecursive(transform);       
    }
    [ServerRpc(RequireOwnership = false)]
    public virtual void InteractionServerRPC(int objectId) { }
    [ClientRpc]
    protected virtual void SolveRiddelClientRPC() { }
    private void Awake()
    {
        InitializeInteractibles();
    }

    private void GetInteractableComponentsRecursive(Transform transform)
    {
        foreach (Transform interactableObject in transform)
        {
            if (interactableObject.TryGetComponent(out InteractableObject interactable))
            {
                interactables.Add(interactable);
            }
            else
            {
                GetInteractableComponentsRecursive(interactableObject);
            }
        }
    }
}
