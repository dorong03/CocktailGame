using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ToolController : MonoBehaviour
{

    [SerializeField]
    private ShakerTool shaker;
    [SerializeField]
    private BarSpoonTool barSpoon;

    private ToolBase currentTool;

    public void Arm(string tool, Func<bool> canStart,Action onBlocked, Action onStarted, Action onComplete)
    {
        currentTool = GetToolById(tool);
        currentTool.ActiveTool(canStart, onBlocked, onStarted, onComplete);
    }

    private ToolBase GetToolById(string id)
    {
        switch(id)
        {
            case "TOOL_000":
                return shaker;
            case "TOOL_001":
                return barSpoon;
            default:
                Debug.LogError($"존재하지 않는 도구 아이디 -> {id}");
                return null;
        }
    }

    public void Abort()
    {
        currentTool.Abort();
        currentTool = null;
    }
}
