using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Guiller_Proyecto_RescueBB_PruebaNivel1
{
    public partial class Inicio : System.Windows.Forms.Form
    {
        DataClasses1DataContext usuarios = new DataClasses1DataContext();
        public Inicio()
        {
            InitializeComponent();
            cargarGrid();
            playSimpleSound();
        }

        void cargarGrid()
        {
            var listargrid = from u in usuarios.JugadorPuntaje
                             orderby u.Monedas descending
                             select u;
            GridDatos.DataSource = listargrid;
        }

        public void Valor(string _dato)
        {
            txtNick.Text = _dato;
        }

        private void btnNuevaPartida_Click(object sender, EventArgs e)
        {
            if (txtNick.Text == "")
            {
                MessageBox.Show("INTRODUCE UN NOMBRE");
            }
            else
            {
                try
                {
                    int contarJugador = 0;
                    var BuscarUsuario = usuarios.JugadorPuntaje.Where(c => c.Nombre == txtNick.Text).Single();
                    contarJugador = BuscarUsuario.Nombre.Length;

                    this.Hide();
                    Nivel1 lvl1 = new Nivel1(txtNick.Text);
                    lvl1.Show();
                }
                catch
                {
                    JugadorPuntaje myJugador = new JugadorPuntaje();
                    myJugador.Nombre = txtNick.Text;
                    usuarios.JugadorPuntaje.InsertOnSubmit(myJugador);
                    usuarios.SubmitChanges();
                    cargarGrid();
                    this.Hide();
                    Nivel1 f1 = new Nivel1(txtNick.Text);
                    f1.Show();
                }
            } 
        }

       

        private void playSimpleSound()
        {
            try
            {

                SoundPlayer simpleSound = new SoundPlayer(@"..\..\Resources\Battleship.wav");
                simpleSound.PlayLooping();
            }
            catch { }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
