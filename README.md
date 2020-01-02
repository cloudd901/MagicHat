# MagicHat
Graphics based random animated hat

I created this dll as part of another project I am working on.
This library will create a Hat and a list of Entries.
The Hat is customizable and uses layers of PictureBoxes that get added to your form.

It is using a specific interface in IRandomTool.cs so it can be plug-n-play with my other project.

- Create the hat object.
- Add Entries to the list.
- Draw the hat.
- Animate the hat.

![Icon](https://github.com/cloudd901/MagicHat/blob/master/HatScreen.jpg)

The example script provided includes nearly all options available for the hat.
Note, this hat does not use as many options as the wheel version.

Draw
- Draw(LocationX, LocationY, Radius);
- or Draw(CenterPoint, Radus);
- Refresh();
  - Calls Draw using the existing location data;

EntryList
- Hat.EntryList;
  - The list of Entries used for the wheel.
- EntryAdd(Entry);
  - Entry type contains a UniqueID(optional), Name, and Color.
  - Returns a unique ticket ID that can optionally be set in the Entry.
- EntryRemove(UniqueID);
  - Uses an ID provided by EntryAdd.
- EntriesClear();
  - Clears the EntryList.
- ShuffleEntries();
  - Randomizes the EntryList.

Action
- Hat.IsBusy;
  - Returns true if the wheel is actively spinning.
- Start();
- or Start(animDirection, randPowerType, randStrength);
  - animDirection can be Clockwise or CoutnerClockwise.
  - randPowerType can be Infinite, Weak, Average, Strong, Super, Random, or Manual.
    - If rPT is Manual, the randStrength will set the number of animations +-3 random.
  - randStrength doesn't do anything if rPT is not Manual.
- Stop();

Other
- IsReadable(Color1, Color2);
  - Public function used to determine text color against the background.
- BringToFront();
  - Brings the PictureBox control to the front of the form.
- SendToBack();
  - Sends the PictureBox control to the back of the form.
- Dispose();
  - Disposes of Image and PictureBox data.
  - Must create a new wheel object after this is called.

Event Handlers
- ToolActionEventHandler(Entry, string[4] actionInfo)
  - This event is called each frame the Hat is moved.
  - Each time the Hat is at the center position:
    - The EntryList is randomly sorted.
    - A new random Entry is picked.
  - The string array provided is a 4 part array.
    - Total Hat animations. "23"
    - Current animation countdown. "10"
    - Current base speed adjustment. "0"
    - Spin Strength. "Random|9"
- ToolStopEventHandler(Entry)
  - This event is called when the wheel comes to a stop.
  - Entry provided is the final selected Entry.

Settings
- Hat.AllowExceptions = true;
  - If false, actions will return null instead of throwing an Exception.
- Hat._ToolProperties;
  - Contains all other wheel settings:

_ToolProperties
- ArrowPosition (not used)
- ArrowImage (not used)
- LineColor (not used)
- LineWidth (not used)
- ForceUniqueEntryColors
- TextToShow (Entry Aura will override CenterColor if TextToShow is not 'none')
- TextColor
- TextColorAuto
- TextFontFamily
- TextFontStyle
- ShadowVisible (not used)
- ShadowColor (not used)
- ShadowPosition (not used)
- ShadowLength (not used)
- CenterVisible
- CenterColor (Entry Aura will override CenterColor if TextToShow is not 'none')
- CenterSize (not used)
