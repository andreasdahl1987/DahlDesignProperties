# Fuel and strategy

Properties from fuel calculations. Calculations are based on very detailed algorithms (have a look in the repository if you wish) to give the most accurate feedback, enabling you to make the best strategy calls.

For all calculations based on fuel/lap data, there are two available sets of properties:

* Based on average fuel/lap
* Based on last lap fuel/lap&#x20;

So the property `FuelDelta` will have a corresponding property `FuelDeltaLL` for calculation based on previous lap.&#x20;

The thought is that if your last lap is more representative for the fuel consumption of the following laps (in case of fuel saving primarily; reaching a group of cars for draft or doing a lap with coast/lifting), you can see how the calculations are affected.&#x20;

| Name                         | Description                                                                                                                                                      | Type |
| ---------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---- |
| FuelDelta                    | Gap between what you have and what you're missing to make it to the end of the sessions.                                                                         |      |
| FuelPitWindowFirst           | The lap number then the pit window opens                                                                                                                         |      |
| FuelPitWindowLast            | The lap number when the pit window closes                                                                                                                        |      |
| FuelMinimumFuelFill          | The least you can fuel on your next stop                                                                                                                         |      |
| FuelMaximumFuelFill          | The largest amount of fuel that makes sense to fill on next stop.                                                                                                |      |
| FuelPitStops                 | The amount of required pit stops for fuel this session. The decimals will give you an idea of how close/far you are from requiring an extra stop/saving a stop.  |      |
| FuelAlert                    | Pit this lap, or run out of fuel.                                                                                                                                |      |
| FuelConserveToSaveAStop      |                                                                                                                                                                  |      |
| FuelSlowestFuelSavePace      |                                                                                                                                                                  |      |
| PitSavePaceLock              |                                                                                                                                                                  |      |
| FuelSaveDeltaValue           |                                                                                                                                                                  |      |
| FuelPerLapOffset             |                                                                                                                                                                  |      |
| FuelPerLapTarget             |                                                                                                                                                                  |      |
| FuelPerLapTargetLastLapDelta |                                                                                                                                                                  |      |
| FuelTargetDeltaCumulative    |                                                                                                                                                                  |      |
