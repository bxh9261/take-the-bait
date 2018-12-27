using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

    /*
     * Creator: Harry McCardell
     * Name: Player
     * Summary: Gives the player and player texture their own class
    */
namespace Take_The_Bait
{
    

    class Player
    {

        int HEIGHT = Take_The_Bait.Height;
        int WIDTH = Take_The_Bait.Width;

        private Texture2D playerTexture;
        private Rectangle playerRectangle;
        private Rectangle lineRectangle;
        private Color playerColor;
        private double score;     //keeps track of player's score 
        private string name;            //player's name


        // Get and Set properties
        public Texture2D PlayerTexture
        {
            get { return playerTexture; }
            set { value = playerTexture; }
        }

        public Rectangle PlayerRectangle
        {
            get { return playerRectangle; }
            set { value = playerRectangle; }
        }

        public Rectangle LineRectangle
        {
            get { return lineRectangle; }
            set { value = lineRectangle; }
        }

        public Double Score
        {
            get { return score; }
            set { score = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public Color PlayerColor
        {
            get { return playerColor; }
            set { playerColor = value; }
        }


        //Parameterized constructor takes in a texture and rectangle
        public Player(Texture2D p_playerTexture, Rectangle p_playerRectangle, Rectangle p_lineRectangle, Color p_playerColor)
        {
            playerTexture = p_playerTexture;
            playerRectangle = p_playerRectangle;
            lineRectangle = p_lineRectangle;
            playerColor = p_playerColor;
            score = 0;    //player score is 0 at start
        }

        //rewrites draw method 
        protected virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(playerTexture, playerRectangle, playerColor);  // Calls Draw() from Spritebatch class
        }


        public void PlayerMove()
        {
            //creating keyboard state for user input 
            KeyboardState kb = Keyboard.GetState();

            //will move character when certain keys are pressed
            if (kb.IsKeyDown(Keys.D) || kb.IsKeyDown(Keys.Right))   //Right
            {
                //move right
                if (playerRectangle.X < WIDTH - 40)
                {
                    playerRectangle.X += 5;
                    lineRectangle.X += 5;
                    playerRectangle.X = Convert.ToInt32(playerRectangle.X);
                    lineRectangle.X = Convert.ToInt32(lineRectangle.X);
                }
                
            }
            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up))   // Up
            {
                //move up
                if (playerRectangle.Y > 0)
                {
                    playerRectangle.Y -= 5;
                    lineRectangle.Y -= 5;
                    playerRectangle.Y = Convert.ToInt32(playerRectangle.Y);
                    lineRectangle.Y = Convert.ToInt32(lineRectangle.Y);
                }
                
            }
            if (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down))   // Down
            {
                //move down
                if (playerRectangle.Y < HEIGHT - 60)
                {
                    playerRectangle.Y += 5;
                    lineRectangle.Y += 5;
                    playerRectangle.Y = Convert.ToInt32(playerRectangle.Y);
                    lineRectangle.Y = Convert.ToInt32(lineRectangle.Y);
                }
                
            }
            if (kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.Left))   // Left
            {
                //move left
                if (playerRectangle.X > 171)
                {
                    playerRectangle.X -= 5;
                    lineRectangle.X -= 5;
                    playerRectangle.X = Convert.ToInt32(playerRectangle.X);
                    lineRectangle.X = Convert.ToInt32(lineRectangle.X);
                }
                
            }
        }
    }
}
