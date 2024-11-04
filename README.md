# Goodbye American English #

Changes American spelling to British spelling, this includes changing the season fall to autumn.

Also changes the game units to the metric system

I've tried to be as thorough as I can but I may have missed some changes, let me know if you spot anything and I'll fix it up.

## To install: ##
- Ensure SMAPI is installed
- Simply unzip the download file into your Mods folder

### NameReplacer ###
Version 1.4.0 introduced the ability to change the display names of any concession snack or object using the included NameReplacer.json file, think of this as an internal content pack. When entering name replacements make sure to put a comma at the end of each entry (except the last one). 

You can safely delete this file if you don't want to change any names. If you want to replace names but have deleted the file, simply create a new JSON file with the name "NameReplacer"

In Version 1.4.1, the NameReplacer.json will generate once the mod is run. Just ignore the file if you don't want to use it.

In Version 1.5.0, the NameReplacer has become more advanced, thanks to Harmony! Now preserve and honey names can be independently or generically edited. These edits are discussed in the Advanced NameReplacer section. 

To turn Harmony patching off, set AllowAdvancedNameReplacer to false in the config. This stops some replacements from occurring but can prevent issues if the game is experiencing problems.

In 1.6.0 the format of the NameReplacer has been updated for readability and to be better compatible with Stardew Valley 1.6. The old format is no longer supported and will show a warning.
#### 1.6.0 ####
In version 1.6.0 the format of the NameReplacer has changed to better support the new object data format.

For Objects: Name replacement entries are in the format: "ItemName": "NewItemName", e.g to turn Hot Pepper into Chilli, the entry would be "Hot Pepper": "Chilli".

For Concession snacks: Name replacement entries are in the format: "ConcessionName_C":"NameToReplaceWith", e.g to turn Cotton Candy into Fairy Floss, the entry would be "Cotton Candy_C":"Fairy Floss"

## Advanced NameReplacer ##

Editing preserve names works a little differently. The format has also been updated in 1.6.0, but the old format is still currently supported. This won't be guaranteed in the future.

#### Generic edits ####

##### 1.6.0 #####
Universal edits to all preserve names i.e all Jelly becomes Jam are in the form "PP_PreserveType":"EditType/NameToReplaceWith".

To break it down:

The PreserveType is the preserve to change. Either: "Juice", "Wine", "Pickles", "Jelly", "Roe", "Wild Honey" or "Honey". Edits for "Wild Honey" replaces the entire object name.

The EditType can be one of "prefix" or "suffix". Basically, put the preserve word before "prefix" or after "suffix" the object name. Either will replace the entire name for "Wild Honey".

E.g "PP_Jelly": "suffix/Jam"

#### Independent edits ####

Unique preserve edits can be done when AllowAdvancedNameReplacer is true (which is the default) in the config. 
Generic edits are mutually exclusive (can only prefix or suffix, not both). Unique edits will override generic edits.

##### 1.6.0 #####
Unique edits are in the form "P_(O)ObjectID_PreserveType":"EditType/NameToReplaceWith".

To break it down:

The ObjectID refers to the object ID of the item to replace the preserve name for. i.e Potato's ID to change Potato Juice to something else.

The PreserveType refers to the preserve to change. Either: "Juice", "Wine", "Pickles", "Jelly", "Roe" or "Honey".

The EditType can be one of "prefix", "suffix" or "replace". Basically, put the preserve word before "prefix" or after "suffix" the object name. "replace" will replace the entire name with something else. For "replace" using {0} will insert the object name in it's place.

E.g "P_(O)613_Jelly": "suffix/Sauce", (this changes Apple Jelly to Apple Sauce)





