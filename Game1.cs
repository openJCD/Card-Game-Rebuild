using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Card_Game_Rebuild
{
    public class Game1 : Game
    {
        public static void PrintHello()
        {
            //Console.WriteLine("Hello from Button World!");
            Debug.WriteLine("Hello from Button World!");
        }
        public struct ScreenScaler
        {
            public Vector2 referenceDimensions;
            public Vector2 screenDimensions;
            public float objectScale;
            public ScreenScaler(Vector2 scrnDims)
            {
                screenDimensions = scrnDims;
                referenceDimensions = new Vector2(1920, 1080);
                objectScale = screenDimensions.X / referenceDimensions.Y;
            }
        }

        // buttons
        public Button testButton;

        // cards
        public Card testCard;

        // textures
        public Texture2D cardbg;
        public Texture2D cardfg;
        public Texture2D buttonbg;
        public static Texture2D statGraphic { get; set; }
        public Texture2D hitbox;

        // fonts
        public SpriteFont cardfont;

        //misc
        public ScreenScaler scaler;
        public GameState gameState;
        private MouseState oldState;
        public MouseState pubState;
        public CardStat testCardStat;

        // managers
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public ContentManager _manager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            // TODO: Add your initialization logic here
            //_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //_graphics.ToggleFullScreen();
            //_graphics.ApplyChanges();

            _graphics.PreferredBackBufferWidth = 720;
            _graphics.PreferredBackBufferHeight = 640;

            scaler = new ScreenScaler(new Vector2(1366, 768));

            gameState = GameState.Debug;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _manager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            // TODO: use this.Content to load your game content here


            // Load content for debug _manager
            cardbg = _manager.Load<Texture2D>("Images/CardBG");
            cardfg = _manager.Load<Texture2D>("Images/card_noimg");
            buttonbg = _manager.Load<Texture2D>("Images/btn_play_bg");
            statGraphic = _manager.Load<Texture2D>("Images/ui_info");
            hitbox = _manager.Load<Texture2D>("Images/rect");

            cardfont = _manager.Load<SpriteFont>("Fonts/RedHatRegular");

            // create new button for debug
            testButton = new Button(buttonbg, new Vector2(300,300), _manager, PrintHello, cardfont, label: "button!");

            // define misc stuff
            testCardStat = new CardStat("dummy", 10, 10, "none", "none");
            testCard = new Card(_manager, cardbg, cardfg, testCardStat, cardfont);
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch (gameState)
            {
                case GameState.MainMenu:
                    UpdateMenu();
                    // DrawMenu();
                    break;

                case GameState.GamePlay:
                    UpdateGame();
                    // DrawGame();
                    break;

                case GameState.GameOver:
                    UpdateEnd();
                    // DrawEnd();
                    break;

                case GameState.Debug:
                    UpdateDebug();
                    // DrawDebug();
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (gameState)
            {
                case GameState.MainMenu:
                    //UpdateMenu();
                    DrawMenu();
                    break;

                case GameState.GamePlay:
                    //UpdateGame();
                    DrawGame();
                    break;

                case GameState.GameOver:
                    //UpdateEnd();
                    DrawEnd();
                    break;

                case GameState.Debug:
                    //UpdateDebug();
                    DrawDebug();
                    break;
            }

            base.Draw(gameTime);
        }

        #region States
        public enum GameState
        {
            MainMenu,
            GamePlay,
            GameOver,
            Debug
        }
        public void UpdateMenu()
        {

        }
        public void DrawMenu()
        {

        }
        public void UpdateGame()
        {

        }
        public void DrawGame()
        {

        }
        public void UpdateEnd()
        {

        }
        public void DrawEnd()
        {

        }
        public void UpdateDebug()
        {
            MouseState newState = Mouse.GetState();
            pubState = Mouse.GetState();

            testButton.Update(pubState, oldState, newState);
            testCard.Update(pubState, oldState, newState);

            oldState = newState;
        }
        public void DrawDebug()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //place drawing code for _spriteBatch here...
            //{
            testButton.Draw(_spriteBatch);
            testCard.Draw(_spriteBatch, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, statGraphic);
            _spriteBatch.Draw(hitbox, testCard.rect, Color.White);
            //_spriteBatch.Draw(statGraphic, statGraphic.Bounds, Color.White);
            //}
            _spriteBatch.End();
        }
        public void ChangeGameState(GameState state)
        {

            switch (state)
            {
                case GameState.MainMenu:
                    gameState = GameState.MainMenu;
                    // DrawMenu();
                    break;

                case GameState.GamePlay:
                    gameState = GameState.GamePlay;
                    // DrawGame();
                    break;

                case GameState.GameOver:
                    gameState = GameState.GameOver;
                    // DrawEnd();
                    break;

                case GameState.Debug:
                    gameState = GameState.Debug;
                    // DrawDebug();
                    break;
            }

        }
        #endregion

    }
}