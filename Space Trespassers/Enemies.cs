using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Space_Trespassers
{
    public class Enemies
    {
        float Health = 2;
        int speed = 15;
        Timer enemyTM = new Timer();
        static Random r = new Random();


        public PictureBox EnemyBox = new PictureBox
        {
            Tag = "enemy",
            BackColor = Color.DarkRed,
            Size = new Size(30, 30),
            Location = new Point(r.Next(-3500, 3500), r.Next(-3500, 3500)),
            SizeMode = PictureBoxSizeMode.Zoom,
        };

        public void drawEnemy(Form form)
        {
            form.Controls.Add(EnemyBox);
            EnemyBox.BringToFront();
            enemyTM.Interval = 15;
            enemyTM.Tick += (object s, EventArgs a) => enemyTM_tick(s, a, form);
            enemyTM.Start();
        }

        private void enemyTM_tick(object sender, EventArgs e, Form form)
        {
            foreach (Control x in form.Controls.OfType<PictureBox>().Where(c => (string)c.Tag == "bulletplayer").Where(x => x.Bounds.IntersectsWith(EnemyBox.Bounds)))
            {
                Health--;
            }

            if (Health <= 0)
            {
                Delete();
            }
            float xDiff = PlayForm.Player.Left - EnemyBox.Left;
            float yDiff = PlayForm.Player.Top - EnemyBox.Top;
            double angle = Math.Atan2(yDiff, xDiff);
            if (Math.Sqrt((Math.Pow(PlayForm.Player.Left - EnemyBox.Left, 2) + Math.Pow(PlayForm.Player.Top - EnemyBox.Top, 2))) > r.Next(100, 700))
            {
                EnemyBox.Left += Convert.ToInt32(Math.Cos(angle) * speed);
                EnemyBox.Top += Convert.ToInt32(Math.Sin(angle) * speed);
            }
            else
            {
                EnemyBox.Left += Convert.ToInt32(Math.Cos(angle + 90) * speed);
                EnemyBox.Top += Convert.ToInt32(Math.Sin(angle + 90) * speed);
            }

            if (r.Next(1, 200) == 3)
            {
                bullet shoot = new bullet();
                shoot.mkBullet(form, EnemyBox, (180 / Math.PI) * angle, 15, Color.Red);
            }
        }

        public void Delete()
        {
            enemyTM.Stop();
            enemyTM.Dispose();
            PlayForm.ActiveForm.Controls.Remove(EnemyBox);
            EnemyBox.Dispose();
        }

    }
    /*public class redEnemy : Enemy
    {


    }*/
}

