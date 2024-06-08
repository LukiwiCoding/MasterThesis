using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TresorController : RiddleController
{

    [SerializeField] private List<TresorOutputComponent> outputs = new ();
    [SerializeField] private List<int> safeSolution = new (){ 1, 9, 7, 8 };

    [ServerRpc(RequireOwnership = false)]
    public override void InteractionServerRPC(int ObjectID = default)
    {
        if (solved.Value) return;

        if(CheckSafeCombination())
        {
            solved.Value = true;
            SolveRiddelClientRPC();
        }
    }
    [ClientRpc]
    protected override void SolveRiddelClientRPC()
    {
        transform.Rotate(Vector3.up * 95);
    }
    private bool CheckSafeCombination()
    {
        for (int i = 0; i < outputs.Count; i++)
        {
            if (outputs[i].GetCurrentDigit != safeSolution[i])
            {
                return false;
            }

        }
        return true;
    }
}
