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
    class NouvellePartieScreen : MenuScreen
    {
        bool _erreur;
        SpriteFont transition_font;
        string ErrorString, partiesimple, tournoi, loadingpartie, retour;
        string menu, manche;
        Languages lang = new Languages();
        Langues langue = new Langues();

        public NouvellePartieScreen(string nouvellepartie)
            : base(nouvellepartie)
        {    
        }

        public NouvellePartieScreen(bool erreur, string nouvellepartie)
            : base(nouvellepartie)
        {
            _erreur = erreur;
        }

        private void InitilizeLanguages()
        {
            ErrorString = lang.AffectationLANG("Nosave", langue);
            partiesimple = lang.AffectationLANG("partieSimple", langue);
            tournoi = lang.AffectationLANG("tournoi", langue);
            loadingpartie = lang.AffectationLANG("Load", langue);
            menu = lang.AffectationLANG("Menu", langue);
            manche = lang.AffectationLANG("Manches", langue);
            retour = lang.AffectationLANG("Retour", langue);


            MenuEntry a = new MenuEntry(partiesimple);
            MenuEntry b = new MenuEntry(tournoi);
            MenuEntry c = new MenuEntry(loadingpartie);
            MenuEntry d = new MenuEntry(retour.ToUpper());

            a.Selected += StartPartieEntrySelected;
            b.Selected += StartTournoiEntrySelected;
            c.Selected += StartChargerEntrySelected;
            d.Selected += MenuEntrySelected;

            MenuEntries.Add(a);
            MenuEntries.Add(b);
            MenuEntries.Add(c);
            MenuEntries.Add(d);
        }

        void StartPartieEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
           ScreenManager.AddScreen(new NombreDeManche(1,manche));
        }

        void StartTournoiEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();

            ScreenManager.AddScreen(new NombreDeManche(2, manche));
        }
        void StartChargerEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new DebutMatch(3, false));
        }
        void MenuEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen(menu));
        }

        protected override void UpdateMenuEntryLocations()
        {
            base.UpdateMenuEntryLocations();

            foreach (var entry in MenuEntries)
            {
                var position = entry.Position;
                position.Y += 60;
                entry.Position = position;
            }
        }

        public override void LoadContent()
        {
            transition_font = ScreenManager.Game.Content.Load<SpriteFont>("Transition");
            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();
            base.LoadContent();
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen(menu));
                
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            if (_erreur) { ScreenManager.SpriteBatch.DrawString(transition_font, ErrorString, new Vector2(480 / 2 - transition_font.MeasureString(ErrorString).X / 2, 550), Color.White); }
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
    
    }
}
