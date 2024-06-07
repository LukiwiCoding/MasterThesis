using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class BookShelfRiddle : RiddleController
{
    [SerializeField] private Vector3 bookMoveDir = new(.1f, 0, 0);
    [SerializeField] private bool[] solution = { true, false, false, true, true };
    [SerializeField] private bool[] currentSolution = { false, false, false, false, false };
    [SerializeField] private Transform door = null;

    [ServerRpc]
    public override void InteractionServerRPC(int bookId)
    {
        if (solved) return; 

        BookShelfRiddleComponent book = interactables[bookId] as BookShelfRiddleComponent;
        if (book == null) return;

        if (book.IsPulled)
            book.transform.position += bookMoveDir;
        else
            book.transform.position -= bookMoveDir;

        book.SetIsPulled(!book.IsPulled);

        currentSolution[bookId] = book.IsPulled;

        if (SolutionIsValid()) 
        { 
            door.position -= Vector3.up * 4;
            print("solved");
        }
    }

    private bool SolutionIsValid()
    {
        for(int i = 0; i < solution.Length; i++)
        {
            if (solution[i] != currentSolution[i]) return false;
        }

        return true;
    }

    protected override void InitializeInteractibles()
    {
        base.InitializeInteractibles();

        for (int i = 0; i < interactables.Count; i++)
        {
            ((BookShelfRiddleComponent)interactables[i]).ObjectID = i;
        }
    }
}
