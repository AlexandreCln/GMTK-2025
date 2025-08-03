using UnityEditor;
using UnityEngine;

// [CustomEditor(typeof(IMPanelColorCycles))]
public class ColorCyclesEditor : Editor
{
    SerializedProperty cyclesColors;
    IMPanelColorCycles targetScript;

    void OnEnable()
    {
        targetScript = (IMPanelColorCycles)target;
        cyclesColors = serializedObject.FindProperty("_cyclesColors");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (targetScript.Manager == null)
        {
            EditorGUILayout.HelpBox("Référence au manager manquante.", MessageType.Warning);
        }
        else
        {
            // Accès direct au champ privé via SerializedObject si possible
            SerializedObject managerSO = new SerializedObject(targetScript.Manager);
            SerializedProperty nbCycles = managerSO.FindProperty("_nbCycles");

            if (nbCycles != null)
            {
                managerSO.Update();

                EditorGUILayout.LabelField("Paramètres du Manager", EditorStyles.boldLabel);
                nbCycles.intValue = EditorGUILayout.IntSlider("Cycles number", nbCycles.intValue, 1, 10);

                managerSO.ApplyModifiedProperties();

                // Appliquer la taille du tableau
                cyclesColors.arraySize = nbCycles.intValue;

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Couleurs", EditorStyles.boldLabel);
                for (int i = 0; i < cyclesColors.arraySize; i++)
                {
                    EditorGUILayout.PropertyField(
                        cyclesColors.GetArrayElementAtIndex(i),
                        new GUIContent($"Couleur {i + 1}")
                    );
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Propriété _nbCycles introuvable dans le manager.", MessageType.Warning);
            }
        }
        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}
