using TMPro;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    public static DebugLogger Instance { get; set; }

    private int logCount = 0;

    [SerializeField]
    public TextMeshProUGUI textField;

    private void Awake()
    {
        Instance = this;
    }

    public void Log(string msg)
    {
        Debug.Log(msg);
        textField.text += $"{msg}\n";
        if (logCount > 15)
        {
            textField.text = textField.text.Substring(textField.text.IndexOf("\n") + 1);
        }
        else
        {
            logCount++;
        }
    }
}
