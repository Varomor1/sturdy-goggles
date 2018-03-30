using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Tetris
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //content

        private SpriteFont defaultFont;

        private Texture2D[] gameTextures_levelTextures;
        private Texture2D gameTextures_Blue;
        private Texture2D gameTextures_Cyan;
        private Texture2D gameTextures_Green;
        private Texture2D gameTextures_Magenta;
        private Texture2D gameTextures_Orange;
        private Texture2D gameTextures_Red;
        private Texture2D gameTextures_Yellow;
        private Texture2D gameTextures_Ghost;

        //gamerelevant variables
        private Field gameField;
        private int difficultyLevel;
        private int[] difficultyTickSpeedMillis;
        private readonly int MAX_LEVEL = 13;

        private TimeSpan TimeSpan_GravityTick;

        private Keys keys_moveLeft;
        private Keys keys_moveRight;
        private Keys keys_softDrop;
        private Keys keys_hardDrop;
        private Keys keys_rotateLeft;
        private Keys keys_rotateRight;
        private Keys keys_switchHold;



        private Keys keys_menuBack;


        //framework relevant variables

        private MenuManager menuManager;

        private readonly int DAS_Delay = 200;
        private readonly int DAS_Speed = 33;
        private TimeSpan TimeSpan_DAS_Delay;
        private TimeSpan TimeSpan_DAS_Repeat;


        private KeyboardState oldState;
        private GAMESTATE gameState;
        private enum GAMESTATE
        {
            GAMESTATE_MAIN_MENU,
            GAMESTATE_PAUSE_MENU,
            GAMESTATE_SCORE_BOARD,
            GAMESTATE_GAME_RUNNING,
            GAMESTATE_OPTIONS_MENU,
            GAMESTATE_GAMEOVER
        };

        private Rectangle gameRect;
        private Rectangle scoreRect;
        private Rectangle infoRect;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 900,
                PreferredBackBufferHeight = 900
            };
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //game framework configuration
            IsMouseVisible = true;

            gameField = new Field();
            difficultyLevel = 2;
            difficultyTickSpeedMillis = new int[MAX_LEVEL];

            for(int iii = 0; iii < MAX_LEVEL; iii++)
            {
                difficultyTickSpeedMillis[iii] = (int)(System.Math.Pow((0.8d - iii * 0.007d), iii)*1000);
            }

            gameState = GAMESTATE.GAMESTATE_MAIN_MENU;


            keys_moveLeft = Keys.A;
            keys_moveRight = Keys.D;
            keys_softDrop = Keys.S;
            keys_hardDrop = Keys.Space;
            keys_rotateLeft = Keys.Y;
            keys_rotateRight = Keys.W;
            keys_switchHold = Keys.R;
      
            keys_menuBack = Keys.Escape;


            base.Initialize();
        }



        private void IncreaseLevel()
        {
            if(difficultyLevel < MAX_LEVEL-1)
                difficultyLevel++;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameTextures_Blue = Content.Load<Texture2D>("bmp/BLUE");
            gameTextures_Cyan = Content.Load<Texture2D>("bmp/CYAN");
            gameTextures_Green = Content.Load<Texture2D>("bmp/GREEN");
            gameTextures_Magenta = Content.Load<Texture2D>("bmp/MAGENTA");
            gameTextures_Orange = Content.Load<Texture2D>("bmp/ORANGE");
            gameTextures_Red = Content.Load<Texture2D>("bmp/RED");
            gameTextures_Yellow = Content.Load<Texture2D>("bmp/YELLOW");
            gameTextures_Ghost = Content.Load<Texture2D>("bmp/GHOST");

            gameTextures_levelTextures = new Texture2D[MAX_LEVEL];
            string levelBmpName = "";
            for(int iii = 1; iii <= MAX_LEVEL; iii++)
            {
                if (iii < 10)
                    levelBmpName = "bmp/LVL_0" + iii + "0";
                else
                    levelBmpName = "bmp/LVL_" + iii + "0";
                gameTextures_levelTextures[iii - 1] = Content.Load<Texture2D>(levelBmpName);
            }

            gameRect = new Rectangle((graphics.PreferredBackBufferWidth - graphics.PreferredBackBufferHeight / 2) / 2, 0, graphics.PreferredBackBufferHeight / 2, graphics.PreferredBackBufferHeight);

            defaultFont = Content.Load<SpriteFont>("Default");


            MenuItem startGame2                = new MenuItem("Start Game", defaultFont, new Point(200, 50));

            //custom menu
            menuManager = new MenuManager();


            #region MainMenu
            Menu mainMenu = new Menu(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            MenuItem startGame = new MenuItem("Start Game", defaultFont, new Point(200,50));
            startGame.ButtonPressed += MenuAction_StartGame;

            MenuItem exitGame = new MenuItem("Exit", defaultFont, new Point(200, 50));
            exitGame.ButtonPressed += MenuAction_Exit;

            MenuItem options = new MenuItem("Options", defaultFont, new Point(200, 50));
            options.ButtonPressed += MenuAction_ToOptions;

            MenuItem scoreBoard = new MenuItem("Score Board", defaultFont, new Point(200, 50));
            scoreBoard.ButtonPressed += MenuAction_ToScoreBoard;

            mainMenu.AddItem(startGame);
            mainMenu.AddItem(options);
            mainMenu.AddItem(scoreBoard);
            mainMenu.AddItem(exitGame);

            mainMenu.ESCPressed += MenuAction_Exit;

            mainMenu.SetOrientationOnAll(MenuItem.MENUITEM_ORIENTATION.MENUITEM_ORIENTATION_CENTERED);
            mainMenu.SetSpacing(Menu.SPACING.SPACING_CLOSE);
            #endregion

            #region GameOverMenu
            Menu gameOverMenu = new Menu(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            MenuItem gameOver_scoreBoard = new MenuItem("Score Board", defaultFont, new Point(200, 50));
            gameOver_scoreBoard.ButtonPressed += MenuAction_ToScoreBoard;

            MenuItem gameOver_restart = new MenuItem("Restart", defaultFont, new Point(200, 50));
            gameOver_restart.ButtonPressed += MenuAction_StartGame;

            MenuItem gameOver_mainMenu = new MenuItem("Back to Main Menu", defaultFont, new Point(200, 50));
            gameOver_mainMenu.ButtonPressed += MenuAction_ToMain;

            MenuItem gameOver_exitGame = new MenuItem("Exit", defaultFont, new Point(200, 50));
            gameOver_exitGame.ButtonPressed += MenuAction_Exit;

            gameOverMenu.AddItem(gameOver_restart);
            gameOverMenu.AddItem(gameOver_scoreBoard);
            gameOverMenu.AddItem(gameOver_mainMenu);
            gameOverMenu.AddItem(gameOver_exitGame);

            gameOverMenu.ESCPressed += MenuAction_ToMain;

            gameOverMenu.SetOrientationOnAll(MenuItem.MENUITEM_ORIENTATION.MENUITEM_ORIENTATION_CENTERED);
            gameOverMenu.SetSpacing(Menu.SPACING.SPACING_CLOSE);
            #endregion

            #region ScoreMenu
            Menu scoreMenu = new Menu(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);


            MenuItem scoreMenu_Blabla1 = new MenuItem("Blablabla", defaultFont, new Point(200, 50));
            MenuItem scoreMenu_Blabla2 = new MenuItem("Lolololol", defaultFont, new Point(200, 50));
            MenuItem scoreMenu_Blabla3 = new MenuItem("Hahahahaha", defaultFont, new Point(200, 50));
            MenuItem scoreMenu_Blabla4 = new MenuItem("OmfgRoflXD", defaultFont, new Point(200, 50));

            MenuItem scoreMenu_mainMenu = new MenuItem("Back to Main Menu", defaultFont, new Point(200, 50));

            scoreMenu_mainMenu.ButtonPressed += MenuAction_ToMain;

            scoreMenu.AddItem(scoreMenu_Blabla1);
            scoreMenu.AddItem(scoreMenu_Blabla2);
            scoreMenu.AddItem(scoreMenu_Blabla3);
            scoreMenu.AddItem(scoreMenu_Blabla4);

            scoreMenu.AddItem(scoreMenu_mainMenu);

            scoreMenu.ESCPressed += MenuAction_ToMain;

            scoreMenu.SetOrientationOnAll(MenuItem.MENUITEM_ORIENTATION.MENUITEM_ORIENTATION_CENTERED);
            scoreMenu.SetSpacing(Menu.SPACING.SPACING_CLOSE);
            #endregion

            #region OptionMenu
            // OPTION MENU
            Menu optionMenu = new Menu(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);


            MenuItem optionMenu_MasterVolume = new MenuItem("Master Volume", defaultFont, new Point(200, 50));

            MenuItem optionMenu_EffectVolume = new MenuItem("Effect Volume", defaultFont, new Point(200, 50));

            MenuItem optionMenu_MusicVolume = new MenuItem("Music Volume", defaultFont, new Point(200, 50));

            MenuItem optionMenu_Hotkeys = new MenuItem("Hotkeys", defaultFont, new Point(200, 50));

            MenuItem optionMenu_MainMenu = new MenuItem("Back to Main Menu", defaultFont, new Point(200, 50));
            optionMenu_MainMenu.ButtonPressed += MenuAction_ToMain;


            optionMenu.AddItem(optionMenu_MasterVolume);
            optionMenu.AddItem(optionMenu_EffectVolume);
            optionMenu.AddItem(optionMenu_MusicVolume);
            optionMenu.AddItem(optionMenu_Hotkeys);
            optionMenu.AddItem(optionMenu_MainMenu);

            optionMenu.ESCPressed += MenuAction_ToMain;

            optionMenu.SetOrientationOnAll(MenuItem.MENUITEM_ORIENTATION.MENUITEM_ORIENTATION_CENTERED);
            optionMenu.SetSpacing(Menu.SPACING.SPACING_CLOSE);

            #endregion
            // Adding the menus

            menuManager.AddMenu("mainMenu", mainMenu);
            menuManager.AddMenu("gameOver", gameOverMenu);
            menuManager.AddMenu("scores", scoreMenu);
            menuManager.AddMenu("options", optionMenu);
            menuManager.SetActiveMenu("mainMenu");

        }

        #region eventhandler for menu items


        private void MenuAction_StartGame(object sender, MenuActionEventArgs e)
        {
            gameField.Restart();
            gameState = GAMESTATE.GAMESTATE_GAME_RUNNING;
        }

        private void MenuAction_Exit(object sender, MenuActionEventArgs e)
        {
            Exit();
        }

        private void MenuAction_ToMain(object sender, MenuActionEventArgs e)
        {
            menuManager.SetActiveMenu("mainMenu");
            gameState = GAMESTATE.GAMESTATE_MAIN_MENU;
        }

        private void MenuAction_ToOptions(object sender, MenuActionEventArgs e)
        {
            menuManager.SetActiveMenu("options");
            gameState = GAMESTATE.GAMESTATE_OPTIONS_MENU;
        }

        private void MenuAction_ToScoreBoard(object sender, MenuActionEventArgs e)
        {
            menuManager.SetActiveMenu("scores");
            gameState = GAMESTATE.GAMESTATE_SCORE_BOARD;
        }

        #endregion

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
            KeyboardState newState = Keyboard.GetState();



            Keys[] keysDown = newState.GetPressedKeys();


            switch (gameState)
            {
                case GAMESTATE.GAMESTATE_GAME_RUNNING:
                {
                    foreach (Keys k in keysDown)
                    {

                        if (oldState.IsKeyUp(k))
                        {
                            if (k == keys_moveLeft) { gameField.MoveLeft(); TimeSpan_DAS_Delay = TimeSpan.Zero; TimeSpan_DAS_Repeat = TimeSpan.Zero; }
                            if (k == keys_moveRight) { gameField.MoveRight(); TimeSpan_DAS_Delay = TimeSpan.Zero; TimeSpan_DAS_Repeat = TimeSpan.Zero; }
                            if (k == keys_hardDrop) { gameField.LockBlock(); }
                            if (k == keys_rotateLeft) { gameField.RotateLeft(); }
                            if (k == keys_rotateRight) { gameField.RotateRight(); }
                            if (k == keys_softDrop) { gameField.MoveDown(); TimeSpan_DAS_Delay = TimeSpan.Zero; TimeSpan_DAS_Repeat = TimeSpan.Zero; }
                            if (k == keys_switchHold) { gameField.SwitchActiveBlockWithHold(); }
                            if (k == keys_menuBack) { gameState = GAMESTATE.GAMESTATE_MAIN_MENU; }
                            }
                        if (oldState.IsKeyDown(k))
                        {
                            TimeSpan_DAS_Delay += gameTime.ElapsedGameTime;
                            TimeSpan_DAS_Repeat += gameTime.ElapsedGameTime;
                            if (TimeSpan_DAS_Delay.TotalMilliseconds >= DAS_Delay && TimeSpan_DAS_Repeat.TotalMilliseconds >= DAS_Speed)
                            {
                                if (k == keys_moveLeft) { gameField.MoveLeft(); TimeSpan_DAS_Repeat = TimeSpan.Zero; }
                                if (k == keys_moveRight) { gameField.MoveRight(); TimeSpan_DAS_Repeat = TimeSpan.Zero; }
                                if (k == keys_softDrop)
                                {
                                    gameField.MoveDown(); TimeSpan_DAS_Repeat = TimeSpan.Zero; TimeSpan_GravityTick = TimeSpan.Zero;
                                }
                            }
                        }
                    }
                    break;
                }
                default:
                {
                    foreach (Keys k in keysDown)
                    {
                        if (oldState.IsKeyUp(k))
                        {
                            menuManager.HandleInput(k);
                        }
                    }
                    break;
                }
            }


            switch (gameState)
            {
                case GAMESTATE.GAMESTATE_GAME_RUNNING:
                    {
                        TimeSpan_GravityTick += gameTime.ElapsedGameTime;
                        if (TimeSpan_GravityTick.TotalMilliseconds >= difficultyTickSpeedMillis[difficultyLevel])
                        {
                            gameField.MoveDown();
                            if (gameField.GetGameOver())
                            {
                                gameState = GAMESTATE.GAMESTATE_GAMEOVER;
                                menuManager.SetActiveMenu("gameOver");
                            }
                            TimeSpan_GravityTick = TimeSpan.Zero;
                        }
                        break;
                    }
                default:
                    break;
            }

            oldState = Keyboard.GetState();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aquamarine);

            // TODO: Add your drawing code here

            switch (gameState)
            {
                case GAMESTATE.GAMESTATE_MAIN_MENU:
                    {
                        spriteBatch.Begin();


                        spriteBatch.DrawString(defaultFont, "###############", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, 25), Color.Black);
                        spriteBatch.DrawString(defaultFont, "## Main Menu   ##", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, 50), Color.Black);
                        spriteBatch.DrawString(defaultFont, "###############", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, 75), Color.Black);

                        foreach (MenuItem mi in menuManager.GetActiveMenu().GetItems())
                        {
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_PASSIVE)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X, mi.GetLocation().Y), Color.Black);
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_ACTIVE)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X - 50, mi.GetLocation().Y), Color.Black);
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_PRESSED)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X - 50, mi.GetLocation().Y), Color.Black);
                        }
                        spriteBatch.End();
                        break;
                    }
                case GAMESTATE.GAMESTATE_PAUSE_MENU:
                    {
                        spriteBatch.Begin();
                        foreach (MenuItem mi in menuManager.GetActiveMenu().GetItems())
                        {
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_PASSIVE)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X, mi.GetLocation().Y), Color.Black);
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_ACTIVE)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X - 50, mi.GetLocation().Y), Color.Black);
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_PRESSED)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X - 50, mi.GetLocation().Y), Color.Black);
                        }
                        spriteBatch.End();
                        break;
                    }
                case GAMESTATE.GAMESTATE_GAME_RUNNING:
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(gameTextures_levelTextures[0], gameRect, Color.White);

                        int[] fieldArray = gameField.GetFieldArray();
                        Rectangle blockRect = new Rectangle(0, 0, 0, 0)
                        {
                            Width = (gameRect.Width / (Config.GetFieldWidth() - 2)),
                            Height = (gameRect.Height / (Config.GetFieldHeight() - 5))
                        };

                        for (int xxx = 0; xxx < Config.GetFieldWidth()-2; xxx++)
                        {
                            for (int yyy = 0; yyy < Config.GetFieldHeight() - 5; yyy++)
                            {
                                int currentFieldValue = fieldArray[(yyy+2) * Config.GetFieldWidth() + xxx + 1];
                                if (currentFieldValue != 0)
                                {
                                    blockRect.X = gameRect.X + blockRect.Width * xxx;
                                    blockRect.Y = gameRect.Y + blockRect.Height * yyy;
                                    switch (currentFieldValue)
                                    {
                                        case 1:
                                            {
                                                spriteBatch.Draw(gameTextures_Cyan, blockRect, Color.White);
                                                break;
                                            }
                                        case 2:
                                            {
                                                spriteBatch.Draw(gameTextures_Yellow, blockRect, Color.White);
                                                break;
                                            }
                                        case 3:
                                            {
                                                spriteBatch.Draw(gameTextures_Magenta, blockRect, Color.White);
                                                break;
                                            }
                                        case 4:
                                            {
                                                spriteBatch.Draw(gameTextures_Green, blockRect, Color.White);
                                                break;
                                            }
                                        case 5:
                                            {
                                                spriteBatch.Draw(gameTextures_Red, blockRect, Color.White);
                                                break;
                                            }
                                        case 6:
                                            {
                                                spriteBatch.Draw(gameTextures_Blue, blockRect, Color.White);
                                                break;
                                            }
                                        case 7:
                                            {
                                                spriteBatch.Draw(gameTextures_Orange, blockRect, Color.White);
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                    //ghost Block drawing
                                    
                                    if (currentFieldValue < 0)
                                    {
                                        spriteBatch.Draw(gameTextures_Ghost, blockRect, Color.White);
                                    }
                                }
                            }
                        }

                        spriteBatch.DrawString(defaultFont, gameField.ToString(), new Vector2(10, 10), Color.Black);

                        spriteBatch.DrawString(defaultFont, "difficulty "+difficultyLevel+" tick \nspeed (millis):\n " + difficultyTickSpeedMillis[difficultyLevel], new Vector2(700, 10), Color.Black);

                        spriteBatch.DrawString(defaultFont, "ghost block \n location:\n " + gameField.GetGhostBlock().GetLocation(), new Vector2(700, 200), Color.Black);
                        spriteBatch.DrawString(defaultFont, "GAME OVER?:\n " + gameField.GetGameOver(), new Vector2(700, 400), Color.Black);


                        spriteBatch.End();
                        break;
                    }
                case GAMESTATE.GAMESTATE_GAMEOVER:
                    {
                        spriteBatch.Begin();


                        spriteBatch.DrawString(defaultFont, "###############", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, 25), Color.Black);
                        spriteBatch.DrawString(defaultFont, "## Game Over! ##", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, 50), Color.Black);
                        spriteBatch.DrawString(defaultFont, "###############", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, 75), Color.Black);

                        foreach (MenuItem mi in menuManager.GetActiveMenu().GetItems())
                        {
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_PASSIVE)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X, mi.GetLocation().Y), Color.Black);
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_ACTIVE)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X - 50, mi.GetLocation().Y), Color.Black);
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_PRESSED)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X - 50, mi.GetLocation().Y), Color.Black);
                        }
                        spriteBatch.End();
                        break;
                    }
                case GAMESTATE.GAMESTATE_SCORE_BOARD:
                    {
                        spriteBatch.Begin();


                        spriteBatch.DrawString(defaultFont, "###############", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, 25), Color.Black);
                        spriteBatch.DrawString(defaultFont, "## Scoreboard! ##", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, 50), Color.Black);
                        spriteBatch.DrawString(defaultFont, "###############", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, 75), Color.Black);

                        foreach (MenuItem mi in menuManager.GetActiveMenu().GetItems())
                        {
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_PASSIVE)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X, mi.GetLocation().Y), Color.Black);
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_ACTIVE)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X - 50, mi.GetLocation().Y), Color.Black);
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_PRESSED)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X - 50, mi.GetLocation().Y), Color.Black);
                        }
                        spriteBatch.End();
                        break;
                    }
                case GAMESTATE.GAMESTATE_OPTIONS_MENU:
                    {
                        spriteBatch.Begin();


                        spriteBatch.DrawString(defaultFont, "###############", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, 25), Color.Black);
                        spriteBatch.DrawString(defaultFont, "###  Options   ###", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, 50), Color.Black);
                        spriteBatch.DrawString(defaultFont, "###############", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, 75), Color.Black);

                        foreach (MenuItem mi in menuManager.GetActiveMenu().GetItems())
                        {
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_PASSIVE)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X, mi.GetLocation().Y), Color.Black);
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_ACTIVE)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X - 50, mi.GetLocation().Y), Color.Black);
                            if (mi.GetState() == MenuItem.MENUITEM_STATE.MENUITEM_STATE_PRESSED)
                                spriteBatch.DrawString(mi.GetFont(), mi.GetText(), new Vector2(mi.GetLocation().X - 50, mi.GetLocation().Y), Color.Black);
                        }
                        spriteBatch.End();
                        break;
                    }
            }
            base.Draw(gameTime);
        }
    }
}
