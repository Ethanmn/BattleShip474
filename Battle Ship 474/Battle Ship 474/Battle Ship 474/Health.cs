using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Battle_Ship_474
{
    class Health
    {
        static int HIT = 1;
        static int ALREADY_HIT = 2;

        int X;
        int Y;
        bool hit;

        public Health(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.hit = false;
        }

        public int getX()
        {
            return this.X;
        }
       
        public int getY()
        {
            return this.Y;
        }

        public bool isHit()
        {
            return hit;
        }

        public int itsAHit()
        {
            if (this.hit)
            {
                return ALREADY_HIT;
            }
            else
            {
                this.hit = true;
                return HIT;
            }
        }
    }
}
