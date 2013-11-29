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
    class TrackTile
    {
        public static int EMPTY = -1;
        public static int MISS = 0;
        public static int HIT = 1;
        int status;

        public TrackTile()
        {
            this.status = EMPTY;
        }

        public int getStatus()
        {
            return this.status;
        }

        public void setStatus(int status)
        {
            this.status = status;
        }

        public void hitEm()
        {
            this.status = HIT;
        }

        public void missEm()
        {
            this.status = MISS;
        }
    }
}
