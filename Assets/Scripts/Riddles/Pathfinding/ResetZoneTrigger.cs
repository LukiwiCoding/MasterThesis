using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ResetZoneTrigger : NetworkBehaviour
{
    private GameObject player = null;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out NetworkPlayerController controller)){
            player = other.gameObject;
        }
    }

    public GameObject GetPlayer() { return player; }
}
