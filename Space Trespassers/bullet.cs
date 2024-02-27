using System;

using System.Drawing;
using System.Windows.Forms;
namespace Space_Trespassers
{
    class bullet
    {
        Form form;
        public PictureBox Bullet = new PictureBox();
        Timer tm = new Timer();

        public void mkBullet(Form aform, Control caster, double angle, int speed, Color color)
        {
            form = aform;
            int BulletLeftDirection = Convert.ToInt32(Math.Cos((Math.PI / 180) * angle) * speed);
            int BulletTopDirection = Convert.ToInt32(Math.Sin((Math.PI / 180) * angle) * speed);
            Bullet.BackColor = color;
            Bullet.Size = new Size(7, 7);
            Bullet.Tag = "bullet" + caster.Tag;
            Bullet.Left = caster.Left + (caster.Width / 2);
            Bullet.Top = caster.Top + (caster.Height / 2);
            form.Controls.Add(Bullet);
            Bullet.BringToFront();
            tm.Interval = 2;
            tm.Tick += (object s, EventArgs a) => tm_Tick(s, a, BulletLeftDirection, BulletTopDirection);
            tm.Start();
        }
        public void tm_Tick(object sender, EventArgs e, int LeftDirection, int TopDirection)
        {
            Bullet.Left += LeftDirection;
            Bullet.Top += TopDirection;
            if (Bullet.Left < -100 || Bullet.Left > 1000 || Bullet.Top < -100 || Bullet.Top > 1000)
            {
                Delete();
            }
        }
        public void Delete()
        {
            tm.Stop();
            tm.Dispose();
            form.Controls.Remove(Bullet);
            Bullet.Dispose();
        }
    }
}
