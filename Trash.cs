using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace Take_The_Bait
{

    /*
     * Creator: Gavin DeWitt
     * Name: Trash
     * Summary: Creates the trash object and sets trash texture. Also checks for collisions.
     */

    class Trash
    {

        int speed;
        Texture2D trashTexture;
        Rectangle trashRectangle;
        int trashX;
        int trashY;
        Random rng;
        private bool isVisible; //checks to see if the trash is visible (used for Collision Method)

        public Texture2D TrashTexture
        {
            get { return trashTexture; }
            set { trashTexture = value; }

            //the value is passed in to equal the trashTexture, not other way around
            //set { value = trashTexture; } //flipped this in Set statement above
        }

        public Rectangle TrashRectangle
        {
            get { return trashRectangle; }
            set { trashRectangle = value; }
            //the value is passed in to equal the trashTexture, not other way around
            //set { value = trashRectangle; } //flipped this in Set statement above
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        //Parameterized constructor takes in a texture and the window dimensions
        public Trash(Texture2D p_trashTexture, int pspeed, int windowWidth, int windowHeight)
        {
            rng = new Random();

            //Set random X that falls into the game window
            trashX = rng.Next(170, (rng.Next(1, 10000) % windowWidth) + 170 );

            //Set Y to put trash right below screen
            trashY = windowHeight + 50;


            //Create trash rectangle and set texture
            trashRectangle = new Rectangle(trashX, trashY, 50, 70);
            trashTexture = p_trashTexture;
            isVisible = true;

            speed = pspeed;
        }

        //rewrites draw method 
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(trashTexture, trashRectangle, Color.White);  // Calls Draw() from Spritebatch 
        }

        //Check for collision with hook
        public bool TrashCollision(Player playerObject)  //takes in actual player object instead of Rectangle of player, easier to call
        {
            if (trashRectangle.Intersects(playerObject.PlayerRectangle)) //checks if intersecting player object's rectangle
            {
                Console.WriteLine("colliding");
                isVisible = false;  //trash not visible anymore because it collided with player object
                return true;
            }
            isVisible = true;   //trash is still visible
            return false;
        }

        public void TrashMove(Player playerObject)
        {
            trashRectangle.Y -= speed;
            TrashCollision(playerObject);    //calls collision itself 
        }
    }
}
