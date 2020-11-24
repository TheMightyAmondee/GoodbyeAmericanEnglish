﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace GoodbyeAmericanEnglish
{ 
    public class ModEntry
        : Mod, IAssetEditor
    {
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
                    || asset.AssetNameEquals("Data\\ObjectInformation")
                    || asset.AssetNameEquals("Data\\TV\\TipChannel")
                    || asset.AssetNameEquals("Data\\TV\\CookingChannel")
                    || asset.AssetNameEquals("Data\\Fish")
                    || asset.AssetNameEquals("Characters\\Dialogue\\MarriageDialogue")
                    || asset.AssetNameEquals("Characters\\Dialogue\\rainy"));
        }

        public void Edit<T>(IAssetData asset)
        {
            foreach(string name in NPCs)
            {
                if (asset.AssetNameEquals($"Characters\\Dialogue\\{name}"))
                {
                    var data = asset.AsDictionary<string, string>().Data;

                    foreach (string key in new List<string>(data.Keys))
                    {
                        if (data[key].Contains("fall on") || data[key].Contains("fall_") || data[key].Contains("citizen") || data[key].Contains("size"))
                        {
                            continue;
                        }

                        data[key] = data[key].Replace("the fall", "autumn");
                        data[key] = data[key].Replace("color", "colour");
                        data[key] = data[key].Replace("favorite", "favourite");
                        data[key] = data[key].Replace("fall", "autumn");
                        data[key] = data[key].Replace("Fall", "Autumn");
                        data[key] = data[key].Replace("ize", "ise");
                        data[key] = data[key].Replace("miles", "kilometres");
                    }
                }

                else if (asset.AssetNameEquals($"Characters\\Dialogue\\MarriageDialogue{name}"))
                {
                    var data = asset.AsDictionary<string, string>().Data;

                    foreach (string key in new List<string>(data.Keys))
                    {
                        if (data[key].Contains("fall_"))
                        {
                            continue;
                        }

                        data[key] = data[key].Replace("the fall", "autumn");
                        data[key] = data[key].Replace("color", "colour");
                        data[key] = data[key].Replace("favorite", "favourite");
                        data[key] = data[key].Replace("fall", "autumn");
                        data[key] = data[key].Replace("Fall", "Autumn");
                        data[key] = data[key].Replace("ize", "ise");
                        data[key] = data[key].Replace("six inches", "fifteen centimetres");
                    }
                }

                else if (asset.AssetNameEquals($"Strings\\schedules\\{name}"))
                {
                    var data = asset.AsDictionary<string, string>().Data;

                    foreach (string key in new List<string>(data.Keys))
                    {
                        data[key] = data[key].Replace("favorite", "favourite");
                    }
                }
            }

            foreach(string location in locations)
            {
                if (asset.AssetNameEquals($"Data\\Events\\{location}"))
                {
                    var data = asset.AsDictionary<string, string>().Data;

                    foreach (string key in new List<string>(data.Keys))
                    {
                        if (data[key].Contains("bgColor") || data[key].Contains("Prize") || data[key].Contains("prize") || data[key].Contains("aplogize"))
                        {
                            continue;
                        }
                        data[key] = data[key].Replace("color", "colour");
                        data[key] = data[key].Replace("Color", "Colour");
                        data[key] = data[key].Replace("favorite", "favourite");
                        data[key] = data[key].Replace("ize", "ise");
                    }
                }
            }

            if (asset.AssetNameEquals("Strings\\StringsFromCSFiles"))
            {
                var data = asset.AsDictionary<string, string>().Data;

                foreach (string key in new List<string>(data.Keys))
                {
                    if (data[key].Contains("prize") || data[key].Contains("size"))
                    {
                        continue;
                    }
                    data[key] = data[key].Replace("the fall", "autumn");
                    data[key] = data[key].Replace("color", "colour");
                    data[key] = data[key].Replace("favorite", "favourite");
                    data[key] = data[key].Replace("Favorite", "Favourite");
                    data[key] = data[key].Replace("fall", "autumn");
                    data[key] = data[key].Replace("Fall", "Autumn");
                    data[key] = data[key].Replace("ize", "ise");
                    data[key] = data[key].Replace("{0} in.", "{0} cm.");
                    data[key] = data[key].Replace("inches", "centimetres");
                }
            }

            else if (asset.AssetNameEquals("Strings\\UI"))
            {
                var data = asset.AsDictionary<string, string>().Data;

                foreach (string key in new List<string>(data.Keys))
                {
                    if (data[key].Contains("[color"))
                    {
                        continue;
                    }
                    data[key] = data[key].Replace("color", "colour");
                    data[key] = data[key].Replace("Color", "Colour");
                    data[key] = data[key].Replace("favorite", "favourite");
                    data[key] = data[key].Replace("Favorite", "Favourite");
                    data[key] = data[key].Replace("ize", "ise");
                    data[key] = data[key].Replace("{0} in.", "{0} cm.");
                }
            }

            else if (asset.AssetNameEquals("Data\\ObjectInformation"))
            {
                IDictionary<int, string> data = asset.AsDictionary<int, string>().Data;
                foreach (int id in new List<int>(data.Keys))
                {
                    string[] array = data[id].Split('/');
                    string descr = array[5];
                    if (descr.Contains("falling"))
                    {
                        continue;
                    }
                    
                    data[id] = data[id].Replace("the fall", "autumn");
                    data[id] = data[id].Replace("fall", "autumn");
                    data[id] = data[id].Replace("color", "colour");
                    data[id] = data[id].Replace("favorite", "favourite");

                    if (id == 497)
                    {
                        data[id] = data[id].Replace("Fall", "Autumn");
                    }
                }
            }
            

            else if (asset.AssetNameEquals($"Characters\\Dialogue\\MarriageDialogue"))
            {
                var data = asset.AsDictionary<string, string>().Data;

                foreach (string key in new List<string>(data.Keys))
                {
                    if (data[key].Contains("fall_"))
                    {
                        continue;
                    }

                    data[key] = data[key].Replace("the fall", "autumn");
                    data[key] = data[key].Replace("color", "colour");
                    data[key] = data[key].Replace("favorite", "favourite");
                    data[key] = data[key].Replace("fall", "autumn");
                    data[key] = data[key].Replace("Fall", "Autumn");
                }
            }

            else if (asset.AssetNameEquals($"Characters\\Dialogue\\rainy"))
            {
                var data = asset.AsDictionary<string, string>().Data;

                foreach (string key in new List<string>(data.Keys))
                {
                    if (data[key].Contains("fall_"))
                    {
                        continue;
                    }

                    data[key] = data[key].Replace("color", "colour");
                    data[key] = data[key].Replace("fall", "autumn");
                    data[key] = data[key].Replace("Fall", "Autumn");
                }
            }

            

            else if (asset.AssetNameEquals("Data\\TV\\TipChannel"))
            {
                var data = asset.AsDictionary<string, string>().Data;
                data["53"] = data["53"].Replace("Fall", "Autumn");

                foreach (string Itemid in new List<string>() { "36", "67", "78", "116", "186", "102" })
                {
                    data[Itemid] = data[Itemid].Replace("fall", "autumn");
                    data[Itemid] = data[Itemid].Replace("favorite", "favourite");
                }
            }

            else if (asset.AssetNameEquals("Data\\TV\\CookingChannel"))
            {
                var data = asset.AsDictionary<string, string>().Data;
                data["53"] = data["53"].Replace("Fall", "Autumn");

                foreach (string Itemid in new List<string>() { "18","27" })
                {
                    data[Itemid] = data[Itemid].Replace("favorite", "favourite");
                }
            }

            else if (asset.AssetNameEquals("Data\\Fish"))
            {

                var data = asset.AsDictionary<int, string>().Data;

                foreach (int Itemid in new List<int>())
                {
                    string[] fields = data[Itemid].Split('/');
                    fields[3] = ((int)Math.Round(int.Parse(fields[3]) * 2.5)).ToString();
                    fields[4] = ((int)Math.Round(int.Parse(fields[4]) * 2.5)).ToString();
                    data[Itemid] = string.Join("/", fields);
                }
            }
        }
    }
}

