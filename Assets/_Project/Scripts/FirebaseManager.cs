using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Firebase
{
    public static class FirebaseManager
    {
        private static FirebaseAuthrization auth;
        private static FirebaseDB db;

        public static event Action<UserData> OnUserLoggedIn;

        public static event Action<UserData> OnUserLoggedOut;

        public static event Action<GroupData> OnGroupCreated;

        public static async void InitializeFirebase()
        {
          //  System.Environment.SetEnvironmentVariable("USE_AUTH_EMULATOR", "no");
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                auth = new FirebaseAuthrization();
                db = new FirebaseDB();
                Debug.Log("Firebase Initialized");
            }
            else
            {
                Debug.LogError($"Firebase dependencies not resolved: {dependencyStatus}");
            }
        }

        public static async UniTask<UserData> RegisterUser(string email, string password, UserRole role, string nickName)
        {
            try
            {
                var user = await auth.RegisterUser(email, password, nickName);
                var userData = new UserData(user.UserId, user.Email, user.DisplayName, role);
                await WriteNewUser(userData);
                OnUserLoggedIn?.Invoke(userData);
                return userData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async UniTask<UserData> LoginUser(string email, string password)
        {
            try
            {
                var user = await auth.LoginUser(email, password);
                var userData = await db.GetUser(user.UserId);
                OnUserLoggedIn?.Invoke(userData);
                return userData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async UniTask<GroupData> GetGroup(string groupId)
        {
            try
            {
                return await db.GetGroup(groupId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void LogoutUser() => auth.LogoutUser();

        private static async UniTask WriteNewUser(UserData userData)
        {
            await db.WriteNewUser(userData);
        }

        public static async UniTask CreateGroup(string groupName, string grade, string subject)
        {
            try
            {
                var newGroup = await db.CreateGroup(groupName, grade, subject, auth.CurrentUser.UserId);
                var teacher = await db.GetUser(newGroup.TeacherId);

                await AssignGroupToUser(teacher.UserId, newGroup.Id);

                OnGroupCreated?.Invoke(newGroup);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async UniTask DeleteGroup(string groupId)
        {
            try
            {
                await db.DeleteGroup(groupId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async UniTask AssignGroupToUser(string userId, string groupId)
        {
            try
            {
                await db.AssignGroupToUser(userId, groupId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async UniTask RemoveGroupFromUser(string userId, string groupId)
        {
            try
            {
                await db.RemoveGroupFromUser(userId, groupId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async UniTask<List<GroupData>> GetGroups(string userId)
        {
            try
            {
                return await db.GetGroups(userId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async UniTask AddStudentToGroup(string groupId, string studentId)
        {
            try
            {
                await db.AddStudentToGroup(groupId, studentId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async UniTask RemoveStudentFromGroup(string groupId, string studentId)
        {
            try
            {
                await db.RemoveStudentFromGroup(groupId, studentId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    [Serializable]
    public class UserData
    {
        [JsonProperty("id")]
        public string UserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("role")]
        public UserRole Role { get; set; }

        [JsonProperty("groupsIds")]
        public List<string> groups { get; set; }

        public UserData(string userId, string email, string name, UserRole role)
        {
            UserId = userId;
            Email = email;
            Name = name;
            Role = role;
            groups = new List<string>();
        }

        public void AssignGroup(string groupId)
        {
            groups.Add(groupId);
        }
    }

    [System.Serializable]
    public class GroupData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("grade")]
        public string Grade { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("teacherId")]
        public string TeacherId { get; set; }

        [JsonProperty("students")]
        public List<string> StudentsIds { get; set; }

        public GroupData(string groupId, string groupName, string grade, string subject, string teacherId)
        {
            Id = groupId;
            Name = groupName;
            Grade = grade;
            Subject = subject;
            TeacherId = teacherId;
            StudentsIds = new List<string>();
        }

        public void AddStudent(string studentId)
        {
            StudentsIds.Add(studentId);
        }

        public void RemoveStudent(string studentId)
        {
            StudentsIds.Remove(studentId);
        }
    }

    public enum UserRole
    {
        teacher,
        student
    }
}