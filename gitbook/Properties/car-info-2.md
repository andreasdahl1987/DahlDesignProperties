# Driving

Properties describing the use of the car, not car spesific

| Name                 | Description                                                                                                   | Type    |
| -------------------- | ------------------------------------------------------------------------------------------------------------- | ------- |
| Idle                 | Wether you're in the cockpit and can control the car, or not.                                                 | boolnea |
| AccelerationTo100KPH | Time from standstill to 100 KPH                                                                               | seconds |
| AccelerationTo200KPH | Time from standstill to 200 KPH                                                                               | seconds |
| BrakeCurveValues     | A string of values representing a 4-second recording of your previous brake pedal input.                      | string  |
| BrakeCurvePeak       | The highest input during your previous brake application                                                      | integer |
| BrakeCurveAUC        | Area under the brake curve (the BrakeCurveValues plotter over 4 seconds). Reflects the total brake work done. | decimal |
| ThrottleCurveValues  | A string of values representing a 3-second recording of your previous throttle pedal input.                   | string  |
| ThrottleAgro         | The time it took from initial throttle application to full throttle on your previous throttle press.          | decimal |
| MinimumCornerSpeed   | Slower speed between last throttle release and throttle re-application                                        | integer |
| StraightLineSpeed    | Highest speed reached before previous brake application                                                       | integer |

