using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Card_Game_Rebuild
{
    public class Animation
    {
        //go visit rbwhitaker.wikidot.com/texture-atlases for this cuz it isn't my code
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        public Animation(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }
        public void Update()
        {
            currentFrame++;
            if (currentFrame == totalFrames)
                currentFrame = 0;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 location, Game1.ScreenScaler scaler)
        {
            int width = (int)(Texture.Width * scaler.objectScale / Columns);
            int height = (int)(Texture.Height * scaler.objectScale / Rows);
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)((int)location.X * scaler.objectScale), (int)((int)location.Y * scaler.objectScale), (int)(width * scaler.objectScale), (int)(height * scaler.objectScale));

            spriteBatch.Draw(Texture, location, sourceRectangle, Color.White, 0f, new Vector2(destinationRectangle.X+destinationRectangle.Width/2, destinationRectangle.Y + destinationRectangle.Height / 2), scaler.objectScale, SpriteEffects.None, 0f);          
        }
        public void forceFrame(int frame)
        {
            currentFrame = frame;
        }
    }
}