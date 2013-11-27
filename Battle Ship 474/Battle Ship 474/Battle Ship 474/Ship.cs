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
        static int MISS = 0;
        static int HIT = 1;
        static int ALREADY_HIT = 2;

        int size;
        int hits;
        //Health[] health;
        List<Health> health;
        String name;

        public Ship()
        {
            this.size = 0;
            this.hits = 0;
            this.name = "Water";
        }

        public Ship(String name)
        {
            this.size = 0;
            this.hits = 0;
            this.name = name;
        }

        public Ship(String name, int x0, int y0)
        {
            this.size = 1;
            this.hits = 1;
            this.health[0] = new Health(x0, y0);
            this.name = name;
        }

        public Ship(String name, int x0, int y0, int x1, int y1)
        {
            this.size = 2;
            this.hits = 2;
            this.health[0] = new Health(x0, y0);
            this.health[1] = new Health(x1, y1);
            this.name = name;
        }

        public Ship(String name, int x0, int y0, int x1, int y1, int x2, int y2)
        {
            this.size = 3;
            this.hits = 3;
            this.health[0] = new Health(x0, y0);
            this.health[1] = new Health(x1, y1);
            this.health[2] = new Health(x2, y2);
            this.name = name;
        }

        public Ship(String name, int x0, int y0, int x1, int y1, int x2, int y2, int x3, int y3)
        {
            this.size = 4;
            this.hits = 4;
            this.health[0] = new Health(x0, y0);
            this.health[1] = new Health(x1, y1);
            this.health[2] = new Health(x2, y2);
            this.health[3] = new Health(x3, y3);
            this.name = name;
        }

        public Ship(String name, int x0, int y0, int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            this.size = 5;
            this.hits = 5;
            this.health[0] = new Health(x0, y0);
            this.health[1] = new Health(x1, y1);
            this.health[2] = new Health(x2, y2);
            this.health[3] = new Health(x3, y3);
            this.health[4] = new Health(x4, y4);
            this.name = name;
        }

        public String getName()
        {
            return this.name;
        }

        public String setName(String name)
        {
            this.name = name;
            return this.name;
        }

        public Health addHealth(int x, int y)
        {
            Health ret = new Health(x, y);
            health.Add(ret);
            this.size += 1;

            return ret;
        }

        public int hit(int x, int y)
        {
            int result = 0;

            if (this.size == 0)
            {
                return MISS;
            }

            for (int i = 0; i < size; i++)
            {
                if (this.health[i].getX() == x && this.health[i].getY() == y)
                {
                    result = this.health[i].itsAHit(x, y);
                }
            }

            if (result == ALREADY_HIT)
            {
                return ALREADY_HIT;
            }
            else
            {
                return HIT;
            }
        }
    }
}
