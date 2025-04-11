using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField][Range(0f, 5f)] float moveSpeed = 1f;

    List<Node> path = new List<Node>();

    GridManager gridManager;
    PathFinder pathFinder;

    bool isMoving = false;

    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        pathFinder = FindFirstObjectByType<PathFinder>();
    }

    public void MoveTo(Vector2Int destination)
    {
        if (isMoving) return;

        StopAllCoroutines();
        path.Clear();

        Vector2Int start = gridManager.GetCoordinatesFromPosition(transform.position); // Player's current position
        path = pathFinder.RequestPathTo(start, destination);

        FindAnyObjectByType<DisplayPath>().ColoredPath(path);

        if (path == null && path.Count < 1) return;
        StartCoroutine(FollowPath());
    } 

    IEnumerator FollowPath()
    {
        isMoving = true;
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 StartPosition = transform.position;
            Vector3 EndPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercentage = 0f;

            transform.LookAt(EndPosition);

            while (travelPercentage < 1f)
            {
                travelPercentage += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(StartPosition, EndPosition, travelPercentage);
                yield return null;
            }

            transform.position = EndPosition;
        }
        isMoving = false;

        //FindObjectOfType<TurnTo>().EndTurn();
    }
}
