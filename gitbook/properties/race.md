# 🏁 Race

Properties relevant to the race.&#x20;

iRacing telemetry is full of seemingly strange developer decisions. One of them is no not update race position untill you're completed a lap. This plugin will give live updated race positions. Not only for you, but also other drivers. Another importat feature is the **LapsRemaining** property, which uses a detailed algorithm for determining how many more laps we can expect to drive. This is affected by a lof of factors, such as:

* Type of session (lap determined, time determined or both)
* Leader pace, leader track position
* Your pace (has its own algorithm for excluding off-track laps, in-laps, out-laps, etc.) and track position
* Pit stop duration
* Quirky things like standing in front or behind start/finish line on the grid and corrections for strange lap-counting i iRacing telemetry

A lot of effort has gone into this property since it is used for fuel and strategy calculations, amongst other things.&#x20;

Another feature worth mentioning is the spotter calls, which will give you the name of the driver on the side of you, and also giving to the distance to the car next to you - for both sides. The plugin will remember the driver first entering your "space", and by process of elimination figure out who is on the other side, and the distance to that driver as well.&#x20;

<table data-view="cards"><thead><tr><th>Name</th><th>Description</th><th>Type</th></tr></thead><tbody><tr><td>LapsRemaining</td><td>Complex algorithm that calculates the remaining laps of the race, based on time left, your pace, leaders pace and track position, amongst other things.</td><td>integer</td></tr><tr><td>LapBalance</td><td>Your position on track when session finishes; how close you are to having a 1 lap shorter/longer session.</td><td>decimal</td></tr><tr><td>RaceFinished</td><td>You finished the race</td><td>boolean</td></tr><tr><td>JokerThisLap</td><td>Whether current lap is a joker lap or not</td><td>boolean</td></tr><tr><td>JokerCount</td><td>Number of joker laps this stint</td><td>integer</td></tr><tr><td>Position</td><td>Live updated race position</td><td>integer</td></tr><tr><td>HotLapPosition</td><td>Your position on "fastest lap" leaderboard. Only differs from "Position" in race sessions.</td><td>integer</td></tr><tr><td>HotlapLivePosition</td><td>Your estimated position on "fastest lap" leaderboard, based on estimated lap time</td><td>integer</td></tr><tr><td>LeftCarGap</td><td>Distance to car on left side (forward/backward)</td><td>decimal</td></tr><tr><td>LeftCarName</td><td>Driver name of right side car</td><td>string</td></tr><tr><td>RightCarGap</td><td>Distance to car on left side (forward/backward)</td><td>decimal</td></tr><tr><td>RightCarName</td><td>Driver name of right side car</td><td>string</td></tr><tr><td>RadioName</td><td>Name of driver using radio</td><td>string</td></tr><tr><td>RadioPosition</td><td>Position of driver using radio</td><td>integer</td></tr><tr><td>RadioIsSpectator</td><td>True of person on radio is not a driver in the race</td><td>boolean</td></tr><tr><td>MyClassColor</td><td>Your class color, hexadecimal format</td><td>hexadecimal</td></tr><tr><td>SoF</td><td>Strength of field</td><td>iRating</td></tr><tr><td>IRChange</td><td>Change in iRating based on current race position</td><td>iRating</td></tr><tr><td>TrackType</td><td>0 = road, 1-3 = rallycross, 4 = dirt road w/o joker laps, 5-7 = short to long ovals, 8 = dirt oval</td><td>0-8</td></tr></tbody></table>



