﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using HarmonyLib;
using StardewModdingAPI.Events;
using System.Linq;
using Netcode;

namespace GoodbyeAmericanEnglish
{
    public class ModEntry
        : Mod
    {
        private ModConfig config;
        private static IModHelper helperstatic;
        private static IMonitor monitorstatic;
        private static Dictionary<string, string> namereplacer = new Dictionary<string, string>();

        // Array to hold NPC names
        private static readonly string[] NPCs =
        {
            "Abigail",
            "Alex",
            "Caroline",
            "Clint",
            "Demetrius",
            "Dwarf",
            "Elliott",
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
            "Leo",
            "LeoMainland",
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
        private static readonly string[] locations =
        {
            "AbandonedJojaMart",
            "AnimalShop",
            "ArchaeologyHouse",
            "BackWoods",
            "BathHouse_Pool",
            "Beach",
            "BoatTunnel",
            "BusStop",
            "CommunityCenter",
            "ElliottHouse",
            "Farm",
            "FarmHouse",
            "FishShop",
            "Forest",
            "HaleyHouse",
            "HarveyRoom",
            "Hospital",
            "IslandFarmHouse",
            "IslandHut",
            "IslandNorth",
            "IslandSouth",
            "IslandWest",
            "JoshHouse",
            "LeahHouse",
            "ManorHouse",
            "Mine",
            "Mountain",
            "QiNutRoom",
            "Railroad",
            "Saloon",
            "SamHouse",
            "SandyHouse",
            "ScienceHouse",
            "SebastianRoom",
            "SeedShop",
            "Sewer",
            "Summit",
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
            this.config = this.Helper.ReadConfig<ModConfig>();
            helperstatic = this.Helper;
            monitorstatic = this.Monitor;
            

            var replacer = this.Helper.Data.ReadJsonFile<NameReplacer>("NameReplacer.json");

            if (replacer == null)
            {
                replacer = new NameReplacer();
                this.Helper.Data.WriteJsonFile("NameReplacer.json", replacer);
            }
            else
            {
                namereplacer = this.Helper.ModContent.Load<Dictionary<string, string>>("NameReplacer.json") ?? null;
            }
            
            helper.Events.Content.AssetRequested += this.AssetRequested;

            if (this.config.AllowAdvancedNameReplacer == true)
            {
                var harmony = new Harmony(this.ModManifest.UniqueID);

                harmony.Patch(
                    original: AccessTools.PropertyGetter(typeof(StardewValley.Object), nameof(StardewValley.Object.DisplayName)),
                    postfix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.DisplayName_Postfix))
                    );
            }
        }

        private static string PreservestringFromEnum(StardewValley.Object.PreserveType preservetype)
        {
            switch (preservetype)
            {
                case StardewValley.Object.PreserveType.Wine:
                    return "Wine";
                case StardewValley.Object.PreserveType.Jelly:
                    return "Jelly";
                case StardewValley.Object.PreserveType.Pickle:
                    return "Pickles";
                case StardewValley.Object.PreserveType.Juice:
                    return "Juice";
                case StardewValley.Object.PreserveType.Roe:
                    return "Roe";
                case StardewValley.Object.PreserveType.DriedFruit:
                    return "DriedFruit";
                case StardewValley.Object.PreserveType.DriedMushroom:
                    return "DriedMushroom";
                case StardewValley.Object.PreserveType.SmokedFish:
                    return "SmokedFish";
                case StardewValley.Object.PreserveType.Bait:
                    return "Bait";
                case StardewValley.Object.PreserveType.Honey:
                    return "Honey";
                default:
                    return "AgedRoe";
            }
        }

