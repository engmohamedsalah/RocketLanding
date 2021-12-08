# Rocket Landing
This is an exercise for simulating rockets landing on the Landing Platform


# Description
You need to design a library that will help determine if rockets can land on a platform. 
Whenever a rocket is getting back from orbit, it needs to check every now and then if it's on a correct trajectory to safely land on a platform. 
The whole landing area (the area that contains the landing platform and surroundings) consists of multiple squares that set a perimeter/dimensions 
that can be described with coordinates (say x and y). Assuming that the landing area has a size of square 100x100 and landing platform 
has a size of a square 10x10 and its top left corner starts at a position 5,5 (please assume that position 0,0 is located at the top left corner of the landing area 
and all positions are relative to it), the library should work as follows:

    • if rocket asks for position 5,5 it replies `ok for landing`
    • if rocket asks for position 16,15, it replies `out of platform`
    • if the rocket asks for a position that has previously been checked by another rocket(only last check counts), 
      it replies with `clash`
    • if the rocket asks for a position that is located next to a position that has previously been checked 
      by another rocket (say, previous rocket asked for position 7,7 and the rocket asks for 7,8 or 6,7 or 6,6), 
      it replies with `clash`.Given the above.
      
# Requirements

Creating a library (just library, it doesn't need to be used on any cli/gui) that will support the following features:

    • rocket can query it to see if it's on a good trajectory to land at any moment
    • library can return one of the following values: ['out of platform' , 'clash', 'ok for landing']
    • more than one rocket can land on the same platform at the same time and rockets need to have at least one unit 
      separation between their landing positions
    • platform size can vary and should be configurable
    
 # Some definition
   
**LandingArea**: the whole land that is expected to have the landing platform

**LandingPlatform**: part of the landing area and it considers the area where the rockers can land

**SeparationUnits**: is the number of units that should be existing between rockets to avoid a crash

**LastCheckedSpot**: consider the invalid area for the incoming rocket. 
When a rocket land on point then all points around this point by SeparationUnits should not be valid landing



    
