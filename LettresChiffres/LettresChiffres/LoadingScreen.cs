using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XMLObject;

namespace LettresChiffres
{
    class LoadingScreen : GameScreen
    {
        private Thread backgroundThread;
        Texture2D _loading2D;

        int _frame = 0;
        int _nombre_frame = 7;
        int _loadingWidth = 64;
        float _timeframe = 200;
        float _timeloading;

        string path, menu;
        Languages lang = new Languages();
        Langues langue = new Langues();

        public LoadingScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.0);
        }

        void BackgroundLoadContent()
        {
            ScreenManager.Game.Content.Load<object>("background");
            ScreenManager.Game.Content.Load<object>("menufont");
            ScreenManager.Game.Content.Load<object>("chiffre_font");
            ScreenManager.Game.Content.Load<object>("Chiffre/chiffres_carre");
            ScreenManager.Game.Content.Load<object>("Chiffre/chiffres_carre_but");
            ScreenManager.Game.Content.Load<object>("Chiffre/diviser");
            ScreenManager.Game.Content.Load<object>("Chiffre/moins");
            ScreenManager.Game.Content.Load<object>("Chiffre/multiplier");
            ScreenManager.Game.Content.Load<object>("Chiffre/plus");
            ScreenManager.Game.Content.Load<object>(lang.path + "dico10");
            ScreenManager.Game.Content.Load<object>(lang.path + "dico9");
            ScreenManager.Game.Content.Load<object>(lang.path + "dico8");
            ScreenManager.Game.Content.Load<object>(lang.path + "dico7");
            ScreenManager.Game.Content.Load<object>(lang.path + "dico6");
            ScreenManager.Game.Content.Load<object>(lang.path + "dico5");
            ScreenManager.Game.Content.Load<object>(lang.path + "dico4");

        }

        public override void LoadContent()
        {
            _loading2D = ScreenManager.Game.Content.Load<Texture2D>("loading");

            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitializeLanguage();

            if (backgroundThread == null)
            {
                backgroundThread = new Thread(BackgroundLoadContent);
                backgroundThread.Start();
            }
            base.LoadContent();
        }

        private void InitializeLanguage()
        {
            menu = lang.AffectationLANG("Menu",langue);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _timeloading += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Loading();

            if (backgroundThread != null && backgroundThread.Join(10))
            {
                backgroundThread = null;
                this.ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen(menu));
                ScreenManager.Game.ResetElapsedTime();
            }
            
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void Loading()
        {
            if (_timeloading > _timeframe)
            {
                if (_frame == _nombre_frame)
                {
                    _frame = 0;
                }
                else
                {
                    _frame++;
                }
                _timeloading = 0;
            }
        }


        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            LoadingDraw();

            ScreenManager.SpriteBatch.End();
        }

        private void LoadingDraw()
        {
            int _afficherWidth = _frame * _loadingWidth;
            Rectangle source = new Rectangle(_afficherWidth, 0, _loadingWidth, _loading2D.Height);
            ScreenManager.SpriteBatch.Draw(_loading2D, new Vector2(210, 300), source, Color.White);
        }
    }
}
