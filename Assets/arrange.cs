using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utils;

[ExecuteInEditMode]
public class arrange : Singleton<arrange>
{
    [MenuItem("DebugMenu/Rearrange")]
    public static void Rearrange()
    {
        for (int i = 0; i < Instance.transform.childCount; i++)
        {
            Instance.transform.GetChild(i).localPosition = new Vector3(i % 10 * 2, 0, i / 10 * 2);
        }
    }
}
