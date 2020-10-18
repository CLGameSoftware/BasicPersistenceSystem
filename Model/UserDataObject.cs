using Assets.Scritps.Interface;

public class UserDataObject : IDataObject
{
    public string Name;
    public string Email;

    public UserDataObject(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public string GetCollectionName() => "User";
    public override string ToString()
    {
        return string.Concat(Name, ", ", Email);
    }
}
