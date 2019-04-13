using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using GameStateManagement;
using Microsoft.Advertising.Mobile.Xna;
using System.Globalization;

namespace LettresChiffres
{
    public class LettresChiffres : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        public const string GameStateKey = "GameState";
        public const string InGameKey = "InGame";

        DrawableAd bannerAd;
        private static readonly string ApplicationId = "e9193919-0227-499f-ae74-d7ed5ab44308";
        private static readonly string AdUnitId = "137706"; //other test values: Image480_80, Image300_50, TextAd

        public LettresChiffres()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;

            Content.RootDirectory = "Content";

            // La fréquence d’image est de 30 i/s pour le Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Augmenter la durée de la batterie sous verrouillage.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new LoadingScreen());

            Languages lang = new Languages();
            
        }

        protected override void Initialize()
        {
            AdGameComponent.Initialize(this, ApplicationId);
            Components.Add(AdGameComponent.Current);
            CreateAd();
            base.Initialize();
        }
        private void CreateAd()
        {
            int width = 480;
            int height = 80;
            int x = (GraphicsDevice.Viewport.Bounds.Width - width) / 2; // centered on the display
            int y = 720;

            bannerAd = AdGameComponent.Current.CreateAd(AdUnitId, new Rectangle(x, y, width, height), true);

        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            base.Draw(gameTime);
        }
    }
}
