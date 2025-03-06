using Firebase;
using UnityEngine;

public static class SessionData
{
    public static UserData CurrentUser { get; set; }

    public static void SetCurrentUser(UserData user)
    {
        CurrentUser = user;
    }
}
