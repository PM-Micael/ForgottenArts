using ForgottenArts.Buffs.BaseBuffs;
using ForgottenArts.Items.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenArts.Other
{
    internal class ChestSpawn : ModSystem
    {
        public override void PostWorldGen()
        {
            int itemsToPlaceInChestChoice = 0;

            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                int[] itemsToPlaceInChest = { Mod.Find<ModItem>("Handelnight").Type };

                if(chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 2 * 36)
                {
                    Random r = new Random();
                    int num = r.Next(1, 3);
                    if(num == 1)
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == 0)
                            {
                                chest.item[inventoryIndex].SetDefaults(itemsToPlaceInChest[itemsToPlaceInChestChoice]);
                                chest.item[inventoryIndex].stack = 1;
                                itemsToPlaceInChestChoice = (itemsToPlaceInChestChoice + 1) % itemsToPlaceInChest.Length;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
