using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPermission : MonoBehaviour
{
    public bool isPlayerUnit = true;
    public bool moved = false;

    private void Start()
    {
        AttackPermission[] attackPermissions = Object.FindObjectsByType<AttackPermission>(FindObjectsSortMode.InstanceID);

        int enemyCount = 0;

        foreach (AttackPermission attackPermission in attackPermissions)
        {
            if (!attackPermission.isPlayerUnit)
            {
                Debug.Log($"Enemy {enemyCount} is {attackPermission.gameObject.name} at {attackPermission.transform.position}");
                enemyCount++;
            }
        }
        Debug.Log($"Total enemies: {enemyCount}");
    }
    public void StartTurn()
    {
        moved = false;
        Debug.Log($"{gameObject.name} start their turn.");
    }

    // Update is called once per frame
    public void EndTurn()
    {

        Debug.Log($"{gameObject.name} end their turn.");
    }

    public void Move(Vector3 newPosition)
    {
        transform.position = newPosition;
        moved = true;
    }

    void Update()
    {
        if (isPlayerUnit && Input.GetKeyDown(KeyCode.M))
        {
            Vector3 newPosition = transform.position + new Vector3(1, 0, 0);
            Move(newPosition);
        }
    }

    void OnMouseDown()
    {

        if (!isPlayerUnit) return;

        Debug.Log("Player clicked!");

        PathFinder pathFinder = FindAnyObjectByType<PathFinder>();
        DisplayPath displayPath = FindAnyObjectByType<DisplayPath>();
        GridManager gridManager = FindAnyObjectByType<GridManager>();

        if (pathFinder == null || displayPath == null || gridManager == null) return;

        AttackPermission nearestEnemy = FindNearestEnemy();



        if (nearestEnemy == null) return;
        Vector2Int start = gridManager.GetCoordinatesFromPosition(transform.position); // Player's current position
        Vector2Int destination = gridManager.GetCoordinatesFromPosition(nearestEnemy.transform.position); // Enemy's position
        List<Node> path = pathFinder.RequestPathTo(start, destination);
        displayPath.ColoredPath(path);
    }

    AttackPermission FindNearestEnemy()
    {
        AttackPermission[] attackPermissions = Object.FindObjectsByType<AttackPermission>(FindObjectsSortMode.InstanceID);
        AttackPermission nearestEnemy = null;

        float nearestDistance = Mathf.Infinity;

        foreach (AttackPermission attackPermission in attackPermissions)
        {
            if (!attackPermission.isPlayerUnit)
            {
                float distance = Vector3.Distance(transform.position, attackPermission.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = attackPermission;
                }
            }
        }
        return nearestEnemy;
    }
}
