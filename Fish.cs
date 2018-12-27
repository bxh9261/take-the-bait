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
     * Name: Fish
     * Summary: Creates the fish object and sets fish texture. Also checks for collisions.
     */


    class Fish
    {
        int speed;
        int pointvalue;
        Texture2D fishTexture;
        Rectangle fishRectangle;
        int fishX;
        int fishY;
        Random rng;
        private bool isVisible; //checks to see if the fish is visible (used for Collision Method)

        public Texture2D FishTexture
        {
            get { return fishTexture; }
            set { fishTexture = value; }

            //the value is passed in to equal the fishTexture, not other way around
            //set { value = fishTexture; } //flipped this in Set statement above
        }

        public Rectangle FishRectangle
        {
            get { return fishRectangle; }
            set { fishRectangle = value; }
                //the value is passed in to equal the fishTexture, not other way around
                //set { value = fishRectangle; } //flipped this in Set statement above
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        //Parameterized constructor takes in a texture and the window dimensions
        public Fish(Texture2D p_fishTexture, int pspeed, int windowWidth, int windowHeight)
        {
            rng = new Random();
            //Set random X that falls into the game window
            fishX = (rng.Next(rng.Next(rng.Next(170, 20000), 20000), 20000))%(windowWidth-220) + 170;

            //Set Y to put fish right below screen
            fishY = windowHeight + 50;


            //Create fish rectangle and set texture
            fishRectangle = new Rectangle(fishX, fishY, 50, 50);
            fishTexture = p_fishTexture;
            isVisible = true;

            pointvalue = pspeed;
            speed = pspeed;
        }

        //rewrites draw method 
        public virtual void Draw(SpriteBatch sb)
        {  
                sb.Draw(fishTexture, fishRectangle, Color.White);  // Calls Draw() from Spritebatch 
        }

        //Check for collision with hook
        public bool FishCollision(Player playerObject)  //takes in actual player object instead of Rectangle of player, easier to call
        {
            if (fishRectangle.Intersects(playerObject.PlayerRectangle)) //checks if intersecting player object's rectangle
            {
                Console.WriteLine("colliding");
                isVisible = false;  //fish not visible anymore because it collided with player object
                playerObject.Score += pointvalue;
                return true;
            }
            isVisible = true;   //fihs is still visible
            return false;
        }

        public virtual void FishMove(Player playerObject)
        {
            fishRectangle.Y -= speed;
            FishCollision(playerObject);    //calls collision itself 
        }

        public int PointValue
        {
            get
            {
                return pointvalue;
            }
        }
    }
}
