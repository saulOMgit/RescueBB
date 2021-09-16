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
    public partial class Nivel1 : Form
    {
        //Conexion SQL
        DataClasses1DataContext usuarios = new DataClasses1DataContext();

        bool goLeft, goRight, jumping;
        int jumpSpeed;
        int force;
        int score = 0;
        int playerSpeed = 9;

        int horizontalSpeed = 5;
        int verticalSpeed = 7;

        int enemyOneSpeed = 11;
        int enemyTwoSpeed = 6;


        public Nivel1(string valor)
        {
            InitializeComponent();
            lbUser.Text = valor.ToString();
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            lbScore.Text = score.ToString();         

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

                            if ((string)x.Name == "horizontalPlatform" && goLeft == false || (string)x.Name == "horizontalPlatform" && goRight == false)
                            {
                                player.Left -= horizontalSpeed;
                            }
                        }
                        x.BringToFront();
                    }
                    //eventos al recolectar monedar
                    if ((string)x.Tag == "coin")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                            score++;
                            playSimpleSound();
                        }
                    }

                    //evento al chocar con enemigos
                    if ((string)x.Tag == "enemy")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            JugadorPuntaje myJugador = usuarios.JugadorPuntaje.Single(u =>
                            u.Nombre == lbUser.Text);
                            myJugador.Nivel = "Nivel 1";
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

            verticalPlatform.Top += verticalSpeed;

            if (verticalPlatform.Top < 107 || verticalPlatform.Top > 325)
            {
                verticalSpeed = -verticalSpeed;
            }

            //Movimiento de los enemigos
            enemy1.Left -= enemyOneSpeed;

            if (enemy1.Left < pictureBox1.Left || enemy1.Left + enemy1.Width > pictureBox1.Left + pictureBox1.Width)
            {
                enemyOneSpeed = -enemyOneSpeed;

                if (enemy1.Left < pictureBox1.Left)
                {
                    enemy1.Image = Properties.Resources.enemyright;
                }
                if (enemy1.Left + enemy1.Width > pictureBox1.Left + pictureBox1.Width)
                {
                    enemy1.Image = Properties.Resources.Enemy;
                }
            }

            enemy2.Left -= enemyTwoSpeed;

            if (enemy2.Left < pictureBox3.Left || enemy2.Left + enemy2.Width > pictureBox3.Left + pictureBox3.Width)
            {
                enemyTwoSpeed = -enemyTwoSpeed;

                if (enemy2.Left < pictureBox3.Left)
                {
                    enemy2.Image = Properties.Resources.enemyright;
                }
                if (enemy2.Left + enemy2.Width > pictureBox3.Left + pictureBox3.Width)
                {
                    enemy2.Image = Properties.Resources.Enemy;
                }

            }

            //Game Over si nos caemos al vacio
            if (player.Top + player.Height > this.ClientSize.Height + 50)
            {
                JugadorPuntaje myJugador = usuarios.JugadorPuntaje.Single(u =>
                  u.Nombre == lbUser.Text);
                myJugador.Nivel = "Nivel 1";
                myJugador.Monedas = score;
                usuarios.SubmitChanges();

                //Si mueres empiezas desde el principio
                Nivel1 lvl1 = new Nivel1(lbUser.Text);
                lvl1.Show();
                this.Close();
            }

            //Evento de Victoria (LLegar al final)
            //Tienes que conseguir 26 monedas para ganar -->&& score == 26
            if (player.Bounds.IntersectsWith(door.Bounds))
            {
                JugadorPuntaje myJugador = usuarios.JugadorPuntaje.Single(u =>
                u.Nombre == lbUser.Text);
                myJugador.Nivel = "Nivel 1";
                myJugador.Monedas = score;
                usuarios.SubmitChanges();

                gametimer.Stop();
                lbScore.Text = score.ToString();
                lbEvento.Text = "Ganaste bro!";

                //Para pasar de nivel
                this.Hide();
                Nivel2 lvl2 = new Nivel2(lbUser.Text, lbScore.Text);
                lvl2.Show();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
        private void playSimpleSound()
        {
            SoundPlayer simpleSound = new SoundPlayer(@"..\..\Resources\coinsound.wav");
            simpleSound.Play();
        }
    }
}
