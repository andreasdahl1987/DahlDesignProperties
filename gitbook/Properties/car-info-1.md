# 🔢 Gear

Properties on gear, shifting and RPM

<table data-view="cards"><thead><tr><th>Name</th><th>Description</th><th>Type</th></tr></thead><tbody><tr><td>SmoothGear</td><td>Removed the "N" showing up between gear shifts. Alternative to the native [Gear] property in SimHub. </td><td>string</td></tr><tr><td>LastGear</td><td>Your previous gear</td><td>string</td></tr><tr><td>LastGearMaxRPM</td><td>Highest RPM reached on previous gear</td><td>rpm</td></tr><tr><td><mark style="color:green;">IdleRPM</mark></td><td>RPM with engine on and clutch pressed</td><td>rpm</td></tr><tr><td><mark style="color:green;">OptimalShiftGear(X)</mark></td><td>Optimal shift point (RPM) going from (X) gear to next</td><td>rpm</td></tr><tr><td><mark style="color:green;">OptimalShiftCurrentGear</mark></td><td>Optimal shift point (RPM) going from current gear to next</td><td>rpm</td></tr><tr><td><mark style="color:green;">OptimalShiftLastGear</mark></td><td>Optimal shift point (RPM) going from previous gear to current gear</td><td>rpm</td></tr><tr><td><mark style="color:green;">ShiftLightRPM</mark></td><td>Calculated RPM at which an alert for shifting gear should trigger. Given just before the optimal RPM is reached. Car/gear specific. Based on throttle input, DRS, P2P, MGU-K activity and reaction time set in plugin settings. Aiming to always make you hit the optimal RPM if shifting with the same reaction time on each shift.</td><td>rpm</td></tr><tr><td>ReactionTime</td><td>Measured reaction time from (RPM > ShiftLightRPM) until you make a gear shift. Useful for setting up your reaction time in plugin settings. Default is 300 ms.</td><td>ms</td></tr></tbody></table>