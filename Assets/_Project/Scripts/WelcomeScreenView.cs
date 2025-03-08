using TMPro;
using UnityEngine;

public class WelcomeScreenView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI welcomeText;
    private void Start()
    {
        welcomeText.text = $"Welcome, {SessionData.CurrentUser.Name}!";

    }
}
