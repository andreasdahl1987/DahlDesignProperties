# 🧍♂ Opponents

There are two disctinct sets of opponent properties, with different usage:

1. Properties on the geographically closest opponents (5 behind and 5 in front).
2. Properties on opponents in key race positions (class leader, overall leader, position ahead, position behind, lucky dog).

Many of the properties in set #1 are extracted directly from available SimHub data, some of them are calculated.&#x20;

The properties in set #2 is purely tracked and calculated, and have some additional useful features such as overtake predictions, race pace, slow lap alert, laps since pit stop and more.&#x20;

The gem in this collection of opponents properties are the **RealGap** properties. SimHub's opponent delta times are estimated, based on lap times and current track positions. The delta time will get smaller in slow corners and longer on the straights. It is not totally useless, but it is indeed very crude. The **RealGap** system gives 100% accurate live delta values, since it is based on stop watches. 3840 stop watches keep track of all the cars in the race, starting/stopping at 60 checkpoints evenly spread out on the track. This means your delta values will refresh 60 times during the course of a lap. _If you do a good corner and **RealGap** is reduced by 0.1 seconds, that means you actually gained 0.1 seconds in that corner._&#x20;
