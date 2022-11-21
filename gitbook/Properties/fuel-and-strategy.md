# ⛽ Fuel and strategy

Properties generated from fuel calculations. Calculations are based on very detailed algorithms (have a look in the repository if you wish) to give the most accurate feedback, enabling you to make the best strategy calls.

For all calculations based on fuel/lap data, there are two available sets of properties:

* Based on average fuel/lap
* Based on last lap fuel/lap

So the property `FuelDelta` will have a corresponding property `FuelDeltaLL` for calculation based on previous lap.

The thought is that if your last lap is more representative for the fuel consumption of the following laps (in case of fuel saving primarily; reaching a group of cars for draft or doing a lap with coast/lifting), you can see how the calculations are affected.

<table data-view="cards"><thead><tr><th>Name</th><th>Description</th><th>Type</th></tr></thead><tbody><tr><td>Approximate calculations</td><td>TRUE if very little data is available to make good fuel calculations. Drive a couple of laps.</td><td>boolean</td></tr><tr><td>FuelDelta</td><td>Gap between what you have and what you're missing to make it to the end of the sessions.</td><td>decimal</td></tr><tr><td>FuelPitWindowFirst</td><td>The lap number then the pit window opens</td><td>integer</td></tr><tr><td>FuelPitWindowLast</td><td>The lap number when the pit window closes</td><td>integer</td></tr><tr><td>FuelMinimumFuelFill</td><td>The least you can fuel on your next stop</td><td>decimal</td></tr><tr><td>FuelMaximumFuelFill</td><td>The largest amount of fuel that makes sense to fill on next stop.</td><td>decimal</td></tr><tr><td>FuelPitStops</td><td>The amount of required pit stops for fuel this session. The decimals will give you an idea of how close/far you are from requiring an extra stop/saving a stop.</td><td>decimal</td></tr><tr><td>FuelAlert</td><td>Pit this lap, or run out of fuel.</td><td>boolean</td></tr><tr><td>FuelConserveToSaveAStop</td><td>Percent reduction in fuel/lap required to save enough fuel to avoid a pit stop</td><td>0-100</td></tr><tr><td>FuelSlowestFuelSavePace</td><td>The slowest lap time you can have for the rest of the race to make fuel saving/tire management worth it (given that you can save a full pit stop)</td><td>timespan</td></tr><tr><td>PitSavePaceLock</td><td>On/off locking in current pace for calculating <code>FuelSlowestFuelSavePace</code></td><td>boolean</td></tr><tr><td>FuelSaveDeltaValue</td><td>How much fuel you're missing/excess to avoid a pit stop based on last lap calculations.</td><td>decimal</td></tr><tr><td>FuelPerLapOffset</td><td>The current added addition/subtraction to average fuel/lap for calculations.</td><td>decimal</td></tr><tr><td>FuelPerLapTarget</td><td>The set fuel/lap target.</td><td>decimal</td></tr><tr><td>FuelPerLapTargetLastLapDelta</td><td>Delta to the set fuel/lap target on previous lap</td><td>decimal</td></tr><tr><td>Lap(1-8)FuelTargetDelta</td><td>Delta to the set fuel/lap target on the previous 8 laps</td><td>decimal</td></tr><tr><td>FuelTargetDeltaCumulative</td><td>The amount of fuel saved compared to target in total on this stint.</td><td>decimal</td></tr></tbody></table>

### Example

You're racing at Nordschleife combined, in a Porsche 911 with a 110L tank. You've calculated with 2 pit stops. A few laps into the race you see your **FuelDelta** is -209.8 L. Simple math tells you that if you stop twice you should be able to fill 2 x 110 = 220 liters, but **FuelPitStops** is 3.01. And **FuelMinimumFuelFill** is 1.1 L. Why?

You need about 12.7L of fuel per lap on this track. The algorithm will calculate how much room is possible to free up in the fuel tank by going as far as possible. In this case, 11.8 liters left in tank when you end the current stint, and you've only room for 98.2L on the first stop. The algorithm will also compute for all needed pit stops ahead, and can tell that on the second pitstop, there will be 10.7 liters left on the tank, only room for 99.3 L. In total 197.5 L. So you're actually missing 12.3 liters. This means a 3rd pit stop is needed. But still, **FuelMinimumFuelFill** is 1.1 L, why not 12.3? The algorithm has calculated that by filling 1.1 L, you can go an extra lap on the next stint, and will have room for more fuel on the 2nd pit stop. So 1.1 L is all you need. In this case, _all you need to save_. **FuelMinimumFuelFill** and will **FuelMaximumFuelFill** always find the amount of fuel that allows you to enter the pit box with 0.2 L fuel remaining on the next stint (given that you'll have at least 2 pit stops). No reason to fill the whole tank if that means you will have 12 liters left on the tank on next pit stop. That just means you drove around with an extra 12 kg for an hour.

### Fuel calculation settings

<figure><img src="../.gitbook/assets/image (2).png" alt=""><figcaption></figcaption></figure>

The plugin menu has some settings relevant to fuel and strategy:

* Fuel per lap target: Manually set the target
* Fuel per lap offset increments: Set the resolution of offset adjustments in Dashboard
* Small Pitstop Fuel Adjustment Increment: Change the size of small fuel adjustments in Dashboard.
* Large Pitstop Fuel Adjustment Increment: Change the size of large fuel adjustments in Dashboard.
* Adjust remaining laps for pit stop duration: For timed sessions, a pit stop might last so long that the race will be 1 lap shorter than what you calculate for. With this checked, the pit stop time will be subtracted from the race duration for calculations, avoiding fueling for that lap that you're not doing. There is an inherent risk to this.
