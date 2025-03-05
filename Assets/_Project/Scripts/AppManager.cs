using Firebase;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    private static AppManager _instance;

    public static AppManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<AppManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("AppManager");
                    _instance = go.AddComponent<AppManager>();
                }
            }
            return _instance;
        }
    }

    private void Start()
    {
        FirebaseManager.InitializeFirebase();
    }

    [ContextMenu("LoginUser")]
    public async void RegisterUser()
    {
        await FirebaseManager.RegisterUser("mohamedalaasalem2@gmail.com", "123456", UserRole.student, "Alaa");
    }

    [ContextMenu("LoginUser")]
    public async void LoginUser()
    {
        await FirebaseManager.LoginUser("mohamedalaasalem2@gmail.com", "123456");
    }

    [ContextMenu("LogoutUser")]
    public void LogoutUser()
    {
        FirebaseManager.LogoutUser();
    }

    [ContextMenu("CreateGroup")]
    public async void CreateGroup()
    {
        await FirebaseManager.CreateGroup("Group 1", "Grade 1", "Math");
    }
}