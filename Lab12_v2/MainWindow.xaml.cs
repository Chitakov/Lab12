using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;
using System.Runtime.Remoting.Contexts;

namespace Lab12_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;" +
                                @"Data Source=F:\EPAM_Labs\Lab12_v2\Lab12_v2\Lab12DBAccess.mdb;");
        public MainWindow()
        {
            InitializeComponent();
            conn.Open();
            Read(conn);
        }
        
        public void Read(OleDbConnection conn)
        {
            OleDbCommand command = new OleDbCommand("Select * from Employee", conn);
            EmployeesGrid.ItemsSource = command.ExecuteReader();
        }
        public void Create(OleDbConnection conn)
        {
            var name = NameTextBox.Text;
            var phonenumber = PhoneNumberTextBox.Text;
            if (!String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(phonenumber))
            {
                string commandCreate = "insert into Employee (Name, PhoneNumber) values (name, phonenumber)";
                OleDbCommand createcommand = new OleDbCommand(commandCreate, conn);
                createcommand.Parameters.AddWithValue("Name", NameTextBox.Text);
                createcommand.Parameters.AddWithValue("PhoneNumber", PhoneNumberTextBox.Text);

                createcommand.ExecuteNonQuery();
                Read(conn);
            }
        }
        public void Update(OleDbConnection conn)
        {
            string name = NameTextBox.Text;
            string phonenumber = PhoneNumberTextBox.Text;
            if (!String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(phonenumber))
            {
                dynamic row_selected = EmployeesGrid.SelectedItem;
                if (row_selected != null)
                {
                    var id = row_selected["Id"];
                    string commandUpdate = "update Employee set Name = '"+name+"', PhoneNumber = '"+ phonenumber +"' where Id ="+ id;
                    OleDbCommand updatecommand = new OleDbCommand(commandUpdate, conn);

                    updatecommand.Parameters.AddWithValue("Id", id);
                    updatecommand.Parameters.AddWithValue("Name", NameTextBox.Text);
                    updatecommand.Parameters.AddWithValue("PhoneNumber", PhoneNumberTextBox.Text);

                    updatecommand.ExecuteNonQuery();
                    Read(conn);
                }
            }
        }
        public void Delete(OleDbConnection conn)
        {
            var name = NameTextBox.Text;
            var phonenumber = PhoneNumberTextBox.Text;
            if (!String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(phonenumber))
            {
                dynamic row_selected = EmployeesGrid.SelectedItem;
                if (row_selected != null)
                {
                    var id = row_selected["Id"];
                    string commandDelete = "delete from Employee where Id ="+id;
                    OleDbCommand deletecommand = new OleDbCommand(commandDelete, conn);
                    deletecommand.Parameters.AddWithValue("Id", id);

                    deletecommand.ExecuteNonQuery();
                    Read(conn);
                }
            }
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Create(conn);
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Update(conn);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Delete(conn);
        }

        private void EmployeesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic row_selected = EmployeesGrid.SelectedItem;

            if (row_selected != null)
            {
                NameTextBox.Text = row_selected["Name"].ToString();
                PhoneNumberTextBox.Text = row_selected["PhoneNumber"].ToString();
            }
        }

        private void PhoneNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void NameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }

}
