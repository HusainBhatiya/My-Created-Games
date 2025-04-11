
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageObs : MonoBehaviour
{
    [SerializeField] public ObsticleData obsData;
    [SerializeField] public GameObject obsPrefab;

    float gridSize = 10f;

    GridManager gridManager;
    void Start()
    {
        gridManager = FindAnyObjectByType<GridManager>();

        if (obsData == null ||  obsPrefab == null)
        {
            Debug.LogError("Obstacle data or prefab is not assigned.");
            return;
        }

        Debug.Log("Blocked positions:");
        foreach (Vector2Int pos in obsData.blocked)
        {
            Debug.Log(pos);
            if (gridManager != null)
            {
                gridManager.BlockNodes(pos);
            }
            if (obsPrefab == null)
            {
                Debug.LogError("Obstacle prefab is not assigned.");
                return;
            }

            Vector3 spawnPos = new Vector3(pos.x * gridSize, 0.5f, pos.y * gridSize);
            Instantiate(obsPrefab, spawnPos, Quaternion.identity);
        }
    }
}
