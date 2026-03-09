# J-Runner with Extras
The Ultimate RGH/JTAG App

System Requirements:
- x86 based Windows PC (i386 or amd64)
- Windows Vista SP2 or later
- dotNET Framework 4.5.2
- USB 2.0 port for hardware devices

Docker integration (experimental and limited to PicoFlasher only).
1. User must be in dialout group.
2. Before starting container, PicoFlasher must be present in the system.
```
mkdir workdir
docker run -it --network=host \
--group-add=keep-groups \
--device=/dev/ttyACM0 \
-e DISPLAY=$DISPLAY \
-v`pwd`/workdir:/root/workdir \
-v /tmp/.X11-unix:/tmp/.X11-unix ghcr.io/hetii/j-runner:v3.4.0-r3
```

[Topic on RealModScene](https://www.realmodscene.com/index.php?/topic/10565-j-runner-with-extras-17559-built-in-timings-bugfixes-and-new-features/)

[Download Latest Stable Package](https://github.com/Octal450/J-Runner-with-Extras/releases/latest)
