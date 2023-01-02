using System;
using System.Collections.Generic;
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

namespace CRUD_WPF_OlaMundo
{
    // /<summary>
    // /Interaction logic for MainWindow.xaml
    // /</summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            txb_nome.Focus();

            carregaGrid();

        }

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-K8FS7AT;Initial Catalog=CRUD_WPF;User ID=sa;Password=Sql@ge1971;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        public void limparCampos()
        {
            txb_nome.Clear();
            txb_sobrenome.Clear();
            txb_idade.Clear();
            txb_email.Clear();
            txb_ID.Clear();
        }

        public void carregaGrid()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select ID, nome as Nome, idade as Idade, email as Email from tabelaCrud", con);
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            dgvDisplay.ItemsSource = dt.DefaultView;
        }

        private void btn_limpar_Click(object sender, RoutedEventArgs e)
        {
            limparCampos();
        }

        public bool isValid()
        {
            if (txb_nome.Text == string.Empty)
            {
                MessageBox.Show("Campo Nome obrigatório", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (txb_sobrenome.Text == string.Empty)
            {
                MessageBox.Show("Campo Sobrenome obrigatório", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (txb_idade.Text == string.Empty)
            {
                MessageBox.Show("Campo Idade obrigatório", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (txb_email.Text == string.Empty)
            {
                MessageBox.Show("Campo E-Mail obrigatório", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void btn_Inserir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into tabelaCrud values (@nome, @sobrenome, @idade, @email)");
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@nome", txb_nome.Text);
                    cmd.Parameters.AddWithValue("@sobrenome", txb_sobrenome.Text);
                    cmd.Parameters.AddWithValue("@idade", txb_idade.Text);
                    cmd.Parameters.AddWithValue("@email", txb_email.Text);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    carregaGrid();
                    MessageBox.Show("Dados inseridos com sucesso!", "Salvo", MessageBoxButton.OK, MessageBoxImage.Information);
                    limparCampos();
                    txb_nome.Focus();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_Deletar_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from tabelaCrud where ID = @id", con);
            try
            {
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@id", txb_ID.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                limparCampos();
                carregaGrid();
                MessageBox.Show("Dados deletados com sucesso!", "Deletado", MessageBoxButton.OK, MessageBoxImage.Information);
                txb_nome.Focus();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Os dados não foram deletados. " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btn_Alterar_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update tabelaCrud set nome = @nome, sobrenome = @sobrenome, idade = @idade, email = @email where ID = @id", con);
            try
            {
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@id", txb_ID.Text);
                cmd.Parameters.AddWithValue("@nome", txb_nome.Text);
                cmd.Parameters.AddWithValue("@sobrenome", txb_sobrenome.Text);
                cmd.Parameters.AddWithValue("@idade", txb_idade.Text);
                cmd.Parameters.AddWithValue("@email", txb_email.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                carregaGrid();
                MessageBox.Show("Dados alterados com sucesso!", "Alterado", MessageBoxButton.OK, MessageBoxImage.Information);
                limparCampos();

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Os dados não foram alterados. " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void dgvDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            DataRowView linhaSelecionada = dg.SelectedItem as DataRowView;

            try
            {
                if (linhaSelecionada != null)
                {
                    txb_nome.Text = linhaSelecionada["nome"].ToString();
                    txb_sobrenome.Text = linhaSelecionada["sobrenome"].ToString();
                    txb_idade.Text = linhaSelecionada["idade"].ToString();
                    txb_email.Text = linhaSelecionada["email"].ToString();
                    txb_ID.Text = linhaSelecionada["id"].ToString();
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Não foi possível selecionar este item.", ex.Message);
            }

        }
    }
}
