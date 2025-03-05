using Firebase.Auth;
using Firebase.Database;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using UnityEngine;

namespace Firebase
{
    public static class FirebaseManager
    {
        private static FirebaseAuthrization auth;
        private static DatabaseReference databaseReference;

        public static async void InitializeFirebase()
        {
            System.Environment.SetEnvironmentVariable("USE_AUTH_EMULATOR", "no");
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                auth = new FirebaseAuthrization();
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase Initialized");
            }
            else
            {
                Debug.LogError($"Firebase dependencies not resolved: {dependencyStatus}");
            }
        }

        public static async Task<FirebaseUser> RegisterUser(string email, string password, UserRole role, string nickName)
        {
            try
            {
                var user = await auth.RegisterUser(email, password, nickName);
                if (user != null) WriteNewUser(new UserData(user.UserId, user.Email, role.ToString()));

                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async Task<FirebaseUser> LoginUser(string email, string password)
        {
            try
            {
                return await auth.LoginUser(email, password);
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }


        public static void LogoutUser() => auth.LogoutUser();

        private static void WriteNewUser(UserData userData)
        {
            string json = JsonUtility.ToJson(userData);

            databaseReference.Child("users").Child(userData.userId).SetRawJsonValueAsync(json);
        }

        public static async Task CreateGroup(string groupName, string grade, string subject)
        {
            string groupId = databaseReference.Child("groups").Push().Key;
            GroupData newGroup = new GroupData(groupId, groupName, grade, subject, auth.CurrentUser.UserId);

            await databaseReference.Child("groups").Child(groupId).SetRawJsonValueAsync(JsonUtility.ToJson(newGroup));
            Debug.Log($"Group Created: {groupName}");
        }

        public static async Task GetGroups()
        {
            var dataSnapshot = await databaseReference.Child("groups").GetValueAsync();
            foreach (var group in dataSnapshot.Children)
            {
                Debug.Log($"Group: {group.Child("name").Value}");
            }
        }

        public static async Task AddStudentToGroup(string groupId, string studentId)
        {
            await databaseReference.Child("groups").Child(groupId).Child("students").Child(studentId).SetValueAsync(true);
            Debug.Log($"Added student {studentId} to Group {groupId}");
        }

        public static async Task RemoveStudentFromGroup(string groupId, string studentId)
        {
            await databaseReference.Child("groups").Child(groupId).Child("students").Child(studentId).RemoveValueAsync();
            Debug.Log($"Removed student {studentId} from Group {groupId}");
        }
    }

    public struct UserData
    {
        public string userId;
        public string email;
        public string role;

        public UserData(string userId, string email, string role)
        {
            this.userId = userId;
            this.email = email;
            this.role = role;
        }
    }

    public struct GroupData
    {
        public string groupId;
        public string groupName;
        public string grade;
        public string subject;
        public string teacherId;

        public GroupData(string groupId, string groupName, string grade, string subject, string teacherId)
        {
            this.groupId = groupId;
            this.groupName = groupName;
            this.grade = grade;
            this.subject = subject;
            this.teacherId = teacherId;
        }
    }

    public enum UserRole
    {
        teacher,
        student
    }
}