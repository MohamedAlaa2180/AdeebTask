using Firebase;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateGroupScreenView : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField inputGroupName;

    public TMP_InputField inputGroupGrade;
    public TMP_InputField inputGroupSubject;
    public Button createGroupButton;
    public TextMeshProUGUI errorMessage;

    private void OnEnable()
    {
        createGroupButton.onClick.AddListener(OnCreateGroupButtonClicked);
    }

    private void OnDisable()
    {
        createGroupButton.onClick.RemoveListener(OnCreateGroupButtonClicked);
    }

    private async void OnCreateGroupButtonClicked()
    {
        try
        {
            await FirebaseManager.CreateGroup(inputGroupName.text.Trim(), inputGroupGrade.text.Trim(), inputGroupSubject.text.Trim());
            gameObject.SetActive(false);
        }
        catch (Exception e)
        {
            DisplayErrorMessage(e.Message);
            throw;
        }
    }

    private void DisplayErrorMessage(string message)
    {
        if (errorMessage != null)
        {
            errorMessage.text = message;
        }
    }
}