using System.Reflection;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public static class GlobalVariableAccessor
{
    /// <summary>
    /// Получить значение глобальной переменной по имени поля.
    /// Например, variableName = "wood_amount".
    /// </summary>
    public static int GetGlobalValue(string variableName)
    {
        var field = typeof(GlobalController).GetField(variableName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
        {
            return (int)field.GetValue(GlobalController.Instance);
        }
        else
        {
            Debug.LogWarning($"Поле {variableName} не найдено в GlobalController.");
            return 0;
        }
    }

    /// <summary>
    /// Добавить значение к глобальной переменной по имени поля.
    /// Например, variableName = "wood_amount".
    /// </summary>
    public static void AddGlobalValue(string variableName, int value)
    {
        var field = typeof(GlobalController).GetField(variableName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
        {
            int current = (int)field.GetValue(GlobalController.Instance);
            field.SetValue(GlobalController.Instance, current + value);
        }
        else
        {
            Debug.LogWarning($"Поле {variableName} не найдено в GlobalController.");
        }
    }

    /// <summary>
    /// Установить конкретное значение в глобальную переменную по имени поля.
    /// </summary>
    public static void SetGlobalValue(string variableName, int value)
    {
        var field = typeof(GlobalController).GetField(variableName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
        {
            field.SetValue(GlobalController.Instance, value);
        }
        else
        {
            Debug.LogWarning($"Поле {variableName} не найдено в GlobalController.");
        }
    }
}
