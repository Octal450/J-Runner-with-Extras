﻿using System;
using System.Collections.Generic;

namespace JRunner.Nand
{
    class ntable
    {
        public struct _nand
        {
            private readonly int cb;
            public int CB { get { return cb; } }

            private readonly string mobo;
            public string MotherBoard { get { return mobo; } }

            private readonly int minDash;
            public int minDashVersion { get { return minDash; } }

            private readonly int maxDash;
            public int maxDashVersion { get { return maxDash; } }

            private readonly int cSeq;
            public int csequence { get { return cSeq; } }

            private readonly variables.hacktypes hack;
            public variables.hacktypes preferredHackType { get { return hack; } }

            private readonly consoles console;
            public consoles Console { get { return console; } }

            public _nand(int cb, string mobo, int mindash, int maxdash, int cseq, variables.hacktypes ht, consoles console)
            {
                this.cb = cb;
                this.mobo = mobo;
                this.minDash = mindash;
                this.maxDash = maxdash;
                this.cSeq = cseq;
                this.hack = ht;
                this.console = console;
            }
        }

        public struct _patch
        {
            public readonly int address, count, patch;
            public readonly string name, consoleMsg, messageBox;

            public _patch(string name, int address, int count, int patch, string consoleMsg, string messageBox)
            {
                this.name = name;
                this.address = address;
                this.count = count;
                this.patch = patch;
                this.consoleMsg = consoleMsg;
                this.messageBox = messageBox;
            }
        }

        public struct _temptable
        {
            public readonly string type;
            public readonly int targetCPU, targetGPU, targetEDRAM, criticalCPU, criticalGPU, criticalEDRAM;

            public _temptable(string type, int targetCPU, int targetGPU, int targetEDRAM, int criticalCPU, int criticalGPU, int criticalEDRAM)
            {
                this.type = type;
                this.targetCPU = targetCPU;
                this.targetGPU = targetGPU;
                this.targetEDRAM = targetEDRAM;
                this.criticalCPU = criticalCPU;
                this.criticalGPU = criticalGPU;
                this.criticalEDRAM = criticalEDRAM;
            }
        }

        #region table

