using UnityEngine;
using UnityEngine.UI;

public class OutputLog : MonoBehaviour
{
    const string constantSection = "Output: ";
    
    internal static OutputLog globalAcess;
        
    [SerializeField]
    private Text txtOutputLog;

    private void Awake()
    {
        if (globalAcess == null)
            globalAcess = this;
        else
            Destroy(this);
    }

    internal void UpdateOutpugLog(string currentLog)
    {
        txtOutputLog.text = string.Concat(constantSection, currentLog.Trim());
    }
}
