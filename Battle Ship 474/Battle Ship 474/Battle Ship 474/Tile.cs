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
    class Tile
    {
        static int MISS = 0;
        static int HIT = 1;
        static int ALREADY_HIT = 2;

        char letter;
        int number;
        int X;
        int Y;
        Ship ship;

        public Tile()
        {
            letter = 'Z';
            number = 0;
            ship = new Ship();
            this.X = -1;
            this.Y = -1;
        }

        public Tile(char letter, int num)
        {
            this.letter = letter;
            this.number = num;
            ship = new Ship();
            this.X = num - 1;
            this.Y = (int)(letter - 'A');
        }

        public char getLetter()
        {
            return this.letter;
        }

        public int getNumber()
        {
            return this.number;
        }

        public Ship getShip()
        {
            return this.ship;
        }

        public char setLetter(char c)
        {
            this.letter = c;
            return this.letter;
        }

        public int setNumber(int i)
        {
            this.number = i;
            return this.number;
        }

        public bool placeShip(Ship ship)
        {
            //Check if there is already a ship there
            if (this.ship != null && this.ship.getSize() == 0)
            {
                this.ship = ship;
                this.ship.addHealth(X, Y);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int hitShip()
        {
            if (this.ship != null && this.ship.getSize() == 0)
            {
                return MISS;
            }

            if (this.ship.isSunk())
            {
                return ALREADY_HIT;
            }

            return this.ship.hit(this.X, this.Y);
        }
    }
}
