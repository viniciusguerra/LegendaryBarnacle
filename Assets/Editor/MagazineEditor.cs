using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(MagazineData))]
public class MagazineDrawer : PropertyDrawer
{
    //for database selection popup
    int selectedMagazineIndex = 0;
    int selectedAmmoIndex = 0;
    bool setValues = false;
    bool load = false;
    bool empty = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        //store original indent levels
        int initialIndent = EditorGUI.indentLevel;

        //define element positions
        float fieldHeight = 16;
        float height = position.y;
        float columnWidth = 103;
        float valueWidth = columnWidth * 4;

        //initial height for drawing under main label
        height += fieldHeight;
        position.x = 0;
        EditorGUI.indentLevel += 2;

        var magNameLabelRect = new Rect(position.x, height, valueWidth, fieldHeight);
        var magNameValueRect = new Rect(position.x + columnWidth, height, valueWidth, fieldHeight);
        height += fieldHeight;

        var magCurrentAmmoNameLabelRect = new Rect(position.x, height, valueWidth, fieldHeight);
        var magCurrentAmmoNameValueRect = new Rect(position.x + columnWidth, height, valueWidth, fieldHeight);
        height += fieldHeight;

        var magCapacityLabelRect = new Rect(position.x, height, valueWidth, fieldHeight);
        var magCapacityValueRect = new Rect(position.x + columnWidth, height, valueWidth, fieldHeight);
        height += fieldHeight;

        var magCaliberLabelRect = new Rect(position.x, height, valueWidth, fieldHeight);
        var magCaliberValueRect = new Rect(position.x + columnWidth, height, valueWidth, fieldHeight);
        height += fieldHeight;

        var magWeightLabelRect = new Rect(position.x, height, valueWidth, fieldHeight);
        var magWeightValueRect = new Rect(position.x + columnWidth, height, valueWidth, fieldHeight);
        height += fieldHeight;

        var magPrefabLabelRect = new Rect(position.x, height, valueWidth, fieldHeight);
        var magPrefabValueRect = new Rect(position.x + columnWidth, height, valueWidth, fieldHeight);
        height += fieldHeight * 2;

        var magazinePopupRect = new Rect(position.x + columnWidth, height, valueWidth / 2, fieldHeight);
        var setButtonRect = new Rect(position.x, height, valueWidth / 3, fieldHeight);

        height += fieldHeight;
        var ammoPopupRect = new Rect(position.x + columnWidth, height, valueWidth / 2, fieldHeight);
        var loadButtonRect = new Rect(position.x, height, valueWidth / 3, fieldHeight);

        height += fieldHeight;
        var emptyButtonRect = new Rect(position.x, height, valueWidth / 3, fieldHeight);

        MagazineDatabase magDatabase = new MagazineDatabase();
        MagazineData[] magTypesFromDatabase = magDatabase.Load().ToArray();
        string[] magNamesFromDatabase = new string[magTypesFromDatabase.Length + 1];
        magNamesFromDatabase[0] = "No Change";

        for (int i = 1; i < magTypesFromDatabase.Length + 1; i++)
        {
            magNamesFromDatabase[i] = magTypesFromDatabase[i - 1].ItemName;
        }

        AmmoDatabase ammoDatabase = new AmmoDatabase();
        AmmoData[] ammoTypesFromDatabase = ammoDatabase.Load().ToArray();
        string[] ammoNamesFromDatabase = new string[ammoTypesFromDatabase.Length + 1];
        ammoNamesFromDatabase[0] = "No Change";

        for (int i = 1; i < ammoTypesFromDatabase.Length + 1; i++)
        {
            ammoNamesFromDatabase[i] = ammoTypesFromDatabase[i - 1].ammoName;
        }

        setValues = EditorGUI.ToggleLeft(setButtonRect, "Set Values", setValues);
        
