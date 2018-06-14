using  UnityEngine;
using UnityEditor;
using System.Collections;
using Biird;

public class CreateIds
{
    [MenuItem("Assets/Create/Biird Ids")]
    public static void CreateBiirdIds()
    {
        BiirdIds asset = ScriptableObject.CreateInstance<BiirdIds>();
        AssetDatabase.CreateAsset(asset,"Assets/Biird/BiirdIds.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
