using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScreenView : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField inputEmail;
    public TMP_InputField inputPassword;
    public TMP_InputField inputName;
    public Toggle toggleStudent;
    public Toggle toggleTeacher;
    public TextMeshProUGUI textMessage;
    public Button buttonRegister;

    private void OnEnable()
    {
        buttonRegister.onClick.AddListener(OnRegisterButtonClicked);
    }

    private void OnDisable()
    {
        buttonRegister.onClick.RemoveListener(OnRegisterButtonClicked);
    }

    public void OnRegisterButtonClicked()
    {
        string email = inputEmail.text.Trim();
        string password = inputPassword.text.Trim();
        string name = inputName.text.Trim();

        // Determine user role
        UserRole role = toggleTeacher.isOn ? UserRole.teacher : UserRole.student; // Default to student if no selection

        // Check for empty fields
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
        {
            DisplayErrorMessage("Please fill in all fields.");
            return;
        }

        // Register user
        RegisterUser(email, password, name, role);
    }

    private async void RegisterUser(string email, string password, string name, UserRole role)
    {
        try
        {
            FirebaseUser user = await FirebaseManager.RegisterUser(email, password, role, name);
        }
        catch (System.Exception e)
        {
            DisplayErrorMessage($"Registration failed. {e.Message}");
            throw;
        }       
    }

    private void DisplayErrorMessage(string message)
    {
        if (textMessage != null)
        {
            textMessage.text = message;
        }
        Debug.Log(message);
    }
}
