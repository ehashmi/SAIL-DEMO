using UnityEngine;
using TMPro;
using System.Text;
using System.Collections.Generic;

public class TMPDebugger : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI debugText;

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.F1;
    public int maxLogs = 100;

    private List<string> logs = new List<string>();
    private bool isVisible = true;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isVisible = !isVisible;
            debugText.gameObject.SetActive(isVisible);
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string color = "white";

        switch (type)
        {
            case LogType.Warning:
                color = "yellow";
                break;
            case LogType.Error:
            case LogType.Exception:
                color = "red";
                break;
        }

        string formattedLog = $"<color={color}>{logString}</color>";

        logs.Add(formattedLog);

        if (logs.Count > maxLogs)
            logs.RemoveAt(0);

        UpdateText();
    }

    void UpdateText()
    {
        StringBuilder builder = new StringBuilder();

        foreach (var log in logs)
        {
            builder.AppendLine(log);
        }

        debugText.text = builder.ToString();
    }
}
