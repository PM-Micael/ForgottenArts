using ForgottenArts.Buffs.BaseBuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ForgottenArts.UI
{
    internal class HeavyUI : ModSystem
    {
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            PlayerClass playerClass = player.GetModPlayer<PlayerClass>();

            Vector2 endurencePosition = new Vector2(20, 120);
            string endurence = $"Enducence: {playerClass.EndurenceCurrent}/{playerClass.EndurenceMax}";

            Vector2 testStacksPosition = new Vector2(20, 140);
            string testStackString = $"Block Multiplier: {playerClass.BlockMultiplier}";

            Vector2 endurenceRegenVector = new Vector2(20, 160);
            string endurenceRegenText = $"Endurence Regeneration: {playerClass.EndurenceRegenRate}";

            Color textColor = Color.Red;

            if(playerClass.EndurenceCurrent == playerClass.EndurenceMax)
            {
                textColor = Color.Green;
            }
            else if(playerClass.EndurenceCurrent > (playerClass.EndurenceMax * 0.66))
            {
                textColor = Color.Yellow;
            }
            else if(playerClass.EndurenceCurrent > (playerClass.EndurenceMax * 0.33))
            {
                textColor = Color.DarkOrange;
            }
            else
            {
                textColor = Color.Red;
            }

            // Draw text
            Utils.DrawBorderString(spriteBatch, endurence, endurencePosition, textColor);
            Utils.DrawBorderString(spriteBatch, testStackString, testStacksPosition, Color.Yellow);
            Utils.DrawBorderString(spriteBatch, endurenceRegenText, endurenceRegenVector, Color.Pink);
        }
    }
}
