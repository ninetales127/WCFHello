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
using System.IO;
using SQLite;
using HelloWorld.Android.Models;

namespace HelloWorld.Android
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        EditText txtusername;
        EditText txtPassword;
        Button btncreate, btn_reg_back;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);

            // Create your application here  
            btncreate = FindViewById<Button>(Resource.Id.btn_reg_create);
            btn_reg_back = FindViewById<Button>(Resource.Id.btn_reg_back);
            btn_reg_back.Click += Btn_reg_back_Click;
            txtusername = FindViewById<EditText>(Resource.Id.txt_reg_username);
            txtPassword = FindViewById<EditText>(Resource.Id.txt_reg_password);
            btncreate.Click += Btncreate_Click;
        }

        private void Btn_reg_back_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }

        private void Btncreate_Click(object sender, EventArgs e)
        {
            try
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
                var db = new SQLiteConnection(dpPath);
                db.CreateTable<LoginTable>();
                LoginTable tbl = new LoginTable();
                tbl.username = txtusername.Text;
                tbl.password = txtPassword.Text;
                db.Insert(tbl);
                Toast.MakeText(this, "Record Added Successfully...,", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }
    }
}