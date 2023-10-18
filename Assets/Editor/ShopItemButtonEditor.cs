using Shop;
using UnityEditor;

[CustomEditor(typeof(ShopItemButton))]
public class ShopItemButtonEditor: Editor
{
    SerializedProperty script;
    SerializedProperty usableByPlayerProperty;
    SerializedProperty p1DescriptionFieldProperty;
    SerializedProperty p2DescriptionFieldProperty;

    private void OnEnable()
    {
        script = serializedObject.FindProperty("m_Script");
        usableByPlayerProperty = serializedObject.FindProperty("usableByPlayer");
        p1DescriptionFieldProperty = serializedObject.FindProperty("p1DescriptionField");
        p2DescriptionFieldProperty = serializedObject.FindProperty("p2DescriptionField");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(script);
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.PropertyField(usableByPlayerProperty);

        if (usableByPlayerProperty.enumValueIndex == 0)
        {
            EditorGUILayout.PropertyField(p1DescriptionFieldProperty);
            EditorGUILayout.PropertyField(p2DescriptionFieldProperty);
        }
        else if (usableByPlayerProperty.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(p1DescriptionFieldProperty);
        }
        else if (usableByPlayerProperty.enumValueIndex == 2)
        {
            EditorGUILayout.PropertyField(p2DescriptionFieldProperty);
        }
        
        // Draw the rest of the properties, excluding the ones we've already drawn.
        DrawPropertiesExcluding(serializedObject, "m_Script", "usableByPlayer", "p1DescriptionField", "p2DescriptionField");

        serializedObject.ApplyModifiedProperties();
    }

}
