using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{
    AttackPermission self;
    [SerializeField] AttackPermission targetPlayer;

    PathFinder pathFinder;
    [SerializeField] GridManager gridManager;

    void Awake()
    {
        self = GetComponent<AttackPermission>();
        pathFinder = FindAnyObjectByType<PathFinder>();
        gridManager = FindAnyObjectByType<GridManager>();
    }

    void Start()
    {
        Debug.Log($"{name} started.");

        self = GetComponent<AttackPermission>();
        if (self == null)
        {
            Debug.LogWarning("Self is not set.");
            return;
        }

        gridManager = FindAnyObjectByType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogWarning("GridManager is not set.");
            return;
        }

        if (targetPlayer == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");

            if (playerObj != null)
            {
                targetPlayer = playerObj.GetComponent<AttackPermission>();
            }
        }

        if (!self.isPlayerUnit && targetPlayer.isPlayerUnit)
        {
            MoveToTarget();
        }
        else
        {
            Debug.LogWarning("Target player unit is not set or this is not a player unit.");
        }
    }

    //public void SetTarget(AttackPermission playerUnit)
    //{
    //    targetPlayer = playerUnit;
    //}

    void MoveToTarget()
    {
        Debug.Log($"{name}: MoveToTarget called.");

        if (targetPlayer == null)
        {
            Debug.LogWarning("Target player unit is not set.");
            return;
        }

        if (gridManager == null)
        {
            Debug.LogWarning("GridManager is not set.");
            return;
        }

        Vector2Int selfCoords = gridManager.GetCoordinatesFromPosition(transform.position);
        Vector2Int playerCoords = gridManager.GetCoordinatesFromPosition(targetPlayer.transform.position);

        Debug.Log($"{name}: Self = {selfCoords}, Player = {playerCoords}");

        if (!gridManager.Grid.ContainsKey(selfCoords))
        {
            Debug.LogError($"{name}: Start coordinate {selfCoords} NOT in grid.");
            return;
        }
        if (!gridManager.Grid.ContainsKey(playerCoords))
        {
            Debug.LogError($"{name}: Destination coordinate {playerCoords} NOT in grid.");
            return;
        }

        if (IsAdjacent(selfCoords, playerCoords))
        {
            Debug.Log($"{name} is already adjacent to player.");
            return;
        }

        List<Node> path = pathFinder.RequestPathTo(selfCoords, playerCoords);

        if (path == null || path.Count < 2)
        {
            Debug.LogWarning($"{name} couldn't find path to player.");
            return;
        }

        Node nextNode = path[1]; // [0] is current position
        Vector3 nextPos = gridManager.GetPositionFromCoordinates(nextNode.coordinates);
        transform.position = nextPos;

        Debug.Log($"{name} moved to {nextNode.coordinates}");
    }

    bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y)) == 1;
    }
}
