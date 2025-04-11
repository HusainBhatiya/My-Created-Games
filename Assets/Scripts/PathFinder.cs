using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;

    public Vector2Int StartCoordinates { get { return startCoordinates; } }


    [SerializeField] Vector2Int destinationCoordinates;

    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    public List<Node> currentPath = new List<Node>();

    void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
    }

    void Start()
    {
        if (gridManager != null)
        {
            grid = gridManager.Grid;

            if (grid.ContainsKey(startCoordinates))
            {
                startNode = grid[startCoordinates];
            }

            if (grid.ContainsKey(destinationCoordinates))
            {
                destinationNode = grid[destinationCoordinates];
            }
            else
            {
                Debug.LogWarning($"Destination node {destinationCoordinates} not found in grid.");
                return;
            }
            GetNewPath();
        }

        Debug.Log($"Start: {startCoordinates}  |  Destination: {destinationCoordinates}");

    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();
        BreathFirstSerach(coordinates);
        return BuildPath();
    }

    public List<Node> RequestPathTo(Vector2Int start, Vector2Int destination)
    {
        destinationCoordinates = destination;

        if (!grid.ContainsKey(start))
        {
            Debug.LogError($"Start coordinate {start} not found in grid!");
            return new List<Node>();
        }
        if (grid.ContainsKey(destination))
        {
            destinationNode = grid[destination];
        }
        else
        {
            Debug.LogError($"Destination coordinate {destination} not found in grid!");
            return new List<Node>();
        }

        return GetNewPath(start);
    }

    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborscoords = currentSearchNode.coordinates + direction;

            if (grid.ContainsKey(neighborscoords))
            {
                neighbors.Add(grid[neighborscoords]);
            }
        }

        foreach (Node neighbor in neighbors)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.ConnectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreathFirstSerach(Vector2Int coordinates)
    {
        if (!grid.ContainsKey(coordinates))
        {
            Debug.LogError($"Grid does'nt contain: {coordinates}");
            return;
        }

        startNode = grid[coordinates];
        startNode.isWalkable = true;
        //destinationNode.isWalkable = true;

        bool isRunning = true;

        frontier.Clear();
        reached.Clear();

        frontier.Enqueue(startNode);
        reached.Add(coordinates, startNode);

        while(frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();

            if (currentSearchNode == null)
            {
                Debug.LogError("Dequeued node is NULL!");
                continue;
            }

            currentSearchNode.isExplored = true;
            ExploreNeighbors();

            if (currentSearchNode == destinationNode)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()
    {
        if (destinationNode == null)
        {
            Debug.LogError("Cannot build path: destinationNode is null!");
            return new List<Node>();
        }


        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.ConnectedTo != null)
        {
            currentNode = currentNode.ConnectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }
        path.Reverse();

        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }
        return false;
    }

    public void NotifyReciever()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }

}
