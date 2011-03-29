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

        protected float _rotation;
        protected float _scale;
        protected Vector2 _position;
        protected float _rotationSpeed; //In radians per second 
        protected float _scaleSpeed;//In factors per second
        protected Vector2 _velocity; //In pixils per second
        protected Animation _defaultAnimation;
        protected bool _dead;//Don't draw or update if you're dead
        protected SpriteEffects _effects;
        protected double _lifeTime;//In seconds

        //Return a rectangle based on the current position and the size of the current default frame
        //May be overridden by a parent class
        public virtual Rectangle Visual { get { return new Rectangle((int)_position.X, (int)_position.Y, _defaultAnimation.GetFrame().Rectangle.Width, _defaultAnimation.GetFrame().Rectangle.Height); } }

        public float Rotation { get { return _rotation; } set { _rotation = value; } }
        public float Scale { get { return _scale; } set { _scale = value; } }
        public float RotationSpeed { get { return _rotationSpeed; } set { _scaleSpeed = value; } }
        public float ScaleSpeed { get { return _scaleSpeed; } set { _scaleSpeed = value; } }
        public Vector2 Velocity { get { return _velocity; } }
        public bool Dead { get { return _dead || _scale <= 0; } set { _dead = value; } }
        public Vector2 Position { get { return _position; } }


        public Sprite(Vector2 position, Animation defaultAnimation, double lifeTime)
        {
            _defaultAnimation = defaultAnimation;
            _position = position;
            _rotation = 0;
            _scale = 1;
            _lifeTime = lifeTime;
            _effects = SpriteEffects.None;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Dead)
                return;
            _rotation += _rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _scale += _scaleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

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
            _defaultAnimation.GetFrame().Draw(_position, batch, _rotation, _scale, _effects);
        }


        public void SetVelocityX(float x)
        {
            _velocity.X = x;
        }

        public void SetVelocityY(float y)
        {
            _velocity.Y = y;
        }

    }
}
