# 🧍♂ Opponents

There are two disctinct sets of opponent properties, with different usage:

* \#1: Properties on the geographically closest opponents, regardless of standing in the race (5 behind and 5 ahead).
* \#2: Properties on opponents in key race positions (class leader, overall leader, position ahead, position behind, lucky dog).

Many of the properties in set #1 are extracted directly from available SimHub data, some of them are calculated.&#x20;

The properties in set #2 is purely tracked and calculated, and have some additional useful features such as overtake predictions, race pace, slow lap alert, laps since pit stop and more.&#x20;

The gem in this collection of opponents properties are the **RealGap** properties. SimHub's opponent delta times are estimated, based on lap times and current track positions. The delta time will get smaller in slow corners and longer on the straights. It is not totally useless, but it is indeed very crude. The **RealGap** system gives 100% accurate live delta values, since it is based on stop watches. 3840 stop watches keep track of all the cars in the race, starting/stopping at 60 checkpoints evenly spread out on the track. This means your delta values will refresh 60 times during the course of a lap. _If you do a good corner and **RealGap** is reduced by 0.1 seconds, that means you actually gained 0.1 seconds in that corner._&#x20;

### #1 Geographically close opponents

All these properties start with `Car(Ahead/Behind)(01-05)`. For instance `CarAhead03Gap`

<table data-view="cards"><thead><tr><th>Name</th><th>Description</th><th>Type</th></tr></thead><tbody><tr><td>Name</td><td>Full name</td><td>string</td></tr><tr><td>Position</td><td>Race position, regular iRacing telemetry, updated once pr. lap</td><td>integer</td></tr><tr><td>Gap</td><td>Relative gap to opponent, estimated from lap time and track position</td><td>seconds</td></tr><tr><td>RaceGap</td><td>Total gap to opponent, estimated from lap time and track position</td><td>seconds</td></tr><tr><td>RealGap</td><td>Total gap to opponent, measured with stop watch</td><td>seconds</td></tr><tr><td>RealRelative</td><td>Relative gap to opponent, measured with stop watch</td><td>seconds</td></tr><tr><td>BestLap</td><td>Best valid lap time</td><td>timespan</td></tr><tr><td>ClassColor</td><td>iRacing class color</td><td>hexadecimal</td></tr><tr><td>ClassDifference</td><td>Difference in class "levels". If you're in an MX5 racing against an LMP2, this would be "-4". If you're a GT4 racing against an MX5, it would be +1.</td><td>-4 to +4</td></tr><tr><td>IsAhead</td><td>Opponent is ahead in terms of race standings</td><td>boolean</td></tr><tr><td>IsClassLeader</td><td>Opponent is class leader</td><td>boolean</td></tr><tr><td>IsInPit</td><td>Opponen is in pit lane/stall</td><td>boolean</td></tr><tr><td>LapsSincePit</td><td>How many laps since last pit stop</td><td>boolean</td></tr><tr><td>P2PStatus</td><td>Opponent has active push to pass (P2P)</td><td>boolean</td></tr><tr><td>P2PCount</td><td>Number of P2P charges left in the session</td><td>0-10</td></tr><tr><td>JokerLaps</td><td>Number of completed joker laps</td><td>integer</td></tr><tr><td>IRating</td><td>iRating</td><td>integer</td></tr><tr><td>License</td><td>License and safety rating</td><td>string</td></tr></tbody></table>

### #2 Opponents in key race positions

#### Properties starting with `Ahead` or `Behind` (refering to race standings, not geographically).

| Name           | Description                                                                                                                                         | Type     |
| -------------- | --------------------------------------------------------------------------------------------------------------------------------------------------- | -------- |
| Name           | Full name                                                                                                                                           | string   |
| Gap            | Total gap to opponent, estimated from lap time and track position                                                                                   | seconds  |
| RealGap        | Total gap to the opponent, measured with stop watch                                                                                                 | seconds  |
| Pace           | Estimated race pace                                                                                                                                 | timespan |
| BestLap        | Best valid lap time                                                                                                                                 | timespan |
| Slow lap       | True if previous lap time slow compared to expected pace                                                                                            | boolean  |
| Prognosis      | Rating your chance of catching (if ahead-property) or being caught up to (if behind-property). 1 = good for you, 5 = bad for you. 0 = can't compute | 0-5      |
| LapsToOvertake | Number of whole laps before you get with 0.5s gap. -1 means "allready there"/"can't compute"/"opponent is faster". 0 means "this lap"               | integer  |
| P2PStatus      | Opponent push to pass (P2P) is active                                                                                                               | boolean  |
| P2PCount       | Opponents remaining P2P uses in the race                                                                                                            | 0-10     |
| IsInPit        |                                                                                                                                                     |          |
| LapsSincePit   |                                                                                                                                                     |          |
| IsConnected    |                                                                                                                                                     |          |
|                |                                                                                                                                                     |          |

