using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using HelloWorld.Android.Models;
using System.IO;

namespace HelloWorld.Android
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        EditText txtusername;
        EditText txtPassword;
        Button btncreate;
        Button btnsign;
        public static String userSessionPref = "userPrefs";
        public static String User_Name = "userName";
        public static String User_Email = "userEmail";
        public static String User_Password = "userPassword";
        ISharedPreferences session;
        String SESSION_NAME, SESSION_EMAIL, SESSION_PASS;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource  
            SetContentView(Resource.Layout.Login);
            checkCredentials();
            initialize();
            session = GetSharedPreferences(userSessionPref, FileCreationMode.Private);
        }

        public void initialize()
        {
            // Get our button from the layout resource,  
            // and attach an event to it  
            btnsign = FindViewById<Button>(Resource.Id.btnlogin);
            btncreate = FindViewById<Button>(Resource.Id.btnregister);
            txtusername = FindViewById<EditText>(Resource.Id.txtusername);
            txtPassword = FindViewById<EditText>(Resource.Id.txtpwd);
            btnsign.Click += Btnsign_Click;
            btncreate.Click += Btncreate_Click;
            CreateDB();
        }
        private void Btncreate_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
        }
        private void Btnsign_Click(object sender, EventArgs e)
        {
            try
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<LoginTable>(); //Call Table  
                var data1 = data.Where(x => (x.username == txtusername.Text || x.email == txtusername.Text) && x.password == txtPassword.Text).FirstOrDefault(); //Linq Query  
                if (data1 != null)
                {
                    //if you want you can tost 
                    Toast.MakeText(this, "Login Success", ToastLength.Short).Show();
                    SESSION_NAME = data1.username;
                    SESSION_EMAIL = data1.email;
                    SESSION_PASS = data1.password;
                    ISharedPreferencesEditor session_editor = session.Edit();
                    session_editor.PutString("username", SESSION_NAME);
                    session_editor.PutString("email", SESSION_EMAIL);
                    session_editor.PutString("pass", SESSION_PASS);
                    session_editor.Commit();
                    Intent n = new Intent(this, typeof(StoryBoardCode));
                    StartActivity(n);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Username/Email or Password invalid", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }
        public string CreateDB()
        {
            var output = "";
            output += "Creating Database if it doesnt exists";
            if (Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3") == null)
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
                var db = new SQLiteConnection(dpPath);
                output += "\n Database Created....";
                Toast.MakeText(this, "Database Created!,", ToastLength.Short).Show();
                return output;
            } //Create New Database  
            else
            {
                output = "Database already exists.";
                Toast.MakeText(this, "User Login Database already exists!,", ToastLength.Short).Show();
                return output;
            }
        }

        // method to check existing credentials
��������public void checkCredentials()
        {
            ISharedPreferences preferences = GetSharedPreferences(userSessionPref, FileCreationMode.Private);
            String email = preferences.GetString("email", "");
            String username = preferences.GetString("username", "");
            Toast.MakeText(this, "Username: " + username + "\nEmail: " + email, ToastLength.Short).Show();
            String pass = preferences.GetString("pass", "");
            if (!username.Equals("") && !email.Equals("") && !pass.Equals(""))
������������{
                Intent n = new Intent(this, typeof(StoryBoardCode));
                StartActivity(n);
                Finish();
            }

        }
    }
}