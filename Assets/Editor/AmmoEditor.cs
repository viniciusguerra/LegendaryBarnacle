﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(AmmoData))]
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

        var ammoNameLabelRect = new Rect(position.x, height, valueWidth, fieldHeight);
        var ammoNameValueRect = new Rect(position.x + columnWidth, height, valueWidth, fieldHeight);
        height += fieldHeight;

        var ammoCaliberLabelRect = new Rect(position.x, height, valueWidth, fieldHeight);
        var ammoCaliberValueRect = new Rect(position.x + columnWidth, height, valueWidth, fieldHeight);
        height += fieldHeight;            

        var ammoDamageLabelRect = new Rect(position.x, height, valueWidth, fieldHeight);
        var ammoDamageValueRect = new Rect(position.x + columnWidth, height, valueWidth, fieldHeight);
        height += fieldHeight;

        var ammoPenetrationLabelRect = new Rect(position.x, height, valueWidth, fieldHeight);
        var ammoPenetrationValueRect = new Rect(position.x + columnWidth, height, valueWidth, fieldHeight);
        height += fieldHeight;

        var ammoStoppingPowerLabelRect = new Rect(position.x, height, valueWidth, fieldHeight);
        var ammoStoppingPowerValueRect = new Rect(position.x + columnWidth, height, valueWidth, fieldHeight);
        height += fieldHeight;

        var ammoWeightLabelRect = new Rect(position.x, height, valueWidth, fieldHeight);
        var ammoWeightValueRect = new Rect(position.x + columnWidth, height, valueWidth, fieldHeight);
        height += fieldHeight;

        var ammoPrefabLabelRect = new Rect(position.x, height, valueWidth, fieldHeight);
        var ammoPrefabValueRect = new Rect(position.x + columnWidth, height, valueWidth, fieldHeight);
        height += fieldHeight * 2;

        var ammoPopupRect = new Rect(position.x + columnWidth, height, valueWidth / 2, fieldHeight);
        var setButtonRect = new Rect(position.x, height, valueWidth / 3, fieldHeight);

        //load ammo types from database       
        AmmoDatabase ammoDatabase = new AmmoDatabase();
        AmmoData[] ammoTypesFromDatabase = ammoDatabase.Load().ToArray();
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
                property.FindPropertyRelative("damage").floatValue = ammoTypesFromDatabase[newIndex - 1].damage;
                property.FindPropertyRelative("penetration").floatValue = ammoTypesFromDatabase[newIndex - 1].penetration;
                property.FindPropertyRelative("stoppingPower").floatValue = ammoTypesFromDatabase[newIndex - 1].stoppingPower;
                property.FindPropertyRelative("weight").floatValue = ammoTypesFromDatabase[newIndex - 1].weight;
                property.FindPropertyRelative("prefabName").stringValue = ammoTypesFromDatabase[newIndex - 1].prefabName;

                setValues = false;
            }

            selectedIndex = 0;
        }

        //draw information        
        EditorGUI.LabelField(ammoNameLabelRect, "Ammo Name");
        EditorGUI.LabelField(ammoNameValueRect, property.FindPropertyRelative("ammoName").stringValue, EditorStyles.boldLabel);

        EditorGUI.LabelField(ammoCaliberLabelRect, "Caliber");
        EditorGUI.LabelField(ammoCaliberValueRect, property.FindPropertyRelative("caliber").stringValue, EditorStyles.boldLabel);            

        EditorGUI.LabelField(ammoDamageLabelRect, "Damage");
        EditorGUI.LabelField(ammoDamageValueRect, property.FindPropertyRelative("damage").floatValue.ToString(), EditorStyles.boldLabel);

        EditorGUI.LabelField(ammoPenetrationLabelRect, "Penetration");
        EditorGUI.LabelField(ammoPenetrationValueRect, property.FindPropertyRelative("penetration").floatValue.ToString(), EditorStyles.boldLabel);

        EditorGUI.LabelField(ammoStoppingPowerLabelRect, "Stopping Power");
        EditorGUI.LabelField(ammoStoppingPowerValueRect, property.FindPropertyRelative("stoppingPower").floatValue.ToString(), EditorStyles.boldLabel);

        EditorGUI.LabelField(ammoWeightLabelRect, "Weight");
        EditorGUI.LabelField(ammoWeightValueRect, property.FindPropertyRelative("weight").floatValue.ToString(), EditorStyles.boldLabel);

        string ammoPrefabsPath = ammoDatabase.PrefabsPath;
        EditorGUI.LabelField(ammoPrefabLabelRect, "Prefab");
        EditorGUI.LabelField(ammoPrefabValueRect, ammoPrefabsPath + property.FindPropertyRelative("prefabName").stringValue, EditorStyles.boldLabel);
        

        // Set indent back to what it was
        EditorGUI.indentLevel = initialIndent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 160;

        //if (property.objectReferenceValue == null)
        //    height = 40;

        return height;
    }
}