        public static _nand[] Table = new _nand[]
        {
            // CBs
            new _nand(1888, "Xenon", 0, 7371, 2, variables.hacktypes.jtag, variables.ctypes[8]),
            new _nand(1897, "Xenon", 0, 7371, 2, variables.hacktypes.jtag, variables.ctypes[8]),
            new _nand(1902, "Xenon", 0, 7371, 2, variables.hacktypes.jtag, variables.ctypes[8]),
            new _nand(1903, "Xenon", 0, 7371, 2, variables.hacktypes.jtag, variables.ctypes[8]),
            new _nand(1920, "Xenon", 0, 7371, 4, variables.hacktypes.jtag, variables.ctypes[8]),
            new _nand(1921, "Xenon", 0, 7371, 5, variables.hacktypes.jtag, variables.ctypes[8]),
            new _nand(8192, "Xenon", 0, 7371, 5, variables.hacktypes.jtag, variables.ctypes[8]),
            new _nand(4540, "Zephyr", 0, 7371, 3, variables.hacktypes.jtag, variables.ctypes[3]),
            new _nand(4558, "Zephyr", 0, 7371, 4, variables.hacktypes.jtag, variables.ctypes[3]),
            new _nand(4570, "Zephyr", 0, 7371, 4, variables.hacktypes.jtag, variables.ctypes[3]),
            new _nand(4580, "Zephyr", 0, 7371, 5, variables.hacktypes.jtag, variables.ctypes[3]),
            new _nand(5760, "Falcon", 0, 7371, 4, variables.hacktypes.jtag, variables.ctypes[2]),
            new _nand(5761, "Falcon", 0, 7371, 4, variables.hacktypes.jtag, variables.ctypes[2]),
            new _nand(5766, "Falcon", 0, 7371, 5, variables.hacktypes.jtag, variables.ctypes[2]),
            new _nand(5770, "Falcon", 0, 7371, 5, variables.hacktypes.jtag, variables.ctypes[2]),
            new _nand(6712, "Jasper", 0, 7371, 5, variables.hacktypes.jtag, variables.ctypes[4]),
            new _nand(6723, "Jasper", 0, 7371, 5, variables.hacktypes.jtag, variables.ctypes[4]),
            new _nand(1922, "Xenon", 8498, 14699, 6, variables.hacktypes.glitch2, variables.ctypes[8]),
            new _nand(1940, "Xenon", 8498, 14699, 6, variables.hacktypes.glitch2, variables.ctypes[8]),
            new _nand(1923, "Xenon", 8498, 14699, 7, variables.hacktypes.glitch2, variables.ctypes[8]),
            new _nand(7373, "Xenon Elpis", 8498, 14699, 7, variables.hacktypes.glitch2, variables.ctypes[8]),
            new _nand(7375, "Xenon Elpis", 8498, 14699, 7, variables.hacktypes.glitch2, variables.ctypes[8]),
            new _nand(4571, "Zephyr", 8498, 14699, 6, variables.hacktypes.glitch, variables.ctypes[3]),
            new _nand(4579, "Zephyr", 8498, 14699, 6, variables.hacktypes.glitch, variables.ctypes[3]),
            new _nand(4572, "Zephyr", 8498, 14699, 7, variables.hacktypes.glitch, variables.ctypes[3]),
            new _nand(4578, "Zephyr", 8498, 14699, 7, variables.hacktypes.glitch, variables.ctypes[3]),
            new _nand(5771, "Falcon", 8498, 14699, 7, variables.hacktypes.glitch, variables.ctypes[2]),
            new _nand(6750, "Jasper", 8498, 14699, 7, variables.hacktypes.glitch, variables.ctypes[4]),
            new _nand(6751, "Jasper", 8498, 14699, 7, variables.hacktypes.glitch, variables.ctypes[4]),
            new _nand(9188, "Trinity", 8498, 14699, 1, variables.hacktypes.glitch2, variables.ctypes[1]),
            new _nand(10918, "Corona", 8498, 14699, 1, variables.hacktypes.glitch2, variables.ctypes[10]),
            new _nand(13121, "Corona", 8498, 14699, 2, variables.hacktypes.glitch2, variables.ctypes[10]),
            new _nand(1925, "Xenon", 14717, 14719, 9, variables.hacktypes.glitch2, variables.ctypes[8]),
            new _nand(1941, "Xenon", 14717, 14719, 9, variables.hacktypes.glitch2, variables.ctypes[8]),
            new _nand(1926, "Xenon", 14717, 14719, 10, variables.hacktypes.glitch2, variables.ctypes[8]),
            new _nand(7377, "Xenon Elpis", 14717, 14719, 10, variables.hacktypes.glitch2, variables.ctypes[8]),
            new _nand(4577, "Zephyr", 14717, 14719, 8, variables.hacktypes.glitch2, variables.ctypes[3]),
            new _nand(4559, "Zephyr", 14717, 14719, 9, variables.hacktypes.glitch2, variables.ctypes[3]),
            new _nand(4576, "Zephyr", 14717, 14719, 9, variables.hacktypes.glitch2, variables.ctypes[3]),
            new _nand(4560, "Zephyr", 14717, 14719, 10, variables.hacktypes.glitch2, variables.ctypes[3]),
            new _nand(4575, "Zephyr", 14717, 14719, 10, variables.hacktypes.glitch2, variables.ctypes[3]),
            new _nand(5772, "Falcon", 14717, 14719, 8, variables.hacktypes.glitch2, variables.ctypes[2]),
            new _nand(5773, "Falcon", 14717, 14719, 10, variables.hacktypes.glitch2, variables.ctypes[2]),
            new _nand(6752, "Jasper", 14717, 14719, 8, variables.hacktypes.glitch2, variables.ctypes[4]),
            new _nand(6753, "Jasper", 14717, 14719, 10, variables.hacktypes.glitch2, variables.ctypes[4]),
            new _nand(9230, "Trinity", 14717, 14719, 3, variables.hacktypes.glitch2, variables.ctypes[1]),
            new _nand(13180, "Corona", 14717, 14719, 3, variables.hacktypes.glitch2, variables.ctypes[10]),
            new _nand(1927, "Xenon", 15572, variables.latest_dashboard, 11, variables.hacktypes.glitch2, variables.ctypes[8]),
            new _nand(1942, "Xenon", 15572, variables.latest_dashboard, 11, variables.hacktypes.glitch2, variables.ctypes[8]),
            new _nand(1928, "Xenon", 15572, variables.latest_dashboard, 12, variables.hacktypes.glitch2, variables.ctypes[8]),
            new _nand(7378, "Xenon Elpis", 15572, variables.latest_dashboard, 12, variables.hacktypes.glitch2, variables.ctypes[8]),
            new _nand(4561, "Zephyr", 15572, variables.latest_dashboard, 11, variables.hacktypes.glitch2, variables.ctypes[3]),
            new _nand(4574, "Zephyr", 15572, variables.latest_dashboard, 11, variables.hacktypes.glitch2, variables.ctypes[3]),
            new _nand(4562, "Zephyr", 15572, variables.latest_dashboard, 12, variables.hacktypes.glitch2, variables.ctypes[3]),
            new _nand(4569, "Zephyr", 15572, variables.latest_dashboard, 12, variables.hacktypes.glitch2, variables.ctypes[3]),
            new _nand(5774, "Falcon", 15572, variables.latest_dashboard, 12, variables.hacktypes.glitch2, variables.ctypes[2]),
            new _nand(6754, "Jasper", 15572, variables.latest_dashboard, 12, variables.hacktypes.glitch2, variables.ctypes[4]),
            new _nand(9231, "Trinity", 15572, variables.latest_dashboard, 4, variables.hacktypes.glitch2, variables.ctypes[1]),
            new _nand(13181, "Corona", 15572, variables.latest_dashboard, 4, variables.hacktypes.glitch2, variables.ctypes[10]),
            new _nand(13182, "Corona", 15572, variables.latest_dashboard, 4, variables.hacktypes.glitch2, variables.ctypes[10]),
            new _nand(16128, "Winchester", 15572, variables.latest_dashboard, 5, variables.hacktypes.glitch2, variables.ctypes[15]),

            // SBs
            new _nand(10375, "Xenon", 4532, variables.latest_dashboard, 0, variables.hacktypes.devgl, variables.ctypes[8]),
            new _nand(14352, "Xenon", 4532, variables.latest_dashboard, 0, variables.hacktypes.devgl, variables.ctypes[8]),
            new _nand(10375, "Zephyr", 4532, variables.latest_dashboard, 0, variables.hacktypes.devgl, variables.ctypes[3]),
            new _nand(14352, "Zephyr", 4532, variables.latest_dashboard, 0, variables.hacktypes.devgl, variables.ctypes[3]),
            new _nand(10375, "Falcon", 4532, variables.latest_dashboard, 0, variables.hacktypes.devgl, variables.ctypes[2]),
            new _nand(14352, "Falcon", 4532, variables.latest_dashboard, 0, variables.hacktypes.devgl, variables.ctypes[2]),
            new _nand(10375, "Jasper", 4532, variables.latest_dashboard, 0, variables.hacktypes.devgl, variables.ctypes[4]),
            new _nand(14352, "Jasper", 4532, variables.latest_dashboard, 0, variables.hacktypes.devgl, variables.ctypes[4]),
            new _nand(10375, "Trinity", 8498, variables.latest_dashboard, 0, variables.hacktypes.devgl, variables.ctypes[1]),
            new _nand(14352, "Trinity", 8498, variables.latest_dashboard, 0, variables.hacktypes.devgl, variables.ctypes[1]),
            new _nand(14352, "Corona", 8498, variables.latest_dashboard, 0, variables.hacktypes.devgl, variables.ctypes[10]),
            new _nand(14352, "Winchester", 8498, variables.latest_dashboard, 0, variables.hacktypes.devgl, variables.ctypes[15])
        };

