using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Card_Game_Rebuild
{
    public class Card
    {
        public ContentManager contentManager { get; set; }

        public Texture2D background { get; set; }
        public Texture2D foreground { get; set; }
        public Texture2D statGraphic { get; set; }

        private SpriteFont font;

        public CardStat stats;

        public Rectangle rect;
        private Vector2 origin;
        private Vector2 fgorigin;

        // animation to be used on hover/click
        public Animation states;

        public CardStatus current_status;

        // animation to be potentially used for the actual attacks/actions in gameplay.
        // public Animation action;
        public Card(ContentManager contentManager, Texture2D background, Texture2D foreground, CardStat stats, SpriteFont font,  Vector2 position = new Vector2())
        {
            this.contentManager = contentManager;
            this.background = background;
            this.foreground = foreground;
            this.stats = stats;
            this.rect = new Rectangle(position.ToPoint(), new Point(background.Width, background.Height));
            this.font = font;
            this.current_status = CardStatus.Static;
        }
        public void Update (MouseState mouse, MouseState oldState, MouseState newState)
        {
            if (rect.Contains(mouse.Position))
            {
                this.current_status = CardStatus.Hover;
                System.Diagnostics.Debug.WriteLine("mouse");
            } 
            else if (!rect.Contains(mouse.Position))
            {
                this.current_status = CardStatus.Static;
            }
        }
        public void Draw(SpriteBatch spriteBatch, int windowWidth, int windowHeight, Texture2D statTexture)
        {
            //|--- Placeholder
            Vector2 pos = new Vector2(rect.X+background.Width/2, rect.Y+background.Height/2);
            //---|
            //dynamic variable declarations
            Vector2 textOrigin = font.MeasureString(this.stats.label) / 2;
            origin = new Vector2(/*rect.X+*/rect.Width / 2, /*rect.Y*/+rect.Height / 2);
            fgorigin = new Vector2(foreground.Width / 2, foreground.Height / 2.45f);
            Rectangle statTexPos = new Rectangle(new Point(windowWidth - statTexture.Bounds.X, windowHeight - statTexture.Bounds.Y), new Point(statTexture.Width, statTexture.Height));

            //draw calls
            spriteBatch.Draw(background, pos, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(foreground, pos, null, Color.White, 0f, fgorigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, this.stats.label, new Vector2(pos.X, pos.Y - (rect.Height / 2.45f)), Color.Black, 0f, textOrigin, 1f, SpriteEffects.None, 0f);
            //situational draw calls
            if (this.current_status == CardStatus.Hover)
            {
                spriteBatch.Draw(statTexture, statTexture.Bounds, Color.White);
            }
        }
    }

    public struct CardStat
    {
        public string label; 
        public int currenthealth;
        public int maxhealth;
        public string weakness;
        public string type;
        public CardStat (string label, int currenthealth, int maxhealth, string weakness, string type)
        {
            this.label = label;
            this.currenthealth = currenthealth;
            this.maxhealth = maxhealth;
            this.weakness = weakness;
            this.type = type;
        }
    }
    public enum CardStatus
    {
        Static,
        Hover,
        Clicked,
        Dragging
    }

    /// <summary>
    /// An interactive button.
    /// </summary>
    public class Button : IDisposable
    {
        public Texture2D background { get; set; }
        public Texture2D icon { get; set; }

        private SpriteFont font;

        public Animation states;

        private ContentManager contentManager { get; set; }

        public string label;
        public BtnState buttonState;
        
        public int backgroundRows;
        public int backgroundColumns;

        public Rectangle rectangle;
        private Vector2 position { get; set; }
        private Vector2 centre { get; set; }

        public Vector2 originalOffset = new Vector2(0, 6);
        public Vector2 offset;

        public delegate void buttonEventHandler(/*object sender, EventArgs e*/);
        public buttonEventHandler onClick;

        /// <summary>
        /// New Button instance
        /// </summary>
        /// <param name="background">Main visual body of button. Static and hover states can be put in one texture. Rectangle bounding box is based on this.</param>
        /// <param name="position">Position Vector</param>
        /// <param name="contentManager"></param>
        /// <param name="buttonAction"></param>
        /// <param name="font"></param>
        /// <param name="backgroundRows"></param>
        /// <param name="backgroundColumns"></param>
        /// <param name="label"></param>
        public Button(Texture2D background, Vector2 position, ContentManager contentManager, buttonEventHandler buttonAction, SpriteFont font, int backgroundRows = 1, int backgroundColumns = 2, string label = "")
        {
            this.background = background;
            this.backgroundRows = backgroundRows;
            this.backgroundColumns = backgroundColumns;

            onClick = buttonAction;

            this.contentManager = contentManager;
            this.states = new Animation(background, 1, 2);
            this.rectangle = new Rectangle((int)position.X, (int)position.Y, background.Width / backgroundColumns, background.Height / backgroundRows);
            this.centre = new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
            this.font = font;
            this.label = label;
            this.offset = originalOffset;
        }
        public void Update(MouseState mouse, MouseState oldState, MouseState newState)
        {
            if (rectangle.Contains(new Point(mouse.X, mouse.Y)))
            {
                buttonState = BtnState.Hover;
                offset.Y = 0; 
                if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                {
                    buttonState = BtnState.Clicked;
                }
            }
            else if (!rectangle.Contains(new Point(mouse.X, mouse.Y)))
            {
                buttonState = BtnState.Static;
                offset = originalOffset;
            }

            if (buttonState == BtnState.Clicked)
            {
                onClick();
            }  
        }

        // make sure this method is placed between the target SpriteBatch's Begin() and End() calls.
        public void Draw(SpriteBatch spriteBatch)
        {
            if (buttonState == BtnState.Static)
                states.forceFrame(0);
            else if (buttonState == BtnState.Hover)
                states.forceFrame(1);
            
            Vector2 origin = font.MeasureString(label) / 2;
            Vector2 pos = new Vector2(rectangle.X, rectangle.Y);
            
            states.Draw(spriteBatch, pos);
            spriteBatch.DrawString(font, label, centre, Color.Black, 0f, origin + offset, 1f, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Warning: this will unload everything contained within the ContentManager used for the Button. Ideally, a new ContentManager is created for each instance. 
        /// </summary>
        public void Dispose()
        {
            contentManager.Unload();
            contentManager.Dispose();
        }
    }
    public enum BtnState
    {
        Static, 
        Hover, 
        Clicked
    }
    //eof
}
