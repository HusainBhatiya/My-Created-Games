using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEvent : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] private PathFinder pathFinder;

    GridManager gridManager;

    Player playerMover;
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        gridManager = FindAnyObjectByType<GridManager>();
        playerMover = FindAnyObjectByType<Player>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                WayPoint tile = hit.collider.GetComponent<WayPoint>();
                if (tile != null)
                {
                    Vector2Int coordinates = tile.coordinates;
                    Debug.Log($"Clicked on tile at coordinates: {coordinates}");

                    // Check if the tile is on the grid
                    if (pathFinder != null && gridManager != null && gridManager.Grid.ContainsKey(coordinates))
                    {
                        Vector2Int start = gridManager.GetCoordinatesFromPosition(transform.position); // player's current position
                        Vector2Int destination = tile.coordinates; // clicked tile
                        List<Node> path = pathFinder.RequestPathTo(start, destination);

                        if (playerMover != null)
                        {
                            playerMover.MoveTo(coordinates);
                        }
                        else
                        {
                            Debug.LogWarning("PlayerMover is not assigned or found in the scene.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Tile {tile.coordinates} is not on the grid!");
                    }
                }
            }
        }
    }
}
