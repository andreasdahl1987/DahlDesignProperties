# DDC

Properties read from a DDC device

| Name                  | Description                                                                    | Type    |
| --------------------- | ------------------------------------------------------------------------------ | ------- |
| DDCclutch             | Clutch output from controller                                                  | 0-100   |
| DDCbitePoint          | Clutch bite point                                                              | 0-100   |
| DDCbrake              | Brake output from controller                                                   | 0-100   |
| DDCthrottle           | Throttle output from controller                                                | 0-100   |
| DDCDDSMode            | Mode of DDS switch                                                             | 0-3     |
| DDCDDSEnabled         | Controller has a DDS switch                                                    | boolean |
| DDCEnabled            | Enable reading from controller                                                 | boolean |
| DDCclutchMode         | Mode of dual clutches                                                          | 0-3     |
| DDCbiteSetting        | Current step in bite point setting. 0 = off, 1 = +/- 10, 2 = +/-1, 3 = +/- 0.1 | 0-3     |
| DDCPreset             | Active preset                                                                  | 1-12    |
| DDCneutralActive      | Neutral button pressed                                                         | boolean |
| DDCneutralMode        | Mode of neutral button                                                         | 0-1     |
| DDCthrottleHoldActive | Throttle hold is active                                                        | boolean |
| DDCmagicActive        | Brake magic is active                                                          | boolean |
| DDCquickSwitchMode    | Mode of main quick switch                                                      | 0-1     |
| DDCquickSwitchActive  | Main quick switch is active, triggering all 4 quick-bound rotary switches.     | boolean |
| DDCB1                 | Value from button field bit 1                                                  | 0-1     |
| DDCB2                 | Value from button field bit 2                                                  | 0-1     |
| DDCB3                 | Value from button field bit 3                                                  | 0-1     |
| DDCB4                 | Value from button field bit 4                                                  | 0-1     |
| DDCR1                 | Value from rotary field bit 1                                                  | 0-1     |
| DDCR2                 | Value from rotary field bit 2                                                  | 0-1     |
| DDCR3                 | Value from rotary field bit 3                                                  | 0-1     |
| DDCR4                 | Value from rotary field bit 4                                                  | 0-1     |
| DDCR5                 | Value from rotary field bit 5                                                  | 0-1     |
| DDCR6                 | Value from rotary field bit 6                                                  | 0-1     |
| DDCR7                 | Value from rotary field bit 7                                                  | 0-1     |
| DDCR8                 | Value from rotary field bit 8                                                  | 0-1     |
| DDCR15                | Value from rotary field bit 15                                                 | 0-1     |

