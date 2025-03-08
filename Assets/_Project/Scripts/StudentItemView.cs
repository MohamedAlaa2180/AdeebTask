using Firebase;
using TMPro;
using UnityEngine;

public class StudentItemView : MonoBehaviour
{
   // [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI idText;

    public void Init(string studentId)
    {
       // nameText.text = studentData.Name;
        idText.text = studentId;
    }
}
