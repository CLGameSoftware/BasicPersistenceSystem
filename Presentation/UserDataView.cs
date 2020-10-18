using UnityEngine.UI;
using UnityEngine;
using Firebase.Database;
using Assets.Scritps;
using System.Text;

public class UserDataView : MonoBehaviour  
{    
    internal static UserDataView globalAcess;

    [SerializeField]
    Text txtUsers;

    private void Awake()
    {
        if (globalAcess == null)
            globalAcess = this;
        else
            Destroy(globalAcess);
    }

    internal async void UpdateUserDataObjectsTextAsync() 
    {
        StringBuilder allUsersText = new StringBuilder();
        DataSnapshot dataSnapshot = await FirebaseDataService.GlobalAcess().GetSnapshotAsync(StoredVariables.UserCollectionName);
        string json = string.Empty;

        foreach (var element in dataSnapshot.Children)
        {
            json = element.GetRawJsonValue();
            UserDataObject user = JsonUtility.FromJson<UserDataObject>(json);
            allUsersText.AppendLine(string.Concat(element.Key, ". ", user.ToString()));
        }

        txtUsers.text = allUsersText.ToString();
    }
}
