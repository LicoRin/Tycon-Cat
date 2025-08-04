using System.Reflection;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public static class GlobalVariableAccessor
{
    /// <summary>
    /// �������� �������� ���������� ���������� �� ����� ����.
    /// ��������, variableName = "wood_amount".
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
            Debug.LogWarning($"���� {variableName} �� ������� � GlobalController.");
            return 0;
        }
    }

    /// <summary>
    /// �������� �������� � ���������� ���������� �� ����� ����.
    /// ��������, variableName = "wood_amount".
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
            Debug.LogWarning($"���� {variableName} �� ������� � GlobalController.");
        }
    }

    /// <summary>
    /// ���������� ���������� �������� � ���������� ���������� �� ����� ����.
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
            Debug.LogWarning($"���� {variableName} �� ������� � GlobalController.");
        }
    }
}
