using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Space_Trespassers
{
    public partial class PlayForm : Form
    {
        bool goLeft, goRight, goUp, goDown, shooting;
        float playerHealth = 10;
        int fps = 0; int fpss;
        double shootBraker;


        int angle = 0;
        int angleSpeed = 10;
        int playerSpeed = 2;
        int backgroundSpeedNow;
        int backgroundSpeed = 10;
        Bitmap playerImage = Properties.Resources.playerShip;


        public static PictureBox Player = new PictureBox
        {
            Name = "Player",
            Tag = "player",
            Size = new Size(25, 25),
            Location = new Point(450, 450),
            SizeMode = PictureBoxSizeMode.StretchImage,
        };

        public PlayForm()
        {
            InitializeComponent();
        }

        private void PlayForm_Load(object sender, EventArgs e)
        {
            this.Controls.Add(Player);
            Player.BringToFront();
            Player.Image = playerImage;
        }

        private void TeleportEvent(object sender, EventArgs e)
        {
            //Background Teleport
            //Background Left border
            if (background.Left > 0)
            {
                background.Left = -3500;
            }
            //Background Right border
            if (background.Left < -3500)
            {
                background.Left = 0;
            }
            //Background Top border
            if (background.Top < -3500)
            {
                background.Top = 0;
            }
            //Background Bottom border
            if (background.Top > 0)
            {
                background.Top = -3500;
            }
            fpss = fps;
            fps = 0;
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    goLeft = false;
                    break;
                case Keys.D:
                    goRight = false;
                    break;
                case Keys.S:
                    goDown = false;
                    break;
                case Keys.W:
                    goUp = false;
                    break;
                case Keys.Space:
                    shooting = false;
                    break;
                case Keys.E:
                    Enemies enemy = new Enemies();
                    enemy.drawEnemy(this);
                    break;

            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    goLeft = true;
                    break;
                case Keys.D:
                    goRight = true;
                    break;
                case Keys.S:
                    goDown = true;
                    break;
                case Keys.W:
                    goUp = true;
                    break;
                case Keys.Space:
                    shooting = true;
                    break;
            }
        }
        private void MainTimerEvent(object sender, EventArgs e)
        {
            angle = angle % 360;
            if (angle < 0)
            {
                angle = 360;
            }

            foreach (Control x in this.Controls.OfType<PictureBox>().Where(c => c != Player))
            {
                x.Left -= Convert.ToInt32(Math.Cos((Math.PI / 180) * angle) * backgroundSpeedNow);
                x.Top -= Convert.ToInt32(Math.Sin((Math.PI / 180) * angle) * backgroundSpeedNow);
            }

            if (Player.Left + Player.Width + 60 <= 900
                && Player.Left >= 60
                && Player.Top >= 60
                && Player.Top + Player.Height + 60 <= 861)
            {
                backgroundSpeedNow = backgroundSpeed;
                Player.Left += Convert.ToInt32(Math.Cos((Math.PI / 180) * angle) * playerSpeed);
                Player.Top += Convert.ToInt32(Math.Sin((Math.PI / 180) * angle) * playerSpeed);
            }

            //Top border
            else if (Player.Top < 60)
            {
                backgroundSpeedNow = backgroundSpeed + playerSpeed;
                if (Math.Sin((Math.PI / 180) * angle) * playerSpeed >= 0)
                {
                    Player.Top += Convert.ToInt32(Math.Sin((Math.PI / 180) * angle) * playerSpeed);
                }
            }
            //Bottom border
            else if (Player.Top + Player.Height + 60 > 861)
            {
                backgroundSpeedNow = backgroundSpeed + playerSpeed;
                if (Math.Sin((Math.PI / 180) * angle) * playerSpeed <= 0)
                {
                    Player.Top += Convert.ToInt32(Math.Sin((Math.PI / 180) * angle) * playerSpeed);
                }
            }
            //Left border
            else if (Player.Left < 60)
            {
                backgroundSpeedNow = backgroundSpeed + playerSpeed;
                if (Math.Cos((Math.PI / 180) * angle) * playerSpeed >= 0)
                {
                    Player.Left += Convert.ToInt32(Math.Cos((Math.PI / 180) * angle) * playerSpeed);
                }
            }
            //Right border
            else if (Player.Left + Player.Width + 60 > 900)
            {
                backgroundSpeedNow = backgroundSpeed + playerSpeed;
                if (Math.Cos((Math.PI / 180) * angle) * playerSpeed <= 0)
                {
                    Player.Left += Convert.ToInt32(Math.Cos((Math.PI / 180) * angle) * playerSpeed);
                }
            }

            int count = 0;
            foreach (Control x in this.Controls.OfType<PictureBox>().Where(c => (string)c.Tag == "bullet"))
            {
                count++;
            }

            fps++;

            txtScore.Text = Convert.ToString("Fps: " + fpss
                + " Angle: " + angle
                + ", Player.Top: " + Player.Top
                + ", Player.Left: " + Player.Left
                + " Cos: " + Math.Cos((Math.PI / 180) * angle)
                + " Sin: " + Math.Sin((Math.PI / 180) * angle)
                + " background.Left: " + background.Left
                + " background.Top: " + background.Top
                + " Number of Bullets: " + count);


            //Directions
            //Left
            if (goLeft && !goRight && !goUp && !goDown)
            {
                if (180 > angle && angle >= 0)
                {
                    angle += angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
                else if (360 > angle && angle > 180)
                {
                    angle -= angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
            }

            //Right
            if (!goLeft && goRight && !goUp && !goDown)
            {
                if (180 >= angle && angle > 0)
                {
                    angle -= angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
                else if (360 > angle && angle > 180)
                {
                    angle += angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
            }

            //Up
            if (!goLeft && !goRight && goUp && !goDown)
            {
                if (angle < 270 && angle > 90)
                {
                    angle += angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
                else if ((270 < angle && angle <= 360) || (90 >= angle && angle >= 0))
                {
                    angle -= angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
            }

            //Down
            if (!goLeft && !goRight && !goUp && goDown)
            {
                if ((270 <= angle && angle <= 360) || (90 > angle && angle >= 0))
                {
                    angle += angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
                else if (angle < 270 && angle > 90)
                {
                    angle -= angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
            }

            //UpRight
            if (!goLeft && goRight && goUp && !goDown && Player.Top > 60)
            {
                if (angle < 315 && angle > 135)
                {
                    angle += angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
                else if ((320 < angle && angle <= 360) || (135 >= angle && angle >= 0))
                {
                    angle -= angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
            }

            //UpLeft
            if (goLeft && !goRight && goUp && !goDown && Player.Top > 60)
            {
                if (angle < 225 && angle > 45)
                {
                    angle += angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
                else if ((230 < angle && angle <= 360) || (45 >= angle && angle >= 0))
                {
                    angle -= angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
            }

            //DownRight
            if (!goLeft && goRight && !goUp && goDown && Player.Top > 60)
            {
                if (angle <= 225 && angle > 45)
                {
                    angle -= angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
                else if ((230 <= angle && angle <= 360) || (35 >= angle && angle >= 0))
                {
                    angle += angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
            }

            //DownLeft
            if (goLeft && !goRight && !goUp && goDown && Player.Top > 60)
            {
                if (angle < 315 && angle > 135)
                {
                    angle -= angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
                else if ((320 < angle && angle <= 360) || (125 >= angle && angle >= 0))
                {
                    angle += angleSpeed;
                    Player.Image = Utilities.RotateImage(playerImage, angle);
                }
            }

            if (shooting)
            {
                shoot();
            }

            foreach (Control x in this.Controls.OfType<PictureBox>().Where(c => (string)c.Tag == "bulletenemy").Where(x => x.Bounds.IntersectsWith(Player.Bounds)))
            {
                playerHealth--;
            }

            if (playerHealth > 1)
            {
                playerHealthBar.Value = Convert.ToInt32(playerHealth);
            }
            else
            {
                playerHealthBar.Value = 0;
                //Player.Image = Properties.Resources.dead;ssss
                Application.Restart();
            }
        }
        private void shoot()
        {
            if (shootBraker >= 3)
            {
                bullet shoot = new bullet();
                shoot.mkBullet(this, Player, angle, 50, Color.Yellow);
                if (shootBraker > 2)
                {
                    shootBraker = 0;
                }
            }
            shootBraker += .5;
        }
    }
}