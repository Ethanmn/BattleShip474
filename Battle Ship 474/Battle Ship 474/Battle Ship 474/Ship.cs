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
    class Ship
    {
        int size;
        int hits;
        Health[] health;

        public Ship(int x0, int y0)
        {
            this.size = 1;
            this.hits = 1;
            health = new Health[this.size];
            this.health[0] = new Health(x0, y0);
        }

        public Ship(int x0, int y0, int x1, int y1)
        {
            this.size = 2;
            this.hits = 2;
            health = new Health[this.size];
            this.health[0] = new Health(x0, y0);
            this.health[1] = new Health(x1, y1);
        }

        public Ship(int x0, int y0, int x1, int y1, int x2, int y2)
        {
            this.size = 3;
            this.hits = 3;
            health = new Health[this.size];
            this.health[0] = new Health(x0, y0);
            this.health[1] = new Health(x1, y1);
            this.health[2] = new Health(x2, y2);
        }

        public Ship(int x0, int y0, int x1, int y1, int x2, int y2, int x3, int y3)
        {
            this.size = 4;
            this.hits = 4;
            health = new Health[this.size];
            this.health[0] = new Health(x0, y0);
            this.health[1] = new Health(x1, y1);
            this.health[2] = new Health(x2, y2);
            this.health[3] = new Health(x3, y3);
        }

        public Ship(int x0, int y0, int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            this.size = 5;
            this.hits = 5;
            health = new Health[this.size];
            this.health[0] = new Health(x0, y0);
            this.health[1] = new Health(x1, y1);
            this.health[2] = new Health(x2, y2);
            this.health[3] = new Health(x3, y3);
            this.health[4] = new Health(x4, y4);
        }

        private void hit(int x, int y)
        {
            for (int i = 0; i < size; i++)
            {
                this.health[i].itsAHit(x, y);
            }
        }
    }
}
