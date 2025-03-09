using Firebase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroupEditScreenView : BaseScreen
{
    [SerializeField] private TextMeshProUGUI groupNameText;
    public TMP_InputField inputStudentId;
    [SerializeField] private Button addStudentButton;
    [SerializeField] private Button removeStudentButton;
    [SerializeField] private Button deleteGroupButton;
    private GroupData groupData;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private StudentItemView studentItemViewPrefab;
    [SerializeField] private Transform studentsContainer;

    public void Init(GroupData groupData)
    {
        this.groupData = groupData;
        groupNameText.text = groupData.Name;
        RefreshStudentsList();
    }

    private void OnEnable()
    {
        addStudentButton.onClick.AddListener(OnAddStudentButtonClicked);
        removeStudentButton.onClick.AddListener(OnRemoveStudentButtonClicked);
        deleteGroupButton.onClick.AddListener(OnDeleteGroupButtonClicked);
        inputStudentId.text = "";
    }

    private void OnDisable()
    {
        addStudentButton.onClick.RemoveListener(OnAddStudentButtonClicked);
        removeStudentButton.onClick.RemoveListener(OnRemoveStudentButtonClicked);
        deleteGroupButton.onClick.RemoveListener(OnDeleteGroupButtonClicked);
    }

    private async void OnDeleteGroupButtonClicked()
    {
        // Delete group
        try
        {
            await FirebaseManager.DeleteGroup(groupData.Id);
            HideScreen();
            HomeScreensManager.Instance.ShowScreen(HomeScreensEnum.GroupsPanelScreen);
        }
        catch (System.Exception)
        {
            DisplayMessage("Failed to delete group");
            throw;
        }
        
    }

    private async void OnAddStudentButtonClicked()
    {
        string studentId = inputStudentId.text.Trim();
        if (!string.IsNullOrEmpty(studentId))
        {
            try
            {
                await FirebaseManager.AssignGroupToUser(studentId, groupData.Id);
                await FirebaseManager.AddStudentToGroup(groupData.Id, studentId);
                RefreshStudentsList();
                DisplayMessage("Student added to group.");
            }
            catch (System.Exception e)
            {
                DisplayMessage(e.Message);
                throw e;
            }
        }
    }

    private async void OnRemoveStudentButtonClicked()
    {
        string studentId = inputStudentId.text.Trim();
        if (!string.IsNullOrEmpty(studentId))
        {
            try
            {
                await FirebaseManager.RemoveGroupFromUser(studentId, groupData.Id);
                await FirebaseManager.RemoveStudentFromGroup(groupData.Id, studentId);
                RefreshStudentsList();
                DisplayMessage("Student removed from group.");
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        // Remove student from group
    }

    private async void RefreshStudentsList()
    {
        foreach (Transform child in studentsContainer)
        {
            Destroy(child.gameObject);
        }

        var data = await FirebaseManager.GetGroup(groupData.Id);
        foreach (var student in data.StudentsIds)
        {
            StudentItemView studentItemView = Instantiate(studentItemViewPrefab, studentsContainer);
            studentItemView.Init(student);
        }
    }

    private void DisplayMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
        Debug.Log(message);
    }
}