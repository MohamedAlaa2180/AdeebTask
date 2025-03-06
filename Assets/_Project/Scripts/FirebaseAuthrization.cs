using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using UnityEngine;

namespace Firebase
{
    public class FirebaseAuthrization
    {
        private FirebaseAuth auth;
        public FirebaseUser CurrentUser { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public Uri PhotoUrl { get; set; }

        public FirebaseAuthrization()
        {
            auth = FirebaseAuth.DefaultInstance;
            auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
        }

        private void AuthStateChanged(object sender, EventArgs e)
        {
            if (auth.CurrentUser != CurrentUser)
            {
                bool signedIn = CurrentUser != auth.CurrentUser && auth.CurrentUser != null
                    && auth.CurrentUser.IsValid();
                if (!signedIn && CurrentUser != null)
                {
                    Debug.Log("Signed out " + CurrentUser.UserId);
                }
                CurrentUser = auth.CurrentUser;
                if (signedIn)
                {
                    Debug.Log("Signed in " + CurrentUser.UserId);
                    DisplayName = CurrentUser.DisplayName ?? "";
                    EmailAddress = CurrentUser.Email ?? "";
                    PhotoUrl = CurrentUser.PhotoUrl ?? null;
                }
            }
        }

        public async Task<FirebaseUser> RegisterUser(string email, string password, string nickName)
        {
            try
            {
                var result = await auth.CreateUserWithEmailAndPasswordAsync(email, password); ;
                CurrentUser = result.User;

                UserProfile profile = new UserProfile { DisplayName = nickName };
                await CurrentUser.UpdateUserProfileAsync(profile);

                Debug.Log($"User registered: {CurrentUser.Email}");

                return CurrentUser; // Returning the FirebaseUser object
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<FirebaseUser> LoginUser(string email, string password)
        {
            try
            {
                var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
                Debug.Log($"User logged in: {result.User.Email}");
                CurrentUser = result.User;
                return CurrentUser; // Returning the FirebaseUser object
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void LogoutUser()
        {
            auth.SignOut();
            Debug.Log("User logged out");
        }
    }
}
