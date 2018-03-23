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
    public partial class Register : Form
    {
        private Login login;

        public Register()
        {
            InitializeComponent();
            this.Text = "Joint Equipment Borrowing System";
            //this.BackColor = Color.FromArgb(1,53,77);
            pictureBox2.Controls.Add(logo);
            pictureBox2.Controls.Add(titleLabel);
            pictureBox2.Controls.Add(subtitleLabel);
            logo.BackColor = Color.Transparent;
            titleLabel.Font = new Font(titleLabel.Font.FontFamily, 50);
            titleLabel.BackColor = Color.Transparent;
            subtitleLabel.BackColor = Color.Transparent;
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

        private void registerButton_Click(object sender, EventArgs e)
        {
            String email = emailTextBox.Text;
            String firstname = firstnameTextBox.Text;
            String lastname = lastnameTextBox.Text;
            String position = positionTextBox.Text;
            String password = passwordTextBox.Text;
            String passwordConfirm = passwordconfirmTextBox.Text;

            if (email == string.Empty || firstname == string.Empty || lastname == string.Empty || position == string.Empty || password == string.Empty || passwordConfirm == string.Empty)
            {
                MessageBox.Show("All Fields are Required!");
            }
            else
            {

                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["email"] = email;
                    values["first_name"] = firstname;
                    values["last_name"] = lastname;
                    values["position"] = position;
                    values["password"] = password;
                    values["password_confirmation"] = passwordConfirm;

                    try
                    {

                        var response = client.UploadValues("http://192.168.43.156:3000/api/user/register", values);

                        var responseString = Encoding.Default.GetString(response);

                        //parsing

                        var rp = new JavaScriptSerializer().Deserialize<Response>(responseString);

                        resultLabel.Text = rp.result;

                        if (rp.result == "success")
                        {
                            MessageBox.Show("Registered!");
                            login = new Login();
                            login.Show();
                            Hide();
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

        private void label6_Click(object sender, EventArgs e)
        {
            login = new Login();
            login.Show();
            Hide();
        }

     
    }
}
