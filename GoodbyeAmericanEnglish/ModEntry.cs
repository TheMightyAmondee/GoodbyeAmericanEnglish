using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace GoodbyeAmericanEnglish
{ 
    public class ModEntry
        : Mod, IAssetEditor
    {
        // Array to hold NPC names
        private static string[] NPCs = 
        {
            "Abigail",
            "Alex",
            "Caroline",
            "Clint",
            "Demetrius",
            "Dwarf",
            "Elliot",
            "Emily",
            "George",
            "Gus",
            "Haley",
            "Harvey",
            "Jas",
            "Jodi",
            "Kent",
            "Krobus",
            "Leah",
            "Lewis",
            "Linus",
            "Marnie",
            "Maru",
            "Mister Qi",
            "Pam",
            "Penny",
            "Pierre",
            "Robin",
            "Sam",
            "Sandy",
            "Shane",
            "Sebastian",
            "Vincent",
            "Willy",
            "Wizard"
        };

        // Array to hold location names
        private static string[] locations =
        {
            "AbandonedJojaMart",
            "AnimalShop",
            "ArchaeologyHouse",
            "BackWoods",
            "BathHouse_Pool",
            "Beach",
            "BusStop",
            "CommunityCenter",
            "ElliottHouse",
            "Farm",
            "FarmHouse",
            "Forest",
            "HaleyHouse",
            "HarveyRoom",
            "Hospital",
            "JoshHouse",
            "LeahHouse",
            "ManorHouse",
            "Mine",
            "Mountain",
            "Railroad",
            "Saloon",
            "SamHouse",
            "SandyHouse",
            "ScienceHouse",
            "SebastianRoom",
            "SeedShop",
            "Sewer",
            "SunRoom",
            "Temp",
            "Tent",
            "Town",
            "Trailer",
            "Trailer_Big",
            "WizardHouse",
            "Woods"
        };

        public override void Entry(IModHelper helper)
        {

        }
        
        // Return true if an asset name matches
        public bool CanEdit<T>(IAssetInfo asset)
        {
            
            foreach(var name in NPCs)
            {
                if (false
                    || asset.AssetNameEquals($"Characters\\Dialogue\\Marriage{name}")
                    || asset.AssetNameEquals($"Characters\\Dialogue\\{name}")
                    || asset.AssetNameEquals($"Strings\\schedules\\{name}"))
                {
                    return true;
                }
            }

            foreach (var location in locations)
            {
                if (false
                    || asset.AssetNameEquals($"Data\\Events\\{location}"))
                {
                    return true;
                }
            }

            // return true if assest name is ANY of the following....
            return (false
                    || asset.AssetNameEquals("Strings\\StringsFromCSFiles")
                    || asset.AssetNameEquals("Strings\\UI")
                    || asset.AssetNameEquals("Strings\\Locations")
                    || asset.AssetNameEquals("Strings\\StringsFromMaps")
                    || asset.AssetNameEquals("Strings\\Notes")
                    || asset.AssetNameEquals("Strings\\Characters")
                    || asset.AssetNameEquals("Data\\ObjectInformation")
                    || asset.AssetNameEquals("Data\\TV\\TipChannel")
                    || asset.AssetNameEquals("Data\\TV\\CookingChannel")
                    || asset.AssetNameEquals("Data\\mail")
                    || asset.AssetNameEquals("Data\\ClothingInformation")
                    || asset.AssetNameEquals("Data\\ExtraDialogue")
                    || asset.AssetNameEquals("Data\\SecretNotes")
                    || asset.AssetNameEquals("Data\\Festivals\\spring13")
                    || asset.AssetNameEquals("Data\\Festivals\\summer11")
                    || asset.AssetNameEquals("Data\\Festivals\\summer28")
                    || asset.AssetNameEquals("Data\\Festivals\\fall27")
                    || asset.AssetNameEquals("Data\\Festivals\\winter25")
                    || asset.AssetNameEquals("Minigames\\Intro")
                    || asset.AssetNameEquals("Data\\BigCraftablesInformation")
                    || asset.AssetNameEquals("Characters\\Dialogue\\MarriageDialogue"));
        }
        // Edit game assets
        public void Edit<T>(IAssetData asset)
        {
            void SpellingFixer()
            {
                var data = asset.AsDictionary<string, string>().Data;

                foreach (string key in new List<string>(data.Keys))
                {
                    if (false 
                        || data[key].Contains("fall on") 
                        || data[key].Contains("fall_") 
                        || data[key].Contains("citizen") 
                        || data[key].Contains("size") 
                        || data[key].Contains("moment") 
                        || data[key].Contains("[color") 
                        || data[key].Contains("bgColor") 
                        || data[key].Contains("Prize") 
                        || data[key].Contains("prize")
                        || data[key].Contains("_apologize") 
                        || data[key].Contains("JoshMom")
                        || data[key].Contains("fallFest"))
                    {
                        continue;
                    }

                    data[key] = data[key].Replace("the fall", "autumn");
                    data[key] = data[key].Replace("color", "colour");
                    data[key] = data[key].Replace("favorite", "favourite");
                    data[key] = data[key].Replace("Favorite", "Favourite");
                    data[key] = data[key].Replace("behavior", "behaviour");
                    data[key] = data[key].Replace("fall", "autumn");
                    data[key] = data[key].Replace("Fall", "Autumn");
                    data[key] = data[key].Replace("ize", "ise");
                    data[key] = data[key].Replace("Center", "Centre");
                    data[key] = data[key].Replace("miles", "kilometres");
                    data[key] = data[key].Replace("mom", "mum");
                    data[key] = data[key].Replace("Mom", "Mum");
                    data[key] = data[key].Replace("six inches", "fifteen centimetres");
                    data[key] = data[key].Replace("center", "centre");
                    data[key] = data[key].Replace("{0} in.", "{0} cm.");
                    data[key] = data[key].Replace("inches", "centimetres");
                    data[key] = data[key].Replace("theater", "theatre");
                    data[key] = data[key].Replace("Theater", "Theatre");
                    data[key] = data[key].Replace("flavor", "flavour");
                    data[key] = data[key].Replace("counselor", "counsellor");
                }
            }

            // Edit character dialogue
            foreach (string name in NPCs)
            {
                if (asset.AssetNameEquals($"Characters\\Dialogue\\{name}"))
                {
                    SpellingFixer();
                }

                // Edit character specific marriage dialogue
                else if (asset.AssetNameEquals($"Characters\\Dialogue\\MarriageDialogue{name}"))
                {
                    SpellingFixer();
                }

                // Edit schedule dialogue
                else if (asset.AssetNameEquals($"Strings\\schedules\\{name}"))
                {
                    SpellingFixer();
                }
            }
           
            // Edit events
            foreach (string location in locations)
            {
                if (asset.AssetNameEquals($"Data\\Events\\{location}"))
                {
                    SpellingFixer();
                }
            }

            // Edit strings
            if (asset.AssetNameEquals("Strings\\StringsFromCSFiles"))
            {
                SpellingFixer();
            }

            // Edit general marriage dialogue
            else if (asset.AssetNameEquals($"Characters\\Dialogue\\MarriageDialogue"))
            {
                SpellingFixer();
            }

            else if (asset.AssetNameEquals("Strings\\UI"))
            {
                SpellingFixer();
            }

            else if (asset.AssetNameEquals("Strings\\Location"))
            {
                SpellingFixer();
            }

            else if (asset.AssetNameEquals("Strings\\StringsFromMaps"))
            {
                var data = asset.AsDictionary<string, string>().Data;

                foreach (string key in new List<string>(data.Keys))
                {

                    data[key] = data[key].Replace("color", "colour");
                    data[key] = data[key].Replace(" Mom", " Mum");
                }
            }

            else if (asset.AssetNameEquals("Strings\\Notes"))
            {
                var data = asset.AsDictionary<string, string>().Data;

                foreach (string key in new List<string>(data.Keys))
                {
                    if (data[key].Contains("prize"))
                    {
                        continue;
                    }

                    data[key] = data[key].Replace("ize", "ise");
                }
            }

            else if (asset.AssetNameEquals("Strings\\Characters"))
            {
                SpellingFixer();
            }

            // Edit data
            else if (asset.AssetNameEquals("Data\\ExtraDialogue"))
            {
                var data = asset.AsDictionary<string, string>().Data;

                foreach (string key in new List<string>(data.Keys))
                {

                    data[key] = data[key].Replace("color", "colour");
                    data[key] = data[key].Replace("favorite", "favourite");
                    data[key] = data[key].Replace(" mom ", " mum ");
                }
            }

            else if (asset.AssetNameEquals("Data\\SecretNotes"))
            {
                IDictionary<int, string> data = asset.AsDictionary<int, string>().Data;

                foreach (int key in new List<int>(data.Keys))
                {
                    data[key] = data[key].Replace("favorite", "favourite");
                    data[key] = data[key].Replace("Mom", "Mum");
                }
            }



            else if (asset.AssetNameEquals("Data\\ClothingInformation"))
            {
                IDictionary<int, string> data = asset.AsDictionary<int, string>().Data;

                foreach (int key in new List<int>(data.Keys))
                {

                    data[key] = data[key].Replace("olor", "olour");
                }
            }

            else if (asset.AssetNameEquals("Data\\ObjectInformation"))
            {
                IDictionary<int, string> data = asset.AsDictionary<int, string>().Data;
                foreach (int key in new List<int>(data.Keys))
                {
                    if (data[key].Contains("falling") || data[key].Contains("size") || data[key].Contains("rized") || data[key].Contains("denizen"))
                    {
                        continue;
                    }

                    data[key] = data[key].Replace("the fall", "autumn");
                    data[key] = data[key].Replace("A fall", "An autumn");
                    data[key] = data[key].Replace("fall", "autumn");
                    data[key] = data[key].Replace("color", "colour");
                    data[key] = data[key].Replace("favorite", "favourite");
                    data[key] = data[key].Replace("ize", "ise");
                    data[key] = data[key].Replace("theater", "theatre");

                    if (key == 497)
                    {
                        data[key] = data[key].Replace("Fall", "Autumn");
                    }
                }
            }

            // Edit other data
            else if (asset.AssetNameEquals("Data\\TV\\TipChannel"))
            {
                var data = asset.AsDictionary<string, string>().Data;
                data["53"] = data["53"].Replace("Fall", "Autumn");

                foreach (string key in new List<string>(data.Keys) { "36", "67", "78", "116", "186", "102" })
                {
                    data[key] = data[key].Replace("fall", "autumn");
                    data[key] = data[key].Replace("favorite", "favourite");
                }
            }

            else if (asset.AssetNameEquals("Data\\TV\\CookingChannel"))
            {
                var data = asset.AsDictionary<string, string>().Data;

                foreach (string key in new List<string>(data.Keys) { "18", "27", "31", "32" })
                {
                    data[key] = data[key].Replace("favorite", "favourite");
                    data[key] = data[key].Replace("ize", "ise");
                }
            }

            else if (asset.AssetNameEquals("Data\\mail"))
            {
                SpellingFixer();
            }

            // Festivals
            else if (asset.AssetNameEquals("Data\\Festivals\\spring13"))
            {
                SpellingFixer();
            }

            else if (asset.AssetNameEquals("Data\\Festivals\\summer11"))
            {
                SpellingFixer();
            }

            else if (asset.AssetNameEquals("Data\\Festivals\\summer28"))
            {
                SpellingFixer();
            }

            else if (asset.AssetNameEquals("Data\\Festivals\\fall27"))
            {
                SpellingFixer();
            }

            else if (asset.AssetNameEquals("Data\\Festivals\\winter25"))
            {
                SpellingFixer();
            }

            else if (asset.AssetNameEquals("Data\\BigCraftablesInformation"))
            {
                IDictionary<int, string> data = asset.AsDictionary<int, string>().Data;

                data[209] = "Mini-Jukebox/1500/-300/Crafting -9/Allows you to play your favourite tunes./true/true/0/Mini-Jukebox";
            }

            else if (asset.AssetNameEquals("Minigames\\Intro"))
            {
                var editor = asset.AsImage();

                Texture2D roadsign = Helper.Content.Load<Texture2D>("assets/Intro_sign.png", ContentSource.ModFolder);

                editor.PatchImage(roadsign, targetArea: new Rectangle(48, 177, 64, 80));
            }
        }
    }
}

