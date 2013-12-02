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
    class Particle
    {
        static float GRAVITY = 9.8f;

        Vector3 position;
        Vector3 velocity;
        Vector3 accel;

        public Particle(Vector3 position, Vector3 velocity, Vector3 accel)
        {
            this.position = position;
            this.velocity = velocity;
            this.accel = accel;
        }

        public Vector3 getPosition()
        {
            return this.position;
        }

        public Vector3 getVelocity()
        {
            return this.velocity;
        }

        public Vector3 getAccel()
        {
            return this.accel;
        }

        public Vector3 gravity()
        {
            this.velocity.Y -= GRAVITY;
            return this.velocity;
        }

        public Vector3 update()
        {
            this.velocity += this.accel;

            this.position += this.velocity;

            return this.position;
        }

        public Vector3 setPosition(Vector3 position)
        {
            this.position = position;

            return this.position;
        }

        public Vector3 setVelocity(Vector3 velocity)
        {
            this.velocity = velocity;

            return this.velocity;
        }

        public Vector3 setAccel(Vector3 accel)
        {
            this.accel = accel;

            return this.accel;
        }
    }
}
