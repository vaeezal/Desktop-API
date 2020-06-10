using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace DesktopWithApi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class frmdesktopapi
        {
            public string itemName { get;set; }
            public string itemContent { get; set; }
            public int itemType { get; set; }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int RowIndex = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[RowIndex];
            txt1.Text = row.Cells[0].Value.ToString();
            txt2.Text = row.Cells[1].Value.ToString();
            if (row.Cells[2].Value.ToString() == "1")
            {
                cbo1.Text = row.Cells[2].Value.ToString()+ ". JSON string";
            }else if (row.Cells[2].Value.ToString() == "2")
            {
                cbo1.Text = row.Cells[2].Value.ToString() + ". XML string";
            }
        }

        void get_data()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:65427/api/frmdesktopapi/" + "Get");
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encode))
            {
                string data = reader.ReadToEnd();
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(data, (typeof(DataTable)));
                dataGridView1.DataSource = dt;
                if (data != "null" && data != "[]" && data != null)
                {
                    DataGridViewColumn column = dataGridView1.Columns[1];
                    column.Width = 560;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clearTexboxes();
            get_data();
        }

         private void button2_Click(object sender, EventArgs e)
        {
            if (txt1.Text == "") {
                MessageBox.Show("don't let item name empty!");
                return;
            }else if(cbo1.Text == "")
            {
                MessageBox.Show("don't let item type empty!");
                return;
            }
            // get ComboBox from dictionary
            KeyValuePair<string, string> selectedEntry
                = (KeyValuePair<string, string>)cbo1.SelectedItem;

            // get selected Key
            string selectedKey = selectedEntry.Key;

            if (isValidItemType())
            {
                var request_ = (HttpWebRequest)WebRequest.Create("http://localhost:65427/api/frmdesktopapi/" + "Retrieve?itemName=" + txt1.Text);
                request_.Method = "GET";
                request_.ContentLength = 0;
                var response_ = (HttpWebResponse)request_.GetResponse();
                Encoding encode_ = System.Text.Encoding.GetEncoding("utf-8");
                using (var reader_ = new System.IO.StreamReader(response_.GetResponseStream(), encode_))
                {
                    string data = reader_.ReadToEnd();

                    if (data != "null" && data != "[]" && data != null)
                    {
                        MessageBox.Show("item name \"" + txt1.Text + "\" already exist, register is not success!");
                        return;
                    }
                }

                var request = (HttpWebRequest)WebRequest.Create("http://localhost:65427/api/frmdesktopapi/" + "Register?itemName=" + txt1.Text + "&itemContent=" + txt2.Text + "&itemType=" + selectedKey);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string data = "itemName=" + txt1.Text + "&itemContent=" + txt2.Text + "&itemType=" + selectedKey;
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var response = (HttpWebResponse)request.GetResponse();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encode))
                {
                }


                var request2= (HttpWebRequest)WebRequest.Create("http://localhost:65427/api/frmdesktopapi/" + "Retrieve?itemName=" + txt1.Text);
                request2.Method = "GET";
                request2.ContentLength = 0;
                var response2 = (HttpWebResponse)request2.GetResponse();
                Encoding encode2 = System.Text.Encoding.GetEncoding("utf-8");
                using (var reader2 = new System.IO.StreamReader(response2.GetResponseStream(), encode2))
                {
                    string data = reader2.ReadToEnd();

                    if (data != "null" && data != "[]" && data != null)
                    {
                        MessageBox.Show("register is success!");
                    }
                }

                get_data();
            }
            else
            {
                string strMsg = "Item Type is not a valid " + (selectedKey == "1" ? "JSONString" : "XMLString!");
                MessageBox.Show(strMsg);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txt1.Text == "")
            {
                MessageBox.Show("don't let item name empty!");
                return;
            }
            else if (cbo1.Text == "")
            {
                MessageBox.Show("don't let item type empty!");
                return;
            }
            // get ComboBox from dictionary
            KeyValuePair<string, string> selectedEntry
                = (KeyValuePair<string, string>)cbo1.SelectedItem;

            // get selected Key
            string selectedKey = selectedEntry.Key;

            if (isValidItemType())
            {
                var request = (HttpWebRequest)WebRequest.Create("http://localhost:65427/api/frmdesktopapi/" + "Change?itemName=" + txt1.Text + "&itemContent=" + txt2.Text + "&itemType=" + selectedKey);
                request.Method = "PUT";
                request.ContentType = "application/x-www-form-urlencoded";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string data = "itemName=" + txt1.Text + "&itemContent=" + txt2.Text + "&itemType=" + selectedKey;
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var response = (HttpWebResponse)request.GetResponse();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encode))
                {
                    if (reader.ReadToEnd() == "true")
                    {
                        MessageBox.Show("change success!");
                    }
                    else
                    {
                        MessageBox.Show("item name \"" + txt1.Text + "\" does not exist, change is not success!");
                    }
                }

                get_data();
            }
            else
            {
                string strMsg = "Item Type is not a valid " + (selectedKey == "1" ? "JSONString" : "XMLString!");
                MessageBox.Show(strMsg);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var request_ = (HttpWebRequest)WebRequest.Create("http://localhost:65427/api/frmdesktopapi/" + "Retrieve?itemName=" + txt1.Text);
            request_.Method = "GET";
            request_.ContentLength = 0;
            var response_ = (HttpWebResponse)request_.GetResponse();
            Encoding encode_ = System.Text.Encoding.GetEncoding("utf-8");
            using (var reader_ = new System.IO.StreamReader(response_.GetResponseStream(), encode_))
            {
                string data = reader_.ReadToEnd();

                if (data != "null" && data != "[]" && data != null)
                {
                }
                else {
                    MessageBox.Show("item name \"" + txt1.Text + "\" does not exist, deregister is not success!");
                    return;
                }
            }

            var request = (HttpWebRequest)WebRequest.Create("http://localhost:65427/api/frmdesktopapi/" + "Deregister?itemName=" + txt1.Text);
            request.Method = "DELETE";
            var response = (HttpWebResponse)request.GetResponse();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encode))
            {
            }

            var request2 = (HttpWebRequest)WebRequest.Create("http://localhost:65427/api/frmdesktopapi/" + "Retrieve?itemName=" + txt1.Text);
            request2.Method = "GET";
            request2.ContentLength = 0;
            var response2 = (HttpWebResponse)request2.GetResponse();
            Encoding encode2 = System.Text.Encoding.GetEncoding("utf-8");
            using (var reader2 = new System.IO.StreamReader(response2.GetResponseStream(), encode2))
            {
                string data = reader2.ReadToEnd();

                if (data != "null" && data != "[]" && data != null)
                {
                    MessageBox.Show("deregister is not success!");
                }
                else
                {
                    MessageBox.Show("deregister is success!");

                }
            }

            get_data();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clearTexboxes();

            var request = (HttpWebRequest)WebRequest.Create("http://localhost:65427/api/frmdesktopapi/" + "RetrieveList?itemName=" + txt4.Text);
            request.Method = "GET";
            request.ContentLength = 0;
            var response = (HttpWebResponse)request.GetResponse();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encode))
            {
                string data = reader.ReadToEnd();
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(data, (typeof(DataTable)));
                dataGridView1.DataSource = dt;
                if (data != "null" && data != "[]" && data != null)
                {
                    DataGridViewColumn column = dataGridView1.Columns[1];
                    column.Width = 560;
                }
            }

            var request2 = (HttpWebRequest)WebRequest.Create("http://localhost:65427/api/frmdesktopapi/" + "Retrieve?itemName=" + txt4.Text);
            request2.Method = "GET";
            request2.ContentLength = 0;
            var response2 = (HttpWebResponse)request2.GetResponse();
            Encoding encode2 = System.Text.Encoding.GetEncoding("utf-8");
            using (var reader2 = new System.IO.StreamReader(response2.GetResponseStream(), encode2))
            {
                string data = reader2.ReadToEnd();

                if (data != "null" && data != "[]" && data != null)
                {
                    MessageBox.Show("itemName " + data + " exist!");
                }
                else
                {
                    MessageBox.Show("itemName \"" + txt4.Text + "\" does not exist!");

                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            clearTexboxes();

            var request = (HttpWebRequest)WebRequest.Create("http://localhost:65427/api/frmdesktopapi/" + "RetrieveList?itemName=" + txt4.Text);
            request.Method = "GET";
            request.ContentLength = 0;
            var response = (HttpWebResponse)request.GetResponse();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encode))
            {
                string data = reader.ReadToEnd();
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(data, (typeof(DataTable)));
                dataGridView1.DataSource = dt;
                if (data != "null" && data != "[]" && data != null)
                {
                    DataGridViewColumn column = dataGridView1.Columns[1];
                    column.Width = 560;
                }
            }

            var request2 = (HttpWebRequest)WebRequest.Create("http://localhost:65427/api/frmdesktopapi/" + "GetType?itemName=" + txt4.Text);
            request2.Method = "GET";
            request2.ContentLength = 0;
            var response2 = (HttpWebResponse)request2.GetResponse();
            Encoding encode2 = System.Text.Encoding.GetEncoding("utf-8");
            using (var reader2 = new System.IO.StreamReader(response2.GetResponseStream(), encode2))
            {
                string data = reader2.ReadToEnd();

                if (data == "1")
                {
                    data = "JSON string";
                }
                else if (data == "2")
                {
                    data = "XML string";
                }

                if (data != "null" && data != "[]" && data != null && data != "0")
                {
                    MessageBox.Show("itemType for itemName \"" + txt4.Text + "\" = " + data);
                }
                else
                {
                    MessageBox.Show("itemName \"" + txt4.Text + "\" does not exist!");

                }
            }
        }
        private void clearTexboxes()
        {
            txt1.Text="";
            txt2.Text="";
            cbo1.Text="";
        }

        private bool isValidItemType()
        {
            // get ComboBox from dictionary
            KeyValuePair<string, string> selectedEntry
                = (KeyValuePair<string, string>)cbo1.SelectedItem;

            // get selected Key
            string selectedKey = selectedEntry.Key;

            bool isValid = false;
            if ((isValidJSON() && selectedKey=="1") || (isValidXML() && selectedKey == "2")) {
                isValid = true;
            }
            return isValid;
        }

        private bool isValidJSON()
        {
            bool isValid = false;

            string strJSON = txt2.Text.Trim();

            if ((strJSON.StartsWith("{") && strJSON.EndsWith("}")) || //For object
                (strJSON.StartsWith("[") && strJSON.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strJSON);
                    isValid= true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    isValid= false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    isValid= false;
                }
            }
            else
            {
                isValid= false;
            }
            return isValid;
        }
        private bool isValidXML()
        {
            bool isValid = false;

            string strXML = txt2.Text.Trim();

            try
            {
                if (!string.IsNullOrEmpty(strXML))
                {
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.LoadXml(strXML);
                    isValid= true;
                }
                else
                {
                    isValid= false;
                }
            }
            catch (System.Xml.XmlException)
            {
                isValid= false;
            }

            return isValid;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            clearTexboxes();
            dataGridView1.DataSource = null;
        }
    }
}
