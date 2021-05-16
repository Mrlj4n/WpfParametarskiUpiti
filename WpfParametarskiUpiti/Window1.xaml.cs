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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace WpfParametarskiUpiti
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private List<Kategorija> VratiListuKategorije()
        {
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnMagacin))
            {
                SqlCommand komanda = new SqlCommand("SELECT * FROM Kategorija", konekcija);

                List<Kategorija> listaKategorija = new List<Kategorija>();

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

                        listaKategorija.Add(k);
                    }

                    return listaKategorija;

                }
                catch (Exception)
                {

                    return null;
                }
            }
        }

        //STAMPAJ =================
        private void Stampaj()
        {
            List<Kategorija> listaKategorija = VratiListuKategorije();
            Combo1.Items.Clear();
            if (listaKategorija != null)
            {
                foreach (Kategorija k in listaKategorija)
                {
                    Combo1.Items.Add(k);
                }
            }
        }

        private void ButtonUbaci_Click(object sender, RoutedEventArgs e)
        {
            string upit = "INSERT INTO Kategorija VALUES (@NazivKategorije,@OpisKategorije)";
            string por = "";

            SqlConnection konekcija = new SqlConnection(Konekcija.cnnMagacin);
            SqlCommand komanda = new SqlCommand(upit, konekcija);
            komanda.Parameters.AddWithValue("@NazivKategorije",TextBoxNaziv.Text);
            komanda.Parameters.AddWithValue("@OpisKategorije", TextBoxOpis.Text);

            try
            {
                konekcija.Open();
                komanda.ExecuteNonQuery();
                Stampaj();
                Resetuj();
            }
            catch (Exception xcp)
            {

                por = xcp.Message;
            }
            finally
            {
                konekcija.Dispose();
            }

            if (!string.IsNullOrWhiteSpace(por))
            {
                MessageBox.Show(por);
            }
            else
            {
                MessageBox.Show("Ubacena nova kategorija");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Stampaj();
        }

        private void Combo1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Combo1.SelectedIndex > -1)
            {
                Kategorija sel = Combo1.SelectedItem as Kategorija;
                TextBoxId.Text = sel.KategorijaId.ToString();
                TextBoxNaziv.Text = sel.NazivKategorije;
                TextBoxOpis.Text = sel.OpisKategorije;
            }
        }

        //Resetuj

        private void Resetuj()
        {
            TextBoxId.Clear();
            TextBoxNaziv.Clear();
            TextBoxOpis.Clear();
            Combo1.SelectedIndex = -1;
        }

        private void ButtonResetuj_Click(object sender, RoutedEventArgs e)
        {
            Resetuj();
        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            string por = "";
            if (Combo1.SelectedIndex > -1)
            {

                var rez = MessageBox.Show("Da li ste sigurni?", "?", MessageBoxButton.OKCancel, MessageBoxImage.Information);

                if (rez == MessageBoxResult.OK)
                {
                    using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnMagacin))
                    {
                        SqlCommand komanda = new SqlCommand("DELETE FROM Kategorija WHERE KategorijaId = @KategorijaId", konekcija);

                        komanda.Parameters.AddWithValue("@KategorijaId", TextBoxId.Text);

                        try
                        {
                            konekcija.Open();
                            komanda.ExecuteNonQuery();
                            Stampaj();
                            Resetuj();
                        }
                        catch (Exception xcp)
                        {
                            por = xcp.Message;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(por))
                    {
                        MessageBox.Show(por);
                    }
                    else
                    {
                        MessageBox.Show("Kategorija obrisana");
                    }
                }
            }
            else
            {
                MessageBox.Show("Odaberite kategoriju");
            }
        }

        private void ButtonPromeni_Click(object sender, RoutedEventArgs e)
        {
            int selId = Combo1.SelectedIndex;
            if (selId > -1)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("UPDATE Kategorija");
                sb.AppendLine("SET NazivKategorije = @NazivKategorije,");
                sb.AppendLine("OpisKategorije=@OpisKategorije");
                sb.AppendLine("WHERE KategorijaId = @KategorijaId");

                string por = "";

                using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnMagacin))
                {
                    SqlCommand komanda = new SqlCommand(sb.ToString(),konekcija);
                    komanda.Parameters.AddWithValue("@NazivKategorije", TextBoxNaziv.Text);
                    komanda.Parameters.AddWithValue("@OpisKategorije", TextBoxOpis.Text);
                    komanda.Parameters.AddWithValue("@KategorijaId", TextBoxId.Text);

                    try
                    {
                        konekcija.Open();
                        komanda.ExecuteNonQuery();
                        Stampaj();
                        Resetuj();

                        Combo1.SelectedIndex = selId;
                    }
                    catch (Exception xcp)
                    {
                        por = xcp.Message;
                    }
                }

                if (!string.IsNullOrWhiteSpace(por))
                {
                    MessageBox.Show(por);
                }
                else
                {
                    MessageBox.Show("Podaci promenjeni");
                }
            }
            else
            {
                MessageBox.Show("Odaberite kategoriju");
            }
        }
    }
}
