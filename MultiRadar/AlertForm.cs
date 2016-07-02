﻿using ACT.RadarViewOrder;
using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace MultiRadar
{
    public partial class AlertForm : Form
    {
        Action callbackSaveSetting = null;
        public Action CallbackSaveSetting
        {
            set { callbackSaveSetting = value; }

        }

        private static System.Timers.Timer t;
        public AlertForm()
        {
            InitializeComponent();
            this.TransparencyKey = this.BackColor;
            this.Opacity = transparent;

            t = new System.Timers.Timer(3000);

            t.Elapsed += OnTimedEvent;
            viewCount = 5;
            t.Enabled = true;

        }
        private int viewCount = 0;
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (RadarViewOrder.hitMobdatas != null)
            {
                if (RadarViewOrder.hitMobdatas.Count > 0)
                {
                    RadarViewOrder.HitMobdata mob = RadarViewOrder.hitMobdatas[RadarViewOrder.hitMobdatas.Count - 1];
                    mob.RemoveAt(RadarViewOrder.hitMobdatas.Count - 1);
                    lbMessage.Text = mob.rank.ToUpper() + ":" + mob.mobName;
                    viewCount = 5;
                    if (mob.rank == "s") { RadarViewOrder.PlaySeS(); }
                    if (mob.rank == "a") { RadarViewOrder.PlaySeA(); }
                    if (mob.rank == "b") { RadarViewOrder.PlaySeB(); }
                    if (mob.rank == "e") { RadarViewOrder.PlaySeB(); }
                }
            }
            switch (viewCount)
            {
                case 5: lbMessage.ForeColor = Color.AliceBlue; break;
                case 4: lbMessage.ForeColor = Color.Yellow; break;
                case 3: lbMessage.ForeColor = Color.Lime; break;
                case 2: lbMessage.ForeColor = Color.Blue; break;
                case 1: lbMessage.ForeColor = Color.White; break;

            }
            viewCount -= 1;
            if (viewCount < 0) { viewCount = 0; }
            if (viewCount > 0)
            {
                this.Visible = true;
            }
            else
            {
                this.Visible = false;
            }
        }

        float transparent = 0.7f;

        private Point mousePoint;
        private bool saveCall = false;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //位置を記憶する
                mousePoint = new Point(e.X, e.Y);
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
                saveCall = true;
            }

        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            if (saveCall)
            {
                saveCall = false;
                if (callbackSaveSetting != null)
                {
                    callbackSaveSetting();
                }
            }
        }
    }
}
