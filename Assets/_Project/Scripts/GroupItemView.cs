using Firebase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroupItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI groupNameText;
    [SerializeField] private TextMeshProUGUI groupSubjectText;
    [SerializeField] private Button editButton;
    private GroupData groupData;

    public void Init(GroupData groupData)
    {
        this.groupData = groupData;
        groupNameText.text = groupData.Name;
        groupSubjectText.text = groupData.Subject;
        editButton.onClick.AddListener(OpenEditScreen);

        if (SessionData.CurrentUser.Role == UserRole.teacher)
        {
            editButton.gameObject.SetActive(true);
        }
        else
        {
            editButton.gameObject.SetActive(false);
        }
    }

    private void OpenEditScreen()
    {
        GroupEditScreenView groupEditScreenView =  (GroupEditScreenView)HomeScreensManager.Instance.ShowScreen(HomeScreensEnum.GroupEditScreen);
        HomeScreensManager.Instance.HideScreen(HomeScreensEnum.GroupsPanelScreen);

        groupEditScreenView.Init(groupData);
    }

    private void OnDisable()
    {
        editButton.onClick.RemoveListener(OpenEditScreen);
    }
}
