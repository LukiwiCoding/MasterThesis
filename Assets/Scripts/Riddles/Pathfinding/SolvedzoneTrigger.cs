using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SolvedzoneTrigger : NetworkBehaviour
{
    [SerializeField] private PathfindingController controller;
    private void OnTriggerEnter(Collider other)
    {
        controller.SetRiddleSolvedServerRPC();
    }
}
