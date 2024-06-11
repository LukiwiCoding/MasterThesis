using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresorInputComponent : InteractableObject
{
    [SerializeField] private bool countUp;
    [SerializeField] private TresorOutputComponent output;
    [SerializeField] private TresorController controller;

    public override void Interact(ulong playerID = default)
    {
        output.UpdateOutputFieldServerRPC(countUp);
        controller.InteractionServerRPC();
    }
}
