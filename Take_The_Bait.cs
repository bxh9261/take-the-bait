using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Take_The_Bait
{
    /*
     * Creator: Brad Hanel, Ricky Norris, Gavin DeWitt, Harry McCarDELl
     * Name: Take_The_Bait
     * Summary: Sets up the game view for the game, and basically does anything front-end. there are almost 400 lines of code in here right now and it's hard to confine it all into one sentence
    */

    enum GameState
    {
        MainMenu,
        Settings,
        Game,
        RulesMenu,
        GameOver,
        Paused
    }

    public class Take_The_Bait : Game
    {
        //bool for showing external tool once
        bool shown = false;

        //this is important later
        bool hell = false;

        //int for whose turn
        int whoseTurn = 1;

        const char DEL = '\xE001';

        //for loading in player names and color
        string[] loaded = new string[3];

        List<String> hs = new List<string>();

        //monogame default code
        GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;

        //randoms are always useful
        Random rgen;

        //constant width and height
        const int WIDTH = 700;
        const int HEIGHT = 700;

        //images
        Texture2D quit;
        Texture2D pause;
        Texture2D settings;
        Texture2D rules;
        Texture2D b;
        Texture2D trash;
        private Texture2D hook;
        Texture2D fishSheet;
        Texture2D fishSheet2;
        Texture2D fishSheet3;
        Texture2D fishSheet4;
        Texture2D fishSheet5;
        Texture2D fishSheet6;
        Texture2D background;

        //text box locations
        Vector2 name1;
        Vector2 name2;
        Vector2 score1;
        Vector2 score2;
        Vector2 set1;
        Vector2 set2;
        Vector2 set3;
        Vector2 set4;
        Vector2 depthCount;
        Vector2 title;
        Vector2 pressenter;

        //object locations
        private Rectangle player1rectangle;
        private Rectangle hook1;
        Rectangle hook2;
        Rectangle hook3;
        Rectangle hook4;
        Rectangle hook5;
        Rectangle hook6;
        Rectangle playrec;
        Rectangle quitrec;
        Rectangle setrec;
        Rectangle rulesrec;
        Rectangle verline;
        Rectangle hookline;
        Rectangle quitrec2;
        Rectangle pauserec;

        //color, probably always white
        Color player1color;

        //spritefont
        SpriteFont sf;

        //for animating fish
        int frame;
        double timePerFrame = 100;
        int numFrames = 2;
        int framesElapsed;

        //for depth counter
        int depth;

        //input string
        private string input;

        //Game State Enum field
        private GameState gameCurrentState;

        //Creating Player object
        Player player1;
        Player player2;

        //Creating fish list collection
        List<Fish> fishList;

        //Create trash list
        List<Trash> trashList;

        //Creating External Tool
        ExternalToolLoader saveExternalToolLoader = new ExternalToolLoader(); //changed this name from save to saveExternalToolLoader

        //Creating external tool -- don't instantiate yet
        ExternalTool2 externalTool2;


        //Creating Mouse
        private MouseState mouse;

        //Lives
        int lives1;
        int lives2;

        public Take_The_Bait()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = WIDTH;
            graphics.PreferredBackBufferWidth = HEIGHT;
            Content.RootDirectory = "Content";

        }

        public static int Height
        {
            get
            {
                return HEIGHT;
            }
        }

        public static int Width
        {
            get
            {
                return WIDTH;
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            //random object
            rgen = new Random();

            //text positions
            name1 = new Vector2(10, 10);
            name2 = new Vector2(10, 210);
            score1 = new Vector2(10, 75);
            score2 = new Vector2(10, 275);
            title = new Vector2(180, 10);
            pressenter = new Vector2(210, 410);
            set1 = new Vector2(10, 10);
            set2 = new Vector2(10, 75);
            set3 = new Vector2(10, 140);
            set4 = new Vector2(10, 205);
            depthCount = new Vector2(10, HEIGHT - 160);

            //image sizes and default positions
            hook1 = new Rectangle(10, 140, 40, 60);
            hook2 = new Rectangle(65, 140, 40, 60);
            hook3 = new Rectangle(120, 140, 40, 60);
            hook4 = new Rectangle(10, 340, 40, 60);
            hook5 = new Rectangle(65, 340, 40, 60);
            hook6 = new Rectangle(120, 340, 40, 60);
            playrec = new Rectangle(Proportions(208), Proportions(300), Proportions(200), Proportions(104));
            quitrec = new Rectangle(Proportions(660), Proportions(866), Proportions(270), Proportions(104));
            setrec = new Rectangle(Proportions(146), Proportions(698), Proportions(320), Proportions(90));
            rulesrec = new Rectangle(Proportions(660), Proportions(496), Proportions(270), Proportions(104));
            quitrec2 = new Rectangle(10, HEIGHT-50, 150, 40);
            verline = new Rectangle(170, 0, 1, 2000);
            pauserec = new Rectangle(10, HEIGHT - 100, 150, 40);

            //input string
            input = "";

            //lists of fish and trash
            fishList = new List<Fish>();
            trashList = new List<Trash>();

            //default depth
            depth = 0;

            //default lives
            lives1 = 3;
            lives2 = 3;

            //sets current game state to the main menu
            gameCurrentState = GameState.MainMenu;

            //default values for loaded
            loaded[0] = "Black";
            loaded[1] = "Player1";
            loaded[2] = "Player2";

            //make a default save file to avoid exceptions
            StreamWriter saveState = new StreamWriter("save.txt");
            saveState.WriteLine(loaded[0] + DEL + loaded[1] + DEL + loaded[2]);
            saveState.WriteLine();
            saveState.Close();

            if (!File.Exists("highscore.txt"))
            {
                StreamWriter hsw = new StreamWriter("highscore.txt");
                for (int i = 0; i < 5; i++)
                {
                    hsw.WriteLine("Player1" + DEL + "0");
                    hs.Add("Player1" + DEL + "0");
                }
                hsw.Close();
            }

            externalTool2 = new ExternalTool2();

            //load a save file if one exists
            SetExternalTool();

            //set image color
            player1color = Color.White;

            //allows mouse to be visible on the screen during any game state
            this.IsMouseVisible = true;

            //Instantiating player1 PLAYER object
            player1rectangle = new Rectangle(171, 30, 40, 60);
            hookline = new Rectangle(177, -HEIGHT + 30, 1, HEIGHT);
            player1 = new Player(hook, player1rectangle, hookline, player1color);
            player1.Name = loaded[1];
            player2 = new Player(hook, player1rectangle, hookline, player1color);
            player2.Name = loaded[2];

        }

        //if no save file is found, throw an exception and player names and colors will be their default values
        public void SetExternalTool()
        {
            try
            {
                loaded = saveExternalToolLoader.Load();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }

        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            quit = Content.Load<Texture2D>("Quit");
            pause = Content.Load<Texture2D>("Pause");
            settings = Content.Load<Texture2D>("Settings");
            rules = Content.Load<Texture2D>("Rules");
            b = Content.Load<Texture2D>("Black");
            //gets hook color from external tool
            HookPicker();
            sf = Content.Load<SpriteFont>("SpriteFont1");
            fishSheet = Content.Load<Texture2D>("fish fish fish");
            fishSheet2 = Content.Load<Texture2D>("fish fish fish green");
            fishSheet3 = Content.Load<Texture2D>("fish fish fish red");
            fishSheet4 = Content.Load<Texture2D>("fish fish fish gold");
            fishSheet5 = Content.Load<Texture2D>("fish fish fish white");
            fishSheet6 = Content.Load<Texture2D>("fish fish fish black");
            trash = Content.Load<Texture2D>("canunot");
            background = Content.Load<Texture2D>("background");

            // TODO: use this.Content to load your game content here
        }

        //takes the hook color the user chooses and assigns it to the appropriate file
        //TODO: put in some actual-non placeholder art
        public void HookPicker()
        {
            if(loaded[0] == "Black")
            {
                //default black hook
                hook = Content.Load<Texture2D>("hook");
            }
            else if(loaded[0] == "Blue")
            {
                //loads blue hook
                hook = Content.Load<Texture2D>("bluehook");
            }
            else if(loaded[0] == "Red")
            {
                //loads red hook
                hook = Content.Load<Texture2D>("redhook");
            }
            else
            {
                //shouldn't reach this code unless the text file is manually edited
                hook = Content.Load<Texture2D>("hook");
            }
            //player1.PlayerTexture = hook;

        }

        public void NamePicker()
        {
            player1.Name = loaded[1];
            player2.Name = loaded[2];
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            //code for making the fish animate frame by frame (only 3 frames)
            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            frame = framesElapsed % numFrames + 1;

            /// <summary>
            /// This switch statement will hold all the logic of the game
            /// Each case has a name so make sure to put the correct stuff in the correct case
            /// All game stuff will go in "case GameState.Game:
            ///     this includes collisions and 
            /// </summary>
            /// 
            KeyboardState present = new KeyboardState();        //creates keyboard state to check when user presses a key
            mouse = Mouse.GetState();                //gets mouse state to check when user clicks a mouse button
            
            present = Keyboard.GetState();


            switch (gameCurrentState)
            {
                case GameState.MainMenu:
                    //Console.WriteLine("in MENU");
                    ScoreReset();
                    /*
                    if (SingleKeyPress(Keys.Enter))
                    {
                        gameCurrentState = GameState.Game;
                    }
                    */
                    if (present.IsKeyDown(Keys.R))
                    {
                        gameCurrentState = GameState.RulesMenu;
                    }
                    else if (present.IsKeyDown(Keys.E))
                    {
                        gameCurrentState = GameState.Settings;
                    }
                    else if (present.IsKeyDown(Keys.P))
                    {
                        gameCurrentState = GameState.Game;
                    }
                    else if (present.IsKeyDown(Keys.Q))
                    {
                        Exit(); //exit game
                    }

                    //if quit, rules, or settings button is clicked, do either of these
                    if (playrec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                    {
                        gameCurrentState = GameState.Game; //changes current game state to game
                    }
                    if (quitrec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                    {
                        Exit();  //Exits
                    }
                    if (rulesrec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                    {
                        gameCurrentState = GameState.RulesMenu; //changes current game state to rules menu
                    }
                    if (setrec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                    {
                        gameCurrentState = GameState.Settings;  //changes current game state to settings menu
                    }
                    break;

                case GameState.Game:
                    //Console.WriteLine("in GAME");

                    //increase depth
                    depth++;

                    SetExternalTool();
                    HookPicker();
                    NamePicker();
                    player1.PlayerTexture = hook;
                    player2.PlayerTexture = hook;
                    if(whoseTurn == 1)
                    {
                        player1.PlayerMove();
                    }
                    else
                    {
                        player2.PlayerMove();
                    }
                    
                    

                    //if quit, rules, or settings button is clicked, do either of these
                    if (quitrec2.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                    {
                        gameCurrentState = GameState.GameOver; //exit game
                    }
                    /*
                    if (pauserec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                    {
                        //pause code
                    }
                    */


                    //Spawn a fish every second
                    FishTrashSpawner();

                    if(fishList.Count != 0 && fishList != null)
                    {
                        try
                        {
                            for (int i = 0; i < fishList.Count; i++)
                            {
                                if (whoseTurn == 1)
                                {
                                    fishList[i].FishMove(player1);
                                }
                                else
                                {
                                    fishList[i].FishMove(player2);
                                }

                                //Check if its offscreen

                                if (fishList[i].FishRectangle.Y < -50)
                                {
                                    fishList.Remove(fishList[i]);
                                }

                                if (fishList[i].IsVisible == false)
                                {
                                    fishList.Remove(fishList[i]);
                                    //player1.Score++;  //increment playerScore
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    

                    if(trashList.Count != 0 && trashList != null)
                    {
                        try
                        {
                            for (int i = 0; i < trashList.Count; i++)
                            {
                                if (whoseTurn == 1)
                                {
                                    trashList[i].TrashMove(player1);
                                }
                                else
                                {
                                    trashList[i].TrashMove(player2);
                                }


                                if (trashList[i].TrashRectangle.Y < -50)
                                {
                                    trashList.Remove(trashList[i]);
                                }

                                if (trashList[i].IsVisible == false)
                                {
                                    trashList.Remove(trashList[i]);
                                    Console.WriteLine("Hitting trash");
                                    //Take away hook
                                    if (whoseTurn == 1)
                                    {
                                        lives1--;
                                        whoseTurn = 2;
                                        trashList.Clear();
                                        fishList.Clear();

                                        gameCurrentState = GameState.Paused;
                                    }
                                    else
                                    {
                                        lives2--;
                                        whoseTurn = 1;
                                        trashList.Clear();
                                        fishList.Clear();

                                        gameCurrentState = GameState.Paused;
                                    }
                                    if (lives2 == 1)
                                    {
                                        depth = 1500;
                                    }
                                    else
                                    {
                                        depth = 0;
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    


                    //ProcessInput();
                    //will need to check collisions here

                    if (present.IsKeyDown(Keys.Q))
                    {
                        gameCurrentState = GameState.GameOver;
                    }

                    if(lives1 == 0 && lives2 == 0)
                    {
                        gameCurrentState = GameState.GameOver;

                        HighScores(player1);
                        HighScores(player2);

                    }
                    break;
                    
                case GameState.Paused:
                    if (present.IsKeyDown(Keys.Enter))
                    {
                        gameCurrentState = GameState.Game;
                    }
                    break;
                    

                case GameState.RulesMenu:
                    //Console.WriteLine("in RULES");
                    if (present.IsKeyDown(Keys.Enter))
                    {
                        gameCurrentState = GameState.MainMenu;
                    }
                    else if (present.IsKeyDown(Keys.E))
                    {
                        gameCurrentState = GameState.Settings;
                    }
                    break;

                case GameState.Settings:
                    //Console.WriteLine("in SETTINGS");
                    if(externalTool2.Visible == false && !shown)  //object externalTool2 was originally called "e" - terrible name, had to change it //"e" was the name bestowed upon this variable by God himself, "e" is a holy name forged within the heavens, do you dare question "e"?
                    {
                        externalTool2 = new ExternalTool2();
                        externalTool2.Show();
                        shown = true;
                    }
                    
                    if (present.IsKeyDown(Keys.Enter))
                    {
                        gameCurrentState = GameState.MainMenu;
                    }
                    else if (present.IsKeyDown(Keys.R))
                    {
                        gameCurrentState = GameState.RulesMenu;
                    }
                    HookPicker();
                    player1.Name = loaded[1];
                    player2.Name = loaded[2];
                    player1.PlayerTexture = hook;
                    player2.PlayerTexture = hook;
                    break;

                case GameState.GameOver:
                    //Console.WriteLine("in OVER");
                    //saveExternalToolLoader.Save(player1);
                    if (SingleKeyPress(Keys.Enter))
                    {
                        gameCurrentState = GameState.MainMenu;
                    }
                    //player1.Score = 0;
                    break;
            }

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            spriteBatch.Begin();

            switch (gameCurrentState)
            {
                case GameState.MainMenu:
                    //spriteBatch.DrawString(sf, ("Take The Bait"), title, Color.Black);
                    //spriteBatch.DrawString(sf, ("Press Enter To Start"), pressenter, Color.Black);

                    spriteBatch.Draw(background, new Rectangle(0, 0, WIDTH, HEIGHT), Color.White);
                    break;

                case GameState.Game:
                    //quit button
                    spriteBatch.Draw(quit, quitrec2, Color.White);

                    //settings button
                    //spriteBatch.Draw(settings, setrec, Color.White);

                    //rules button
                    //spriteBatch.Draw(rules, rulesrec, Color.White);

                    //line separating menu and game
                    spriteBatch.Draw(b, verline, Color.Black);

                    

                    //hooks which determine number of lives a player has
                    if (lives1 == 3)
                    {
                        spriteBatch.Draw(hook, hook1, Color.White);
                        spriteBatch.Draw(hook, hook2, Color.White);
                        spriteBatch.Draw(hook, hook3, Color.White);
                    }
                    else if (lives1 == 2)
                    {
                        spriteBatch.Draw(hook, hook1, Color.White);
                        spriteBatch.Draw(hook, hook2, Color.White);
                    }
                    else if (lives1 == 1)
                    {
                        spriteBatch.Draw(hook, hook1, Color.White);
                    }

                    if (lives2 == 3)
                    {
                        spriteBatch.Draw(hook, hook4, Color.White);
                        spriteBatch.Draw(hook, hook5, Color.White);
                        spriteBatch.Draw(hook, hook6, Color.White);
                    }
                    else if (lives2 == 2)
                    {
                        spriteBatch.Draw(hook, hook4, Color.White);
                        spriteBatch.Draw(hook, hook5, Color.White);
                    }
                    else if (lives2 == 1)
                    {
                        spriteBatch.Draw(hook, hook4, Color.White);
                    }
                     
                    


                    //Player names and scores
                    spriteBatch.DrawString(sf, (loaded[1]), name1, Color.Black);
                    spriteBatch.DrawString(sf, (player2.Name), name2, Color.Black);
                    spriteBatch.DrawString(sf, Convert.ToString(player1.Score), score1, Color.Black);
                    spriteBatch.DrawString(sf, (Convert.ToString(player2.Score)), score2, Color.Black);

                    //depth
                    spriteBatch.DrawString(sf, "Depth: " + depth.ToString(), depthCount, Color.Black);

                    //Title
                    spriteBatch.DrawString(sf, ("Take The Bait"), title, Color.Black);


                    //Background
                    //spriteBatch.Draw(background, new Rectangle(171, 0, WIDTH-171, Height), Color.White);

                    if(whoseTurn == 1)
                    {
                        spriteBatch.Draw(hook, player1.PlayerRectangle, Color.White);
                        spriteBatch.DrawString(sf, (player1.Name + " - Fish on!"), new Vector2(300, 10), Color.Black);

                        //hook line
                        spriteBatch.Draw(b, player1.LineRectangle, Color.Black);
                    }
                    else
                    {
                        spriteBatch.Draw(hook, player2.PlayerRectangle, Color.White);
                        spriteBatch.DrawString(sf, (player2.Name + " - Reel 'em in!"), new Vector2(300, 10), Color.Black);

                        //hook line
                        spriteBatch.Draw(b, player2.LineRectangle, Color.Black);
                    }
                    
                    for(int i = 0; i < fishList.Count; i++)  //loops through fish list
                    {

                        //if fish is visible draw it, else do not draw
                        if (fishList[i].IsVisible == true)
                        {
                            //spriteBatch.Draw(fishList[i].FishTexture, fishList[i].FishRectangle, Color.White);
                            spriteBatch.Draw(fishList[i].FishTexture, fishList[i].FishRectangle, new Rectangle(0 + frame * 200, 0, 200, 200), Color.White);
                        }
                    }

                    for (int i = 0; i < trashList.Count; i++)  //loops through fish list
                    {
                        //if fish is visible draw it, else do not draw
                        if (trashList[i].IsVisible == true)
                        {
                            //spriteBatch.Draw(fishList[i].FishTexture, fishList[i].FishRectangle, Color.White);
                            spriteBatch.Draw(trashList[i].TrashTexture, trashList[i].TrashRectangle, Color.White);
                        }
                    }

                    break;

                    //Displays rules menu
                case GameState.RulesMenu:
                    spriteBatch.DrawString(sf, ("Rules: Use your hook to catch fish, but avoid any obstacles in your path! \n" +
                    "Get hit three times and you lose a life! \n" +
                    "3 lives until you're done. \n" +
                    "You get more points per fish the deeper you go! \n" +
                    "At the end of the game, the player with the most points wins \n\n\n\n" +
                    "Press ENTER to go into the game!"), name1, Color.Black);
                    break;

                    //Displays settings menu (for picking hook color)
                    //DISPLAYS EXTERNAL TOOL
                case GameState.Settings:

                    spriteBatch.DrawString(sf, "Press \"ENTER\" to return to Main Menu", name1, Color.Black);
                    break;

                //Game over menu
                case GameState.GameOver:
                    spriteBatch.DrawString(sf, "Press \"ENTER\" to return to Main Menu", name1, Color.Black);

                    //will display winner of the game. does not read player scores for somereason
                    if (player1.Score > player2.Score)
                    {
                        spriteBatch.DrawString(sf, player1.Name + " is the winner! \nWith a total of " + player1.Score + "lbs. of fish!", new Vector2(100, 100), Color.Black);
                    }
                    else if (player1.Score < player2.Score)
                    {
                        spriteBatch.DrawString(sf, player2.Name + " is the winner! \nWith a total of " + player2.Score + "lbs. of fish!", new Vector2(100, 100), Color.Black);
                    }
                    else if (player1.Score == player2.Score)
                    {
                        spriteBatch.DrawString(sf, player1.Name + " and " + player2.Name + " finish in a tie! \nWith a total of " + (player1.Score) + " lbs. of fish!", new Vector2(100, 100), Color.Black);
                    }
                    


                    break;

                //Pause in between lives
                case GameState.Paused:
                    if (whoseTurn == 1)
                    {
                        spriteBatch.DrawString(sf, player1.Name + " is up next...", new Vector2(100,100), Color.Black);
                        spriteBatch.DrawString(sf, "Press Enter to cast your line!", new Vector2(100, 150), Color.Black);
                    }
                    else
                    {
                        spriteBatch.DrawString(sf, player2.Name + " is up next...", new Vector2(100, 100), Color.Black);
                        spriteBatch.DrawString(sf, "Press Enter to cast your line!", new Vector2(100, 150), Color.Black);
                    }
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        // Single Key Press Method
        bool SingleKeyPress(Keys key)
        {
            KeyboardState kb = Keyboard.GetState();
            KeyboardState preKb; 

            if (kb.IsKeyDown(Keys.Enter))
            {
                return true;
            }
            preKb = kb;
            return false;
        }

        public void ScoreReset()
        {
            lives1 = 3;
            lives2 = 3;
            player1.Score = 0;
            player2.Score = 0;
            depth = 0;
            shown = false;
            whoseTurn = 1;
        }

        public void FishTrashSpawner()
        {

            if(depth < 1000)
            {
                if (depth % 50 == 0)
                {
                    fishList.Add(new Fish(fishSheet, 1, WIDTH, HEIGHT));
                }
                if (depth % 75 == 0)
                {
                    trashList.Add(new Trash(trash, 1, WIDTH, HEIGHT));
                }
            }
            else if (depth > 1500 && depth < 2850)
            {
                if (depth % 50 == 0)
                {
                    fishList.Add(new Fish(fishSheet2, 3, WIDTH, HEIGHT));
                }
                if (depth % 50 == 0)
                {
                    trashList.Add(new Trash(trash, 3, WIDTH, HEIGHT));
                }
            }
            else if (depth > 3000 && depth < 4400)
            {
                if (depth % 50 == 0)
                {
                    fishList.Add(new Fish(fishSheet3, 5, WIDTH, HEIGHT));
                }
                if (depth % 25 == 0)
                {
                    trashList.Add(new Trash(trash, 5, WIDTH, HEIGHT));
                }
            }
            else if (depth > 4500 && depth < 5950)
            {
                if (depth % 25 == 0)
                {
                    fishList.Add(new Fish(fishSheet4, 8, WIDTH, HEIGHT));
                }
                if (depth % 12 == 0)
                {
                    trashList.Add(new Trash(trash, 8, WIDTH, HEIGHT));
                }
            }
            
            else if (depth > 6000 && depth < 9990)
            {
                if (depth % 25 == 0)
                {
                    fishList.Add(new Fish(fishSheet5, 15, WIDTH, HEIGHT));
                }
                if (depth % 12 == 0)
                {
                    trashList.Add(new Trash(trash, 15, WIDTH, HEIGHT));
                }
            }
            else if (depth > 10000)
            {
                if (!hell)
                {
                    Console.WriteLine("Welcome to hell");
                    hell = true;
                }
                if (depth % 25 == 0)
                {
                    fishList.Add(new Fish(fishSheet6, 25, WIDTH, HEIGHT));
                }
                if (depth % 10 == 0)
                {
                    trashList.Add(new Trash(trash, 25, WIDTH, HEIGHT));
                }
            }


        }

        private void HighScores(Player player)
        {
            hs.Clear();
            StreamReader sr = new StreamReader("highscore.txt");
            for (int j = 0; j < 5; j++)
            {
                hs.Add(sr.ReadLine());
            }
            sr.Close();
            for (int i = 0; i < 5; i++)
            {
                

                string[] scorelist = new string[2];
                Console.WriteLine(hs[0]);
                Console.WriteLine(hs[1]);
                Console.WriteLine(hs[2]);
                Console.WriteLine(hs[3]);
                Console.WriteLine(hs[4]);

                scorelist = hs[i].Split(DEL);
                int.TryParse(scorelist[1], out int s);
                if (player.Score > s)
                {
                    List<string> temp = new List<string>();
                    if (i == 0)
                    {
                        temp.Add(player.Name + DEL + player.Score);
                        temp.Add(hs[0]);
                        temp.Add(hs[1]);
                        temp.Add(hs[2]);
                        temp.Add(hs[3]);
                    }
                    if (i == 1)
                    {
                        temp.Add(hs[0]);
                        temp.Add(player.Name + DEL + player.Score);
                        temp.Add(hs[1]);
                        temp.Add(hs[2]);
                        temp.Add(hs[3]);
                    }
                    if (i == 2)
                    {
                        temp.Add(hs[0]);
                        temp.Add(hs[1]);
                        temp.Add(player.Name + DEL + player.Score);
                        temp.Add(hs[2]);
                        temp.Add(hs[3]);
                    }
                    if (i == 3)
                    {
                        temp.Add(hs[0]);
                        temp.Add(hs[1]);
                        temp.Add(hs[2]);
                        temp.Add(player.Name + DEL + player.Score);
                        temp.Add(hs[3]);
                    }
                    if (i == 4)
                    {
                        temp.Add(hs[0]);
                        temp.Add(hs[1]);
                        temp.Add(hs[2]);
                        temp.Add(hs[3]);
                        temp.Add(player.Name + DEL + player.Score);
                    }
                    hs = temp;
                    break;
                }
            }

            StreamWriter sw = new StreamWriter("highscore.txt");
            for (int i = 0; i < 5; i++)
            {
                sw.WriteLine(hs[i]);
            }
            sw.Close();
        }

        //since the original main menu image is 1080*1080
        private int Proportions(int pix)
        {
            return (WIDTH*pix/1080);
        }

    }

}
