using Firebase.Auth;
using Firebase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreenView : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField inputEmail;
    public TMP_InputField inputPassword;
    public TextMeshProUGUI errorMessage;
    public Button buttonLogin;

    private void Awake()
    {
        inputPassword.contentType = TMP_InputField.ContentType.Password;
    }

    private void OnEnable()
    {
        buttonLogin.onClick.AddListener(OnLoginButtonClicked);
    }

    private void OnDisable()
    {
        buttonLogin.onClick.RemoveListener(OnLoginButtonClicked);
    }

    public void OnLoginButtonClicked()
    {
        string email = inputEmail.text.Trim();
        string password = inputPassword.text.Trim();

        // Check for empty fields
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            DisplayErrorMessage("Please fill in all fields.");
            return;
        }

        // Register user
        LoginUser(email, password);
    }

    private async void LoginUser(string email, string password)
    {
        try
        {
            await FirebaseManager.LoginUser(email, password);
        }
        catch (System.Exception e)
        {
            DisplayErrorMessage($"Login failed. {e.Message}");
            throw;
        }
    }

    private void DisplayErrorMessage(string message)
    {
        if (errorMessage != null)
        {
            errorMessage.text = message;
        }
        Debug.Log(message);
    }
}
