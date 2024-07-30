using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevlerInteraction : NetworkBehaviour, IInteractable
{
    [SerializeField]private GameObject[] doors;
    [SerializeField] private Vector3 rotation;
    public void Interact(ulong senderId = 0)
    {
        transform.GetChild(0).Rotate(rotation);
        foreach(GameObject door in doors) door.SetActive(false);
    }
}
