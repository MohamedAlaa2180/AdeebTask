using Cysharp.Threading.Tasks;
using Firebase;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GroupsPanelScreenView : MonoBehaviour
{
    [SerializeField] private GroupItemView groupItemViewPrefab;
    [SerializeField] private Transform groupsContainer;
    [SerializeField] private Button createGroupButton;
    private List<GroupItemView> activeGroups = new List<GroupItemView>();

    private void Awake()
    {
        if (SessionData.CurrentUser.Role == UserRole.teacher)
        {
            createGroupButton.gameObject.SetActive(true);
        }
        else
        {
            createGroupButton.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        FirebaseManager.OnGroupCreated += OnGroupCreated;
    }

    private void OnDisable()
    {
        FirebaseManager.OnGroupCreated -= OnGroupCreated;
    }

    private void Start()
    {
        RefreshGroups();
    }

    private async void RefreshGroups()
    {
        foreach (Transform child in groupsContainer)
        {
            Destroy(child.gameObject);
        }
        activeGroups.Clear();

        await UniTask.WaitUntil(() => SessionData.CurrentUser.UserId != "");

        var groups = await FirebaseManager.GetGroups(SessionData.CurrentUser.UserId);

        Debug.Log($"Groups count: {groups.Count}");

        foreach (var group in groups)
        {
            var groupItemView = Instantiate(groupItemViewPrefab, groupsContainer);
            activeGroups.Add(groupItemView);
            groupItemView.Init(group);
        }
    }

    private void OnGroupCreated(GroupData data)
    {
        var groupItemView = Instantiate(groupItemViewPrefab, groupsContainer);
        activeGroups.Add(groupItemView);
        groupItemView.Init(data);
    }
}