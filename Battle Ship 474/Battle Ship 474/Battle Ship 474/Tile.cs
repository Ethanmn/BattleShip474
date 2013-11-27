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
        char letter;
        int number;
        Ship ship;

        public Tile()
        {
            letter = 'Z';
            number = 0;
        }

        public Tile(char letter, int num)
        {
            this.letter = letter;
            this.number = num;
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
            this.ship = ship;
            return true;
        }
    }
}
