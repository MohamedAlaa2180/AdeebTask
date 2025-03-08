using Cysharp.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Firebase
{
    public class FirebaseDB
    {
        private DatabaseReference databaseReference;

        public FirebaseDB()
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public async UniTask WriteNewUser(UserData userData)
        {
            string json = JsonConvert.SerializeObject(userData);
            await databaseReference.Child("users").Child(userData.UserId).SetRawJsonValueAsync(json);
        }

        public async UniTask<UserData> GetUser(string userId)
        {
            try
            {
                DataSnapshot snapshot = await databaseReference.Child("users").Child(userId).GetValueAsync();

                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    UserData userData = JsonConvert.DeserializeObject<UserData>(json);
                    Debug.Log($"User Found: {userData.Name} ({userData.Role})");
                    return userData;
                }
                else
                {
                    Debug.LogWarning("User data not found in database.");
                    throw new Exception("User data not found in database.");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async UniTask<GroupData> GetGroup(string groupId)
        {
            try
            {
                DataSnapshot snapshot = await databaseReference.Child("groups").Child(groupId).GetValueAsync();

                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    GroupData groupData = JsonConvert.DeserializeObject<GroupData>(json);
                    Debug.Log($"Group Found: {groupData.Name})");
                    return groupData;
                }
                else
                {
                    Debug.LogWarning("Group data not found in database.");
                    throw new Exception("Group data not found in database.");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async UniTask<GroupData> CreateGroup(string groupName, string grade, string subject, string userId)
        {
            try
            {
                string groupId = databaseReference.Child("groups").Push().Key;
                GroupData newGroup = new GroupData(groupId, groupName, grade, subject, userId);

                string json = JsonConvert.SerializeObject(newGroup);
                await databaseReference.Child("groups").Child(groupId).SetRawJsonValueAsync(json);
                Debug.Log("Group Created Successfully");
                return newGroup;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async UniTask<List<GroupData>> GetGroups(string userId)
        {
            List<GroupData> groups = new List<GroupData>();

            try
            {
                // Step 1: Get the user's groups list
                DataSnapshot userSnapshot = await databaseReference.Child("users").Child(userId).Child("groups").GetValueAsync();

                if (!userSnapshot.Exists)
                {
                    Debug.LogWarning("User is not part of any groups.");
                    return groups; // Return an empty list
                }

                List<string> groupIds = new List<string>();

                foreach (var group in userSnapshot.Children)
                {
                    groupIds.Add((string)group.Value); // Get group IDs from user's "groups" node
                }

                // Step 2: Fetch group details for each group ID
                foreach (string groupId in groupIds)
                {
                    DataSnapshot groupSnapshot = await databaseReference.Child("groups").Child(groupId).GetValueAsync();

                    if (groupSnapshot.Exists)
                    {
                        string json = groupSnapshot.GetRawJsonValue();
                        GroupData groupData = JsonConvert.DeserializeObject<GroupData>(json);
                        groups.Add(groupData);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error retrieving user's groups: {e.Message}");
            }

            return groups;
        }

        public async UniTask AddStudentToGroup(string groupId, string studentId)
        {
            try
            {
                DatabaseReference studentsRef = databaseReference.Child("groups").Child(groupId).Child("students");

                // Get current list of student IDs
                DataSnapshot snapshot = await studentsRef.GetValueAsync();
                List<string> studentIds = new List<string>();

                if (snapshot.Exists)
                {
                    // Deserialize existing student list
                    studentIds = JsonConvert.DeserializeObject<List<string>>(snapshot.GetRawJsonValue());
                }

                // Add new student only if not already in the list
                if (!studentIds.Contains(studentId))
                {
                    studentIds.Add(studentId);
                    await studentsRef.SetRawJsonValueAsync(JsonConvert.SerializeObject(studentIds));
                    Debug.Log($"Added student {studentId} to Group {groupId}");
                }
                else
                {
                    Debug.Log($"Student {studentId} is already in Group {groupId}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error adding student: {e.Message}");
            }
        }

        public async UniTask RemoveStudentFromGroup(string groupId, string studentId)
        {
            try
            {
                DatabaseReference studentsRef = databaseReference.Child("groups").Child(groupId).Child("students");

                // Get current list of student IDs
                DataSnapshot snapshot = await studentsRef.GetValueAsync();
                List<string> studentIds = new List<string>();

                if (snapshot.Exists)
                {
                    studentIds = JsonConvert.DeserializeObject<List<string>>(snapshot.GetRawJsonValue());
                }

                // Remove student from list
                if (studentIds.Contains(studentId))
                {
                    studentIds.Remove(studentId);
                    await studentsRef.SetRawJsonValueAsync(JsonConvert.SerializeObject(studentIds));
                    Debug.Log($"Removed student {studentId} from Group {groupId}");
                }
                else
                {
                    Debug.Log($"Student {studentId} is not in Group {groupId}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error removing student: {e.Message}");
            }
        }

        public async UniTask AssignGroupToUser(string userId, string groupId)
        {
            try
            {
                DatabaseReference groupsRef = databaseReference.Child("users").Child(userId).Child("groups");

                // Get the current list of groups
                DataSnapshot snapshot = await groupsRef.GetValueAsync();
                List<string> groupIds = new List<string>();

                if (snapshot.Exists)
                {
                    groupIds = JsonConvert.DeserializeObject<List<string>>(snapshot.GetRawJsonValue());
                }

                // Add group only if it's not already in the list
                if (!groupIds.Contains(groupId))
                {
                    groupIds.Add(groupId);
                    await groupsRef.SetRawJsonValueAsync(JsonConvert.SerializeObject(groupIds));
                    Debug.Log($"Assigned group {groupId} to user {userId}");
                }
                else
                {
                    Debug.Log($"User {userId} is already assigned to group {groupId}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error assigning group: {e.Message}");
            }
        }

        public async UniTask RemoveGroupFromUser(string userId, string groupId)
        {
            try
            {
                DatabaseReference groupsRef = databaseReference.Child("users").Child(userId).Child("groups");

                // Get the current list of groups
                DataSnapshot snapshot = await groupsRef.GetValueAsync();
                List<string> groupIds = new List<string>();

                if (snapshot.Exists)
                {
                    groupIds = JsonConvert.DeserializeObject<List<string>>(snapshot.GetRawJsonValue());
                }

                // Remove group if it exists in the list
                if (groupIds.Contains(groupId))
                {
                    groupIds.Remove(groupId);
                    await groupsRef.SetRawJsonValueAsync(JsonConvert.SerializeObject(groupIds));
                    Debug.Log($"Removed group {groupId} from user {userId}");
                }
                else
                {
                    Debug.Log($"Group {groupId} not found in user's list.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error removing group: {e.Message}");
            }
        }
    }
}