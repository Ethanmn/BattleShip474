using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
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
        public static int HOR = 1, VER = 2;

        int size;
        int orientation = 0;
        int hits;
        List<Health> health;
        String name;
        int x = -1, y = -1;

        public Ship()
        {
            this.size = 0;
            this.hits = 0;
            health = new List<Health>();
            this.name = "Water";
        }

        public Ship(String name)
        {
            this.size = 0;
            this.hits = 0;
            health = new List<Health>();
            this.name = name;
        }

        public void setStart(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int getStartX() { return x; }
        public int getStartY() { return y; }
        public void setOrientation(int o)
        {
            this.orientation = o;
        }
        public int getOrientation() { return orientation; }

        public int getSize()
        {
            return this.size;
        }

        public int getHits()
        {
            return this.hits;
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

        public bool isSunk()
        {
            return (this.hits == this.size) ? true : false;
        }

        public Health addHealth(int x, int y)
        {
            Health newHealth = new Health(x, y);
            health.Add(newHealth);
            this.size += 1;

            return newHealth;
        }

        public int hit(int x, int y)
        {
            int result = -1;

            for (int i = 0; i < this.size; i++)
            {
                if (this.health[i].getX() == x && this.health[i].getY() == y)
                {
                    result = this.health[i].itsAHit();
                }
            }

            if (result == ALREADY_HIT)
            {
                return ALREADY_HIT;
            }

            if (result == HIT)
            {
                hits += 1;
                return HIT;
            }

            if (result == MISS)
                return MISS;

            return -1;
        }
    }
}
