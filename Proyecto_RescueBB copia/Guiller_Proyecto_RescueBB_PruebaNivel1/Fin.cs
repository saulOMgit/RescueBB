using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Guiller_Proyecto_RescueBB_PruebaNivel1
{
    public partial class Fin : Form
    {
        DataClasses1DataContext usuarios = new DataClasses1DataContext();
        public Fin()
        {
            InitializeComponent();
            cargarGrid();
        }

        void cargarGrid()
        {
            var listargrid = from u in usuarios.JugadorPuntaje
                             orderby u.Monedas descending
                             select u;
            GridDatos.DataSource = listargrid;
        }

        private void Fin_Load(object sender, EventArgs e)
        {
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnVuelveInicio_Click(object sender, EventArgs e)
        {
            this.Hide();
            Inicio initation = new Inicio();
            initation.Show();
        }
    }
}
