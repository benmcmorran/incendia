using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Incendia
{
    public class Camera2D
    {
        private Vector2 _location;
        private float _rotation;
        private float _zoom;
        private bool _needUpdate = true;
        private Rectangle _cachedBoundingBox;
        private bool _isMovingUsingScreenAxes;

        public int Shake { get; set; }

        public Camera2D()
            : this(Vector2.Zero, 0.0f, 1.0f, true)
        { }

        public Camera2D(Vector2 location, float rotation, float zoom, bool isMovingUsingScreenAxes)
        {
            Location = location;
            Rotation = rotation;
            Zoom = zoom;
            IsMovingUsingScreenAxes = isMovingUsingScreenAxes;
        }

        public Vector2 Location
        {
            get { return _location + new Vector2(Global.rand.Next(-Shake, Shake), Global.rand.Next(-Shake, Shake)); }
            set
            {
                _location = value;
                _needUpdate = true;
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value % MathHelper.TwoPi;
                _needUpdate = true;
            }
        }

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = MathHelper.Clamp(value, 0f, float.PositiveInfinity);
                _needUpdate = true;
            }
        }

        public bool IsMovingUsingScreenAxes
        {
            get { return _isMovingUsingScreenAxes; }
            set { _isMovingUsingScreenAxes = value; }
        }

        public float UpScreenAxis
        {
            get { return _rotation; }
        }

        public float RightScreenAxis
        {
            get { return _rotation + MathHelper.PiOver2; }
        }     
   
        public float DownScreenAxis
        {
            get { return _rotation + MathHelper.Pi; }
        }

        public float LeftScreenAxis
        {
            get { return _rotation + MathHelper.Pi + MathHelper.PiOver2; }
        }

        public void Move(Vector2 movement)
        {
            if (_isMovingUsingScreenAxes)
            {
                Matrix rotationMatrix = Matrix.CreateRotationZ(-_rotation);
                _location += Vector2.Transform(movement, rotationMatrix);
            }
            else
            {
                _location.X += movement.X;
                _location.Y += movement.Y;
            }
        }

        public Rectangle BoundingBox(Viewport viewport)
        {
            if (_needUpdate)
            {
                Vector2 screenSize = new Vector2(viewport.Width, viewport.Height);

                Vector2[] cameraCorners = new Vector2[4];
                Matrix view = Matrix.CreateRotationZ(_rotation) *
                                Matrix.CreateScale(1 / _zoom) *
                                Matrix.CreateTranslation(_location.X, _location.Y, 0);

                cameraCorners[0] = new Vector2(-screenSize.X / 2, -screenSize.Y / 2);
                cameraCorners[1] = new Vector2(screenSize.X / 2, -screenSize.Y / 2);
                cameraCorners[2] = new Vector2(-screenSize.X / 2, screenSize.Y / 2);
                cameraCorners[3] = new Vector2(screenSize.X / 2, screenSize.Y / 2);

                Vector2.Transform(cameraCorners, ref view, cameraCorners);

                float lowX, highX, lowY, highY;
                var orderedCameraCorners = from point in cameraCorners
                                           orderby point.X ascending
                                           select point;
                lowX = orderedCameraCorners.ElementAt<Vector2>(0).X;
                highX = orderedCameraCorners.ElementAt<Vector2>(3).X;

                orderedCameraCorners = from point in cameraCorners
                                       orderby point.Y ascending
                                       select point;
                lowY = orderedCameraCorners.ElementAt<Vector2>(0).Y;
                highY = orderedCameraCorners.ElementAt<Vector2>(3).Y;

                _cachedBoundingBox = new Rectangle(
                    (int)Math.Floor(lowX),
                    (int)Math.Floor(lowY),
                    (int)Math.Ceiling(highX - lowX),
                    (int)Math.Ceiling(highY - lowY));
            }

            return _cachedBoundingBox;
        }

        public bool IsInView(Rectangle rectangle, Viewport viewport)
        {
            return rectangle.Intersects(BoundingBox(viewport));
        }

        public Matrix ViewTransformationMatrix(Viewport viewport)
        {
            Vector2 screenSize = new Vector2(viewport.Width, viewport.Height);

            Matrix viewMatrix = Matrix.CreateTranslation(-_location.X, -_location.Y, 0) *
                Matrix.CreateScale(_zoom) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateTranslation((screenSize.X / 2), (screenSize.Y / 2), 0);

            // Don't scale the depth (Z axis)
            viewMatrix.M33 = 1.0f;

            return viewMatrix;
        }
    }
}