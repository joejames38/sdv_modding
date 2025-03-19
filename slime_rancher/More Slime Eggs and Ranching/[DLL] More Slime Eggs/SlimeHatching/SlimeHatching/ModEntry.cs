using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Monsters;
using Microsoft.Xna.Framework;
using StardewValley.Buildings;

namespace GoldSlimeMod
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            // Subscribe to the DayStarted event to perform checks at the start of each in-game day
            helper.Events.GameLoop.DayStarted += GameLoop_DayStarted1;
        }

        private void GameLoop_DayStarted1(object? sender, DayStartedEventArgs e)
        {
            if (!Context.IsWorldReady) return;

            foreach (var building in Game1.getFarm().buildings)
            {
                Monitor.Log($"Building type found: {building.GetType().FullName}", LogLevel.Debug);
            }

            foreach (var building in Game1.getFarm().buildings)
            {
                if (building.buildingType.Value == "Slime Hutch") // Check the building type
                {
                    CheckForHatchedSlimes(building);
                }
            }
        }

        private void CheckForHatchedSlimes(Building building)
        {
            // Ensure we're only working with Slime Hutch buildings
            if (building.buildingType.Value != "Slime Hutch") return;

            // Check for incubators inside the building's objects
            foreach (var obj in building.indoors.Value.objects.Values) // Access 'indoors' safely
            {
                if (obj is StardewValley.Object incubator && incubator.Name == "Slime Incubator")
                {
                    if (incubator.heldObject.Value != null && incubator.MinutesUntilReady <= 0)
                    {
                        if (incubator.heldObject.Value.Name == "GoldSlimeEgg")
                        {
                            SpawnGoldSlime(building, incubator.TileLocation);
                            incubator.heldObject.Value = null; // Clear the incubator
                        }
                    }
                }
            }
        }

        private void SpawnGoldSlime(Building building, Vector2 location)
        {
            if (building is not null && building.indoors.Value is GameLocation indoors)
            {
                indoors.spawnMonster(
                    "Green Slime", // Monster type
                    (int)location.X, (int)location.Y, // Tile location
                    1 // Number of slimes to spawn
                );

                //// Modify the newly spawned slimes
                //foreach (var character in indoors.characters)
                //{
                //    if (character is GreenSlime slime && slime.Position == location * 64f)
                //    {
                //        slime.Name = "Gold Slime";
                //        slime.Sprite.Color = new Color(255, 215, 0); // Gold color
                //        slime.DamageToFarmer = 10;
                //        slime.Health = 100;
                //        break; // Modify only the first matching slime
                //    }
                //}
            }
        }
    }
}
