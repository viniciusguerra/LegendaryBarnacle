using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
    public static T CopyComponent<T>(this GameObject destination, T original) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }

        return copy as T;
    }
}
