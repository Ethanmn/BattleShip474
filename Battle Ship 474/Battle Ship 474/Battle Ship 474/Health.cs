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
        int X;
        int Y;
        bool hit;

        public Health(int x, int y)
        {
            this.X = x;
            this.Y = y;
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

        public bool itsAHit(int x, int y)
        {
            if (this.X == x && this.Y == y && !this.hit)
            {
                this.hit = true;
            }

            return this.hit;
        }
    }
}
