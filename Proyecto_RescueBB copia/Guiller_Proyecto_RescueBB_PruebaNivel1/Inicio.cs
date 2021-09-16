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
        //Esta va a ser nuestra primera ventana, Servira para iniciar sesión en el juego

        //Conexión con la Base de datos
        DataClasses1DataContext usuarios = new DataClasses1DataContext();
        public Inicio()
        {
            InitializeComponent();
            cargarGrid();
            playSimpleSound();
        }

        //Carga en el Grid los datos de nuestra Database, Serán ordenador por las monedas para dar sensación de Ranking
        void cargarGrid()
        {
            var listargrid = from u in usuarios.JugadorPuntaje
                             orderby u.Monedas descending
                             select u;
            GridDatos.DataSource = listargrid;
        }

        //Con esto importaremos el usuario para que nos ponga el nombre en la parte superior de cada pantalla y tambien lo usaremos para actualizar la Database
        public void Valor(string _dato)
        {
            txtNick.Text = _dato;
        }

        //Comprobamos que hayamos ingresado un nombre, para evitarlo en caso contrario
        //En caso de que no exista el usuario lo creara en la base de datos y si existe podremos jugar igual sobreescribiendo la partida
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

       
        //BSO Por Guillermo Iglesias
        private void playSimpleSound()
        {
            try
            {

                //SoundPlayer simpleSound = new SoundPlayer(@"..\..\Resources\Battleship.wav");
                SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.MainMusic);

                simpleSound.PlayLooping();
            }
            catch { }
        }
        //Botón para salir de la aplicación
        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
