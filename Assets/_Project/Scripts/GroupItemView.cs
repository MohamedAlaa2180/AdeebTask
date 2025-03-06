using Firebase;
using TMPro;
using UnityEngine;

public class GroupItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI groupNameText;
    [SerializeField] private TextMeshProUGUI groupSubjectText;
    public void Init(GroupData groupData)
    {
        groupNameText.text = groupData.Name;
        groupSubjectText.text = groupData.Subject;
    }
}
