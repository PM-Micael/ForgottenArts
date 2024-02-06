using Humanizer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.WorldBuilding;

public class MyModdedItemDungeonChest : ILoadable
{
    public void Load(Mod mod)
    {
        // Subscribe to a world generation hook
        WorldGenHooks.ChestItemInsertion += AddMyItemToDungeonChests;
    }

    public void Unload()
    {
        // Clean up by unsubscribing from the hook when the mod is unloaded
        WorldGenHooks.ChestItemInsertion -= AddMyItemToDungeonChests;
    }

    private void AddMyItemToDungeonChests(On.Chest.orig_SetupShop orig, Chest chest, int type)
    {
        // Original method call
        orig(chest, type);

        // Check if the chest is a dungeon chest by type
        // Note: You may need to adjust the chest type check based on your needs and the version of tModLoader
        if (type == ChestID.DungeonChest)
        {
            // Define the item and its spawn chance
            int itemToAdd = ModContent.ItemType<YourModdedItem>(); // Replace YourModdedItem with your item's class name
            int spawnChance = 5; // Example: 20% chance to add the item

            // Randomly decide whether to add the item based on the spawn chance
            if (Main.rand.Next(100) < spawnChance)
            {
                // Find the first empty slot in the chest
                for (int i = 0; i < Chest.maxItems; i++)
                {
                    if (chest.item[i].type == ItemID.None || chest.item[i].stack == 0)
                    {
                        chest.item[i].SetDefaults(itemToAdd);
                        chest.item[i].stack = 1; // Set the item stack size if necessary
                        break; // Exit the loop once the item is added
                    }
                }
            }
        }
    }
}
