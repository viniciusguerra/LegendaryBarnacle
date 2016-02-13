using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

[CustomPropertyDrawer(typeof(MagazineData))]
public class MagazineDrawer : PropertyDrawer
{
    //for database selection popup
    int selectedMagazineIndex = 0;
    int selectedAmmoIndex = 0;
    bool setValues = false;
    bool load = false;
    bool empty = false;

    bool hasValues;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MagazineData valueCheck = FindMagazineDataInTargetProperty(property);

        if (valueCheck == null || string.IsNullOrEmpty(valueCheck.itemName))
            hasValues = false;
        else
            hasValues = true;

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
        string[] magNamesFromDatabase = new string[magTypesFromDatabase.Length + 2];
        magNamesFromDatabase[0] = "No Change";
        magNamesFromDatabase[1] = "Null";

        for (int i = 2; i < magTypesFromDatabase.Length + 2; i++)
        {
            magNamesFromDatabase[i] = magTypesFromDatabase[i - 2].ItemName;
        }

        AmmoDatabase ammoDatabase = new AmmoDatabase();
        AmmoData[] ammoTypesFromDatabase = ammoDatabase.Load().ToArray();
        string[] ammoNamesFromDatabase = new string[ammoTypesFromDatabase.Length + 1];
        ammoNamesFromDatabase[0] = "No Change";

        for (int i = 1; i < ammoTypesFromDatabase.Length + 1; i++)
        {
            ammoNamesFromDatabase[i] = ammoTypesFromDatabase[i - 1].ammoName;
        }

        if (!hasValues)
        {
            setButtonRect.y = magCapacityLabelRect.y;
            magazinePopupRect.y = setButtonRect.y;
        }

        setValues = EditorGUI.ToggleLeft(setButtonRect, "Set Magazine", setValues);        
        
        if (setValues)
        {
            empty = false;
            load = false;

            //draw database selection popup
            int newIndex = EditorGUI.Popup(magazinePopupRect, selectedMagazineIndex, magNamesFromDatabase);

            if (newIndex != selectedMagazineIndex && newIndex != 0)
            {
                //set current property values to selected database item
                MagazineData value = newIndex == 1 ? null : magTypesFromDatabase[newIndex - 2];

                SetMagazineDataInTargetProperty(property, value);

                setValues = false;            
            }

            selectedMagazineIndex = 0;
        }

        if (hasValues)
        {
            load = EditorGUI.ToggleLeft(loadButtonRect, "Load", load);

            if (load)
            {
                setValues = false;
                empty = false;

                MagazineData targetMagData = FindMagazineDataInTargetProperty(property);

                //draw database selection popup
                int newIndex = EditorGUI.Popup(ammoPopupRect, selectedAmmoIndex, ammoNamesFromDatabase);

                if (newIndex != selectedAmmoIndex && newIndex != 0)
                {
                    targetMagData.CurrentAmmo = ammoTypesFromDatabase[newIndex - 1];
                    targetMagData.CurrentAmmoCount = targetMagData.Capacity;

                    load = false;
                }

                selectedAmmoIndex = 0;
            }

            empty = EditorGUI.ToggleLeft(emptyButtonRect, "Clear Ammo", empty);

            if (empty)
            {
                empty = false;
                setValues = false;
                load = false;

                //((MagazineData)fieldInfo.GetValue(property.serializedObject.targetObject)).CurrentAmmo = null;
                //property.FindPropertyRelative("currentAmmoCount").intValue = 0;

                MagazineData targetMagData = FindMagazineDataInTargetProperty(property);

                //Debug.Log(targetMagData == null ? "MagazineData not found" : targetMagData.itemName + " found!");

                targetMagData.CurrentAmmo = null;
                targetMagData.CurrentAmmoCount = 0;
            }
        }

        if (hasValues)
        {
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
        }
        else
        {
            EditorGUI.LabelField(magNameLabelRect, "Magazine is null");
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = initialIndent;

        EditorGUI.EndProperty();
    }

    //TODO: get the actual magazine value from array
    private static MagazineData FindMagazineDataInTargetProperty(SerializedProperty property)
    {
        MagazineData targetMagData = null;

        PropertyInfo[] objectProperties = property.serializedObject.targetObject.GetType().GetProperties();

        foreach (PropertyInfo info in objectProperties)
        {
            if (info.PropertyType.Equals(typeof(MagazineData)))
            {
                targetMagData = info.GetValue(property.serializedObject.targetObject, null) as MagazineData;
            }
            else
            {
                if (info.PropertyType.Equals(typeof(ItemData)))
                {
                    PropertyInfo[] dataProperties = info.GetType().GetProperties();

                    PropertyInfo containerProperty = Array.Find(dataProperties, x => x.PropertyType.Equals(typeof(MagazineData)));

                    if (containerProperty != null)
                    {
                        object containedMagData = info.GetValue(property.serializedObject.targetObject, null);

                        if (containedMagData != null)
                            targetMagData = containedMagData as MagazineData;
                    }
                }
            }
        }

        return targetMagData;
    }

    private static void SetMagazineDataInTargetProperty(SerializedProperty property, MagazineData value)
    {
        PropertyInfo[] objectProperties = property.serializedObject.targetObject.GetType().GetProperties();

        foreach (PropertyInfo info in objectProperties)
        {
            if (info.PropertyType.Equals(typeof(MagazineData)))
            {
                info.SetValue(property.serializedObject.targetObject, value, null);
            }
            else
            {
                if (info.PropertyType.Equals(typeof(ItemData)))
                {
                    PropertyInfo[] dataProperties = info.GetType().GetProperties();

                    PropertyInfo containerProperty = Array.Find(dataProperties, x => x.PropertyType.Equals(typeof(MagazineData)));

                    if (containerProperty != null)
                    {
                        containerProperty.SetValue(property.serializedObject.targetObject, value, null);                        
                    }
                }
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height;

        if (FindMagazineDataInTargetProperty(property) != null)
            height = 176;
        else
            height = 64;

        return height;
    }
}