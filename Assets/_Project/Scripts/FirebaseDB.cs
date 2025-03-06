using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task WriteNewUser(UserData userData)
        {
            string json = JsonConvert.SerializeObject(userData);
            await databaseReference.Child("users").Child(userData.UserId).SetRawJsonValueAsync(json);
        }

        public async Task<UserData> GetUser(string userId)
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

        public async Task<GroupData> CreateGroup(string groupName, string grade, string subject, string userId)
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

        public async Task<List<GroupData>> GetGroups(string userId)
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
                    groupIds.Add(group.Key); // Get group IDs from user's "groups" node
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

        public async Task AddStudentToGroup(string groupId, string studentId)
        {
            await databaseReference.Child("groups").Child(groupId).Child("students").Child(studentId).SetValueAsync(true);
            Debug.Log($"Added student {studentId} to Group {groupId}");
        }

        public async Task RemoveStudentFromGroup(string groupId, string studentId)
        {
            await databaseReference.Child("groups").Child(groupId).Child("students").Child(studentId).RemoveValueAsync();
            Debug.Log($"Removed student {studentId} from Group {groupId}");
        }

        public async Task AssignGroupToUser(string userId, string groupId)
        {
            DatabaseReference groupRef = databaseReference.Child("users").Child(userId).Child("groups");

            await groupRef.Child(groupId).SetValueAsync(true);
        }
    }
}