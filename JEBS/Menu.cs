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
    public partial class Menu : Form
    {
        private Login login;
        public Menu()
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

            itemConditionComboBox.Items.Add("Used");
            itemConditionComboBox.Items.Add("Brand New");
            itemConditionComboBox.Items.Add("Damaged");

            editItemConditionComboBox.Items.Add("Used");
            editItemConditionComboBox.Items.Add("Brand New");
            editItemConditionComboBox.Items.Add("Damaged");

            returnListView.View = View.Details;
            returnListView.GridLines = true;
            returnListView.FullRowSelect = true;
            returnListView.Columns.Add("ID", 30);
            returnListView.Columns.Add("Item", 100);
            returnListView.Columns.Add("Item_ID", 50);
            returnListView.Columns.Add("Name", 100);
            returnListView.Columns.Add("Stud. Number", 100);
            returnListView.Columns.Add("Course", 70);
            returnListView.Columns.Add("Yr & Sec", 70);

            editItemListView.View = View.Details;
            editItemListView.GridLines = true;
            editItemListView.FullRowSelect = true;
            editItemListView.Columns.Add("ID", 50);
            editItemListView.Columns.Add("Name", 100);
            editItemListView.Columns.Add("Code", 100);
            editItemListView.Columns.Add("Condition", 100);
            editItemListView.Columns.Add("Still Available?", 100);

            button1.ForeColor = Color.SkyBlue;

            FillComboBox();

        }

        public void FillComboBox()
        {
            using (var client = new WebClient())
            {
                try
                {
                    var list_response = client.DownloadString("http://192.168.43.156:3000/api/item/show");

                    //parsing 

                    var rp = new JavaScriptSerializer().Deserialize<ListItemDetails>(list_response);

                    string[] array = new string[99];


                    for (int x = 0; x < rp.itemDetailsList.Count; x++)
                    {
                        if (array.Contains(rp.itemDetailsList[x].name))
                        {
                            //do nothing
                        }
                        else
                        {
                            itemDropDown.Items.Add(rp.itemDetailsList[x].name);
                            array[x] = rp.itemDetailsList[x].name;
                        }

                    }
                }
                catch (WebException xcp)
                {
                    MessageBox.Show(xcp.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            borrowPanel.Visible = true;
            returnPanel.Visible = false;
            additemPanel.Visible = false;
            edititemPanel.Visible = false;
            button1.ForeColor = Color.SkyBlue;
            button2.ForeColor = Color.White;
            button3.ForeColor = Color.White;
            button4.ForeColor = Color.White;
            itemDropDown.Items.Clear();
            FillComboBox();

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            borrowPanel.Visible = false;
            returnPanel.Visible = true;
            refreshButton.PerformClick();
            additemPanel.Visible = false;
            edititemPanel.Visible = false;
            button2.ForeColor = Color.SkyBlue;
            button1.ForeColor = Color.White;
            button3.ForeColor = Color.White;
            button4.ForeColor = Color.White;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            borrowPanel.Visible = false;
            returnPanel.Visible = false;
            additemPanel.Visible = true;
            edititemPanel.Visible = false;
            button3.ForeColor = Color.SkyBlue;
            button1.ForeColor = Color.White;
            button2.ForeColor = Color.White;
            button4.ForeColor = Color.White;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            borrowPanel.Visible = false;
            returnPanel.Visible = false;
            additemPanel.Visible = false;
            edititemPanel.Visible = true;
            editItemRefreshButton.PerformClick();
            button4.ForeColor = Color.SkyBlue;
            button1.ForeColor = Color.White;
            button2.ForeColor = Color.White;
            button3.ForeColor = Color.White;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void submitButton_Click(object sender, EventArgs e)
        {

            String name = itemDropDown.Text;
            String borrowers_name = borrowersnameTextBox.Text;
            String borrowers_student_number = borrowersStudNumTextBox.Text;
            String borrowers_course = borrowersCourseTextBox.Text;
            String borrowers_year_and_section = borrowersYearAndSectionTextBox.Text;

            if (name == string.Empty || borrowers_name == string.Empty || borrowers_student_number == string.Empty || borrowers_course == string.Empty || borrowers_year_and_section == string.Empty)
            {
                MessageBox.Show("All Fields Are Required!");
            }
            else
            {

                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["name"] = name;
                    values["borrowers_name"] = borrowers_name;
                    values["borrowers_student_number"] = borrowers_student_number;
                    values["borrowers_course"] = borrowers_course;
                    values["borrowers_year_and_section"] = borrowers_year_and_section;

                    try
                    {
                        var response = client.UploadValues("http://192.168.43.156:3000/api/item/borrowItem", values);

                        var responseString = Encoding.Default.GetString(response);

                        //parsing

                        var rp = new JavaScriptSerializer().Deserialize<Response>(responseString);

                        if (rp.result == "success")
                        {
                            itemDropDown.Text = String.Empty;
                            borrowersnameTextBox.Text = String.Empty;
                            borrowersStudNumTextBox.Text = String.Empty;
                            borrowersCourseTextBox.Text = String.Empty;
                            borrowersYearAndSectionTextBox.Text = String.Empty;
                            MessageBox.Show("Item Borrowed.");

                        }
                        else
                        {
                            MessageBox.Show("Failed. Try Again");
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

        private void refreshButton_Click(object sender, EventArgs e)
        {
            returnListView.Items.Clear();

            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["email"] = "email"; //dummy

                try
                {

                    var list_response = client.UploadValues("http://192.168.43.156:3000/api/item/listBorrowedItem", values);

                    var list_responseString = Encoding.Default.GetString(list_response);

                    //parsing

                    var rp = new JavaScriptSerializer().Deserialize<ListItems>(list_responseString);


                    for (int x = 0; x < rp.items.Count; x++)
                    {
                        string[] array = new string[8];
                        ListViewItem itm;

                        array[0] = rp.items[x].id;
                        array[1] = rp.items[x].item.name;
                        array[2] = rp.items[x].item.id;
                        array[3] = rp.items[x].borrowers_name;
                        array[4] = rp.items[x].borrowers_student_number;
                        array[5] = rp.items[x].borrowers_course;
                        array[6] = rp.items[x].borrowers_year_and_section;

                        itm = new ListViewItem(array);
                        returnListView.Items.Add(itm);
                    }
                }
                catch (WebException xcp)
                {
                    MessageBox.Show(xcp.Message);
                }

            }
        }

        class ListItemResponse
        {

            public string id
            {
                get;
                set;
            }

            public string borrowers_name
            {
                get;
                set;
            }

            public string borrowers_student_number
            {
                get;
                set;
            }

            public string borrowers_course
            {
                get;
                set;
            }

            public string borrowers_year_and_section
            {
                get;
                set;
            }

            public ItemName item
            {
                get;
                set;
            }
        }

        class ListItems
        {
            public List<ListItemResponse> items
            {
                get;
                set;
            }
        }

        class ItemName
        {
            public string name
            {
                get;
                set;
            }

            public string id
            {
                get;
                set;
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

        private void returnButton_Click(object sender, EventArgs e)
        {
            string selected_id = null;
            string selected_item_id = null;

            if (returnListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select An Item First!");
            }
            else
            {

                selected_id = returnListView.SelectedItems[0].SubItems[0].Text;
                selected_item_id = returnListView.SelectedItems[0].SubItems[2].Text;


                //MessageBox.Show(selected_id + ", " + selected_item + ", " + selected_item_id + ", " + selected_borrowers_name + ", " + selected_borrowers_stud_num + ", " + selected_borrowers_course + ", " + selected_borrowers_yr_and_sec);

                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["id"] = selected_id;
                    values["item_id"] = selected_item_id;

                    try
                    {


                        var response = client.UploadValues("http://192.168.43.156:3000/api/item/returnItem", values);

                        var responseString = Encoding.Default.GetString(response);

                        //parsing

                        var rp = new JavaScriptSerializer().Deserialize<Response>(responseString);

                        if (rp.result == "success")
                        {
                            MessageBox.Show("Item Returned.");
                            //returnListView.Items.Remove(returnListView.SelectedItem);
                            foreach (ListViewItem eachItem in returnListView.SelectedItems)
                            {
                                returnListView.Items.Remove(eachItem);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed. Try Again");
                        }

                    }
                    catch (WebException xcp)
                    {
                        MessageBox.Show(xcp.Message);
                    }

                }

            }
        }

        private void addItemButton_Click(object sender, EventArgs e)
        {
            String item_name = itemNameTextBox.Text;
            String item_code = itemCodeTextBox.Text;
            String item_condition = itemConditionComboBox.Text;

            if (item_name == string.Empty || item_code == string.Empty || item_condition == string.Empty)
            {
                MessageBox.Show("All Fields Are Required!");
            }
            else
            {

                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["name"] = item_name;
                    values["code"] = item_code;
                    values["condition"] = item_condition;


                    try
                    {
                        var response = client.UploadValues("http://192.168.43.156:3000/api/item/addItem", values);

                        var responseString = Encoding.Default.GetString(response);

                        //parsing
                       
                        var rp = new JavaScriptSerializer().Deserialize<Response>(responseString);

                        if (rp.result == "success")
                        {
                            itemNameTextBox.Text = String.Empty;
                            itemCodeTextBox.Text = String.Empty;
                            itemConditionComboBox.Text = String.Empty;
                            MessageBox.Show("Item Added.");

                        }
                        else
                        {
                            MessageBox.Show("Failed. Try Again");
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

        private void editItemRefreshButton_Click(object sender, EventArgs e)
        {
            editItemListView.Items.Clear();

            using (var client = new WebClient())
            {
                try
                {

                    var list_response = client.DownloadString("http://192.168.43.156:3000/api/item/show");

                    //parsing 

                    var rp = new JavaScriptSerializer().Deserialize<ListItemDetails>(list_response);


                    for (int x = 0; x < rp.itemDetailsList.Count; x++)
                    {
                        string[] array = new string[8];
                        ListViewItem itm;
                        array[0] = rp.itemDetailsList[x].id;
                        array[1] = rp.itemDetailsList[x].name;
                        array[2] = rp.itemDetailsList[x].code;
                        array[3] = rp.itemDetailsList[x].condition;
                        if (rp.itemDetailsList[x].available == true)
                        {
                            array[4] = "Yes";
                        }
                        else
                        {
                            array[4] = "No";
                        }

                        itm = new ListViewItem(array);
                        editItemListView.Items.Add(itm);
                    }
                }
                catch (WebException xcp)
                {
                    //throw xcp;
                    MessageBox.Show(xcp.Message);
                }
            }
        }


        class ItemDetails
        {
            public string id
            {
                get;
                set;
            }
            public string name
            {
                get;
                set;
            }
            
            public string condition
            {
                get;
                set;
            }

            public string code
            {
                get;
                set;
            }

            public Boolean available
            {
                get;
                set;
            }

        }

        class ListItemDetails
        {
            public List<ItemDetails> itemDetailsList
            {
                get;
                set;
            }
        }
        
        string selectedItemID = null;
        string selectedItemAvailability = null;

        private void editButton_Click(object sender, EventArgs e)
        {
            
            string selectedItemName = null;
            string selectedItemCode = null;
            string selectedItemCondition = null;

            if (editItemListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select An Item First!");
            }
            else
            {
            
                selectedItemID = editItemListView.SelectedItems[0].SubItems[0].Text;
                selectedItemName = editItemListView.SelectedItems[0].SubItems[1].Text;
                selectedItemCode = editItemListView.SelectedItems[0].SubItems[2].Text;
                selectedItemCondition = editItemListView.SelectedItems[0].SubItems[3].Text;
                selectedItemAvailability = editItemListView.SelectedItems[0].SubItems[4].Text;

                if (selectedItemAvailability == "Yes")
                {
                    isAvailableCheckBox.Checked = true;
                }
                else
                {
                    isAvailableCheckBox.Checked = false;
                }

                editPanel.Visible = true;
                edititemPanel.Visible = false;
                editItemNameTextbox.Text = selectedItemName;
                editItemCodeTextbox.Text = selectedItemCode;
                editItemConditionComboBox.Text = selectedItemCondition;
            }
            
        }

        private void editItemCancelButton_Click(object sender, EventArgs e)
        {
            editPanel.Visible = false;
            borrowPanel.Visible = false;
            returnPanel.Visible = false;
            additemPanel.Visible = false;
            edititemPanel.Visible = true;
        }

        private void editItemSubmitButton_Click(object sender, EventArgs e)
        {
            String item_id = selectedItemID; 
            String item_name = editItemNameTextbox.Text;
            String item_code = editItemCodeTextbox.Text;
            String item_condition = editItemConditionComboBox.Text;
            String available;

            if (isAvailableCheckBox.Checked == true)
            {
                available = "true";
            }
            else
            {
                available = "false";
            }

            if (item_name == string.Empty || item_code == string.Empty || item_condition == string.Empty)
            {
                MessageBox.Show("All Fields Are Required!");
            }
            else
            {

                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["id"] = item_id;
                    values["name"] = item_name;
                    values["code"] = item_code;
                    values["condition"] = item_condition;
                    values["available"] = available;


                    try
                    {
                        var response = client.UploadValues("http://192.168.43.156:3000/api/item/editItem", values);

                        var responseString = Encoding.Default.GetString(response);

                        //parsing

                        var rp = new JavaScriptSerializer().Deserialize<Response>(responseString);

                        if (rp.result == "success")
                        {
                            itemNameTextBox.Text = String.Empty;
                            itemCodeTextBox.Text = String.Empty;
                            itemConditionComboBox.Text = String.Empty;

                            MessageBox.Show("Item Updated!");

                            editPanel.Visible = false;
                            borrowPanel.Visible = false;
                            returnPanel.Visible = false;
                            additemPanel.Visible = false;
                            edititemPanel.Visible = true;

                            editItemRefreshButton.PerformClick();                    

                        }
                        else
                        {
                            MessageBox.Show("Failed. Try Again");
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

        private void button5_Click(object sender, EventArgs e)
        {
            login = new Login();
            login.Show();
            Hide();
        }

    }
}
