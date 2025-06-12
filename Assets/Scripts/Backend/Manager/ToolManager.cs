using UnityEngine;
using System.Collections.Generic;

public class ToolManager : MonoBehaviour
{
    public ToolController toolController;

    void Start()
    {
        if (toolController == null)
            toolController = FindObjectOfType<ToolController>();

        StartCoroutine(toolController.GetAllTools(OnToolsReceived));
    }

    void OnToolsReceived(List<Tool> tools)
    {
        foreach (var t in tools)
            Debug.Log("Tool: " + t.name);
    }
}