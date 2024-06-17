using UnityEngine;

public enum DeveloperConsoleLogType
{
    ERROR,
    SUCCESS
}

public class DeveloperConsoleLog
{
    public string message;
    public Color color;

    public DeveloperConsoleLog(string message, DeveloperConsoleLogType type = DeveloperConsoleLogType.SUCCESS)
    {
        this.message = message;

        if (type == DeveloperConsoleLogType.ERROR)
        {
            color = Color.red;
        }
        else
        {
            color = Color.green;
        }
    }
}
