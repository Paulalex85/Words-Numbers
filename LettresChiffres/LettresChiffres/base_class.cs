using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using XMLObject;

namespace LettresChiffres
{
    class base_class : GameScreen
    {
        SpriteFont transition_font;

        MouseState state = Mouse.GetState();
        MouseState previousState;

        XML_Serializer xmls = new XML_Serializer();
        Sauvegarde save;
        Languages lang = new Languages();
        Langues langue = new Langues();

        public base_class()
        {
        }

        private void InitilizeLanguages()
        {
            //Simple_string = lang.AffectationLANG("Simple_string", langue);

        }

        public override void LoadContent()
        {

            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            Input();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void Input()
        {
            state = Mouse.GetState();
            if (previousState.LeftButton == ButtonState.Pressed &&
                state.LeftButton == ButtonState.Released)
            {
            }
            previousState = state;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
