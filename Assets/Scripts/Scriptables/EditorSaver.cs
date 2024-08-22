using Alchemy.Inspector;
using UnityEditor;
using UnityEngine;

public class EditorSaver : MonoBehaviour
{
    [SerializeField] [InlineEditor]
    private EditDataAsset editDataAsset;
    
    [Button]
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Save()
    {
        EditorUtility.SetDirty(editDataAsset.wardData);
        EditorUtility.SetDirty(editDataAsset.spriteData);
        EditorUtility.SetDirty(editDataAsset.wardView);
        EditorUtility.SetDirty(editDataAsset.spriteView);
        AssetDatabase.SaveAssets();
    }
}
