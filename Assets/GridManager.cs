using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Range(5, 10)]
    public int PREFAB_SIZE = 5;
    public Vector2 gridSize = new (5, 5);
    public GameObject gridPrefab;
    public void GenerateGrid()
    {
        for (int i = 0; i < gridSize.x; i++)
        {
            for(int j = 0; j < gridSize.y; j++)
            {
                GameObject gridElement = Instantiate(gridPrefab);
                gridElement.transform.position = new Vector3(i* PREFAB_SIZE, 1, j* PREFAB_SIZE);
                gridElement.transform.SetParent(transform, false);
            }
        }
    }
}
