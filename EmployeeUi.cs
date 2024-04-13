using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace S1V3
{
    public partial class EmployeeUi : Form
    {
        private DataGridView tblEmployee;
        private DataTable dataTable;
        private Employee employee;
        private Color valid;
        private Color invalid;
        private Color initial;
        public EmployeeUi()
        {
            InitializeComponent();
            initialize();           
        }

        private void initialize()
        {
            LoadView();
            LoadTable();
            LoadForm();
        }
        private void LoadView()
        {
            valid = Color.LightGreen;
            invalid = Color.LightPink;
            initial = Color.White;
            employee = new Employee();
            dataTable = new DataTable();
            tblEmployee = new DataGridView();
            tblEmployee.Dock = DockStyle.Fill;
            tableLayoutPanel1.Controls.Add(tblEmployee);
           
        }

        private void LoadForm()
        {
            employee = new Employee();
            LoadGender();
            LoadDesignation();
            LoadStatus();

            txtName.Text = "";
            txtName.BackColor = initial;

            txtNic.Text = "";
            txtNic.BackColor = initial;

            txtMobile.Text = "076";
            txtMobile.BackColor = initial;

            txtEmail.Text = "@gmail.com";
            txtEmail.BackColor = initial;

            cmbGender.BackColor = initial;
            cmbDesignation.BackColor = initial;
            cmbStatus.BackColor = initial;
        }

        private void LoadTable()
        {
            List<Employee> employees = EmployeeController.Get(null);
            FillTable(employees);
        }

        private void LoadGender()
        {
            List<Gender> genderList = GenderController.Get();
            cmbSearchGender.Items.Add("Select a Gender");
            cmbGender.Items.Add("Select a Gender");
            cmbSearchGender.SelectedIndex = 0;
            cmbGender.SelectedIndex = 0;

            foreach (Gender gender in genderList)
            {
                cmbSearchGender.Items.Add(gender);
                cmbGender.Items.Add(gender);
            }
            cmbSearchGender.DisplayMember = "Name";
            cmbGender.DisplayMember = "Name";
        }

        public void LoadDesignation()
        {
            List<Designation> desList = DesignationController.Get();
            cmbDesignation.Items.Add("Select a Designation");
            cmbDesignation.SelectedIndex = 0;

            foreach (Designation designation in desList)
            {
                cmbDesignation.Items.Add(designation);
            }
            cmbDesignation.DisplayMember = "Name";
        }

        public void LoadStatus()
        {
            List<Employeestatus> stsList = EmployeestatusController.Get();
            cmbStatus.Items.Add("Select a Status");
            cmbStatus.SelectedIndex = 0;

            foreach (Employeestatus employeestatus in stsList)
            {
                cmbStatus.Items.Add(employeestatus);
            }
            cmbStatus.DisplayMember = "Name";
        }

        private void FillTable(List<Employee> employees)
        {
            dataTable.Clear();
            dataTable.Columns.Clear();

            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("NIC", typeof(string));
            dataTable.Columns.Add("Gender", typeof(string));
            dataTable.Columns.Add("Designation", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));

            foreach (Employee emp in employees)
            {
                dataTable.Rows.Add(emp.Id, emp.Name, emp.Nic, emp.Gender.Name, emp.Designation.Name, emp.Employeestatus.Name);
            }

            tblEmployee.DataSource = dataTable;
        }

        private void ClearSearch(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Clear?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                txtSearchName.Text = "";
                cmbSearchGender.SelectedIndex = 0;
                LoadTable();
            }
        }

        private void SeachByName(object sender, EventArgs e)
        {
            Hashtable hashtable = new Hashtable();
            string name = txtSearchName.Text.Trim();

            if (!string.IsNullOrEmpty(name))
            {
                hashtable.Add("name", name);
            }

            if (cmbSearchGender.SelectedItem != null && cmbSearchGender.SelectedIndex != 0)
            {
                Gender selectedGender = (Gender)cmbSearchGender.SelectedItem;
                hashtable.Add("gender", selectedGender);
            }

            if (hashtable.Count > 0)
            {
                List<Employee> employees = EmployeeController.Get(hashtable);
                FillTable(employees);
            }
            else
            {
                MessageBox.Show("Please provide at least one search value.");
            }
        }

        private string GetErrors()
        {
            string errors = "";

            if (employee.Name == null) errors = errors + "\n Invalid Name"; 
            if (employee.DOB == null || (DateTime.Now.Year - employee.DOB.Year) < 18) errors = errors + "\n Invalid DOB"; 
            if (employee.Nic == null) errors = errors + "\n Invalid Nic"; 
            if (employee.Mobile == null) errors = errors + "\n Invalid Mobile"; 
            if (employee.Email == null) errors = errors + "\n Invalid Email"; 
            if (employee.Gender == null) errors = errors + "\n Invalid Gender"; 
            if (employee.Designation == null) errors = errors + "\n Invalid Designation"; 
            if (employee.Employeestatus == null) errors = errors + "\n Invalid Employeestatus"; 

            return errors;
        }

        private void Add(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string namepttn = "^[A-Z][a-z]*$";
            Match nameMatch = Regex.Match(name, namepttn);
            if(nameMatch.Success)
            {
                employee.Name = name;
                txtName.BackColor = valid;
            }
            else { txtName.BackColor = invalid; }

            string nic = txtNic.Text;
            string nicpttn = "^[0-9]{9}V$";
            Match nicMatch = Regex.Match(nic, nicpttn);
            if (nicMatch.Success)
            {
                employee.Nic = nic;
                txtNic.BackColor = valid;
            }
            else { txtNic.BackColor = invalid; }

            DateTime dobirth = txtDate.Value;
            employee.DOB = dobirth;
            if (!((DateTime.Now.Year - dobirth.Year) < 18))
            {
                txtDate.BackColor = valid;
                txtDate.CalendarTitleBackColor = valid;
                txtDate.CalendarForeColor = valid;
                txtDate.CalendarMonthBackground = valid;
                employee.DOB = dobirth;
            }
            else
            {
                txtDate.BackColor = invalid;
                txtDate.CalendarTitleBackColor = invalid;
                txtDate.CalendarForeColor = invalid;
                txtDate.CalendarMonthBackground = invalid;
            }



            string mobile = txtMobile.Text;
            string mobilepttn = "^0[0-9]{9}$";
            Match mobileMatch = Regex.Match(mobile, mobilepttn);
            if (mobileMatch.Success)
            {
                employee.Mobile = mobile;
                txtMobile.BackColor = valid;
            }
            else { txtMobile.BackColor = invalid; }

            string email = txtEmail.Text;
            string emailpttn = "^[a-z]*@[a-z]*.[a-z]*$";
            Match emailMatch = Regex.Match(email, emailpttn);
            if (emailMatch.Success)
            {
                employee.Email = email;
                txtEmail.BackColor = valid;
            }
            else { txtEmail.BackColor = invalid; }

            if (cmbGender.SelectedItem != null && cmbGender.SelectedIndex != 0 && cmbGender.SelectedItem is Gender)
            {
                employee.Gender = (Gender)cmbGender.SelectedItem;
                cmbGender.BackColor = valid;
            }
            else { cmbGender.BackColor = invalid; }

            if (cmbDesignation.SelectedItem != null && cmbDesignation.SelectedIndex != 0 && cmbDesignation.SelectedItem is Designation)
            {
                employee.Designation = (Designation)cmbDesignation.SelectedItem;
                cmbDesignation.BackColor = valid;
            }
            else { cmbDesignation.BackColor = invalid; }

            if (cmbStatus.SelectedItem != null && cmbStatus.SelectedIndex != 0 && cmbStatus.SelectedItem is Employeestatus)
            {
                employee.Employeestatus = (Employeestatus)cmbStatus.SelectedItem;
                cmbStatus.BackColor = valid;
            }
            else { cmbStatus.BackColor = invalid; }
                

            string errors = GetErrors();

            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
            }
            else
            {                
                string confMsg = "Are you sure to add this Employee?\n\n";
                confMsg += "Name: " + employee.Name + "\n";
                confMsg += "DOB: " + employee.DOB + "\n";
                confMsg += "NIC: " + employee.Nic + "\n";
                confMsg += "Mobile: " + employee.Mobile + "\n";
                confMsg += "Email: " + employee.Email + "\n";
                if (employee.Gender != null)
                    confMsg += "Gender: " + employee.Gender.Name + "\n";
                if (employee.Designation != null)
                    confMsg += "Designation: " + employee.Designation.Name + "\n";
                if (employee.Employeestatus != null)
                    confMsg += "Status: " + employee.Employeestatus.Name + "\n";

                DialogResult result = MessageBox.Show(confMsg, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {                 
                   string sr = EmployeeController.Post(employee);
                    if(sr == "1")
                    {
                        LoadTable();
                        LoadForm();
                        MessageBox.Show("Successfully Saved");
                    }
                    else { MessageBox.Show("Failed to Saved as : " + sr); }
                }
            }
        }
    }
}
