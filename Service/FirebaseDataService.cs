using UnityEngine;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;
using Assets.Scritps.Interface;
using System;
using System.Threading.Tasks;
using System.Linq;

internal class FirebaseDataService : MonoBehaviour
{
    private const string databaseUrl = "https://basic-persistence-prototype-v1.firebaseio.com/";

    internal static FirebaseDataService instance;
    private DatabaseReference dataReference;


    internal static FirebaseDataService GlobalAcess()
    {
        if (instance == null)
            instance = new FirebaseDataService();

        return instance;
    }

    private FirebaseDataService()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(databaseUrl);
        InitializeDatabaseService();
    }

    private void InitializeDatabaseService()
    {
        try
        {
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(databaseUrl);
            dataReference = FirebaseDatabase.DefaultInstance.RootReference;
        }
        catch (Exception e)
        {
            Debug.LogError("Falha ao atualizar referência ao firebse. Erro: " + e);
        }
    }

    internal async void PostAsync(string referenceIdString, IDataObject data)
    {
        string json = JsonUtility.ToJson(data);
        await dataReference.Child(data.GetCollectionName()).Child(referenceIdString).SetRawJsonValueAsync(json);
    }

    internal async Task<DataSnapshot> GetSnapshotAsync(string collectionName) => await FirebaseDatabase.DefaultInstance.GetReference(collectionName).GetValueAsync();
    
    internal async Task<T> GetAsync<T>(string collectionName, string idReferenceString) where T : IDataObject
    {
        DataSnapshot snapshot = await GetSnapshotAsync(collectionName);
        string json = snapshot.Child(idReferenceString).GetRawJsonValue();
        return JsonUtility.FromJson<T>(json);
    }

    internal async Task<int> GetNextAvailableId(string collectionName)
    {
        DataSnapshot snapshot = await GetSnapshotAsync(collectionName);
        var lastInsertedDataObject = snapshot.Children.OrderByDescending(x => x.Key).First();
        int nextAvailableId = Convert.ToInt32(lastInsertedDataObject.Key) + 1;
        return nextAvailableId;
    }

    internal async void DeleteDataObjectAsync(string collectionName, string idReferenceString)
    {
        await dataReference.Child(collectionName).Child(idReferenceString).RemoveValueAsync();
    }
}
