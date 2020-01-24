using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Data.SqlClient;
using System.Data;

namespace MyZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["MyZooManager.Properties.Settings.MyZooManagerConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            ShowZoos();
            ShowAllAnimals();
        }

        private void ShowZoos()
        {
            try
            {
                SqlDataAdapter sqlAdapter = new SqlDataAdapter("select * from zoo", sqlConnection);
                using (sqlAdapter)
                {
                    DataTable zooTable = new DataTable();
                    sqlAdapter.Fill(zooTable);
                    listZoos.DisplayMemberPath = "Location";
                    listZoos.SelectedValuePath = "Id";
                    listZoos.ItemsSource = zooTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ShowZooAnimals()
        {
            if (listZoos.SelectedItem != null && listZoos.SelectedValue != null)
            {
                try
                {
                    string sql = "select * from Animal inner join ZooAnimal on Animal.Id=ZooAnimal.AnimalId where ZooAnimal.ZooId=@ZooId";
                    SqlCommand sqlCmd = new SqlCommand(sql, sqlConnection);
                    sqlCmd.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd);
                    using (sqlAdapter)
                    {
                        DataTable zooAnimalsTable = new DataTable();
                        sqlAdapter.Fill(zooAnimalsTable);
                        listZooAnimal.DisplayMemberPath = "Name";
                        listZooAnimal.SelectedValuePath = "Id";
                        listZooAnimal.ItemsSource = zooAnimalsTable.DefaultView;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                listZooAnimal.ItemsSource = null;
            }
        }

        private void ShowAllAnimals()
        {
            try
            {
                SqlDataAdapter sqlAdapter = new SqlDataAdapter("Select * From Animal", sqlConnection);
                using (sqlAdapter)
                {
                    DataTable animalsTable = new DataTable();
                    sqlAdapter.Fill(animalsTable);
                    listAnimals.DisplayMemberPath = "Name";
                    listAnimals.SelectedValuePath = "Id";
                    listAnimals.ItemsSource = animalsTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ListZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowZooAnimals();
            if (listZoos.SelectedItem != null && listZoos.SelectedValue != null)
            {
                textHelper.Text = (listZoos.SelectedItem as DataRowView)["Location"].ToString();
            }
            else
            {
                textHelper.Text = "";
            }
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            if (listZoos.SelectedItem != null)
            {
                string sql = "delete from Zoo where Id=@ZooId";
                SqlCommand sqlCmd = new SqlCommand(sql, sqlConnection);
                sqlCmd.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                using (sqlCmd)
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlCmd.ExecuteScalar();
                    }
                    catch (Exception exDel)
                    {
                        MessageBox.Show(exDel.Message);
                    }
                    finally
                    {
                        sqlConnection.Close();
                        ShowZoos();
                    }
                }
            }
        }

        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            string sql = "insert into Zoo values (@Location)";
            SqlCommand sqlCmd = new SqlCommand(sql, sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Location", textHelper.Text);
            using (sqlCmd)
            {
                try
                {
                    sqlConnection.Open();
                    sqlCmd.ExecuteScalar();
                }
                catch (Exception exDel)
                {
                    MessageBox.Show(exDel.Message);
                }
                finally
                {
                    sqlConnection.Close();
                    ShowZoos();
                }
            }
        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            if (listZoos.SelectedItem != null)
            {
                string sql = "update Zoo set Location=@Location where Id=@ZooId";
                SqlCommand sqlCmd = new SqlCommand(sql, sqlConnection);
                sqlCmd.Parameters.AddWithValue("@Location", textHelper.Text);
                sqlCmd.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                using (sqlCmd)
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlCmd.ExecuteScalar();
                    }
                    catch (Exception exDel)
                    {
                        MessageBox.Show(exDel.Message);
                    }
                    finally
                    {
                        sqlConnection.Close();
                        ShowZoos();
                    }
                }
            }
        }

        private void ListAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listAnimals.SelectedItem != null && listAnimals.SelectedValue != null)
            {
                textHelper.Text = (listAnimals.SelectedItem as DataRowView)["Name"].ToString();
            }
            else
            {
                textHelper.Text = "";
            }
        }

        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (listAnimals.SelectedItem != null)
            {
                string sql = "delete from Animal where Id=@AnimalId";
                SqlCommand sqlCmd = new SqlCommand(sql, sqlConnection);
                sqlCmd.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);
                using (sqlCmd)
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlCmd.ExecuteScalar();
                    }
                    catch (Exception exDel)
                    {
                        MessageBox.Show(exDel.Message);
                    }
                    finally
                    {
                        sqlConnection.Close();
                        ShowAllAnimals();
                    }
                }
            }
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (listAnimals.SelectedItem != null)
            {
                try
                {
                    string sql = "insert into Animal values (@Name)";
                    SqlCommand sqlCmd = new SqlCommand(sql, sqlConnection);
                    sqlCmd.Parameters.AddWithValue("@Name", textHelper.Text);
                    using (sqlCmd)
                    {
                        try
                        {
                            sqlConnection.Open();
                            sqlCmd.ExecuteScalar();
                        }
                        catch (Exception exDel)
                        {
                            MessageBox.Show(exDel.Message);
                        }
                        finally
                        {
                            sqlConnection.Close();
                            ShowAllAnimals();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (listAnimals.SelectedItem != null)
            {
                try
                {
                    string sql = "update Animal set Name=@Name where Id=@AnimalId";
                    SqlCommand sqlCmd = new SqlCommand(sql, sqlConnection);
                    sqlCmd.Parameters.AddWithValue("@Name", textHelper.Text);
                    sqlCmd.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);
                    using (sqlCmd)
                    {
                        try
                        {
                            sqlConnection.Open();
                            sqlCmd.ExecuteScalar();
                        }
                        catch (Exception exDel)
                        {
                            MessageBox.Show(exDel.Message);
                        }
                        finally
                        {
                            sqlConnection.Close();
                            ShowAllAnimals();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void DeleteAnimalFromZoo_Click(object sender, RoutedEventArgs e)
        {
            if (listZoos.SelectedItem!=null && listZooAnimal.SelectedItem != null)
            {
                string sql = "delete from ZooAnimal where AnimalId=@AnimalId and ZooId=@ZooId";
                SqlCommand sqlCmd = new SqlCommand(sql, sqlConnection);
                sqlCmd.Parameters.AddWithValue("@AnimalId", listZooAnimal.SelectedValue);
                sqlCmd.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                using (sqlCmd)
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlCmd.ExecuteScalar();
                    }
                    catch (Exception exDel)
                    {
                        MessageBox.Show(exDel.Message);
                    }
                    finally
                    {
                        sqlConnection.Close();
                        ShowZooAnimals();
                    }
                }
            }
        }

        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            if (listZoos.SelectedItem != null && listAnimals.SelectedItem != null)
            {
                string sql = "insert into ZooAnimal values (@ZooId,@AnimalId)";
                SqlCommand sqlCmd = new SqlCommand(sql, sqlConnection);
                sqlCmd.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);
                sqlCmd.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                using (sqlCmd)
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlCmd.ExecuteScalar();
                    }
                    catch (Exception exDel)
                    {
                        MessageBox.Show(exDel.Message);
                    }
                    finally
                    {
                        sqlConnection.Close();
                        ShowZooAnimals();
                    }
                }
            }
        }
    }
}
