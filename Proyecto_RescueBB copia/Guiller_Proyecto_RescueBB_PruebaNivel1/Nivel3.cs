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
    public partial class Nivel3 : Form
    {
        //Conexion SQL
        DataClasses1DataContext usuarios = new DataClasses1DataContext();

        bool goLeft, goRight, jumping;
        int jumpSpeed;
        int force;
        int score;
        int playerSpeed = 9;

        int horizontalSpeed = 3;
        int verticalSpeed = 5;

        int enemyOneSpeed = 5;

        public Nivel3(string valor, String score)
        {
            InitializeComponent();
            lbUser.Text = valor.ToString();
            lbScore.Text = score.ToString();
            playBackground();
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            player.Top += jumpSpeed;

            if (goLeft == true)
            {
                player.Left -= playerSpeed;
            }
            if (goRight == true)
            {
                player.Left += playerSpeed;
            }

            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            if (jumping == true)
            {
                jumpSpeed = -10;
                force -= 1;
            }
            else
            {
                jumpSpeed = 15;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    //Evento que nos permite volver al saltar volviendo a estar en contacto con una plataforma
                    if ((string)x.Tag == "platform")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            force = 8;
                            player.Top = x.Top - player.Height;

                            if ((string)x.Name == "horizontalPlatform1" && goLeft == false || (string)x.Name == "horizontalPlatform1" && goRight == false)
                            {
                                player.Left -= horizontalSpeed;
                            }
                        }
                        x.BringToFront();
                    }

                    //eventos al recolectar monedas
                    if ((string)x.Tag == "coin")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                            score++;
                            lbScore.Text = (int.Parse(lbScore.Text) + 1).ToString();

                            wMediaPlayer.URL = @"C:\Users\Tarde\Desktop\Asata Saúl Otero Melchor\RescueBB\Proyecto_RescueBB copia\Guiller_Proyecto_RescueBB_PruebaNivel1\Resources\coinsound.wav";
                            wMediaPlayer.Ctlcontrols.play();

                        }
                    }

                    //evento al chocar con enemigos
                    if ((string)x.Tag == "enemy")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            JugadorPuntaje myJugador = usuarios.JugadorPuntaje.Single(u =>
                                u.Nombre == lbUser.Text);
                            myJugador.Nivel = "Nivel 3";
                            myJugador.Monedas = score;
                            usuarios.SubmitChanges();

                            gametimer.Stop();
                            lbScore.Text = score.ToString();
                            lbEvento.Text = "Cagaste";

                            //Si mueres empiezas desde el principio
                            Nivel1 lvl1 = new Nivel1(lbUser.Text);
                            lvl1.Show();
                            this.Close();
                        }
                    }
                }
            }

            //Movimiento de las plataformas
            verticalPlatform1.Top += verticalSpeed;

            if (verticalPlatform1.Top < 135 || verticalPlatform1.Top > 478)
            {
                verticalSpeed = -verticalSpeed;
            }

            horizontalPlatform1.Left -= horizontalSpeed;

            if (horizontalPlatform1.Left < 575 || horizontalPlatform1.Left > 955)
            {
                horizontalSpeed = -horizontalSpeed;
            }

            //Movimiento de los enemigos
            enemy1.Left -= enemyOneSpeed;

            if (enemy1.Left < verticalPlatform1.Left || enemy1.Left + enemy1.Width > verticalPlatform1.Left + verticalPlatform1.Width)
            {
                enemyOneSpeed = -enemyOneSpeed;

                if (enemy1.Left < verticalPlatform1.Left)
                {
                    enemy1.Image = Properties.Resources.patoright;
                }
                if (enemy1.Left + enemy1.Width > verticalPlatform1.Left + verticalPlatform1.Width)
                {
                    enemy1.Image = Properties.Resources.Pato;
                }

            }

            //Game Over si nos caemos al vacio
            if (player.Top + player.Height > this.ClientSize.Height + 50)
            {
                JugadorPuntaje myJugador = usuarios.JugadorPuntaje.Single(u =>
                u.Nombre == lbUser.Text);
                myJugador.Nivel = "Nivel 3";
                myJugador.Monedas = score;
                usuarios.SubmitChanges();


                gametimer.Stop();
                lbScore.Text = score.ToString();
                lbEvento.Text = "Sa matao Paco!";

                //Si mueres empiezas desde el principio
                Nivel1 lvl1 = new Nivel1(lbUser.Text);
                lvl1.Show();
                this.Close();
            }

            //Evento de Victoria (LLegar al final)
            if (player.Bounds.IntersectsWith(goal.Bounds))
            {
                JugadorPuntaje myJugador = usuarios.JugadorPuntaje.Single(u =>
                u.Nombre == lbUser.Text);
                myJugador.Nivel = "Nivel 3";
                myJugador.Monedas = int.Parse(lbScore.Text);
                usuarios.SubmitChanges();

                gametimer.Stop();
                lbEvento.Text = "Ganaste bro!";

                this.Hide();
                Fin final = new Fin();
                final.Show();
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
                player.Image = Properties.Resources.playerleft;

            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
                player.Image = Properties.Resources.player;

            }
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
            if (e.KeyCode == Keys.Escape) //SI PULSAMOS EL ESCAPE NOS CIERRA EL JUEGO
            {
                Application.Exit();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (jumping == true)
            {
                jumping = false;
            }
        }

        private void playBackground()
        {
            SoundPlayer background = new SoundPlayer(Properties.Resources.Level3Music);
            background.PlayLooping();

        }

    }
}
