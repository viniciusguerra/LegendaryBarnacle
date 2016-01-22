using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(Ammo))]
public class AmmoDrawer : PropertyDrawer
{
    //for database selection popup
    int selectedIndex = 0;
    bool setValues = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);        

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        //define element positions
        float fieldHeight = 16;
        float height = position.y;
        float columnWidth = 120;

        if (property.serializedObject != null)
        {
            var ammoNameLabelRect = new Rect(position.x, height, columnWidth * 2, fieldHeight);
            var ammoNameValueRect = new Rect(position.x + columnWidth, height, columnWidth, fieldHeight);
            height += fieldHeight;

            var ammoCaliberLabelRect = new Rect(position.x, height, columnWidth * 2, fieldHeight);
            var ammoCaliberValueRect = new Rect(position.x + columnWidth, height, columnWidth, fieldHeight);
            height += fieldHeight;

            var ammoTipLabelRect = new Rect(position.x, height, columnWidth * 2, fieldHeight);
            var ammoTipValueRect = new Rect(position.x + columnWidth, height, columnWidth, fieldHeight);
            height += fieldHeight;

            var ammoDamageLabelRect = new Rect(position.x, height, columnWidth * 2, fieldHeight);
            var ammoDamageValueRect = new Rect(position.x + columnWidth, height, columnWidth, fieldHeight);
            height += fieldHeight;

            var ammoPenetrationLabelRect = new Rect(position.x, height, columnWidth * 2, fieldHeight);
            var ammoPenetrationValueRect = new Rect(position.x + columnWidth, height, columnWidth, fieldHeight);
            height += fieldHeight;

            var ammoPrefabLabelRect = new Rect(position.x, height, columnWidth * 2, fieldHeight);
            var ammoPrefabValueRect = new Rect(position.x + columnWidth, height, columnWidth * 4, fieldHeight);
            height += fieldHeight * 2;

            var ammoPopupRect = new Rect(position.x + columnWidth, height, columnWidth, fieldHeight);
            var setButtonRect = new Rect(position.x, height, columnWidth, fieldHeight);

            //load ammo types from database
            Ammo[] ammoTypesFromDatabase = AmmoDatabase.Load().ToArray();
            string[] ammoNamesFromDatabase = new string[ammoTypesFromDatabase.Length + 1];
            ammoNamesFromDatabase[0] = "No Change";

            for (int i = 1; i < ammoTypesFromDatabase.Length + 1; i++)
            {
                ammoNamesFromDatabase[i] = ammoTypesFromDatabase[i - 1].ammoName;
            }

            setValues = EditorGUI.ToggleLeft(setButtonRect, "Set Values", setValues);

            //draw popup only if set values is enabled
            if (setValues)
            {
                //draw database selection popup
                int newIndex = EditorGUI.Popup(ammoPopupRect, selectedIndex, ammoNamesFromDatabase);

                if (newIndex != selectedIndex && newIndex != 0)
                {
                    //set current property values to selected database item
                    property.FindPropertyRelative("ammoName").stringValue = ammoTypesFromDatabase[newIndex - 1].ammoName;
                    property.FindPropertyRelative("caliber").stringValue = ammoTypesFromDatabase[newIndex - 1].caliber;
                    property.FindPropertyRelative("tip").stringValue = ammoTypesFromDatabase[newIndex - 1].tip;
                    property.FindPropertyRelative("damage").floatValue = ammoTypesFromDatabase[newIndex - 1].damage;
                    property.FindPropertyRelative("penetration").floatValue = ammoTypesFromDatabase[newIndex - 1].penetration;
                    property.FindPropertyRelative("prefabName").stringValue = ammoTypesFromDatabase[newIndex - 1].prefabName;

                    setValues = false;
                }

                selectedIndex = 0;
            }

            //draw information        
            EditorGUI.LabelField(ammoNameLabelRect, "Ammo Name: ");
            EditorGUI.LabelField(ammoNameValueRect, property.FindPropertyRelative("ammoName").stringValue, EditorStyles.boldLabel);

            EditorGUI.LabelField(ammoCaliberLabelRect, "Caliber: ");
            EditorGUI.LabelField(ammoCaliberValueRect, property.FindPropertyRelative("caliber").stringValue, EditorStyles.boldLabel);

            EditorGUI.LabelField(ammoTipLabelRect, "Tip: ");
            EditorGUI.LabelField(ammoTipValueRect, property.FindPropertyRelative("tip").stringValue, EditorStyles.boldLabel);

            EditorGUI.LabelField(ammoDamageLabelRect, "Damage: ");
            EditorGUI.LabelField(ammoDamageValueRect, property.FindPropertyRelative("damage").floatValue.ToString(), EditorStyles.boldLabel);

            EditorGUI.LabelField(ammoPenetrationLabelRect, "Penetration: ");
            EditorGUI.LabelField(ammoPenetrationValueRect, property.FindPropertyRelative("penetration").floatValue.ToString(), EditorStyles.boldLabel);

            EditorGUI.LabelField(ammoPrefabLabelRect, "Prefab: ");
            EditorGUI.LabelField(ammoPrefabValueRect, AmmoDatabase.ammoPrefabsPath + property.FindPropertyRelative("prefabName").stringValue, EditorStyles.boldLabel);
            //GameObject prefabAtPath = AssetDatabase.LoadAssetAtPath<GameObject>(AmmoDatabase.ammoPrefabsPath + property.FindPropertyRelative("prefabName").stringValue);
            //EditorGUI.ObjectField(ammoPrefabValueRect, prefabAtPath != null ? prefabAtPath : null, typeof(GameObject), false);
        }
        else
        {
            var nullLabel = new Rect(position.x, height, columnWidth * 2, fieldHeight);
            EditorGUI.LabelField(nullLabel, "Object Is Null");
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 140;

        //if (property.objectReferenceValue == null)
        //    height = 40;

        return height;
    }
}