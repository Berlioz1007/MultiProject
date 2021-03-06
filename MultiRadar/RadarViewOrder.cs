﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace ACT.RadarViewOrder
{
    using MultiProject;
    using Radardata;
    using System.IO;
    using System.Windows;
    public static class RadarViewOrder
    {
        static bool playerView = false;
        private static int fontSize;
        public static int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
        public static int opacity;
        public static int Opacity
        {
            get { return opacity; }
            set { opacity = value; }
        }


        public static bool PlayerView
        {
            get { return playerView; }
            set { playerView = value; }
        }

        public static int bX = 0;
        public static int bY = 0;
        public static int bW = 0;
        public static int bH = 0;
        public static int infoX = 0;
        public static int infoY = 0;
        public static int scale = 16;
        static float scaleX = 0;
        static float scaleY = 0;

        private static bool soundEnable = true;
        public static bool SoundEnable
        {
            get {return soundEnable; }
            set { soundEnable = value; }
        }

        public static PointF GetBasePosition(int addX, int addY)
        {
            return new PointF(bX + addX, bY + addY);
        }

        public static double myRadian;
        public static Rect oldPlayerRect;
        public static Rect PlayerRect()
        {
            oldPlayerRect = new Rect((int)(scaleX * bW / bW) - 2, (int)(scaleY * bH / bH) - 2, 4, 4);
            return oldPlayerRect;
        }
        public static int keepWindowHeight;



        private static double getRadian(double x, double y, double x2, double y2)
        {
            double radian = Math.Atan2(y2 - y, x2 - x);
            return radian;
        }

        public static void SetBasePosition(int posX, int posY, int width, int height)
        {
            bX = posX;
            bY = posY;
            bH = height;
            bW = width;
            infoX = 0;// bX;
            infoY = 54;
            scaleX = bW / 2f;
            scaleY = bH / 2f;

        }

        public static Rect MobRect(float myX, float myY, float mobX, float mobY)
        {
            float x =   myX + (scale * (RadarZoom - 20))- (mobX);//0-2000
            float y =   myY + (scale * (RadarZoom - 20))- (mobY);

            x = (x * bW) / (scale * (RadarZoom - 20) * 2);// (scale * 2);//  400;
            y = (y * bH) / (scale * (RadarZoom - 20) * 2);// (scale * 2);//400;

            return new Rect((int)x-3, (int)y, 3, 3);
        }

        public static Point AreaPos()
        {
            float x = (145);//0-2000
            float y = (145);

            x = (x * bW) / (scale * (RadarZoom - 20));// (scale * 2);//  400;
            y = (y * bH) / (scale * (RadarZoom - 20));// (scale * 2);//400;

            return new Point((int)x , (int)y);
        }

        public static Rect AreaRect(int value)
        {
            float x = (value);//0-2000
            float y = (value);

            x = (x * bW) / (scale * (RadarZoom - 20));// (scale * 2);//  400;
            y = (y * bH) / (scale * (RadarZoom - 20));// (scale * 2);//400;

            return new Rect((int)x, (int)y, 0, 0);
        }

        public static RadarZoomSelect radarZoomSelect;

        public enum RadarZoomSelect : int { 
            mob=1,
            hum=2,
            id=3
        }
       
        public static int radarZoomMob;
        public static int radarZoomHum;
        public static int radarZoomId;

        public static int RadarZoom
        {
            set{
                switch (radarZoomSelect)
                {
                    case RadarZoomSelect.mob:
                        if (LuckUpS) { return; }
                        radarZoomMob = value;break;
                    case RadarZoomSelect.hum:
                        radarZoomHum = value; break;
                    case RadarZoomSelect.id:
                        radarZoomId = value; break;
                }
            }
            get {
                switch (radarZoomSelect)
                {
                    case RadarZoomSelect.mob:
                        if (LuckUpS) { return RadardataInstance.viewOptionData.DefaultZoomoutValue; }
                        return radarZoomMob;
                    case RadarZoomSelect.hum:
                        return radarZoomHum;
                    case RadarZoomSelect.id:
                        return radarZoomId;
                    default:
                        return 0; 
                }
            }
        }
        private static int MIN_ZOOM_VALUE = -999;
        public static bool isRadarWindowAnimation { get; set; }

        public static void ZoomIn()
        {
            RadarZoom = RadarZoom > MIN_ZOOM_VALUE ? RadarZoom - 1 : MIN_ZOOM_VALUE;
        }
        public static void ZoomOut()
        {
            RadarZoom = RadarZoom < 10 ? RadarZoom + 1 : 10;
        }

        // CP & GP //
        public static int GPSound;
        public static int CPSound;
        // CP & GP //
        public static Combatant oldMyData;
        private static Combatant newMyData;
        public static Combatant myData
        {
            get { return newMyData; }
            set
            {
                newMyData = newMyData ?? value;
                if (newMyData.PosX != value.PosX || newMyData.PosY != value.PosY)
                {
                    oldMyData = newMyData ?? value;
                    newMyData = value;
                    myRadian = getRadian(oldMyData.PosX, oldMyData.PosY, newMyData.PosX, newMyData.PosY);
                }
                else
                {
                    return;
                }
            }
        }
        private static System.Media.SoundPlayer SeA = null;
        private static System.Media.SoundPlayer SeB = null;
        private static System.Media.SoundPlayer SeE = null;
        private static System.Media.SoundPlayer SeS = null;

        private static System.Media.SoundPlayer SeGP = null;
        private static System.Media.SoundPlayer SeCP = null;

        private static bool SeEnable = true;


        private static string sePathName;
        public static string SePathName
        {
            set {
                if (Directory.Exists(value))
                {
                    sePathName = value;
                    SeEnable = true; ;
                }
                else
                {
                    SeEnable = false;
                }
            }
        }

        public static bool LuckUpS { get; set; }
  

        public static void PlaySeS()
        {
            if (SeEnable != true) { return; }
            SeS = SeS ?? new System.Media.SoundPlayer(sePathName + "/s.wav");
            //SeS.PlaySync();
            SeS.Play();
        }
        public static void PlaySeA()
        {
            if (SeEnable != true) { return; }
            SeA = SeA ?? new System.Media.SoundPlayer(sePathName + "/a.wav");
            SeA.Play();
        }
        public static void PlaySeB()
        {
            if (SeEnable != true) { return; }
            SeB = SeB ?? new System.Media.SoundPlayer(sePathName + "/b.wav");
            SeB.Play();
        }
        public static void PlaySeE()
        {
            if (SeEnable != true) { return; }
            SeE = SeE ?? new System.Media.SoundPlayer(sePathName + "/e.wav");
            SeE.Play();
        }
        public static void PlaySeCP()
        {
            if (SeEnable != true) { return; }
            SeCP = SeCP ?? new System.Media.SoundPlayer(sePathName + "/cp.wav");
            SeCP.Play();
        }
        public static void PlaySeGP()
        {
            if (SeEnable != true) { return; }
            SeGP = SeGP ?? new System.Media.SoundPlayer(sePathName + "/gp.wav");
            SeGP.Play();
        }




        public static bool windowsStatus;
        public static List<HitMobdata> hitMobdatasFromLog;
        public static void AddHitMobfromLog(HitMobdata mobdata)
        {
            hitMobdatasFromLog = hitMobdatasFromLog ?? new List<HitMobdata>();
            hitMobdatasFromLog.Add(mobdata);
        }

        public class HitMobdata
        {
            public string mobName;
            public string rank;
            public HitMobdata(string _mobName, string _rank)
            {
                mobName = _mobName;
                rank = _rank;              
            }
            public void RemoveAt(int Index)
            {
                if (hitMobdatasFromLog.Count > Index && Index >-1)
                {
                    hitMobdatasFromLog.RemoveAt(Index);
                }
            }
        }

        public static int fontTop
        {
            //Base -14
            //fontSize Base 7
            get { return ( fontSize + 7) * -1; }
        }


    }
}
