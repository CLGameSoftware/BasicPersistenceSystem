using System;
using UnityEngine;
using UnityEngine.UI;
public class InputFieldElements : MonoBehaviour
{
    [SerializeField]
    private InputField ipfId;
    
    [SerializeField]
    private InputField ipfName;

    [SerializeField]
    private InputField ipfEmail;

    internal static InputFieldElements globalAcess;

    private void Awake()
    {
        if (globalAcess == null)
            globalAcess = this;
        else
            Destroy(this);
    }

    internal int GetIdInputFieldText()
    {
        int defaultResult = default(byte);
        string ipfIdLiteral = ipfId.text.Trim();
        return int.TryParse(ipfIdLiteral, out defaultResult) ? Convert.ToInt32(ipfIdLiteral) : defaultResult;
    }
    
    internal string GetNameInputFieldText() => ipfName.text.Trim();
    internal string GetEmailInputFieldText() => ipfEmail.text.ToLower().Trim();

    internal bool AllFieldsAreNotFilled()
    {
        return GetIdInputFieldText() == default(byte) 
            || string.IsNullOrEmpty(GetNameInputFieldText()) 
            || string.IsNullOrEmpty(GetEmailInputFieldText());
    }

    internal void FillFieldsByUserDataObject(UserDataObject data)
    {
        ipfName.text = data.Name;
        ipfEmail.text = data.Email;
    }
}
