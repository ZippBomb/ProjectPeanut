using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spell), true)]
public class SpellEditor : Editor {

    bool showCastLogic = true;

    public override void OnInspectorGUI() {

        Spell spell = (Spell) target;

        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, "castLogic", "castLogicType");

        EditorGUILayout.Space();

        SpellCastLogic.Type logicType = (SpellCastLogic.Type) EditorGUILayout.EnumPopup("Cast logic type", spell.castLogicType);
        if (logicType != spell.castLogicType) {

            spell.castLogicType = logicType;
            spell.castLogic = InstantiateCastLogic(logicType);

        }
        if (spell.castLogic == null) {

            Debug.LogError("Invalid cast logic on spell '" + spell.name + "'.");
            return;

        }

        SerializedProperty prop = serializedObject.FindProperty("castLogic");

        EditorGUILayout.PropertyField(prop, true);
        serializedObject.ApplyModifiedProperties();

    }

    private SpellCastLogic InstantiateCastLogic(SpellCastLogic.Type type) {

        switch (type) {

            case SpellCastLogic.Type.Instant: return new SpellCastLogic();
            case SpellCastLogic.Type.Cooldown: return new CooldownCastLogic();
            default: break;

        }

        Debug.LogError("Invalid Cast logic type '" + type.ToString() + "', could not instatiate cast logic.");
        return null;
        
    }

}