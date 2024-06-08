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
        if (solved) return;

        if(CheckSafeCombination())
        {
            print("solved");
            transform.Rotate(Vector3.up * 95);
        }
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