        if (setValues)
        {
            empty = false;
            load = false;

            //draw database selection popup
            int newIndex = EditorGUI.Popup(magazinePopupRect, selectedMagazineIndex, magNamesFromDatabase);

            if (newIndex != selectedMagazineIndex && newIndex != 0)
            {
                //set current property values to selected database item
                property.FindPropertyRelative("itemName").stringValue = magTypesFromDatabase[newIndex - 1].ItemName;
                property.FindPropertyRelative("capacity").intValue = magTypesFromDatabase[newIndex - 1].Capacity;
                property.FindPropertyRelative("caliber").stringValue = magTypesFromDatabase[newIndex - 1].Caliber;
                property.FindPropertyRelative("itemWeight").floatValue = magTypesFromDatabase[newIndex - 1].Weight;
                property.FindPropertyRelative("prefabName").stringValue = magTypesFromDatabase[newIndex - 1].PrefabName;                
            }

            selectedMagazineIndex = 0;
        }

        load = EditorGUI.ToggleLeft(loadButtonRect, "Load", load);

        if (load)
        {
            setValues = false;
            empty = false;

            //draw database selection popup
            int newIndex = EditorGUI.Popup(ammoPopupRect, selectedAmmoIndex, ammoNamesFromDatabase);

            if (newIndex != selectedAmmoIndex && newIndex != 0)
            {
                MagazineData targetMagData = fieldInfo.GetValue(property.serializedObject.targetObject) as MagazineData;
                targetMagData.CurrentAmmo = ammoTypesFromDatabase[newIndex - 1];

                property.FindPropertyRelative("currentAmmoCount").intValue = property.FindPropertyRelative("capacity").intValue;
            }

            selectedAmmoIndex = 0;
        }

        empty = EditorGUI.ToggleLeft(emptyButtonRect, "Empty", empty);

        if (empty)
        {
            setValues = false;
            load = false;

            ((MagazineData)fieldInfo.GetValue(property.serializedObject.targetObject)).CurrentAmmo = null;
            property.FindPropertyRelative("currentAmmoCount").intValue = 0;
        }

        //draw information        
        EditorGUI.LabelField(magNameLabelRect, "Magazine Name");
        EditorGUI.LabelField(magNameValueRect, property.FindPropertyRelative("itemName").stringValue, EditorStyles.boldLabel);

        string ammoName = property.FindPropertyRelative("currentAmmo") != null && !string.IsNullOrEmpty(property.FindPropertyRelative("currentAmmo").FindPropertyRelative("ammoName").stringValue) ?
            property.FindPropertyRelative("currentAmmo").FindPropertyRelative("ammoName").stringValue :
            "No Ammo";

        EditorGUI.LabelField(magCurrentAmmoNameLabelRect, "Current Ammo");
        EditorGUI.LabelField(magCurrentAmmoNameValueRect, ammoName, EditorStyles.boldLabel);

        EditorGUI.LabelField(magCapacityLabelRect, "Capacity");
        EditorGUI.LabelField(magCapacityValueRect, property.FindPropertyRelative("currentAmmoCount").intValue.ToString() + '/' + property.FindPropertyRelative("capacity").intValue.ToString(), EditorStyles.boldLabel);

        EditorGUI.LabelField(magCaliberLabelRect, "Caliber");
        EditorGUI.LabelField(magCaliberValueRect, property.FindPropertyRelative("caliber").stringValue, EditorStyles.boldLabel);

        EditorGUI.LabelField(magWeightLabelRect, "Weight");
        EditorGUI.LabelField(magWeightValueRect, property.FindPropertyRelative("itemWeight").floatValue.ToString(), EditorStyles.boldLabel);

        string ammoPrefabsPath = magDatabase.PrefabsPath;
        EditorGUI.LabelField(magPrefabLabelRect, "Prefab");
        EditorGUI.LabelField(magPrefabValueRect, ammoPrefabsPath + property.FindPropertyRelative("prefabName").stringValue, EditorStyles.boldLabel);

        // Set indent back to what it was
        EditorGUI.indentLevel = initialIndent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 176;

        //if (property.objectReferenceValue == null)
        //    height = 40;

        return height;
    }
}