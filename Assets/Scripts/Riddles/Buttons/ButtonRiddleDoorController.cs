using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ButtonRiddleDoorController : RiddleController
{
    [SerializeField] private float activTime = 1.5f;
    [SerializeField] private Transform door;
    [ServerRpc(RequireOwnership = false)]
    public override void InteractionServerRPC(int buttonID)
    {
        if (solved.Value) return;

        ButtonRiddleController button = interactables[buttonID] as ButtonRiddleController;
        if (button.IsActive) return;

        button.SetTimerServerRPC(activTime);
        button.transform.position += button.ActivationDir;

        if (CheckButtonStates())
        {
            solved.Value = true;
        }
    }
    [ClientRpc]
    protected override void SolveRiddelClientRPC()
    {
        door.position = Vector3.up * -4f;
    }
    protected override void InitializeInteractibles()
    {
        base.InitializeInteractibles();

        for (int i = 0; i < interactables.Count; i++)
        {
            ((ButtonRiddleController)interactables[i]).ObjectID = i;
        }
    }
    private bool CheckButtonStates()
    {
        for(int i = 0;i < interactables.Count;i++)
        {
            if (!((ButtonRiddleController)interactables[i]).IsActive) return false;
        }

        return true;
    }
}