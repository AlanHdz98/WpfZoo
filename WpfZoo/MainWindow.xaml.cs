using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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


namespace WpfZoo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();


            string conectionString = ConfigurationManager.ConnectionStrings["WpfZoo.Properties.Settings.EscDirectaDbConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(conectionString);
            miTextBox.Clear();
            muestraZoos();
            MostrarTodosAnimales();

        }

        private void muestraZoos()
        {
            

            try
            {
                string consulta = "select * from Zoo";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, sqlConnection);

                using (sqlDataAdapter)
                {


                    DataTable zooTabla = new DataTable();
                    sqlDataAdapter.Fill(zooTabla);

                    ListaZoos.DisplayMemberPath = "Ubicacion";
                    ListaZoos.SelectedValuePath = "Id";
                    ListaZoos.ItemsSource = zooTabla.DefaultView;


                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        private void muestraAnimalesAsociados()
        {
            

            try
            {
                string consulta = " select * from Animal a Inner Join AnimalZoo az on a.Id= az.Animalid where az.ZooId = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);

                    DataTable animalTabla = new DataTable();
                    sqlDataAdapter.Fill(animalTabla);

                    ListaAnimalesAsociados.DisplayMemberPath = "Nombre";
                    ListaAnimalesAsociados.SelectedValuePath = "Id";
                    ListaAnimalesAsociados.ItemsSource = animalTabla.DefaultView;


                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        private void ListaZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            muestraAnimalesAsociados();
            MuestraZooElegidoEnTextBox();
           
            
        }

        private void MostrarTodosAnimales()
        {

            try
            {
                string consulta = " select * from Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, sqlConnection);

                using (sqlDataAdapter)
                {


                    DataTable AnimalesTabla = new DataTable();
                    sqlDataAdapter.Fill(AnimalesTabla);

                    ListadeAnimales.DisplayMemberPath = "Nombre";
                    ListadeAnimales.SelectedValuePath = "Id";
                    ListadeAnimales.ItemsSource = AnimalesTabla.DefaultView;


                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        private void MuestraZooElegidoEnTextBox()
        {


            try
            {
                string consulta = "select Ubicacion from Zoo where Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);

                    DataTable ZooTabla = new DataTable();
                    sqlDataAdapter.Fill(ZooTabla);

                    miTextBox.Text = ZooTabla.Rows[0]["Ubicacion"].ToString();


                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        private void ListadeAnimales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraAnimalElegidoEnTextBox();
        }
        private void MuestraAnimalElegidoEnTextBox()
        {


            try
            {
                string consulta = "select Nombre from Animal where Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", ListadeAnimales.SelectedValue);

                    DataTable AnimalesTabla = new DataTable();
                    sqlDataAdapter.Fill(AnimalesTabla);

                    miTextBox.Text = AnimalesTabla.Rows[0]["Nombre"].ToString();


                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        private void EliminarZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Delete from Zoo where Id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                muestraZoos();
            }
            
        }

        private void AgregarZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Insert Into Zoo values (@ubicacion)";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ubicacion", miTextBox.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                muestraZoos();
                miTextBox.Clear();
            }
        }

        private void AgregarAnimalZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Insert Into AnimalZoo values (@ZooId, @AnimalId)";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListadeAnimales.SelectedValue);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                muestraAnimalesAsociados();
               
            }
        }

        private void EliminarAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Delete from Animal where Id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListadeAnimales.SelectedValue);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                MostrarTodosAnimales();
            }

        }

        private void NuevoAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Insert Into Animal values (@Animal)";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Animal", miTextBox.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                MostrarTodosAnimales();
                miTextBox.Clear();
            }
        }

       

        private void ActualizarZoo_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string consulta = "Update Zoo Set Ubicacion = @ubicacion where Id = @zooId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Ubicacion", miTextBox.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                muestraZoos();
                miTextBox.Clear();

            }


        }

        private void ActualizarAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Update Animal Set Nombre = @nombre where Id = @animalId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@animalId", ListadeAnimales.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@nombre", miTextBox.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                MostrarTodosAnimales();
                miTextBox.Clear();

            }
        }
    }
}
