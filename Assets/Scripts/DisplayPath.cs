using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPath : MonoBehaviour
{
    [SerializeField] Color pathColor = Color.green;
    List<WayPoint> coloredTiles = new List<WayPoint>();

    public void ColoredPath(List<Node> path)
    {
        ClearPreviousColoredPath();

        foreach (Node node in path)
        {
            Vector3 worldPos = new Vector3(node.coordinates.x * 10f, 0, node.coordinates.y * 10f);
            Collider[] hits = Physics.OverlapSphere(worldPos, 0.1f);

            foreach (var hit in hits)
            {
                WayPoint tile = hit.GetComponent<WayPoint>();
                if (tile != null)
                {
                    tile.SetColor(pathColor);
                    coloredTiles.Add(tile);
                }
            }
        }
    }

    public void ClearPreviousColoredPath()
    {
        WayPoint wp = FindAnyObjectByType<WayPoint>();
        foreach (WayPoint tile in coloredTiles)
        {
            tile.ResetColor();
        }
        coloredTiles.Clear();
    }
}