        public static _patch[] patchTable = new _patch[]
        {
            new _patch("FuseBlow", 0x0000C000, 0x00000000, 0x38800000, "WARNING: Nand includes fuse blowing patches!!!", "IMPORTANT:\n\nThis nand has patches that will cause fuses to be BLOWN if you run a malicious xex"),
            new _patch("XLUSB", 0x000E3A7C, 0x00000001, 0x3CE02000, "XL USB patches detected", "This NAND has XL USB patches applied\n\nUSBs not formatted via FATXplorer, and all USB memory units, will no longer work\n\nIf you don't want this, generate an image without the XL USB checked under \"Patches/Drive Patches\""),
            new _patch("XLHDD", 0x0015D8EC, 0x00000001, 0x39401000, "XL HDD patches detected", "This NAND has XL HDD patches applied\n\nIf you don't want this, generate an image without the XL HDD checked under \"Patches/Drive Patches\""),
            new _patch("UsbdSec", 0x000D8748, 0x00000002, 0x38600001, "UsbdSec patch detected", ""),
            new _patch("CoronaKeyFix", 0x00003B8C, 0x00000001, 0x389F0010, "Corona key fix detected", "")
        };

        public static _temptable[] defaultTempTable = new _temptable[]
        {
            new _temptable("Xenon", 80, 83, 85, 100, 110, 117),
            new _temptable("XenonRefurb", 80, 75, 78, 100, 100, 102),
            new _temptable("Zephyr", 80, 75, 78, 100, 100, 102),
            new _temptable("Falcon", 80, 75, 78, 100, 100, 102),
            new _temptable("Jasper", 80, 71, 73, 95, 90, 92),
            new _temptable("Tonasket", 80, 75, 77, 95, 90, 92),
            new _temptable("Trinity", 82, 78, 76, 89, 82, 82),
            new _temptable("Corona", 82, 78, 76, 89, 82, 82),
            new _temptable("Winchester", 82, 78, 76, 91, 82, 91)
        };

        #endregion

        public static variables.hacktypes getHackfromCB(int CB)
        {
            foreach (_nand n in Table)
            {
                if (n.CB == CB) return n.preferredHackType;
            }
            return variables.hacktypes.nothing;
        }

        public static int getCBFromDash(consoles c, int dash)
        {
            foreach (_nand n in Table)
            {
                if (n.Console.ID == c.ID && dash >= n.minDashVersion && dash <= n.maxDashVersion) return n.CB;
            }
            return 0;
        }

        public static bool isGlitch1Able(int CB)
        {
            variables.hacktypes prefHack = variables.hacktypes.nothing;

            foreach (_nand n in Table)
            {
                if (n.CB == CB)
                {
                    prefHack = n.preferredHackType;
                    if (prefHack == variables.hacktypes.glitch || prefHack == variables.hacktypes.jtag)
                    {
                        return true;
                    }
                    else return false;
                }
            }

            return true; // Fail true so if we don't know, allow it
        }
    }
}
