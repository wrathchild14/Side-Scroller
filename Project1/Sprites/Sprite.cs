﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.Managers;
using Project1.Models;
using Project1.TileMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project1
{
    public abstract class Sprite : Component
    {
        public bool Dead = false;

        protected AnimationManager animation_manager_;
        protected Dictionary<string, Animation> animations_;

        protected Texture2D texture_;
        protected Vector2 position_;
        protected float layer_ { get; set; }

        public Color Colour { get; set; }
        public float Opacity { get; set; }
        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }

        public Vector2 Position
        {
            get { return position_; }
            set
            {
                position_ = value;

                if (animation_manager_ != null)
                    animation_manager_.Position = position_;
            }
        }

        public void Die()
        {
            Dead = true;
        }

        public float X
        {
            get { return Position.X; }
            set
            {
                Position = new Vector2(value, Position.Y);
            }
        }

        public float Y
        {
            get { return Position.Y; }
            set
            {
                Position = new Vector2(Position.X, value);
            }
        }

        public float Layer
        {
            get { return layer_; }
            set
            {
                layer_ = value;

                if (animation_manager_ != null)
                    animation_manager_.Layer = layer_;
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                //int x = 0;
                //int y = 0;
                int width = 0;
                int height = 0;

                if (texture_ != null)
                {
                    width = texture_.Width;
                    height = texture_.Height;
                } else if (animation_manager_ != null)
                {
                    width = animation_manager_.FrameWidth;
                    height = animation_manager_.FrameHeight;
                }

                //return new Rectangle((int)(Position.X - Origin.X), (int)(Position.Y - Origin.Y), (int)(width * Scale), (int)(height * Scale));
                return new Rectangle((int)(Position.X), (int)(Position.Y), (int)(width * Scale), (int)(height * Scale));
            }
        }

        public bool IsRemoved { get; set; }

        public Sprite(Texture2D texture)
        {
            texture_ = texture;
            Opacity = 1f;
            Scale = 1f;
            Origin = new Vector2(0, 0);
            Colour = Color.White;
        }

        public Sprite(Dictionary<string, Animation> animations)
        {
            animations_ = animations;
            animation_manager_ = new AnimationManager(animations_.First().Value);
            Opacity = 1f;
            Scale = 1f;
            Colour = Color.White;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (texture_ != null)
                spriteBatch.Draw(texture_, Position, null, Colour * Opacity, Rotation, Origin, Scale, SpriteEffects.None, Layer);

            animation_manager_?.Draw(spriteBatch);
        }

        internal void Collision(CollisionTile tile, int x_offset, int y_offset)
        {
            Rectangle tile_rectangle = tile.Rectangle;
            if (Rectangle.TouchTopOf(tile_rectangle))
                Y = tile_rectangle.Y - Rectangle.Height;

            if (Rectangle.TouchLeftOf(tile_rectangle))
                X = tile_rectangle.X - Rectangle.Width - 2;

            if (Rectangle.TouchRightOf(tile_rectangle))
                X = tile_rectangle.X + tile_rectangle.Width + 4;

            if (Rectangle.TouchBottomOf(tile_rectangle))
                Y = tile_rectangle.Y + tile_rectangle.Height + Rectangle.Height / 2;

            if (X < 0) X = 0;
            if (X > x_offset - Rectangle.Width) X = x_offset - Rectangle.Width;
            if (Y < 0) Y = 0;
            if (Y > y_offset - Rectangle.Height) Y = y_offset - Rectangle.Height;
        }

        public virtual void OnCollide(Sprite sprite)
        {

        }

        public bool IsTouching(Sprite sprite)
        {
            return this.Rectangle.Right >= sprite.Rectangle.Left &&
                this.Rectangle.Left <= sprite.Rectangle.Right &&
                this.Rectangle.Bottom >= sprite.Rectangle.Top &&
                this.Rectangle.Top <= sprite.Rectangle.Bottom;
        }

        public bool IsTouchingTopOf(Sprite sprite)
        {
            return this.Rectangle.Right >= sprite.Rectangle.Left &&
                this.Rectangle.Left <= sprite.Rectangle.Right &&
                this.Rectangle.Bottom >= sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Top;
        }

        public bool IsTouchingLeftOf(Sprite sprite)
        {
            return this.Rectangle.Bottom >= sprite.Rectangle.Top &&
              this.Rectangle.Top <= sprite.Rectangle.Bottom &&
              this.Rectangle.Right >= sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Left;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #region Positions
        public Vector2 TopLeft
        {
            get
            {
                return new Vector2(Rectangle.X, Rectangle.Y);
            }
        }

        public Vector2 TopRight
        {
            get
            {
                return new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y);
            }
        }

        public Vector2 BottomLeft
        {
            get
            {
                return new Vector2(Rectangle.X, Rectangle.Y + Rectangle.Height);
            }
        }

        public Vector2 BottomRight
        {
            get
            {
                return new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height);
            }
        }

        public Vector2 Centre
        {
            get
            {
                return new Vector2(Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + (Rectangle.Height / 2));
            }
        }
        #endregion
    }
}