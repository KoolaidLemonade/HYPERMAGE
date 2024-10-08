using HYPERMAGE.Spells;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HYPERMAGE.Managers
{
    public static class InputManager

    {
        private static MouseState _oldMouse;
        public static bool Clicked { get; private set; }
        public static bool RightClicked { get; private set; }
        public static Vector2 MousePosition { get; private set; }

        public static bool LeftMouseDown { get; private set; }
        public static bool RightMouseDown { get; private set; }

        private static Vector2 _direction;
        public static Vector2 _lastDirection = new(0, 1);
        public static Vector2 Direction => _direction;
        public static bool Moving => _direction != Vector2.Zero;

        public static bool _dashing;

        public static float _dashCooldown = 100;
        public static void Update()
        {
            var ms = Mouse.GetState();

            LeftMouseDown = ms.LeftButton == ButtonState.Pressed;
            RightMouseDown = ms.RightButton == ButtonState.Pressed;

            Clicked = _oldMouse.LeftButton == ButtonState.Released && ms.LeftButton == ButtonState.Pressed;
            RightClicked = _oldMouse.RightButton == ButtonState.Released && ms.RightButton == ButtonState.Pressed;
            MousePosition = new Vector2(ms.Position.X, ms.Position.Y) / new Vector2(1920 / 320, 1080 / 180);

            _oldMouse = ms;

            _direction = Vector2.Zero;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.GetPressedKeyCount() > 0)
            {
                if (keyboardState.IsKeyDown(Keys.A)) _direction.X--;
                if (keyboardState.IsKeyDown(Keys.D)) _direction.X++;
                if (keyboardState.IsKeyDown(Keys.W)) _direction.Y--;
                if (keyboardState.IsKeyDown(Keys.S)) _direction.Y++;

                if (_direction != Vector2.Zero)
                {
                    _lastDirection = _direction;
                }
            }

            if (keyboardState.IsKeyDown(Keys.LeftShift) && _dashCooldown > 100f)
            {
                _dashCooldown = 0;
                _dashing = true;
            }

            _dashCooldown++;
        }
    }
}