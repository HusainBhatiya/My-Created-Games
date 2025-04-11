using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnityWindowEditor : EditorWindow
{
    ObsticleData obsData;

    [MenuItem("Window/Obsticle Data")]
    public static void ShowWindow()
    {
        GetWindow(typeof(UnityWindowEditor));
    }

    void OnGUI()
    {
        obsData = (ObsticleData)EditorGUILayout.ObjectField("Obsticle Data", obsData, typeof(ObsticleData), false);

        if (obsData == null)
        {
            EditorGUILayout.HelpBox("Please assign an ObstacleData asset.", MessageType.Warning);
            return;
        }


        for (int y = 9; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < 10; x++)
            {
                if (obsData.blocked.Contains(new Vector2Int(x, y)))
                {
                    GUI.backgroundColor = Color.red;
                }
                else
                {
                    GUI.backgroundColor = Color.white;
                }
                if (GUILayout.Button("", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (obsData.blocked.Contains(pos))
                    {
                        obsData.blocked.Remove(pos);
                    }
                    else
                    {
                        obsData.blocked.Add(pos);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(obsData); // Marks asset as modified
            AssetDatabase.SaveAssets(); // Saves the change to disk

        }
    }
}
