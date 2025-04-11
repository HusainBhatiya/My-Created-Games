using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] public bool isPlaceable = true;
    [SerializeField] Renderer tileRenderer;
    [SerializeField] public GameObject PathDisplayer;
    Color originalColor;

    public bool IsPlacable
    {
        get { return isPlaceable; }
    }

    GridManager gridManager;
    public Vector2Int coordinates;

    void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();

        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if (!isPlaceable)
            {
                gridManager.BlockNodes(coordinates);
            }
        }

        if (tileRenderer != null)
        {
            originalColor = tileRenderer.material.color;
        }

        if (tileRenderer == null)
        {
            tileRenderer = GetComponent<Renderer>();
        }
    }

    public void EnablePathDisplayer(bool enable)
    {
        if (PathDisplayer != null)
        {
            PathDisplayer.SetActive(enable);
        }
    }

    public void SetColor(Color newColor)
    {
        if (tileRenderer != null)
        {
            tileRenderer.material.color = newColor;
        }
    }

    public void ResetColor()
    {
        if (tileRenderer != null)
        {
            tileRenderer.material.color = originalColor;
        }
    }
}
