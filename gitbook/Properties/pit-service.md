# 🔧 Pit service

The plugin offers a lot of options for setting up your pitstop using automated text commands. This is covered in datail in the Dashboard documentation. By mapping car attributes with regards to fueling time, tire change time, wing adjustments, etc., as well as mapping out tracks for calculating drive-through times, we're able to quite accurately determine the time lost from pit stops. The whole pit stop has been reverse engineered through hours and hours of testing with stop watches, resulting in an algorithm that can tell you much much time you need to change the left front tire, fuel 4 liters and do 7 clicks on the front wing. Any combination of services is covered.

In the process I've discovered quirks like changing the tires on the right side of the car is faster on Lime Rock Park than on Bathurst (apparently, the side ofthe car facing the garages is faster, which is kind of realistic), but the difference between left and right side of the car changes between cars. For some cars there is no difference. Also, some combinations of tire changes nullifies these differences. Some cars, like Indycars, have fancy antimated pitcrew on some of the US tracks. In that case, the tire change times are different.

Needless to say, a lot of effort has gone into this, and it also makes mapping out cars and tracks more demanding. The reward is that with accurate pit stop predictions, you can get accurate predictions on where you are in the field on pit exit, allowing you to make strategy calls like pitting early to get out in free air. You can also adjust your fuel strategy on time limited sessions, knowing that the long pit stop will mean 1 lap shorter race. A bunch of properties spins of the calculations of pit stop duration.

That being said, there is an inherent inaccuracy to these properties, due to iRacing's randomness in pit service timings. For some cars, the same tire change ranges between 23.0 and 29.2 seconds. For another car it is between 11.2 and 11.7. In general I've found that cars used in competetive series where pit stop are common, there is litte variation.

| Name                 | Description               | Type    |
| -------------------- | ------------------------- | ------- |
| PitToggleLF/RF/LR/RR | On/off toggled for change | boolean |
| PitToggleFuel        | On/off fueling ordered    | boolean |
| PitToggleWindscreen  | On/off windscreen tearoff | boolean |
| PitToggleRepair      | On/off repair ordered     | boolean |
