# J-Runner with Extras + DirtyPico360
The Ultimate RGH/JTAG App

System Requirements:
- x86 based Windows PC (i386 or amd64)
- Windows Vista SP2 or later
- dotNET Framework 4.5.2
- USB 2.0 port for hardware devices

This is a fork of Octal450's J-Runner with Extras that adds support for programming glitch chips with a Raspberry Pi Pico.
This should be treated as a proof of concept, has had limited testing, and nothing about it is perfect. 
I do not plan to offer any future support for this project. With the limitations of the Pico and DirtyJtag there's not much more that can be improved upon (at least with my limited knowledge).
If you need fully tested and stable glitch chip programming please purchase a xFlash360 from a trusted reseller: https://themodshop.co/shop/xflasher

## USE AT YOUR OWN RISK
## I AM NOT RESPONSIBLE FOR ANY DAMAGE TO YOUR GLITCH CHIP OR CONSOLE
## THIS IS NOT A REPLACMENT FOR PICOFLASHER. DIRTYPICO ONLY SUPPORTS GLITCH CHIP FLASHING. PICOFLASHER FIRMWARE IS STILL NEEDED FOR NAND FLASHING.

I did not create any of the software/firmware used in this project. This is simply a suite of open source software and I've named the project DirtyPico360.

Pico-DirtyJtag v1.04 : https://github.com/phdussud/pico-dirtyJtag

UrJtag v2021.03: https://sourceforge.net/projects/urjtag/files/

J-Runner with Extras: https://github.com/Octal450/J-Runner-with-Extras

Usage:
- In the J-Runner folder navigate to /common/dirtypico for setup files
- Flash pico-DirtyJtag.uf2 to your Pico
- Open Zadig and install the LibUSB-win32 driver to the device labeled DirtyJtag
- Use the following pinout for connecting your Pico to the glitch chip:
  
| Pin name | GPIO   | Pico Pin Number |
|:---------|:-------| -          |
| TDI      | GPIO16 | 21         |
| TDO      | GPIO17 | 22         |
| TCK      | GPIO18 | 24         |
| TMS      | GPIO19 | 25         |
| GND      |        | 23         |
| VCC      |        | 36         |

- Flash a timing file as you normally would in J-Runner

If you use the build of UrJtag I compiled outside of J-Runner you will get a bunch of TDO mismatch errors when flashing the SVF. I believe this is a limitation of DirtyJtag failing during the verification portion of the SVF. I do not have enough knowledge of CPLD programming to confirm this nor to fix it. During my testing it appears these errors can be ignore and the glitch chip will still function normally.

Videos of testing:

https://www.youtube.com/watch?v=eC3C9Ab8bRU

https://www.youtube.com/watch?v=Xp1X5jC78bA

