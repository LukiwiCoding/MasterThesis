using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BackportController : NetworkBehaviour, IInteractable
{
    [SerializeField] private TeleporterController teleporterController;
    [SerializeField] private List<GameObject> makeInvisible;
    [SerializeField] private GameObject secretDoor;
    public void Interact(ulong senderId = 0)
    {
        teleporterController.ActivateTeleportersServerRPC();
        ClearOfficeClientRPC();
    }

    [ClientRpc]
    private void ClearOfficeClientRPC()
    {
        foreach (GameObject obj in makeInvisible) { obj.SetActive(false); }
        secretDoor.transform.position = new Vector3(2.67f, 0, -1.88f);
    }
}
