using Firebase;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WelcomeScreenView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI welcomeText;
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private Button logoutButton;

    private void Start()
    {
        welcomeText.text = $"Welcome, {SessionData.CurrentUser.Name}!";
        idText.text = $"ID: {SessionData.CurrentUser.UserId}";
    }

    private void OnEnable()
    {
        logoutButton.onClick.AddListener(OnLogoutButtonClicked);
    }

    private void OnDisable()
    {
        logoutButton.onClick.RemoveListener(OnLogoutButtonClicked);
    }

    private void OnLogoutButtonClicked()
    {
        FirebaseManager.LogoutUser();
        SceneManager.LoadScene("LoginRegisterScene");
    }
}