        private static void DisplayName_Postfix(StardewValley.Object __instance, ref string __result)
        {
            try
            {
                if (namereplacer != null && __instance.preserve.Value.HasValue == true)
                {
                    string preservedName = (__instance.preservedParentSheetIndex.Value != null) ? ItemRegistry.GetDataOrErrorItem("(O)" + __instance.preservedParentSheetIndex.Value).DisplayName : null;
                    if (preservedName != null && namereplacer.ContainsKey($"P_{"(O)" + __instance.preservedParentSheetIndex.Value}_{PreservestringFromEnum(__instance.preserve.Value.GetValueOrDefault())}") == true)
                    {
                        var itemidvalue = namereplacer[$"P_{"(O)" + __instance.preservedParentSheetIndex.Value}_{PreservestringFromEnum(__instance.preserve.Value.GetValueOrDefault())}"];
                        var newname = __instance.displayName;
                       
                        string nameextension = (__instance.IsRecipe ? (((CraftingRecipe.craftingRecipes.ContainsKey(__instance.displayName) && CraftingRecipe.craftingRecipes[__instance.displayName].Split('/')[2].Split(' ').Count() > 1) ? (" x" + CraftingRecipe.craftingRecipes[__instance.displayName].Split('/')[2].Split(' ')[1]) : "") + Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12657")) : "");
                        string nameprefix = __instance.orderData.Value == "QI_COOKING" ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Fresh_Prefix", __instance.displayName) : "";

                        string[] fields = itemidvalue.Split('/');

                        if (fields.Length == 2)
                        {
                            switch (fields[0])
                            {
                                case "suffix":
                                    newname = $"{preservedName} {fields[1]}";
                                    break;
                                case "prefix":
                                    newname = $"{fields[1]} {preservedName}";
                                    break;
                                case "replace":
                                default:
                                    newname = fields[1];
                                    break;
                            }
                            __result = nameprefix + newname + nameextension;
                        }
                    }                  
                }
            }
            catch (Exception ex)
            {
                monitorstatic.Log($"Failed to replace name. Details:\n{ex}", LogLevel.Error);
            }
           
        }
        private void AssetRequested(object sender, AssetRequestedEventArgs e)
        {

            bool IsEditableAsset()
            {
                foreach (var name in NPCs)
                {
                    // If asset name contains any iteration in NPCs array, return true
                    if (false
                        || e.NameWithoutLocale.IsEquivalentTo($"Characters\\Dialogue\\Marriage{name}")
                        || e.NameWithoutLocale.IsEquivalentTo($"Characters\\Dialogue\\{name}")
                        || e.NameWithoutLocale.IsEquivalentTo($"Strings\\schedules\\{name}"))
                    {
                        return true;
                    }
                }

                foreach (var location in locations)
                {
                    // If asset name contains any iteration in locations array, return true
                    if (false
                        || e.NameWithoutLocale.IsEquivalentTo($"Data\\Events\\{location}"))
                    {
                        return true;
                    }
                }

                // return true if assest name is ANY of the following....
                return (false
                        || e.NameWithoutLocale.IsEquivalentTo("Strings\\StringsFromCSFiles")
                        || e.NameWithoutLocale.IsEquivalentTo("Strings\\UI")
                        || e.NameWithoutLocale.IsEquivalentTo("Strings\\Locations")
                        || e.NameWithoutLocale.IsEquivalentTo("Strings\\StringsFromMaps")
                        || e.NameWithoutLocale.IsEquivalentTo("Strings\\Notes")
                        || e.NameWithoutLocale.IsEquivalentTo("Strings\\Characters")
                        || e.NameWithoutLocale.IsEquivalentTo("Strings\\SpecialOrderStrings")
                        || e.NameWithoutLocale.IsEquivalentTo("Strings\\Objects")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\TV\\TipChannel")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\TV\\CookingChannel")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\mail")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\ClothingInformation")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\ExtraDialogue")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\SecretNotes")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\NPCGiftTastes")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\Quests")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\Blueprints")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\Bundles")
                        || e.NameWithoutLocale.IsEquivalentTo("Strings\\Weapons")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\hats")//
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\Fish")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\RandomBundles")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\ObjectContextTags")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\Concessions")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\Movies")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\MoviesReactions")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\spring13")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\spring24")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\summer11")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\summer28")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\fall27")
                        || e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\winter25")
                        || e.NameWithoutLocale.IsEquivalentTo("Minigames\\Intro")
                        || e.NameWithoutLocale.IsEquivalentTo("Strings\\BigCraftables")
                        || e.NameWithoutLocale.IsEquivalentTo("Characters\\Dialogue\\MarriageDialogue")
                        );
            }

            if (IsEditableAsset() == true)
            {
                e.Edit(asset =>
                {
                    // Method to hold common word replacements and conditions
                    void SpellingFixer()
                    {
                        var data = asset.AsDictionary<string, string>().Data;

                        foreach (string key in new List<string>(data.Keys))
                        {
                            // Replace specified string with new string

                            data[key] = data[key].Replace("olor", "olour");
                            data[key] = data[key].Replace("behavior", "behaviour");
                            data[key] = data[key].Replace("ize", "ise");
                            data[key] = data[key].Replace("izing", "ising");
                            data[key] = data[key].Replace("zation", "sation");
                            data[key] = data[key].Replace(" center", " centre");
                            data[key] = data[key].Replace(" Center", " Centre");
                            data[key] = data[key].Replace("mom", "mum");
                            data[key] = data[key].Replace("Mom", "Mum");
                            data[key] = data[key].Replace("theater", "theatre");
                            data[key] = data[key].Replace("Theater", "Theatre");
                            data[key] = data[key].Replace("counselor", "counsellor");
                            data[key] = data[key].Replace("onor", "onour");
                            data[key] = data[key].Replace("umor", "umour");
                            data[key] = data[key].Replace("avor", "avour");
                            data[key] = data[key].Replace("eighbor", "eighbour");
                            data[key] = data[key].Replace("traveling", "travelling");
                            data[key] = data[key].Replace("travele", "travelle");
                            data[key] = data[key].Replace("cozy", "cosy");
                            data[key] = data[key].Replace("fiber", "fibre");
                            data[key] = data[key].Replace("efense", "efence");
                            data[key] = data[key].Replace("airplane", "aeroplane");


                            if (this.config.MetricSystem == true)
                            {
                                data[key] = data[key].Replace("twenty miles", "thirty kilometres");
                                data[key] = data[key].Replace("six inches", "fifteen centimetres");
                                data[key] = data[key].Replace("{0} in.", "{0} cm.");
                                data[key] = data[key].Replace("inches", "centimetres");
                            }

                            if (this.config.FalltoAutumn == true)
                            {
                                data[key] = data[key].Replace("the fall", "autumn");
                                data[key] = data[key].Replace("fall", "autumn");
                                data[key] = data[key].Replace("Fall", "Autumn");

                                data[key] = data[key].Replace("autumnen", "fallen");
                                data[key] = data[key].Replace("autumn out", "fall out");
                                data[key] = data[key].Replace("autumning", "falling");
                                data[key] = data[key].Replace("autumn on", "fall on");
                                data[key] = data[key].Replace("autumns", "falls");
                                data[key] = data[key].Replace("autumn asleep", "fall asleep");
                                data[key] = data[key].Replace("autumn prey", "fall prey");
                                data[key] = data[key].Replace("autumnFest", "fallFest");
                                data[key] = data[key].Replace("Autumn Of Planet", "Fall Of Planet");
                                data[key] = data[key].Replace("autumn_", "fall_");
                                data[key] = data[key].Replace("curtains autumn", "curtains fall");
                                data[key] = data[key].Replace("autumn for", "fall for");
                                data[key] = data[key].Replace("LinusAutumn", "LinusFall");
                            }

                            // Correct word replacement that shouldn't occur
                            data[key] = data[key].Replace("mument", "moment");
                            data[key] = data[key].Replace("JoshMum", "JoshMom");
                            data[key] = data[key].Replace(" sise", " size");
                            data[key] = data[key].Replace("citisen", "citizen");
                            data[key] = data[key].Replace("cardamum", "cardamom");
                            data[key] = data[key].Replace("bgColour", "bgColor");
                            data[key] = data[key].Replace("Prise", "Prize");
                            data[key] = data[key].Replace(" prise", " prize");
                            data[key] = data[key].Replace("_apologise", "_apologize");
                            data[key] = data[key].Replace("WildColour", "WildColor");
                            data[key] = data[key].Replace("honourary", "honorary");
                        }
                    }

                    foreach (string name in NPCs)
                    {
                        // Edit character specific dialogue
                        if (e.NameWithoutLocale.IsEquivalentTo($"Characters\\Dialogue\\{name}"))
                        {
                            SpellingFixer();
                        }

                        // Edit character specific marriage dialogue
                        else if (e.NameWithoutLocale.IsEquivalentTo($"Characters\\Dialogue\\MarriageDialogue{name}"))
                        {
                            SpellingFixer();
                        }

                        // Edit schedule dialogue
                        else if (e.NameWithoutLocale.IsEquivalentTo($"Strings\\schedules\\{name}"))
                        {
                            SpellingFixer();
                        }
                    }

                    // Edit events
                    foreach (string location in locations)
                    {
                        if (e.NameWithoutLocale.IsEquivalentTo($"Data\\Events\\{location}"))
                        {
                            SpellingFixer();
                        }
                    }

                    // Edit strings
                    if (e.NameWithoutLocale.IsEquivalentTo("Strings\\StringsFromCSFiles"))
                    {
                        SpellingFixer();
                        IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;

                        if (namereplacer != null)
                        {
                            foreach (string itemid in new List<string>(namereplacer.Keys))
                            {
                                string[] keyfields = itemid.Split('_');
                                string[] fields = namereplacer[itemid].Split('/');
                                if (fields.Length != 2)
                                {
                                    continue;
                                }

                                if (fields[0] != "prefix" && fields[0] != "suffix")
                                {
                                    continue;
                                }

                                if (keyfields[0] == "PP" && fields[0] == "suffix")
                                {
                                    switch (keyfields[1])
                                    {
                                        case "Juice":
                                            data["Object.cs.12726"] = data["Object.cs.12726"].Replace("Juice", $"{fields[1]}");
                                            break;
                                        case "Wine":
                                            data["Object.cs.12730"] = data["Object.cs.12730"].Replace("Wine", $"{fields[1]}");
                                            break;
                                        case "Pickles":
                                            data["Object.cs.12735"] = "{0} " + $"{fields[2]}";
                                            break;
                                        case "Jelly":
                                            data["Object.cs.12739"] = data["Object.cs.12739"].Replace("Jelly", $"{fields[1]}");
                                            break;
                                        case "Wild Honey":
                                            data["Object.cs.12750"] = data["Object.cs.12750"].Replace("Wild Honey", $"{fields[1]}");
                                            break;
                                        case "Honey":
                                            data["Object.cs.12760"] = data["Object.cs.12760"].Replace("Honey", $"{fields[1]}");
                                            break;
                                        case "Roe":
                                            data["Roe_DisplayName"] = data["Roe_DisplayName"].Replace("Roe", $"{fields[1]}");
                                            data["AgedRoe_DisplayName"] = data["AgedRoe_DisplayName"].Replace("Roe", $"{fields[1]}");
                                            break;
                                    }
                                }

                                else if (keyfields[0] == "PP" && fields[0] == "prefix")
                                {
                                    switch (keyfields[1])
                                    {
                                        case "Juice":
                                            data["Object.cs.12726"] = $"{fields[1]}" + " {0}";
                                            break;
                                        case "Wine":
                                            data["Object.cs.12730"] = $"{fields[1]}" + " {0}";
                                            break;
                                        case "Pickles":
                                            data["Object.cs.12735"] = data["Object.cs.12735"].Replace("Pickled", $"{fields[1]}");
                                            break;
                                        case "Jelly":
                                            data["Object.cs.12739"] = $"{fields[1]}" + " {0}";
                                            break;
                                        case "Wild Honey":
                                            data["Object.cs.12750"] = data["Object.cs.12750"].Replace("Wild Honey", $"{fields[1]}");
                                            break;
                                        case "Honey":
                                            data["Object.cs.12760"] = $"{fields[1]}" + " {0}";
                                            break;
                                        case "Roe":
                                            data["Roe_DisplayName"] = $"{fields[1]}" + " {0}";
                                            data["AgedRoe_DisplayName"] = $"Aged {fields[1]}" + " {0}";
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    // Edit general marriage dialogue
                    else if (e.NameWithoutLocale.IsEquivalentTo($"Characters\\Dialogue\\MarriageDialogue"))
                    {
                        SpellingFixer();
                    }

                    // Edit UI strings
                    else if (e.NameWithoutLocale.IsEquivalentTo("Strings\\UI"))
                    {
                        SpellingFixer();
                    }

                    // Edit location strings
                    else if (e.NameWithoutLocale.IsEquivalentTo("Strings\\Locations"))
                    {
                        SpellingFixer();
                    }

                    // Edit strings from maps
                    else if (e.NameWithoutLocale.IsEquivalentTo("Strings\\StringsFromMaps"))
                    {
                        SpellingFixer();
                    }

                    // Edit library books
                    else if (e.NameWithoutLocale.IsEquivalentTo("Strings\\Notes"))
                    {
                        SpellingFixer();
                    }

                    // Edit character strings
                    else if (e.NameWithoutLocale.IsEquivalentTo("Strings\\Characters"))
                    {
                        SpellingFixer();
                    }

                    // Edit extra dialogue
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\ExtraDialogue"))
                    {
                        SpellingFixer();
                    }

                    // Edit secret notes
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\SecretNotes"))
                    {
                        IDictionary<int, string> data = asset.AsDictionary<int, string>().Data;


                        foreach (int key in new List<int>(data.Keys))
                        {
                            // Replace specified string with new string
                            data[key] = data[key].Replace("favorite", "favourite");
                            data[key] = data[key].Replace("Mom", "Mum");
                            data[key] = data[key].Replace("efense", "efence");
                        }
                    }

                    // Edit clothing information
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\ClothingInformation"))
                    {
                        IDictionary<int, string> data = asset.AsDictionary<int, string>().Data;

                        foreach (int key in new List<int>(data.Keys))
                        {
                            // Replace specified string with new string
                            data[key] = data[key].Replace("olor", "olour");
                        }
                    }

                    // Edit Object information data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Strings\\Objects"))
                    {

                        IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;

                        foreach (string key in new List<string>(data.Keys))
                        {
                            // Skip replacement if string is any of the following
                            if (false
                                || data[key].Contains("falling")
                                || data[key].Contains("size")
                                || data[key].Contains("rized")
                                || data[key].Contains("denizen"))
                            {
                                continue;
                            }

                            if (this.config.FalltoAutumn == true)
                            {
                                data[key] = data[key].Replace("the fall", "autumn");
                                data[key] = data[key].Replace("A fall", "An autumn");
                                data[key] = data[key].Replace("fall", "autumn");

                                // Only replace string value for a specific key
                                if (key == "FallSeeds_Name")
                                {
                                    data[key] = data[key].Replace("Fall", "Autumn");
                                }

                                else if (key == "CornSeeds_Description")
                                {
                                    data[key] = "Plant these in the summer or in autumn. Takes 14 days to mature, and continues to produce after first harvest.";
                                }
                            }
                            // Replace string with new word

                            data[key] = data[key].Replace("color", "colour");
                            data[key] = data[key].Replace("avor", "avour");
                            data[key] = data[key].Replace("Fossilized", "Fossilised");
                            data[key] = data[key].Replace("Deluxe Fertilizer", "Deluxe Fertiliser");
                            data[key] = data[key].Replace("Quality Fertilizer", "Quality Fertiliser");
                            data[key] = data[key].Replace("Basic Fertilizer", "Basic Fertiliser");
                            data[key] = data[key].Replace("Tree Fertilizer", "Tree Fertiliser");
                            data[key] = data[key].Replace("appetizer", "appetiser");
                            data[key] = data[key].Replace("fertilize", "fertilise");
                            data[key] = data[key].Replace("theater", "theatre");
                            data[key] = data[key].Replace("zation", "sation");

                            try
                            {
                                if (namereplacer != null)
                                {
                                    foreach (string itemid in new List<string>(namereplacer.Keys))
                                    {
                                        string[] fields = namereplacer[itemid].Split('/');

                                        if (fields.Count() == 1)
                                        {
                                            data[$"{itemid}_Name"] = fields[0];
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                this.Monitor.LogOnce("NameReplacer.json not found");
                            }

                        }
                    }

                    // Edit TV channel data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\TV\\TipChannel"))
                    {
                        SpellingFixer();
                    }

                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\TV\\CookingChannel"))
                    {
                        SpellingFixer();
                    }

                    // Edit mail data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\mail"))
                    {
                        SpellingFixer();
                    }

                    // Edit egg festival data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\spring13"))
                    {
                        SpellingFixer();
                    }

                    // Edit flower dance data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\spring24"))
                    {
                        SpellingFixer();
                    }

                    // Edit luau data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\summer11"))
                    {
                        SpellingFixer();
                    }

                    // Edit dance of the moonlight jellies data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\summer28"))
                    {
                        SpellingFixer();
                    }

                    // Edit spirits eve data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\fall27"))
                    {
                        SpellingFixer();
                    }

                    // Edit feast of the winter star data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\winter25"))
                    {
                        SpellingFixer();
                    }

                    // Edit blueprints data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\Blueprints"))
                    {
                        SpellingFixer();
                    }

                    // Edit gift taste data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\NPCGiftTastes"))
                    {
                        SpellingFixer();
                    }

                    // Edit quest data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\Quests"))
                    {
                        IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;

                        foreach (string key in new List<string>(data.Keys))
                        {
                            // Replace specified string with new string
                            data[key] = data[key].Replace("avor", "avour");
                            data[key] = data[key].Replace("mom", "mum");
                        }
                    }

                    // Edit weapons data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Strings\\Weapons"))
                    {
                        IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;

                        foreach (string key in new List<string>(data.Keys))
                        {
                            // Replace specified string with new string
                            data[key] = data[key].Replace("favorite", "favourite");
                            data[key] = data[key].Replace("honor", "honour");
                        }
                    }

                    // Edit a single entry in hats
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\hats"))
                    {
                        IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;

                        // Replace specified key value with new value
                        data["30"] = "Watermelon Band/The colour scheme was inspired by the beloved summer melon./true/false//Watermelon Band";
                    }

                    // Edit a single entry in hats
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\Bundles") && this.config.FalltoAutumn == true)
                    {
                        var data = asset.AsDictionary<string, string>().Data;

                        foreach (string key in new List<string>(data.Keys))
                        {
                            // Replace specified string with new string
                            data[key] = data[key].Replace("Fall", "Autumn");
                        }
                    }

                    // Edit a single entry in BigCraftables
                    else if (e.NameWithoutLocale.IsEquivalentTo("Strings\\BigCraftables"))
                    {
                        IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;

                        // Replace specified key value with new value
                        data["MiniJukebox_Description"] = "Allows you to play your favourite tunes.";
                        data["BoneMill_Description"] = "Turns bone items into fertilisers.";
                    }

                    // Patch Intro tilesheet with new sign image
                    else if (e.NameWithoutLocale.IsEquivalentTo("Minigames\\Intro") && this.config.MetricSystem == true)
                    {
                        var editor = asset.AsImage();

                        // Get image to patch to tilesheet
                        Texture2D roadsign = Helper.ModContent.Load<Texture2D>("assets/Intro_sign.png");

                        // Patch image on tilesheet
                        editor.PatchImage(roadsign, targetArea: new Rectangle(48, 177, 64, 80));
                    }

                    // Edit movie data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\Movies"))
                    {
                        var movieDatas = asset.Data as List<StardewValley.GameData.Movies.MovieData>;

                        foreach (var movie in movieDatas)
                        {
                            switch (movie.Title)
                            {
                                case "The Brave Little Sapling":
                                    MovieEditor(movie, " ", " ", 4, "spring0_4", "demoralized", "demoralised");
                                    break;
                                case "Wumbus":
                                    MovieEditor(movie, "centered", "centred", 6, "summer1_6", "humor", "humour");
                                    break;
                                case "The Zuzu City Express":
                                    MovieEditor(movie, "theater", "theatre", 0, " ", " ", " ");
                                    break;

                            }

                            if (this.config.MetricSystem == true)
                            {
                                switch (movie.Title)
                                {
                                    case "Natural Wonders: Exploring Our Vibrant World":
                                        MovieEditor(movie, " ", " ", 1, "spring1_1", "80 miles", "128 kilometres");
                                        break;
                                    case "It Howls In The Rain":
                                        MovieEditor(movie, " ", " ", 1, "fall1_1", "30 miles", "48 kilometres");
                                        break;
                                }
                            }
                        }
                        // Method to edit movie description and a movie scene
                        void MovieEditor(StardewValley.GameData.Movies.MovieData movieData, string descoriginal, string descreplace, int scenenumber, string scenename, string original, string replace)
                        {
                            movieData.Description = movieData.Description.Replace(descoriginal, descreplace);
                            var sceneID = movieData.Scenes[scenenumber].Id;
                            var scene = movieData.Scenes.FirstOrDefault(s => s.Id == sceneID);

                            if (scene != null && sceneID == scenename)
                            {
                                scene.Text = scene.Text.Replace(original, replace);
                            }
                        }
                    }

                    // Edit movie reaction data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\MoviesReactions"))
                    {
                        var Reactions = asset.Data as List<StardewValley.GameData.Movies.MovieCharacterReaction>;

                        // Method to edit before movie reactions
                        void ReactionsEditorBefore(int NPCindex, int reactionindex, string original, string replacement)
                        {
                            Reactions[NPCindex].Reactions[reactionindex].SpecialResponses.BeforeMovie.Text = Reactions[NPCindex].Reactions[reactionindex].SpecialResponses.BeforeMovie.Text.Replace(original, replacement);
                        }

                        // Method to edit after movie reactions
                        void ReactionsEditorAfter(int NPCindex, int reactionindex, string original, string replacement)
                        {
                            Reactions[NPCindex].Reactions[reactionindex].SpecialResponses.AfterMovie.Text = Reactions[NPCindex].Reactions[reactionindex].SpecialResponses.AfterMovie.Text.Replace(original, replacement);
                        }

                        // Penny
                        ReactionsEditorBefore(0, 0, "mom", "mum");
                        ReactionsEditorAfter(0, 8, "favorite", "favourite");
                        // Krobus
                        ReactionsEditorBefore(2, 0, "recognize", "recognise");
                        ReactionsEditorBefore(2, 1, "center", "centre");
                        // Haley
                        ReactionsEditorBefore(19, 0, "favorite", "favourite");
                        ReactionsEditorBefore(19, 1, "favorite", "favourite");
                        // George
                        ReactionsEditorBefore(4, 4, "Theaters", "Theatres");
                        // Evelyn
                        ReactionsEditorBefore(6, 2, "theater", "theatre");
                        // Sam
                        ReactionsEditorBefore(14, 3, "theater", "theatre");
                        // Maru
                        ReactionsEditorBefore(20, 2, "theater", "theatre");
                        // Vincent
                        ReactionsEditorBefore(15, 4, "mom", "mum");
                        // Demetrius
                        ReactionsEditorBefore(23, 4, "analyze", "analyse");
                        // Dwarf
                        ReactionsEditorAfter(25, 1, "mesmerizing", "mesmerising");
                    }

                    // Edit concession data
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\Concessions"))
                    {
                        var Snacks = asset.Data as List<StardewValley.GameData.Movies.ConcessionItemData>;

                        // Method to edit a concession item description
                        void ConcessionsDescriptionEditor(int index, string original, string replacement)
                        {
                            Snacks[index].Description = Snacks[index].Description.Replace(original, replacement);
                        }

                        // Jasmine tea
                        ConcessionsDescriptionEditor(1, "flavored", "flavoured");
                        // Black liquorice
                        Snacks[11].DisplayName = "Black Liquorice";
                        // Kale smoothie
                        ConcessionsDescriptionEditor(16, "fiber", "fibre");
                        // Rock candy
                        ConcessionsDescriptionEditor(23, "Flavored", "Flavoured");

                        try
                        {
                            if (namereplacer != null)
                            {
                                foreach (string itemid in new List<string>(namereplacer.Keys))
                                {
                                    string[] keyfields = itemid.Split('_');
                                    if (keyfields[0] == "C")
                                    {
                                        Snacks[Convert.ToInt32(keyfields[1])].DisplayName = namereplacer[itemid];
                                    }
                                }
                            }
                        }
                        catch
                        {
                            this.Monitor.LogOnce("NameReplacer.json not found");
                        }
                    }

                    //Edit random bundles
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\RandomBundles") && this.config.FalltoAutumn == true)
                    {
                        var bundle = asset.Data as List<StardewValley.GameData.Bundles.RandomBundleData>;

                        void BundleNameReplacer(int roomindex, int bundlesetindex, int bundleindex, string newname)
                        {
                            bundle[roomindex].BundleSets[bundlesetindex].Bundles[bundleindex].Name = newname;
                        }

                        BundleNameReplacer(0, 0, 2, "Autumn Foraging");
                        BundleNameReplacer(1, 0, 2, "Autumn Crops");

                    }

                    // Edit fish data to convert inches to centimetres
                    else if (e.NameWithoutLocale.IsEquivalentTo("Data\\Fish") && this.config.MetricSystem == true)
                    {
                        IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;

                        foreach (string key in new List<string>(data.Keys))
                        {
                            // Skip replacement for trap fish, they don't have a size
                            if (false
                                || key == "715"
                                || key == "717"
                                || key == "723"
                                || key == "372"
                                || key == "720"
                                || key == "718"
                                || key == "719"
                                || key == "721"
                                || key == "716"
                                || key == "722")
                            {
                                continue;
                            }

                            string[] fields = data[key].Split('/');

                            fields[3] = ((int)Math.Round((int.Parse(fields[3]) * 2.54))).ToString();
                            fields[4] = ((int)Math.Round((int.Parse(fields[4]) * 2.54))).ToString();

                            data[key] = string.Join("/", fields);
                        }
                    }

                    // Edit special order strings
                    else if (e.NameWithoutLocale.IsEquivalentTo("Strings\\SpecialOrderStrings"))
                    {

                        IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;

                        // Replace specified string with new string
                        data["Gus_Name"] = data["Gus_Name"].Replace("Omelet", "Omelette");
                        data["Gus_RE_Greeting_0"] = data["Gus_RE_Greeting_0"].Replace("omelet", "omelette");
                        data["Gus_RE_Greeting_1"] = data["Gus_RE_Greeting_1"].Replace("omelet", "omelette");
                        data["Evelyn_Text"] = data["Evelyn_Text"].Replace("avor", "avour");

                    }
                });
            }
        }
    }
}

