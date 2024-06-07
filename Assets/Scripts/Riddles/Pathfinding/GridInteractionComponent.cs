using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInteractionComponent : InteractableObject
{
    [SerializeField] private bool isWalkable = false;
    private PathfindingController controller;
    public bool IsWalkable { get { return isWalkable; } }

    public void Awake()
    {
        controller = GetComponentInParent<PathfindingController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        controller.InteractionServerRPC(ObjectID);
    }
}
