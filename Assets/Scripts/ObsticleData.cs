using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObsticleData", menuName = "Scriptable Object/Obsticle Data")]
public class ObsticleData : ScriptableObject
{
    public List<Vector2Int> blocked = new List<Vector2Int>();
}
