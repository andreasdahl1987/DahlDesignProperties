# Install

### Download .dll

{% embed url="https://github.com/andreasdahl1987/DahlDesignProperties/releases" %}

* Get the latest release from GitHub repository page. You'll only need the `DahlDesign.dll` file.
* The file goes into your SimHub main folder. Typically C:\Program Files(x86)\SimHub
* Restart SimHub and you'll get a prompt that a new plugin has been found.&#x20;

### Plugin in SimHub

You'll find the plugin here. You can also check the box to have a shortcut to the plugin menu on the left hand side. The menu has a lot of useful content, so I would strongly recommend it.&#x20;

<figure><img src="../.gitbook/assets/simhub1.PNG" alt=""><figcaption></figcaption></figure>

### Additional setup

#### Wheel slip

A special feature that requires a little extra setup is the **wheel slip properties.** These properties use the ShakeITMotorsV3Plugin to get slip values, then filters these values a bit.&#x20;

You'll have to activate the slip effects in the ShakeIT Motors plugin page and export the values as a properties named "WheelSlip". Case sensitive. If you dont need the effect for you Arduino, just set volume to 0.&#x20;

<figure><img src="../.gitbook/assets/wheel slip.png" alt=""><figcaption></figcaption></figure>

#### Shifters

Tell the plugin what buttons you use for gear shifting. A few properties, such as traction control emulation, is dependent on this. Use the "Pressed" press type.&#x20;

<figure><img src="../.gitbook/assets/image (2) (1).png" alt=""><figcaption></figcaption></figure>
