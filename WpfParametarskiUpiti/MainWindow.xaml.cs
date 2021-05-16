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
using System.Data.SqlClient;
using System.Data;

namespace WpfParametarskiUpiti
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string poruka = "";
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnMagacin))
            {
                SqlCommand komanda = new SqlCommand("SELECT * FROM Kategorija", konekcija);

                try
                {
                    konekcija.Open();
                    SqlDataReader dr = komanda.ExecuteReader();

                    while (dr.Read())
                    {
                        Kategorija k = new Kategorija
                        {
                            KategorijaId = dr.GetInt32(0),
                            NazivKategorije = dr.GetString(1),
                            OpisKategorije = dr.GetString(2)
                        };

                        Combo1.Items.Add(k);
                        Combo1.SelectedIndex = 0;
                    }

                }
                catch (Exception xcp)
                {
                    poruka = xcp.Message;
                }
            }

            if (!string.IsNullOrWhiteSpace(poruka))
            {
                MessageBox.Show(poruka);
            }
        }

        private void Combo1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Kategorija k = Combo1.SelectedItem as Kategorija;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ProizvodId, NazivProizvoda, Cena");
            sb.AppendLine("FROM Proizvod");
            sb.AppendLine("WHERE KategorijaId= @KategorijaId");
            string poruka = "";
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnMagacin))
            {
                SqlCommand komanda = new SqlCommand(sb.ToString(), konekcija);
                //SqlParameter KategorijaIdParam = new SqlParameter("@KategorijaId", SqlDbType.Int);
                ////KategorijaIdParam.Direction = ParameterDirection.Input; //podrazumevano
                //KategorijaIdParam.Value = k.KategorijaId;
                //komanda.Parameters.Add(KategorijaIdParam);

                komanda.Parameters.AddWithValue("@KategorijaId", k.KategorijaId);

                try
                {
                    konekcija.Open();
                    SqlDataReader dr = komanda.ExecuteReader();
                    sb.Clear();
                    while (dr.Read())
                    {
                        sb.AppendLine($"{dr[0].ToString()} {dr[1].ToString()} {dr[2].ToString()}");
                    }

                    TextBox1.Text = sb.ToString();
                }
                catch (Exception xcp)
                {
                    poruka = xcp.Message;
                }
            }

            if (!string.IsNullOrWhiteSpace(poruka))
            {
                MessageBox.Show(poruka);
            }
        }
    }
}
