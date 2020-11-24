using System;
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
        private ModConfig config;
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

        public override void Entry(IModHelper helper)
        {
            this.config = this.Helper.ReadConfig<ModConfig>();
        }
        
        public bool CanEdit<T>(IAssetInfo asset)
        {
            foreach(var name in NPCs)
            {
                if (false
                    || asset.AssetNameEquals($"Characters\\Dialogue\\Marriage{name}")
                    || asset.AssetNameEquals($"Characters\\Dialogue\\{name}"))
                {
                    return true;
                }
            }

            // return true if assest name is ANY of the following....
            return (false
                    || asset.AssetNameEquals("Strings\\StringsFromCSFiles")
                    || asset.AssetNameEquals("Data\\ObjectInformation")
                    || asset.AssetNameEquals("Data\\TV\\TipChannel")
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
                        if (data[key].Contains("fall on") || data[key].Contains("fall_"))
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
            }
            if (asset.AssetNameEquals("Strings\\StringsFromCSFiles"))
            {
                var data = asset.AsDictionary<string, string>().Data;
                data["Utility.cs.5682"] = "Autumn";
                data["fall"] = "autumn";
            }

            else if (asset.AssetNameEquals("Data\\ObjectInformation"))
            {
                IDictionary<int, string> data = asset.AsDictionary<int, string>().Data;
                foreach (int id in new List<int>(data.Keys))
                {
                    string[] array = data[id].Split('/');
                    string descr = array[5];
                    if (descr.Contains("falling"))
                        continue;

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

            else if (asset.AssetNameEquals($"Characters\\Dialogue\\MarriageDialogue{NPCs}"))
            {
                var data = asset.AsDictionary<string, string>().Data;

                foreach(string key in new List<string>(data.Keys))
                {
                    if(data[key].Contains("fall_"))
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

                foreach (string Itemid in new List<string>() { "36", "67", "78", "116", "186" })
                {
                    data[Itemid] = data[Itemid].Replace("fall", "autumn");
                }
            }
        }
    }
}

