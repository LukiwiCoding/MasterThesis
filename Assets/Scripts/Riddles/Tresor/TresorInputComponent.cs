using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresorInputComponent : InteractableObject
{
    [SerializeField] private bool countUp;
    [SerializeField] private TresorOutputComponent output;
    [SerializeField] private TresorController controller;

    public override void Interact()
    {
        output.UpdateOutputFieldServerRPC(countUp);
        controller.InteractionServerRPC();
    }
}
