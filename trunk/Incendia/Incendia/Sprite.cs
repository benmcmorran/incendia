using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Incendia
{
    /// <summary>
    /// You guys know what this is.
    /// </summary>
    class Sprite
    {

        protected float _rotation; // In radians
        protected float _scale; //In factors
        protected Vector2 _position; //Location of upper-right corner in tiles
        protected float _rotationSpeed; //In radians per second 
        protected float _scaleSpeed;//In factors per second
        protected Vector2 _velocity; //In tiles per second
        protected Animation _defaultAnimation;
        protected bool _dead;//Don't draw or update if you're dead
        protected SpriteEffects _effects; //Flipping horizontally, vertically or not at all
        protected double _lifeTime;//In seconds

        //Return a rectangle based on the current position and the size of the current default frame
        //May be overridden by a parent class
        public virtual Rectangle Visual { get { return new Rectangle((int)_position.X, (int)_position.Y, (int)((float)_defaultAnimation.GetFrame().Rectangle.Width / Global.PixelsPerTile), (int)((float)_defaultAnimation.GetFrame().Rectangle.Height / Global.PixelsPerTile)); } }

        public float Rotation { get { return _rotation; } set { _rotation = value; } }
        public float Scale { get { return _scale; } set { _scale = value; } }
        public float RotationSpeed { get { return _rotationSpeed; } set { _scaleSpeed = value; } }
        public float ScaleSpeed { get { return _scaleSpeed; } set { _scaleSpeed = value; } }
        public Vector2 Velocity { get { return _velocity; } }
        public bool Dead { get { return _dead || _scale <= 0; } set { _dead = value; } }
        public Vector2 Position { get { return _position / Global.PixelsPerTile; } } //Location of upper-right corner in tiles
        public Vector2 PositionCenter { get { return new Vector2(_position.X + Visual.Width, _position.Y + Visual.Height) / Global.PixelsPerTile; } } //Location of center in tiles


        public Sprite(Vector2 position, Animation defaultAnimation, double lifeTime)
        {
            _defaultAnimation = defaultAnimation;
            _position = position;
            _rotation = 0;
            _scale = 1;
            _lifeTime = lifeTime;
            _effects = SpriteEffects.None;
        }

        public virtual void Update(GameTime gameTime, PlayState map)
        {
            if (Dead)
                return;
            _rotation += _rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _scale += _scaleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * Global.PixelsPerTile;

            if (_lifeTime >= 0)
            {
                _lifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_lifeTime <= 0)
                    Dead = true;
            }
        }

        public virtual void Draw(SpriteBatch batch)
        {
            if (Dead)
                return;
            _defaultAnimation.GetFrame().Draw(new Vector2(_position.X + Visual.Width * (Global.PixelsPerTile / 2), _position.Y + Visual.Height * (Global.PixelsPerTile / 2)), batch, _rotation, _scale, _effects);
        }


        //Field Accessors
        public void SetVelocityX(float x)
        {
            _velocity.X = x;
        }

        public void SetVelocityY(float y)
        {
            _velocity.Y = y;
        }
        public void SetPositionX(float x)
        {
            _position.X = x * Global.PixelsPerTile;
        }

        public void SetPositionY(float y)
        {
            _position.Y = y * Global.PixelsPerTile;
        }

    }
}
