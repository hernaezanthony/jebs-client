using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Web.Script.Serialization;

namespace JEBS
{   
   

    public partial class   Login : Form
    {
        private Register register;
        private Menu menu;

        public Login()
        {
            InitializeComponent();
            this.Text = "Joint Equipment Borrowing System";
            //this.BackColor = Color.FromArgb(1,53,77);
            pictureBox2.Controls.Add(pictureBox1);
            pictureBox2.Controls.Add(titleLabel);
            pictureBox2.Controls.Add(subtitleLabel);
            pictureBox1.BackColor = Color.Transparent;
            titleLabel.Font = new Font (titleLabel.Font.FontFamily,50);
            titleLabel.BackColor = Color.Transparent;
            subtitleLabel.BackColor = Color.Transparent;
            passwordTextBox.Text = "";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.MaxLength = 14;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String email = emailTextBox.Text;
            String password = passwordTextBox.Text;

            if (email == string.Empty || password == string.Empty)
            {
                MessageBox.Show("All Fields are Required!");
            }

            if(!email.Contains("@"))
            {
                MessageBox.Show("Please Enter Valid Email Address!");
            }
            else 
            {



                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["email"] = email;
                    values["password"] = password;

                    try
                    {

                        var response = client.UploadValues("http://192.168.43.156:3000/api/user/login", values);

                        var responseString = Encoding.Default.GetString(response);

                        //parsing

                        var rp = new JavaScriptSerializer().Deserialize<Response>(responseString);

                        //resultLabel.Text = rp.result;

                        if (rp.result == "success")
                        {
                            menu = new Menu();
                            menu.Show();
                            Hide();
                        }
                        else
                        {
                            MessageBox.Show("Try Again!");
                        }
                    }
                    catch (WebException xcp)
                    {
                        //throw xcp;
                        MessageBox.Show(xcp.Message);
                    }


                }
            }
        }

       

        class Response
        {

            public string result
            {
                get;
                set;
            }

            public override string ToString()
            {
                return string.Format("Result:{0}", result);
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {
            register = new Register();
            register.Show();
            this.Hide();
        }


    }
}