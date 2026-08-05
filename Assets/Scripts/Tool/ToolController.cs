using System;
using UnityEngine;

public class ToolController : MonoBehaviour
{

    [SerializeField]
    private ShakerTool shaker;
    [SerializeField]
    private BarSpoonTool barSpoon;

    private ToolBase currentTool;
    
    public void BeginStart(Func<bool> canStart,Action onBlocked, Action onStarted, Action onComplete)
    {
        currentTool.ActiveTool(canStart, onBlocked, onStarted, onComplete);
    }

    public void ShowTool(string id)
    {
        currentTool = GetToolById(id);
        currentTool.Show();
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
                Debug.LogError($"�������� �ʴ� ���� ���̵� -> {id}");
                return null;
        }
    }

    public void Abort()
    {
        if (currentTool != null)
        {
            currentTool.Abort();
            currentTool.Hide();
            currentTool = null;   
        }
    }
}
