using Assets.Scritps;
using Assets.Scritps.Presentation;
using Assets.Scritps.Interface;
using System.Threading.Tasks;
using UnityEngine;

public class Events : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public async void InsertUserAsyncEvent()
    {
        int referenceId = InputFieldElements.globalAcess.GetIdInputFieldText();

        if (await CannotInsertAsync(referenceId))
            return;

        IDataObject user = GetUserDataObjectByInputFieldElements();

        try
        {
            FirebaseDataService.GlobalAcess().PostAsync(referenceId.ToString(), user);
            OutputLog.globalAcess.UpdateOutpugLog("Inserido com sucesso.");
        }
        catch
        {
            OutputLog.globalAcess.UpdateOutpugLog("Falha ao inserir.");
        }
    }

    public async void UpdateUserAsyncEvent()
    {
        int refereceId = InputFieldElements.globalAcess.GetIdInputFieldText();

        if (await CannotDeleteOrUpdateAsync(refereceId))
            return;

        IDataObject user = GetUserDataObjectByInputFieldElements();

        try
        {
            FirebaseDataService.GlobalAcess().PostAsync(refereceId.ToString(), user);
            OutputLog.globalAcess.UpdateOutpugLog("Atualizado com sucesso.");
        }
        catch
        {
            OutputLog.globalAcess.UpdateOutpugLog("Falha ao atualizar.");
        }
    }

    private async Task<UserDataObject> GetUserAsync(string referenceIdString)
    {
        return await FirebaseDataService.GlobalAcess().GetAsync<UserDataObject>(StoredVariables.UserCollectionName, referenceIdString);
    }

    internal async void GetUserAsyncEvent()
    {
        int refereceId = InputFieldElements.globalAcess.GetIdInputFieldText();

        if (IsNotInputIdCorrectlyFilled(refereceId))
            return;

        UserDataObject data = await GetUserAsync(refereceId.ToString());

        if (data == null)
        {
            OutputLog.globalAcess.UpdateOutpugLog("Id inexistente.");
            return;
        }

        InputFieldElements.globalAcess.FillFieldsByUserDataObject(data);
        OutputLog.globalAcess.UpdateOutpugLog("Usuário obtido.");
    }

    internal async void DeleteUserAsyncEvent()
    {
        int refereceId = InputFieldElements.globalAcess.GetIdInputFieldText();

        if (await CannotDeleteOrUpdateAsync(refereceId))
            return;

        FirebaseDataService.GlobalAcess().DeleteDataObjectAsync(StoredVariables.UserCollectionName, refereceId.ToString());
        OutputLog.globalAcess.UpdateOutpugLog("Usuário deletado.");
    }

    public void CallGetAllSceneEvent()
    {
        SceneModifier.LoadSceneBySceneName("GetAll");
    }

    public void CallSampleSceneEvent()
    {
        SceneModifier.LoadSceneBySceneName("SampleScene");
    }

    public async void GetAllUserDataObjectsAsyncEvent()
    {
        UserDataView.globalAcess.UpdateUserDataObjectsTextAsync();
    }

    #region Private Utilities

    private UserDataObject GetUserDataObjectByInputFieldElements() => new UserDataObject(InputFieldElements.globalAcess.GetNameInputFieldText(),
                                                                                         InputFieldElements.globalAcess.GetEmailInputFieldText());

    private async Task<bool> CannotInsertAsync(int referenceId)
    {
        if (InputFieldElements.globalAcess.AllFieldsAreNotFilled() || referenceId == default(byte))
        {
            OutputLog.globalAcess.UpdateOutpugLog("Preencha todos os campos corretamente.");
            return true;
        }

        if (await GetUserAsync(referenceId.ToString()) != null)
        {
            int nextAvailableId = await FirebaseDataService.GlobalAcess().GetNextAvailableId(StoredVariables.UserCollectionName);
            OutputLog.globalAcess.UpdateOutpugLog(string.Concat("Id já existente. Tente ", nextAvailableId, "."));
            return true;
        }

        return false;
    }

    private bool IsNotInputIdCorrectlyFilled(int referenceId)
    {
        if (referenceId == default(byte))
        {
            OutputLog.globalAcess.UpdateOutpugLog("Informe o id.");
            return true;
        }

        return false;
    }

    private async Task<bool> CannotDeleteOrUpdateAsync(int referenceId)
    {
        if (IsNotInputIdCorrectlyFilled(referenceId))
            return true;

        if (await GetUserAsync(referenceId.ToString()) == null)
        {
            OutputLog.globalAcess.UpdateOutpugLog("Id inexistente.");
            return true;
        }

        return false;
    }


    #endregion

}
