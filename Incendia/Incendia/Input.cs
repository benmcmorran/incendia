using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Incendia
{
    /// <summary>
    /// The class which handles all input via static methods. It must be updated in Game1.
    /// </summary>
    class Input
    {
        static KeyboardState currentKeyboardState;
        static MouseState currentMouseState;
        static KeyboardState oldKeyboardState;
        static MouseState oldMouseState;

        public static void Update()
        {
            oldKeyboardState = currentKeyboardState;
            oldMouseState = currentMouseState;
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
        }

        //For the Keyboard
        public static bool KeyHeld(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public static bool KeyReleased(Keys key)
        {
            return currentKeyboardState.IsKeyUp(key);
        }

        public static bool KeyJustPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key);
        }

        public static bool KeyJustReleased(Keys key)
        {
            return currentKeyboardState.IsKeyUp(key) && oldKeyboardState.IsKeyDown(key);
        }

        //For the Mouse
        public static bool MouseLeftClicked()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool MouseRightClicked()
        {
            return currentMouseState.RightButton == ButtonState.Pressed;
        }

        public static bool MouseLeftReleased()
        {
            return currentMouseState.LeftButton == ButtonState.Released;
        }

        public static bool MouseRightReleased()
        {
            return currentMouseState.RightButton == ButtonState.Released;
        }

        public static bool MouseLeftJustPressed()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
        }

        public static bool MouseRightJustPressed()
        {
            return currentMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
        }

        public static bool MouseLeftJustReleased()
        {
            return currentMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool MouseRightJustReleased()
        {
            return currentMouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed;
        }

        public static float DistanceToMouse(Vector2 point)
        {
            return (float)Math.Sqrt((point.X - currentMouseState.X) * (point.X - currentMouseState.X) + (point.Y - currentMouseState.Y) * (point.Y - currentMouseState.Y));
        }

        public static float AngleToMouse(Vector2 point)
        {
            return (float)Math.Atan2(point.Y - currentMouseState.Y, point.X - currentMouseState.X) + (float)Math.PI;
        }
    }
}
