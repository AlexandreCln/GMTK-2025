#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class MaterialSetter : MonoBehaviour
{
    [ContextMenu("Appliquer matériaux selon tag")]
    public void ApplyMaterialsByTag()
    {
        foreach (Transform child in _tiles)
        {
            var renderer = child.GetComponent<Renderer>();
            if (renderer == null) continue;

            Material mat = null;

            switch (child.tag)
            {
                case "Red":
                    mat = _redMaterial;
                    break;
                case "Blue":
                    mat = _blueMaterial;
                    break;
                case "Yellow":
                    mat = _yellowMaterial;
                    break;
                case "Green":
                    mat = _greenMaterial;
                    break;
                default:
                    mat = _defaultMaterial;
                    break;
            }

            if (mat != null)
            {
                Undo.RecordObject(renderer, "Change Material");
                renderer.sharedMaterial = mat; // `sharedMaterial` pour éditer dans l’éditeur
                EditorUtility.SetDirty(renderer);
            }
        }
    }

    [SerializeField] Transform _tiles;
    [SerializeField] Material _defaultMaterial;
    [SerializeField] Material _redMaterial;
    [SerializeField] Material _blueMaterial;
    [SerializeField] Material _yellowMaterial;
    [SerializeField] Material _greenMaterial;
}
#endif
