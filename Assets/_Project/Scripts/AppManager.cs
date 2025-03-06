using Firebase;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : GenericSingletonClass<AppManager>
{
    private void OnEnable()
    {
        FirebaseManager.OnUserLoggedIn += OnUserLoggedIn;
        FirebaseManager.OnUserLoggedOut += OnUserLoggedOut;
    }

    private void OnDisable()
    {
        FirebaseManager.OnUserLoggedIn -= OnUserLoggedIn;
        FirebaseManager.OnUserLoggedOut -= OnUserLoggedOut;
    }

    private void OnUserLoggedIn(UserData userData)
    {
        SessionData.SetCurrentUser(userData);
        SceneManager.LoadScene("HomeScene");
    }

    private void OnUserLoggedOut(UserData userData)
    {
        SceneManager.LoadScene("LoginRegisterScene");
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