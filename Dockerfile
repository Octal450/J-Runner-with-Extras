FROM archlinux AS runner

ENV DISPLAY=:0
ENV TERM=xterm
ENV WINEARCH=wow64
ENV WINEPREFIX=/root/.wine
ENV WINEDEBUG=-all,-err+winedevice

WORKDIR /root/

# It seems that system.reg file is not saved immediately after the wine process is completed,
# so we are waiting for it to be updated, otherwise, the final image may not be aware of the installed application.
# More at: https://serverfault.com/questions/1082578/wine-in-docker-reg-add-only-keeps-effects-temporarily
RUN printf "\n[multilib]\nInclude = /etc/pacman.d/mirrorlist" >> /etc/pacman.conf \
    && pacman -Syyu --noconfirm \
    && pacman -S --noconfirm wine wine-mono xorg-server-xvfb \
    && (nohup Xvfb "$DISPLAY" -screen 0 1024x768x24 >/dev/null 2>&1 &) \
    && echo "Xvfb started: $(ps|grep Xvfb)" \
    && wait_for_change(){ f=$WINEPREFIX/system.reg; b=$(stat -c %Y "$f" 2>/dev/null); "$@"; while [ "$(stat -c %Y "$f" 2>/dev/null)" = "$b" ]; do sleep 1; done; } \
    && echo "Configure wine:" \
    && LIBGL_ALWAYS_SOFTWARE=1 wait_for_change sh -c 'WINEDLLOVERRIDES=mscoree,mshtml= wineboot -u' \
    && echo "Install vcredist:" \
    && curl -sSL -o /tmp/vcredist_x86.exe https://download.microsoft.com/download/1/6/5/165255E7-1014-4D0A-B094-B6A430A6BFFC/vcredist_x86.exe \
    && LIBGL_ALWAYS_SOFTWARE=1 wait_for_change wineconsole /tmp/vcredist_x86.exe /q \
    && echo "Add COM1 to registry:" \
    && wait_for_change wine reg add "HKLM\System\CurrentControlSet\Enum\USB\VID_600D&PID_7001\INST_001\Device Parameters" /v PortName /t REG_SZ /d COM1 /f \
    && echo "Associate log extension with notepad:" \
    && wait_for_change wine reg add "HKCR\.log" /ve /d logfile /f \
    && wait_for_change wine reg add "HKCR\logfile\shell\open\command" /ve /d "notepad.exe \"%1\"" /f \
    && echo "Cleanup:" \
    && killall Xvfb \
    && pacman -Rns --noconfirm xorg-server-xvfb \
    && printf "Y\ny\n" | pacman -Scc \
    && rm -rf /tmp/*.exe /tmp/*.msi /etc/pacman.d/gnupg/S.gpg-agent*

# Install J-Runner:
# COPY J-Runner-with-Extras-V3.4.0-r2.zip .
# RUN unzip J-Runner-with-Extras-V3.4.0-r2.zip -d jrunner
COPY jr_release jrunner

WORKDIR /root/workdir

# Run J-Runner
# CMD /bin/sh -c 'for f in ../jrunner/*; do ln -fs "$f" .; done && wine JRunner.exe'
CMD /bin/sh -c 'trap "for f in ../jrunner/*; do rm -f \"\$(basename \"\$f\")\"; done" EXIT INT TERM; \
for f in ../jrunner/*; do ln -fs "$f" .; done; \
wine JRunner.exe'
