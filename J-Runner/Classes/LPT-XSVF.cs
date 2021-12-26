using System;
using System.Diagnostics;
using System.IO;

namespace JRunner
{
    class LPT_XSVF
    {
        /*============================================================================
         * XSVF #define
        ============================================================================*/
        //private static string XSVF_VERSION = "1.01";
        /*============================================================================
        * XSVF Global Variables
        ============================================================================*/
        private static int DebugLevel = 0;
        private static int MAX_LEN = 36;
        private static int fileStreamindex = 0;
        private static long filesize = 0;
        static tagSXsvfInfo XsvfInfo = new tagSXsvfInfo();
        static FileStream fs;
        #region DoCMD
        /* Array of XSVF command functions.  Must follow command byte value order! */
        xsvf_pfDoCmds[] xsvf_pfDoCmd = new xsvf_pfDoCmds[]{
            xsvfDoXCOMPLETE,        /*  0 */
            xsvfDoXTDOMASK,         /*  1 */
            xsvfDoXSIR,             /*  2 */
            xsvfDoXSDR,             /*  3 */
            xsvfDoXRUNTEST,         /*  4 */
            xsvfDoIllegalCmd,       /*  5 */
            xsvfDoIllegalCmd,       /*  6 */
            xsvfDoXREPEAT,          /*  7 */
            xsvfDoXSDRSIZE,         /*  8 */
            xsvfDoXSDRTDO,          /*  9 */
            xsvfDoXSETSDRMASKS,     /* 10 */
            xsvfDoXSDRINC,          /* 11 */
            xsvfDoXSDRBCE,          /* 12 */
            xsvfDoXSDRBCE,          /* 13 */
            xsvfDoXSDRBCE,          /* 14 */
            xsvfDoXSDRTDOBCE,       /* 15 */
            xsvfDoXSDRTDOBCE,       /* 16 */
            xsvfDoXSDRTDOBCE,       /* 17 */
            xsvfDoXSTATE,           /* 18 */
            xsvfDoXENDXR,           /* 19 */
            xsvfDoXENDXR,           /* 20 */
            xsvfDoXSIR2,            /* 21 */
            xsvfDoXCOMMENT,         /* 22 */
            xsvfDoXWAIT             /* 23 */
            /* Insert new command functions here */
        };
        #endregion

        /*============================================================================
         * XSVF Type Declarations
        ============================================================================*/

        /*****************************************************************************
        * Struct:       SXsvfInfo
        * Description:  This structure contains all of the data used during the
        *               execution of the XSVF.  Some data is persistent, predefined
        *               information (e.g. lRunTestTime).  The bulk of this struct's
        *               size is due to the lenVal structs (defined in lenval.h)
        *               which contain buffers for the active shift data.  The MAX_LEN
        *               #define in lenval.h defines the size of these buffers.
        *               These buffers must be large enough to store the longest
        *               shift data in your XSVF file.  For example:
        *                   MAX_LEN >= ( longest_shift_data_in_bits / 8 )
        *               Because the lenVal struct dominates the space usage of this
        *               struct, the rough size of this struct is:
        *                   sizeof( SXsvfInfo ) ~= MAX_LEN * 7 (number of lenVals)
        *               xsvfInitialize() contains initialization code for the data
        *               in this struct.
        *               xsvfCleanup() contains cleanup code for the data in this
        *               struct.
        *****************************************************************************/
        struct tagSXsvfInfo
        {
            /* XSVF status information */
            public int ucComplete;          /* 0 = running; 1 = complete */
            public XSVFCOMMAND ucCommand;   /* Current XSVF command byte */
            public long lCommandCount;      /* Number of commands processed */
            public XSVFERROR iErrorCode;    /* An error code. 0 = no error. */

            /* TAP state/sequencing information */
            public XSVFTAPSTATE ucTapState;      /* Current TAP state */
            public XSVFTAPSTATE ucEndIR;         /* ENDIR TAP state (See SVF) */
            public XSVFTAPSTATE ucEndDR;         /* ENDDR TAP state (See SVF) */

            /* RUNTEST information */
            public byte ucMaxRepeat;        /* Max repeat loops (for xc9500/xl) */
            public long lRunTestTime;       /* Pre-specified RUNTEST time (usec) */

            /* Shift Data Info and Buffers */
            public long lShiftLengthBits;   /* Len. current shift data in bits */
            public short sShiftLengthBytes; /* Len. current shift data in bytes */

            public lenval lvTdi;            /* Current TDI shift data */
            public lenval lvTdoExpected;    /* Expected TDO shift data */
            public lenval lvTdoCaptured;    /* Captured TDO shift data */
            public lenval lvTdoMask;        /* TDO mask: 0=dontcare; 1=compare */

            /* XSDRINC Data Buffers */
            public lenval lvAddressMask;    /* Address mask for XSDRINC */
            public lenval lvDataMask;       /* Data mask for XSDRINC */
            public lenval lvNextData;       /* Next data for XSDRINC */
        }

        struct lenval
        {
            public short len;   /* number of chars in this value */
            public byte[] val;         /* bytes of data */
            public lenval(short len, byte[] val)
            {
                this.len = len;
                this.val = val;
            }
        }

        /* Declare pointer to functions that perform XSVF commands */
        delegate XSVFERROR xsvf_pfDoCmds();

        /*============================================================================
        * XSVF Command Bytes
        ============================================================================*/
        enum XSVFCOMMAND
        {

            /* encodings of xsvf instructions */
            XCOMPLETE = 0,  // 0x00
            XTDOMASK = 1,   // 0x01
            XSIR = 2,       // 0x01
            XSDR = 3,       // unused
            XRUNTEST = 4,   // 0x04
            /* Reserved = 5, */
            /* Reserved = 6, */
            XREPEAT = 7,    // 0x07
            XSDRSIZE = 8,   // 0x08
            XSDRTDO = 9,    // 0x09
            XSETSDRMASKS = 10,  // unused
            XSDRINC = 11,   // unused
            XSDRB = 12,     // unused
            XSDRC = 13,     // unused
            XSDRE = 14,     // unused
            XSDRTDOB = 15,  // unused
            XSDRTDOC = 16,  // unused
            XSDRTDOE = 17,  // unused
            XSTATE = 18,    // 0x12
            XENDIR = 19,    // 0x13
            XENDDR = 20,    // 0x14
            XSIR2 = 21,     // unused
            XCOMMENT = 22,  // unused
            XWAIT = 23,     // 0x17     
            /* Insert new commands here */
            /* and add corresponding xsvfDoCmd function to xsvf_pfDoCmd below. */
            XLASTCMD = 24   /* Last command marker */

        }

        /*============================================================================
        * XSVF Command Parameter Values
        ============================================================================*/
        enum XSVFSTATE
        {
            XSTATE_RESET = 0,   /* 4.00 parameter for XSTATE */
            XSTATE_RUNTEST = 1  /* 4.00 parameter for XSTATE */
        }

        enum XSVFENDXR
        {
            RUNTEST = 0, /* 4.04 parameter for XENDIR/DR */
            PAUSE = 1    /* 4.04 parameter for XENDIR/DR */
        }

        /* TAP states */
        enum XSVFTAPSTATE : byte
        {
            RESET = 0x00,
            RUNTEST = 0x01,   /* a.k.a. IDLE */
            SELECTDR = 0x02,
            CAPTUREDR = 0x03,
            SHIFTDR = 0x04,
            EXIT1DR = 0x05,
            PAUSEDR = 0x06,
            EXIT2DR = 0x07,
            UPDATEDR = 0x08,
            IRSTATES = 0x09,  /* All IR states begin here */
            SELECTIR = 0x09,
            CAPTUREIR = 0x0A,
            SHIFTIR = 0x0B,
            EXIT1IR = 0x0C,
            PAUSEIR = 0x0D,
            EXIT2IR = 0x0E,
            UPDATEIR = 0x0F
        }

        enum XSVFERROR : byte
        {
            /* 4.04 [NEW] Error codes for xsvfExecute. */
            /* Must #define XSVF_SUPPORT_ERRORCODES in micro.c to get these codes */
            NONE = 0,
            UNKNOWN = 1,
            TDOMISMATCH = 2,
            MAXRETRIES = 3,   /* TDO mismatch after max retries */
            ILLEGALCMD = 4,
            ILLEGALSTATE = 5,
            DATAOVERFLOW = 6,  /* Data > lenVal MAX_LEN buffer size*/
            /* Insert new errors here */
            LAST = 7
        }

        /*============================================================================
        * Utility Functions
        ============================================================================*/

        /*****************************************************************************
        * Function:     xsvfInfoInit
        * Description:  Initialize the xsvfInfo data.
        * Parameters:   pXsvfInfo   - ptr to the XSVF info structure.
        * Returns:      int         - 0 = success; otherwise error.
        *****************************************************************************/
        static XSVFERROR xsvfInfoInit()
        {
            XsvfInfo.ucComplete = 0;
            XsvfInfo.ucCommand = XSVFCOMMAND.XCOMPLETE;
            XsvfInfo.lCommandCount = 0;
            XsvfInfo.iErrorCode = XSVFERROR.NONE;
            XsvfInfo.ucMaxRepeat = 0;
            XsvfInfo.ucTapState = XSVFTAPSTATE.RESET;
            XsvfInfo.ucEndIR = XSVFTAPSTATE.RUNTEST;
            XsvfInfo.ucEndDR = XSVFTAPSTATE.RUNTEST;
            XsvfInfo.lShiftLengthBits = 0L;
            XsvfInfo.sShiftLengthBytes = 0;
            XsvfInfo.lRunTestTime = 0L;

            return (0);
        }

        /*****************************************************************************
        * Function:     xsvfGetAsNumBytes
        * Description:  Calculate the number of bytes the given number of bits
        *               consumes.
        * Parameters:   lNumBits    - the number of bits.
        * Returns:      short       - the number of bytes to store the number of bits.
        *****************************************************************************/
        static short xsvfGetAsNumBytes(long lNumBits)
        {
            return ((short)((lNumBits + 7L) / 8L));
        }

        /*****************************************************************************
        * Function:     xsvfTmsTransition
        * Description:  Apply TMS and transition TAP controller by applying one TCK
        *               cycle.
        * Parameters:   sTms    - new TMS value.
        * Returns:      void.
        *****************************************************************************/
        static void xsvfTmsTransition(byte sTms)
        {
            PortInterop.setPort(PortInterop.TMS, sTms);
            PortInterop.setPort(PortInterop.TCK, 0);
            PortInterop.setPort(PortInterop.TCK, 1);
        }

        /*****************************************************************************
        * Function:     xsvfGotoTapState
        * Description:  From the current TAP state, go to the named TAP state.
        *               A target state of RESET ALWAYS causes TMS reset sequence.
        *               All SVF standard stable state paths are supported.
        *               All state transitions are supported except for the following
        *               which cause an XSVF_ERROR_ILLEGALSTATE:
        *                   - Target==DREXIT2;  Start!=DRPAUSE
        *                   - Target==IREXIT2;  Start!=IRPAUSE
        * Parameters:   pucTapState     - Current TAP state; returns final TAP state.
        *               ucTargetState   - New target TAP state.
        * Returns:      int             - 0 = success; otherwise error.
        *****************************************************************************/
        static XSVFERROR xsvfGotoTapState(ref XSVFTAPSTATE pucTapState, XSVFTAPSTATE ucTargetState)
        {
            int i;
            XSVFERROR iErrorCode;

            iErrorCode = XSVFERROR.NONE;
            if (ucTargetState == XSVFTAPSTATE.RESET)
            {
                /* If RESET, always perform TMS reset sequence to reset/sync TAPs */
                xsvfTmsTransition(1);
                for (i = 0; i < 5; ++i)
                {
                    PortInterop.setPort(PortInterop.TCK, 0);
                    PortInterop.setPort(PortInterop.TCK, 1);
                }
                pucTapState = XSVFTAPSTATE.RESET;
#if Debug
                WriteLine(3, "   TMS Reset Sequence -> Test-Logic-Reset");
                WriteLine(3, "   TAP State = {0}", (XSVFTAPSTATE)ucTargetState);
#endif
            }
            else if ((ucTargetState != pucTapState) &&
                      (((ucTargetState == XSVFTAPSTATE.EXIT2DR) && (pucTapState != XSVFTAPSTATE.PAUSEDR)) ||
                        ((ucTargetState == XSVFTAPSTATE.EXIT2IR) && (pucTapState != XSVFTAPSTATE.PAUSEIR))))
            {
                /* Trap illegal TAP state path specification */
                iErrorCode = XSVFERROR.ILLEGALSTATE;
            }
            else
            {
                if (ucTargetState == pucTapState)
                {
                    /* Already in target state.  Do nothing except when in DRPAUSE
                       or in IRPAUSE to comply with SVF standard */
                    if (ucTargetState == XSVFTAPSTATE.PAUSEDR)
                    {
                        xsvfTmsTransition(1);
                        pucTapState = XSVFTAPSTATE.EXIT2DR;
#if Debug
                        WriteLine(3, "   TAP State = {0}", (XSVFTAPSTATE)pucTapState);
#endif
                    }
                    else if (ucTargetState == XSVFTAPSTATE.PAUSEIR)
                    {
                        xsvfTmsTransition(1);
                        pucTapState = XSVFTAPSTATE.EXIT2IR;
#if Debug
                        WriteLine(3, "   TAP State = {0}", (XSVFTAPSTATE)pucTapState);
#endif
                    }
                }

                /* Perform TAP state transitions to get to the target state */
                while (ucTargetState != pucTapState)
                {
                    switch (pucTapState)
                    {
                        case XSVFTAPSTATE.RESET:
                            xsvfTmsTransition(0);
                            pucTapState = XSVFTAPSTATE.RUNTEST;
                            break;
                        case XSVFTAPSTATE.RUNTEST:
                            xsvfTmsTransition(1);
                            pucTapState = XSVFTAPSTATE.SELECTDR;
                            break;
                        case XSVFTAPSTATE.SELECTDR:
                            if (ucTargetState >= XSVFTAPSTATE.IRSTATES)
                            {
                                xsvfTmsTransition(1);
                                pucTapState = XSVFTAPSTATE.SELECTIR;
                            }
                            else
                            {
                                xsvfTmsTransition(0);
                                pucTapState = XSVFTAPSTATE.CAPTUREDR;
                            }
                            break;
                        case XSVFTAPSTATE.CAPTUREDR:
                            if (ucTargetState == XSVFTAPSTATE.SHIFTDR)
                            {
                                xsvfTmsTransition(0);
                                pucTapState = XSVFTAPSTATE.SHIFTDR;
                            }
                            else
                            {
                                xsvfTmsTransition(1);
                                pucTapState = XSVFTAPSTATE.EXIT1DR;
                            }
                            break;
                        case XSVFTAPSTATE.SHIFTDR:
                            xsvfTmsTransition(1);
                            pucTapState = XSVFTAPSTATE.EXIT1DR;
                            break;
                        case XSVFTAPSTATE.EXIT1DR:
                            if (ucTargetState == XSVFTAPSTATE.PAUSEDR)
                            {
                                xsvfTmsTransition(0);
                                pucTapState = XSVFTAPSTATE.PAUSEDR;
                            }
                            else
                            {
                                xsvfTmsTransition(1);
                                pucTapState = XSVFTAPSTATE.UPDATEDR;
                            }
                            break;
                        case XSVFTAPSTATE.PAUSEDR:
                            xsvfTmsTransition(1);
                            pucTapState = XSVFTAPSTATE.EXIT2DR;
                            break;
                        case XSVFTAPSTATE.EXIT2DR:
                            if (ucTargetState == XSVFTAPSTATE.SHIFTDR)
                            {
                                xsvfTmsTransition(0);
                                pucTapState = XSVFTAPSTATE.SHIFTDR;
                            }
                            else
                            {
                                xsvfTmsTransition(1);
                                pucTapState = XSVFTAPSTATE.UPDATEDR;
                            }
                            break;
                        case XSVFTAPSTATE.UPDATEDR:
                            if (ucTargetState == XSVFTAPSTATE.RUNTEST)
                            {
                                xsvfTmsTransition(0);
                                pucTapState = XSVFTAPSTATE.RUNTEST;
                            }
                            else
                            {
                                xsvfTmsTransition(1);
                                pucTapState = XSVFTAPSTATE.SELECTDR;
                            }
                            break;
                        case XSVFTAPSTATE.SELECTIR:
                            xsvfTmsTransition(0);
                            pucTapState = XSVFTAPSTATE.CAPTUREIR;
                            break;
                        case XSVFTAPSTATE.CAPTUREIR:
                            if (ucTargetState == XSVFTAPSTATE.SHIFTIR)
                            {
                                xsvfTmsTransition(0);
                                pucTapState = XSVFTAPSTATE.SHIFTIR;
                            }
                            else
                            {
                                xsvfTmsTransition(1);
                                pucTapState = XSVFTAPSTATE.EXIT1IR;
                            }
                            break;
                        case XSVFTAPSTATE.SHIFTIR:
                            xsvfTmsTransition(1);
                            pucTapState = XSVFTAPSTATE.EXIT1IR;
                            break;
                        case XSVFTAPSTATE.EXIT1IR:
                            if (ucTargetState == XSVFTAPSTATE.PAUSEIR)
                            {
                                xsvfTmsTransition(0);
                                pucTapState = XSVFTAPSTATE.PAUSEIR;
                            }
                            else
                            {
                                xsvfTmsTransition(1);
                                pucTapState = XSVFTAPSTATE.UPDATEIR;
                            }
                            break;
                        case XSVFTAPSTATE.PAUSEIR:
                            xsvfTmsTransition(1);
                            pucTapState = XSVFTAPSTATE.EXIT2IR;
                            break;
                        case XSVFTAPSTATE.EXIT2IR:
                            if (ucTargetState == XSVFTAPSTATE.SHIFTIR)
                            {
                                xsvfTmsTransition(0);
                                pucTapState = XSVFTAPSTATE.SHIFTIR;
                            }
                            else
                            {
                                xsvfTmsTransition(1);
                                pucTapState = XSVFTAPSTATE.UPDATEIR;
                            }
                            break;
                        case XSVFTAPSTATE.UPDATEIR:
                            if (ucTargetState == XSVFTAPSTATE.RUNTEST)
                            {
                                xsvfTmsTransition(0);
                                pucTapState = XSVFTAPSTATE.RUNTEST;
                            }
                            else
                            {
                                xsvfTmsTransition(1);
                                pucTapState = XSVFTAPSTATE.SELECTDR;
                            }
                            break;
                        default:
                            iErrorCode = XSVFERROR.ILLEGALSTATE;
                            pucTapState = ucTargetState;    /* Exit while loop */
                            break;
                    }
#if Debug
                    WriteLine(3, "   TAP State = {0}", (XSVFTAPSTATE)pucTapState);
#endif
                }
            }

            return (iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfShiftOnly
        * Description:  Assumes that starting TAP state is SHIFT-DR or SHIFT-IR.
        *               Shift the given TDI data into the JTAG scan chain.
        *               Optionally, save the TDO data shifted out of the scan chain.
        *               Last shift cycle is special:  capture last TDO, set last TDI,
        *               but does not pulse TCK.  Caller must pulse TCK and optionally
        *               set TMS=1 to exit shift state.
        * Parameters:   lNumBits        - number of bits to shift.
        *               plvTdi          - ptr to lenval for TDI data.
        *               plvTdoCaptured  - ptr to lenval for storing captured TDO data.
        *               iExitShift      - 1=exit at end of shift; 0=stay in Shift-DR.
        * Returns:      void.
        *****************************************************************************/
        static void xsvfShiftOnly(long lNumBits, ref lenval plvTdi, ref lenval plvTdoCaptured, bool iExitShift)
        {
            byte[] pucTdi = plvTdi.val;
            //Array.Reverse(pucTdi);
            byte ucTdiByte;
            byte ucTdoByte;
            byte ucTdoBit;
            int i, pucTdiIndex = plvTdi.len, pucTdoIndex = 0;

            /* assert( ( ( lNumBits + 7 ) / 8 ) == plvTdi->len ); */

            /* Initialize TDO storage len == TDI len */
            if (plvTdoCaptured.len >= 0)
            {
                plvTdoCaptured.len = plvTdi.len;
                plvTdoCaptured.val = new byte[plvTdi.len];
                pucTdoIndex = plvTdi.len;
            }

            /* Shift LSB first.  val[N-1] == LSB.  val[0] == MSB. */
            while (lNumBits != 0)
            {
                ucTdiByte = pucTdi[--pucTdiIndex];
                ucTdoByte = 0x00;
                for (i = 0; lNumBits != 0 && i < 8; i++)
                {
                    --lNumBits;
                    if (iExitShift && lNumBits == 0)
                    {
                        /* Exit Shift-DR state */
                        PortInterop.setPort(PortInterop.TMS, 1);
                    }

                    /* Set the new TDI value */
                    PortInterop.setPort(PortInterop.TDI, (byte)((ucTdiByte >> i) & 1));
                    //ucTdiByte >>= 1;

                    /* Set TCK low */
                    PortInterop.setPort(PortInterop.TCK, 0);

                    if (pucTdoIndex != 0)
                    {
                        /* Save the TDO value */
                        ucTdoBit = PortInterop.readTDOBit();
                        ucTdoByte |= (byte)(ucTdoBit << i);
                    }

                    /* Set TCK high */
                    PortInterop.setPort(PortInterop.TCK, 1);
                }
                /* Save the TDO byte value */
                if (pucTdoIndex != 0)
                {
                    (plvTdoCaptured.val[--pucTdoIndex]) = ucTdoByte;
                }
            }
        }

        /*****************************************************************************
        * Function:     xsvfShift
        * Description:  Goes to the given starting TAP state.
        *               Calls xsvfShiftOnly to shift in the given TDI data and
        *               optionally capture the TDO data.
        *               Compares the TDO captured data against the TDO expected
        *               data.
        *               If a data mismatch occurs, then executes the exception
        *               handling loop upto ucMaxRepeat times.
        * Parameters:   pucTapState     - Ptr to current TAP state.
        *               ucStartState    - Starting shift state: Shift-DR or Shift-IR.
        *               lNumBits        - number of bits to shift.
        *               plvTdi          - ptr to lenval for TDI data.
        *               plvTdoCaptured  - ptr to lenval for storing TDO data.
        *               plvTdoExpected  - ptr to expected TDO data.
        *               plvTdoMask      - ptr to TDO mask.
        *               ucEndState      - state in which to end the shift.
        *               lRunTestTime    - amount of time to wait after the shift.
        *               ucMaxRepeat     - Maximum number of retries on TDO mismatch.
        * Returns:      int             - 0 = success; otherwise TDO mismatch.
        * Notes:        XC9500XL-only Optimization:
        *               Skip the waitTime() if plvTdoMask->val[0:plvTdoMask->len-1]
        *               is NOT all zeros and sMatch==1.
        *****************************************************************************/
        static XSVFERROR xsvfShift(ref XSVFTAPSTATE pucTapState, XSVFTAPSTATE ucStartState, long lNumBits, ref lenval plvTdi,
                       ref lenval plvTdoCaptured, ref lenval plvTdoExpected, ref lenval plvTdoMask, XSVFTAPSTATE ucEndState,
                       long lRunTestTime, byte ucMaxRepeat)
        {
            XSVFERROR iErrorCode;
            bool iMismatch = false;
            byte ucRepeat;
            bool iExitShift;

            iErrorCode = XSVFERROR.NONE;
            ucRepeat = 0;
            iExitShift = (ucStartState != ucEndState);

#if Debug
            WriteLine(3, "   Shift Length = {0}", lNumBits);
            WriteLine(4, "    TDI          = 0x{0}", ByteArrayToString(plvTdi.val));
            WriteLine(4, "    TDO Expected =   {0}", ByteArrayToString(plvTdoExpected.val));
#endif

            if (lNumBits == 0)
            {
                /* Compatibility with XSVF2.00:  XSDR 0 = no shift, but wait in RTI */
                if (lRunTestTime != 0)
                {
                    /* Wait for prespecified XRUNTEST time */
                    xsvfGotoTapState(ref pucTapState, XSVFTAPSTATE.RUNTEST);
#if Debug
                    WriteLine(3, "   Wait = {0} usec", lRunTestTime);
#endif
                    PortInterop.waitTime(lRunTestTime);
                }
            }
            else
            {
                do
                {
                    /* Goto Shift-DR or Shift-IR */
                    xsvfGotoTapState(ref pucTapState, ucStartState);
                    /* Shift TDI and capture TDO */
                    xsvfShiftOnly(lNumBits, ref plvTdi, ref plvTdoCaptured, iExitShift);
                    if (plvTdoExpected.len != 0 && plvTdoExpected.val != null)
                    {
                        /* Compare TDO data to expected TDO data */
                        if (EqualLenVal(plvTdoExpected, plvTdoCaptured, plvTdoMask) == 0)
                        {
                            iMismatch = true;
                        }
                        else iMismatch = false;
                    }

                    if (iExitShift)
                    {
                        /* Update TAP state:  Shift->Exit */
                        ++(pucTapState);
#if Debug
                        WriteLine(3, "   TAP State = {0}", (XSVFTAPSTATE)pucTapState);
#endif
                        if (iMismatch && lRunTestTime != 0 && (ucRepeat < ucMaxRepeat))
                        {
#if Debug
                            WriteLine(4, "    TDO Expected =   {0}", ByteArrayToString(plvTdoExpected.val));
                            WriteLine(4, "    TDO Captured = 0x{0}", ByteArrayToString(plvTdoCaptured.val));
                            WriteLine(4, "    TDO Mask     = 0x{0}", ByteArrayToString(plvTdoMask.val));
                            WriteLine(3, "   Retry #%d\n", (ucRepeat + 1));
#endif
                            /* Do exception handling retry - ShiftDR only */
                            xsvfGotoTapState(ref pucTapState, XSVFTAPSTATE.PAUSEDR);
                            /* Shift 1 extra bit */
                            xsvfGotoTapState(ref pucTapState, XSVFTAPSTATE.SHIFTDR);
                            /* Increment RUNTEST time by an additional 25% */
                            lRunTestTime += (lRunTestTime >> 2);
                        }
                        else
                        {
                            /* Do normal exit from Shift-XR */
                            xsvfGotoTapState(ref pucTapState, ucEndState);
                        }

                        if (lRunTestTime != 0)
                        {
                            /* Wait for prespecified XRUNTEST time */
                            xsvfGotoTapState(ref pucTapState, XSVFTAPSTATE.RUNTEST);
#if Debug
                            WriteLine(3, "   Wait = {0} usec\n", lRunTestTime);
#endif
                            PortInterop.waitTime(lRunTestTime);
                        }
                    }
                } while (iMismatch && (ucRepeat++ < ucMaxRepeat));
            }

            if (iMismatch)
            {
#if Debug
                WriteLine(1, " TDO Expected =   {0}", ByteArrayToString(plvTdoExpected.val));
                WriteLine(1, " TDO Captured = 0x{0}", ByteArrayToString(plvTdoCaptured.val));
                WriteLine(1, " TDO Mask     = 0x{0}", ByteArrayToString(plvTdoMask.val));
#endif
                if (ucMaxRepeat != 0 && (ucRepeat > ucMaxRepeat))
                {
                    iErrorCode = XSVFERROR.MAXRETRIES;
                }
                else
                {
                    iErrorCode = XSVFERROR.TDOMISMATCH;
                }
            }

            return (iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfBasicXSDRTDO
        * Description:  Get the XSDRTDO parameters and execute the XSDRTDO command.
        *               This is the common function for all XSDRTDO commands.
        * Parameters:   pucTapState         - Current TAP state.
        *               lShiftLengthBits    - number of bits to shift.
        *               sShiftLengthBytes   - number of bytes to read.
        *               plvTdi              - ptr to lenval for TDI data.
        *               lvTdoCaptured       - ptr to lenval for storing TDO data.
        *               iEndState           - state in which to end the shift.
        *               lRunTestTime        - amount of time to wait after the shift.
        *               ucMaxRepeat         - maximum xc9500/xl retries.
        * Returns:      int                 - 0 = success; otherwise TDO mismatch.
        *****************************************************************************/
        static XSVFERROR xsvfBasicXSDRTDO(ref XSVFTAPSTATE pucTapState, long lShiftLengthBits, short sShiftLengthBytes,
                              ref lenval plvTdi, ref lenval plvTdoCaptured, ref lenval plvTdoExpected,
                              ref lenval plvTdoMask, XSVFTAPSTATE ucEndState, long lRunTestTime, byte ucMaxRepeat)
        {
            readVal(ref plvTdi, sShiftLengthBytes);
            if (plvTdoExpected.len != -1)
            {
                readVal(ref plvTdoExpected, sShiftLengthBytes);
            }
            return (xsvfShift(ref XsvfInfo.ucTapState, XSVFTAPSTATE.SHIFTDR, lShiftLengthBits,
                               ref plvTdi, ref plvTdoCaptured, ref plvTdoExpected, ref plvTdoMask,
                               ucEndState, lRunTestTime, ucMaxRepeat));
        }

        /*****************************************************************************
        * Function:     xsvfDoSDRMasking
        * Description:  Update the data value with the next XSDRINC data and address.
        * Example:      dataVal=0x01ff, nextData=0xab, addressMask=0x0100,
        *               dataMask=0x00ff, should set dataVal to 0x02ab
        * Parameters:   plvTdi          - The current TDI value.
        *               plvNextData     - the next data value.
        *               plvAddressMask  - the address mask.
        *               plvDataMask     - the data mask.
        * Returns:      void.
        *****************************************************************************/
        static void xsvfDoSDRMasking(ref lenval plvTdi, lenval plvNextData, lenval plvAddressMask, lenval plvDataMask)
        {
            int i;
            byte ucTdi;
            byte ucTdiMask;
            byte ucDataMask;
            byte ucNextData;
            byte ucNextMask;
            short sNextData;

            /* add the address Mask to dataVal and return as a new dataVal */
            addVal(ref plvTdi, plvTdi, plvAddressMask);

            ucNextData = 0;
            ucNextMask = 0;
            sNextData = plvNextData.len;
            for (i = plvDataMask.len - 1; i >= 0; --i)
            {
                /* Go through data mask in reverse order looking for mask (1) bits */
                ucDataMask = plvDataMask.val[i];
                if (ucDataMask != 0x00)
                {
                    /* Retrieve the corresponding TDI byte value */
                    ucTdi = plvTdi.val[i];

                    /* For each bit in the data mask byte, look for 1's */
                    ucTdiMask = 1;
                    while (ucDataMask != 0x00)
                    {
                        if ((ucDataMask & 1) != 0x00)
                        {
                            if (ucNextMask != 0x00)
                            {
                                /* Get the next data byte */
                                ucNextData = plvNextData.val[--sNextData];
                                ucNextMask = 1;
                            }

                            /* Set or clear the data bit according to the next data */
                            if ((ucNextData & ucNextMask) != 0x00)
                            {
                                ucTdi |= ucTdiMask;       /* Set bit */
                            }
                            else
                            {
                                ucTdi &= (byte)(~ucTdiMask);  /* Clear bit */
                            }

                            /* Update the next data */
                            ucNextMask <<= 1;
                        }
                        ucTdiMask <<= 1;
                        ucDataMask >>= 1;
                    }

                    /* Update the TDI value */
                    plvTdi.val[i] = ucTdi;
                }
            }
        }


        /*============================================================================
        * XSVF Command Functions (type = TXsvfDoCmdFuncPtr)
        * These functions update pXsvfInfo->iErrorCode only on an error.
        * Otherwise, the error code is left alone.
        * The function returns the error code from the function.
        ============================================================================*/


        /// <summary>
        /// Function:     xsvfDoIllegalCmd
        /// Description:  Function place holder for illegal/unsupported commands.
        /// </summary>
        /// <returns>int         - 0 = success;  non-zero = error.</returns>
        static XSVFERROR xsvfDoIllegalCmd()
        {
            Console.WriteLine("ERROR:  Encountered unsupported command #{0} ({1})",
                             ((byte)(XsvfInfo.ucCommand)),
                             (XsvfInfo.ucCommand.ToString()));
            XsvfInfo.iErrorCode = XSVFERROR.ILLEGALCMD;
            return (XsvfInfo.iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfDoXCOMPLETE
        * Description:  XCOMPLETE (no parameters)
        *               Update complete status for XSVF player.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXCOMPLETE()
        {
            XsvfInfo.ucComplete = 1;
            return (XSVFERROR.NONE);
        }

        /*****************************************************************************
        * Function:     xsvfDoXTDOMASK
        * Description:  XTDOMASK <lenVal.TdoMask[XSDRSIZE]>
        *               Prespecify the TDO compare mask.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXTDOMASK()
        {
            readVal(ref XsvfInfo.lvTdoMask, XsvfInfo.sShiftLengthBytes);
#if Debug
            WriteLine(4, "    TDO Mask     = 0x{0}", ByteArrayToString(XsvfInfo.lvTdoMask.val));
            WriteLine(4, "");
#endif
            return (XSVFERROR.NONE);
        }

        /*****************************************************************************
        * Function:     xsvfDoXSIR
        * Description:  XSIR <(byte)shiftlen> <lenVal.TDI[shiftlen]>
        *               Get the instruction and shift the instruction into the TAP.
        *               If prespecified XRUNTEST!=0, goto RUNTEST and wait after
        *               the shift for XRUNTEST usec.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXSIR()
        {
            byte ucShiftIrBits;
            short sShiftIrBytes;
            XSVFERROR iErrorCode;

            /* Get the shift length and store */
            ucShiftIrBits = readByte();
            sShiftIrBytes = xsvfGetAsNumBytes(ucShiftIrBits);
#if Debug
            WriteLine(3, "   XSIR length = {0}", (ucShiftIrBits));
#endif
            if (sShiftIrBytes > MAX_LEN)
            {
                iErrorCode = XSVFERROR.DATAOVERFLOW;
            }
            else
            {
                lenval nill = new lenval(-1, null);
                /* Get and store instruction to shift in */
                readVal(ref XsvfInfo.lvTdi, xsvfGetAsNumBytes(ucShiftIrBits));

                /* Shift the data */
                iErrorCode = xsvfShift(ref XsvfInfo.ucTapState, XSVFTAPSTATE.SHIFTIR, ucShiftIrBits, ref XsvfInfo.lvTdi,
                    /*plvTdoCaptured*/ref nill, /*plvTdoExpected*/ref nill,
                    /*plvTdoMask*/ref nill, XsvfInfo.ucEndIR, XsvfInfo.lRunTestTime, /*ucMaxRepeat*/0);
            }

            if (iErrorCode != XSVFERROR.NONE)
            {
                XsvfInfo.iErrorCode = iErrorCode;
            }
            return (iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfDoXSIR2
        * Description:  XSIR <(2-byte)shiftlen> <lenVal.TDI[shiftlen]>
        *               Get the instruction and shift the instruction into the TAP.
        *               If prespecified XRUNTEST!=0, goto RUNTEST and wait after
        *               the shift for XRUNTEST usec.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXSIR2()
        {
            long lShiftIrBits;
            short sShiftIrBytes;
            XSVFERROR iErrorCode;

            /* Get the shift length and store */
            readVal(ref XsvfInfo.lvTdi, 2);
            lShiftIrBits = value(XsvfInfo.lvTdi.val);
            sShiftIrBytes = xsvfGetAsNumBytes(lShiftIrBits);
#if Debug
            WriteLine(3, "   XSIR2 length = {0}", lShiftIrBits);
#endif
            if (sShiftIrBytes > MAX_LEN)
            {
                iErrorCode = XSVFERROR.DATAOVERFLOW;
            }
            else
            {
                lenval nill = new lenval(-1, null);
                /* Get and store instruction to shift in */
                readVal(ref XsvfInfo.lvTdi, xsvfGetAsNumBytes(lShiftIrBits));

                /* Shift the data */
                iErrorCode = xsvfShift(ref XsvfInfo.ucTapState, XSVFTAPSTATE.SHIFTIR,
                                         lShiftIrBits, ref XsvfInfo.lvTdi,
                    /*plvTdoCaptured*/ref nill, /*plvTdoExpected*/ref nill,
                    /*plvTdoMask*/ref nill, XsvfInfo.ucEndIR,
                                         XsvfInfo.lRunTestTime, /*ucMaxRepeat*/0);
            }

            if (iErrorCode != XSVFERROR.NONE)
            {
                XsvfInfo.iErrorCode = iErrorCode;
            }
            return (iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfDoXSDR
        * Description:  XSDR <lenVal.TDI[XSDRSIZE]>
        *               Shift the given TDI data into the JTAG scan chain.
        *               Compare the captured TDO with the expected TDO from the
        *               previous XSDRTDO command using the previously specified
        *               XTDOMASK.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXSDR()
        {
            XSVFERROR iErrorCode;
            readVal(ref XsvfInfo.lvTdi, XsvfInfo.sShiftLengthBytes);
            /* use TDOExpected from last XSDRTDO instruction */
            iErrorCode = xsvfShift(ref XsvfInfo.ucTapState, XSVFTAPSTATE.SHIFTDR,
                                     XsvfInfo.lShiftLengthBits, ref XsvfInfo.lvTdi,
                                     ref XsvfInfo.lvTdoCaptured,
                                     ref XsvfInfo.lvTdoExpected,
                                     ref XsvfInfo.lvTdoMask, XsvfInfo.ucEndDR,
                                     XsvfInfo.lRunTestTime, XsvfInfo.ucMaxRepeat);
            if (iErrorCode != XSVFERROR.NONE)
            {
                XsvfInfo.iErrorCode = iErrorCode;
            }
            return (iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfDoXRUNTEST
        * Description:  XRUNTEST <uint32>
        *               Prespecify the XRUNTEST wait time for shift operations.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXRUNTEST()
        {
            readVal(ref XsvfInfo.lvTdi, 4);
            XsvfInfo.lRunTestTime = value(XsvfInfo.lvTdi.val);
#if Debug
            WriteLine(3, "   XRUNTEST = {0}", XsvfInfo.lRunTestTime);
#endif
            return (XSVFERROR.NONE);
        }

        /*****************************************************************************
        * Function:     xsvfDoXREPEAT
        * Description:  XREPEAT <byte>
        *               Prespecify the maximum number of XC9500/XL retries.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXREPEAT()
        {
            XsvfInfo.ucMaxRepeat = readByte();
#if Debug
            WriteLine(3, "   XREPEAT = {0}", XsvfInfo.ucMaxRepeat);
#endif
            return (XSVFERROR.NONE);
        }

        /*****************************************************************************
        * Function:     xsvfDoXSDRSIZE
        * Description:  XSDRSIZE <uint32>
        *               Prespecify the XRUNTEST wait time for shift operations.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXSDRSIZE()
        {
            XSVFERROR iErrorCode;
            iErrorCode = XSVFERROR.NONE;
            readVal(ref XsvfInfo.lvTdi, 4);
            XsvfInfo.lShiftLengthBits = value(XsvfInfo.lvTdi.val);
            XsvfInfo.sShiftLengthBytes = xsvfGetAsNumBytes(XsvfInfo.lShiftLengthBits);
#if Debug
            WriteLine(3, "   XSDRSIZE = {0}", XsvfInfo.lShiftLengthBits);
#endif
            if (XsvfInfo.sShiftLengthBytes > MAX_LEN)
            {
                iErrorCode = XSVFERROR.DATAOVERFLOW;
                XsvfInfo.iErrorCode = iErrorCode;
            }
            return (iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfDoXSDRTDO
        * Description:  XSDRTDO <lenVal.TDI[XSDRSIZE]> <lenVal.TDO[XSDRSIZE]>
        *               Get the TDI and expected TDO values.  Then, shift.
        *               Compare the expected TDO with the captured TDO using the
        *               prespecified XTDOMASK.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXSDRTDO()
        {
            XSVFERROR iErrorCode;
            iErrorCode = xsvfBasicXSDRTDO(ref XsvfInfo.ucTapState,
                                            XsvfInfo.lShiftLengthBits,
                                            XsvfInfo.sShiftLengthBytes,
                                            ref XsvfInfo.lvTdi,
                                            ref XsvfInfo.lvTdoCaptured,
                                            ref XsvfInfo.lvTdoExpected,
                                            ref XsvfInfo.lvTdoMask,
                                            XsvfInfo.ucEndDR,
                                            XsvfInfo.lRunTestTime,
                                            XsvfInfo.ucMaxRepeat);
            if (iErrorCode != XSVFERROR.NONE)
            {
                XsvfInfo.iErrorCode = iErrorCode;
            }
            return (iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfDoXSETSDRMASKS
        * Description:  XSETSDRMASKS <lenVal.AddressMask[XSDRSIZE]>
        *                            <lenVal.DataMask[XSDRSIZE]>
        *               Get the prespecified address and data mask for the XSDRINC
        *               command.
        *               Used for xc9500/xl compressed XSVF data.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXSETSDRMASKS()
        {
            /* read the addressMask */
            readVal(ref XsvfInfo.lvAddressMask, XsvfInfo.sShiftLengthBytes);
            /* read the dataMask    */
            readVal(ref XsvfInfo.lvDataMask, XsvfInfo.sShiftLengthBytes);
#if Debug
            WriteLine(4, "    Address Mask = 0x");
            WriteLine(4, ByteArrayToString(XsvfInfo.lvAddressMask.val));
            WriteLine(4, "\n");
            WriteLine(4, "    Data Mask    = 0x");
            WriteLine(4, ByteArrayToString(XsvfInfo.lvDataMask.val));
            WriteLine(4, "\n");
#endif
            return (XSVFERROR.NONE);
        }

        /*****************************************************************************
        * Function:     xsvfDoXSDRINC
        * Description:  XSDRINC <lenVal.firstTDI[XSDRSIZE]> <byte(numTimes)>
        *                       <lenVal.data[XSETSDRMASKS.dataMask.len]> ...
        *               Get the XSDRINC parameters and execute the XSDRINC command.
        *               XSDRINC starts by loading the first TDI shift value.
        *               Then, for numTimes, XSDRINC gets the next piece of data,
        *               replaces the bits from the starting TDI as defined by the
        *               XSETSDRMASKS.dataMask, adds the address mask from
        *               XSETSDRMASKS.addressMask, shifts the new TDI value,
        *               and compares the TDO to the expected TDO from the previous
        *               XSDRTDO command using the XTDOMASK.
        *               Used for xc9500/xl compressed XSVF data.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXSDRINC()
        {
            XSVFERROR iErrorCode;
            int iDataMaskLen;
            byte ucDataMask;
            byte ucNumTimes;
            byte i;

            readVal(ref XsvfInfo.lvTdi, XsvfInfo.sShiftLengthBytes);
            iErrorCode = xsvfShift(ref XsvfInfo.ucTapState, XSVFTAPSTATE.SHIFTDR,
                                     XsvfInfo.lShiftLengthBits,
                                     ref XsvfInfo.lvTdi, ref XsvfInfo.lvTdoCaptured,
                                     ref XsvfInfo.lvTdoExpected,
                                     ref XsvfInfo.lvTdoMask, XsvfInfo.ucEndDR,
                                     XsvfInfo.lRunTestTime, XsvfInfo.ucMaxRepeat);
            if (iErrorCode != XSVFERROR.NONE)
            {
                /* Calculate number of data mask bits */
                iDataMaskLen = 0;
                for (i = 0; i < XsvfInfo.lvDataMask.len; ++i)
                {
                    ucDataMask = XsvfInfo.lvDataMask.val[i];
                    while (ucDataMask != 0x00)
                    {
                        iDataMaskLen += (ucDataMask & 1);
                        ucDataMask >>= 1;
                    }
                }

                /* Get the number of data pieces, i.e. number of times to shift */
                ucNumTimes = readByte();

                /* For numTimes, get data, fix TDI, and shift */
                for (i = 0; iErrorCode != XSVFERROR.NONE && (i < ucNumTimes); ++i)
                {
                    readVal(ref XsvfInfo.lvNextData,
                             xsvfGetAsNumBytes(iDataMaskLen));
                    xsvfDoSDRMasking(ref XsvfInfo.lvTdi,
                                      XsvfInfo.lvNextData,
                                      XsvfInfo.lvAddressMask,
                                      XsvfInfo.lvDataMask);
                    iErrorCode = xsvfShift(ref XsvfInfo.ucTapState,
                        XSVFTAPSTATE.SHIFTDR, XsvfInfo.lShiftLengthBits,
                        ref XsvfInfo.lvTdi, ref XsvfInfo.lvTdoCaptured,
                        ref XsvfInfo.lvTdoExpected, ref XsvfInfo.lvTdoMask,
                        XsvfInfo.ucEndDR, XsvfInfo.lRunTestTime, XsvfInfo.ucMaxRepeat);
                }
            }
            if (iErrorCode != XSVFERROR.NONE)
            {
                XsvfInfo.iErrorCode = iErrorCode;
            }
            return (iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfDoXSDRBCE
        * Description:  XSDRB/XSDRC/XSDRE <lenVal.TDI[XSDRSIZE]>
        *               If not already in SHIFTDR, goto SHIFTDR.
        *               Shift the given TDI data into the JTAG scan chain.
        *               Ignore TDO.
        *               If cmd==XSDRE, then goto ENDDR.  Otherwise, stay in ShiftDR.
        *               XSDRB, XSDRC, and XSDRE are the same implementation.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXSDRBCE()
        {
            XSVFTAPSTATE ucEndDR;
            XSVFERROR iErrorCode;
            ucEndDR = ((XsvfInfo.ucCommand == XSVFCOMMAND.XSDRE) ?
                                        XsvfInfo.ucEndDR : XSVFTAPSTATE.SHIFTDR);
            lenval nill = new lenval(-1, null);
            iErrorCode = xsvfBasicXSDRTDO(ref XsvfInfo.ucTapState,
                                            XsvfInfo.lShiftLengthBits,
                                            XsvfInfo.sShiftLengthBytes,
                                            ref XsvfInfo.lvTdi,
                /*plvTdoCaptured*/ref nill, /*plvTdoExpected*/ref nill,
                /*plvTdoMask*/ref nill, ucEndDR,
                /*lRunTestTime*/0, /*ucMaxRepeat*/0);
            if (iErrorCode != XSVFERROR.NONE)
            {
                XsvfInfo.iErrorCode = iErrorCode;
            }
            return (iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfDoXSDRTDOBCE
        * Description:  XSDRB/XSDRC/XSDRE <lenVal.TDI[XSDRSIZE]> <lenVal.TDO[XSDRSIZE]>
        *               If not already in SHIFTDR, goto SHIFTDR.
        *               Shift the given TDI data into the JTAG scan chain.
        *               Compare TDO, but do NOT use XTDOMASK.
        *               If cmd==XSDRTDOE, then goto ENDDR.  Otherwise, stay in ShiftDR.
        *               XSDRTDOB, XSDRTDOC, and XSDRTDOE are the same implementation.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXSDRTDOBCE()
        {
            XSVFTAPSTATE ucEndDR;
            XSVFERROR iErrorCode;
            ucEndDR = ((XsvfInfo.ucCommand == XSVFCOMMAND.XSDRTDOE) ?
                                        XsvfInfo.ucEndDR : XSVFTAPSTATE.SHIFTDR);
            lenval nill = new lenval(-1, null);
            iErrorCode = xsvfBasicXSDRTDO(ref XsvfInfo.ucTapState,
                                            XsvfInfo.lShiftLengthBits,
                                            XsvfInfo.sShiftLengthBytes,
                                            ref XsvfInfo.lvTdi,
                                            ref XsvfInfo.lvTdoCaptured,
                                            ref XsvfInfo.lvTdoExpected,
                /*plvTdoMask*/ref nill, ucEndDR,
                /*lRunTestTime*/0, /*ucMaxRepeat*/0);
            if (iErrorCode != XSVFERROR.NONE)
            {
                XsvfInfo.iErrorCode = iErrorCode;
            }
            return (iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfDoXSTATE
        * Description:  XSTATE <byte>
        *               <byte> == XTAPSTATE;
        *               Get the state parameter and transition the TAP to that state.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXSTATE()
        {
            XSVFTAPSTATE ucNextState;
            XSVFERROR iErrorCode;
            ucNextState = (XSVFTAPSTATE)readByte();
            iErrorCode = xsvfGotoTapState(ref XsvfInfo.ucTapState, ucNextState);
            if (iErrorCode != XSVFERROR.NONE)
            {
                XsvfInfo.iErrorCode = iErrorCode;
            }
            return (iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfDoXENDXR
        * Description:  XENDIR/XENDDR <byte>
        *               <byte>:  0 = RUNTEST;  1 = PAUSE.
        *               Get the prespecified XENDIR or XENDDR.
        *               Both XENDIR and XENDDR use the same implementation.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXENDXR()
        {
            XSVFERROR iErrorCode;
            XSVFENDXR ucEndState;

            iErrorCode = XSVFERROR.NONE;
            ucEndState = (XSVFENDXR)readByte();
            if ((ucEndState != XSVFENDXR.RUNTEST) && (ucEndState != XSVFENDXR.PAUSE))
            {
                iErrorCode = XSVFERROR.ILLEGALSTATE;
            }
            else
            {

                if (XsvfInfo.ucCommand == XSVFCOMMAND.XENDIR)
                {
                    if (ucEndState == XSVFENDXR.RUNTEST)
                    {
                        XsvfInfo.ucEndIR = XSVFTAPSTATE.RUNTEST;
                    }
                    else
                    {
                        XsvfInfo.ucEndIR = XSVFTAPSTATE.PAUSEIR;
                    }
#if Debug
                    WriteLine(3, "   ENDIR State = {0}", (XSVFTAPSTATE)XsvfInfo.ucEndIR);
#endif
                }
                else    /* XENDDR */
                {
                    if (ucEndState == XSVFENDXR.RUNTEST)
                    {
                        XsvfInfo.ucEndDR = XSVFTAPSTATE.RUNTEST;
                    }
                    else
                    {
                        XsvfInfo.ucEndDR = XSVFTAPSTATE.PAUSEDR;
                    }
#if Debug
                    WriteLine(3, "   ENDDR State = {0}", (XSVFTAPSTATE)XsvfInfo.ucEndDR);
#endif
                }
            }

            if (iErrorCode != XSVFERROR.NONE)
            {
                XsvfInfo.iErrorCode = iErrorCode;
            }
            return (iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfDoXCOMMENT
        * Description:  XCOMMENT <text string ending in \0>
        *               <text string ending in \0> == text comment;
        *               Arbitrary comment embedded in the XSVF.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXCOMMENT()
        {
            /* Use the comment for debugging */
            /* Otherwise, read through the comment to the end '\0' and ignore */
            byte ucText;

            if (DebugLevel > 0)
            {
                Console.WriteLine(' ');
            }

            do
            {
                ucText = readByte();
                if (DebugLevel > 0)
                {
                    Console.WriteLine(ucText != 0x00 ? (char)ucText : '\n');
                }
            } while (ucText != 0x00);

            XsvfInfo.iErrorCode = XSVFERROR.NONE;

            return (XsvfInfo.iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfDoXWAIT
        * Description:  XWAIT <wait_state> <end_state> <wait_time>
        *               If not already in <wait_state>, then go to <wait_state>.
        *               Wait in <wait_state> for <wait_time> microseconds.
        *               Finally, if not already in <end_state>, then goto <end_state>.
        * Parameters:   pXsvfInfo   - XSVF information pointer.
        * Returns:      int         - 0 = success;  non-zero = error.
        *****************************************************************************/
        static XSVFERROR xsvfDoXWAIT()
        {
            XSVFTAPSTATE ucWaitState;
            XSVFTAPSTATE ucEndState;
            long lWaitTime;

            /* Get Parameters */
            /* <wait_state> */
            readVal(ref XsvfInfo.lvTdi, 1);
            ucWaitState = (XSVFTAPSTATE)XsvfInfo.lvTdi.val[0];

            /* <end_state> */
            readVal(ref XsvfInfo.lvTdi, 1);
            ucEndState = (XSVFTAPSTATE)XsvfInfo.lvTdi.val[0];

            /* <wait_time> */
            readVal(ref XsvfInfo.lvTdi, 4);
            lWaitTime = value(XsvfInfo.lvTdi.val);
#if Debug
            WriteLine(3, "   XWAIT:  state = {0}; time = {1}",
                             (XSVFTAPSTATE)ucWaitState, lWaitTime);
#endif
            /* If not already in <wait_state>, go to <wait_state> */
            if (XsvfInfo.ucTapState != ucWaitState)
            {
                xsvfGotoTapState(ref XsvfInfo.ucTapState, ucWaitState);
            }

            /* Wait for <wait_time> microseconds */
            PortInterop.waitTime(lWaitTime);

            /* If not already in <end_state>, go to <end_state> */
            if (XsvfInfo.ucTapState != ucEndState)
            {
                xsvfGotoTapState(ref XsvfInfo.ucTapState, ucEndState);
            }

            return (XSVFERROR.NONE);
        }


        /*============================================================================
        * Execution Control Functions
        ============================================================================*/

        /*****************************************************************************
        * Function:     xsvfInitialize
        * Description:  Initialize the xsvf player.
        *               Call this before running the player to initialize the data
        *               in the SXsvfInfo struct.
        *               xsvfCleanup is called to clean up the data in SXsvfInfo
        *               after the XSVF is played.
        * Parameters:   pXsvfInfo   - ptr to the XSVF information.
        * Returns:      int - 0 = success; otherwise error.
        *****************************************************************************/
        XSVFERROR xsvfInitialize()
        {
            /* Initialize values */
            XsvfInfo.iErrorCode = xsvfInfoInit();

            if (XsvfInfo.iErrorCode == XSVFERROR.NONE)
            {
                /* Initialize the TAPs */
                XsvfInfo.iErrorCode = xsvfGotoTapState(ref (XsvfInfo.ucTapState), XSVFTAPSTATE.RESET);
            }

            return (XsvfInfo.iErrorCode);
        }

        /*****************************************************************************
        * Function:     xsvfRun
        * Description:  Run the xsvf player for a single command and return.
        *               First, call xsvfInitialize.
        *               Then, repeatedly call this function until an error is detected
        *               or until the pXsvfInfo->ucComplete variable is non-zero.
        *               Finally, call xsvfCleanup to cleanup any remnants.
        * Parameters:   pXsvfInfo   - ptr to the XSVF information.
        * Returns:      int         - 0 = success; otherwise error.
        *****************************************************************************/
        XSVFERROR xsvfRun()
        {
            /* Process the XSVF commands */
            if ((XsvfInfo.iErrorCode == XSVFERROR.NONE) && (XsvfInfo.ucComplete == 0))
            {
                /* read 1 byte for the instruction */
                XsvfInfo.ucCommand = (XSVFCOMMAND)readByte();
                ++(XsvfInfo.lCommandCount);

                if (XsvfInfo.ucCommand < XSVFCOMMAND.XLASTCMD) // valid command 0 - 24
                {
                    /* Execute the command.  Func sets error code. */
#if Debug
                    WriteLine(2, "  {0}", XsvfInfo.ucCommand);
#endif
                    /* If your compiler cannot take this form,
                       then convert to a switch statement */
                    xsvf_pfDoCmd[(byte)XsvfInfo.ucCommand]();
                }
                else
                {
                    /* Illegal command value.  Func sets error code. */
                    xsvfDoIllegalCmd();
                }
            }

            return (XsvfInfo.iErrorCode);
        }

        /*============================================================================
        * xsvfExecute() - The primary entry point to the XSVF player
        ============================================================================*/

        /*****************************************************************************
        * Function:     xsvfExecute
        * Description:  Process, interpret, and apply the XSVF commands.
        *               See port.c:readByte for source of XSVF data.
        * Parameters:   none.
        * Returns:      int - Legacy result values:  1 == success;  0 == failed.
        *****************************************************************************/
        XSVFERROR xsvfExecute()
        {
            xsvfInitialize();

            while (XsvfInfo.iErrorCode == XSVFERROR.NONE && (XsvfInfo.ucComplete == 0))
            {
                xsvfRun();
            }

            if (XsvfInfo.iErrorCode != XSVFERROR.NONE)
            {
                Console.WriteLine("{0}", XsvfInfo.iErrorCode.ToString());
                Console.WriteLine("ERROR at or near XSVF command #{0}.  See line #{1} in the XSVF ASCII file.",
                                 XsvfInfo.lCommandCount, XsvfInfo.lCommandCount);
            }
            else
            {
                Console.WriteLine("SUCCESS - Completed XSVF execution.");
            }

            //xsvfCleanup();

            return (XsvfInfo.iErrorCode);
        }

        /*============================================================================
        * lenval() - routines for using the lenVal data structure
        ============================================================================*/

        /*****************************************************************************
        * Function:     readVal
        * Description:  read from XSVF numBytes bytes of data into x.
        * Parameters:   plv         - ptr to lenval in which to put the bytes read.
        *               sNumBytes   - the number of bytes to read.
        * Returns:      void.
        *****************************************************************************/
        static void readVal(ref lenval plv, short sNumBytes)
        {
            plv.val = new byte[sNumBytes];
            int i = 0;
            plv.len = sNumBytes;        /* set the length of the lenVal        */
            for (i = 0; sNumBytes != i; i++)
            {
                /* read a byte of data into the lenVal */
                plv.val[i] = readByte();
            }
        }

        /*****************************************************************************
        * Function:     value
        * Description:  Extract the long value from the lenval array.
        * Parameters:   plvValue    - ptr to lenval.
        * Returns:      long        - the extracted value.
        *****************************************************************************/
        static long value(byte[] val)
        {
            return Convert.ToInt32(ByteArrayToString(val), 16);
            //byte[] ar = new byte[8];
            //Buffer.BlockCopy(val, 0, ar, 8 - 8, val.Length);
            //return BitConverter.ToInt64(ar, 0);
        }

        /*****************************************************************************
        * Function:     EqualLenVal
        * Description:  Compare two lenval arrays with an optional mask.
        * Parameters:   plvTdoExpected  - ptr to lenval #1.
        *               plvTdoCaptured  - ptr to lenval #2.
        *               plvTdoMask      - optional ptr to mask (=0 if no mask).
        * Returns:      short   - 0 = mismatch; 1 = equal.
        *****************************************************************************/
        static short EqualLenVal(lenval plvTdoExpected, lenval plvTdoCaptured, lenval plvTdoMask)
        {
            short sEqual;
            short sIndex;
            byte ucByteVal1;
            byte ucByteVal2;
            byte ucByteMask;

            sEqual = 1;
            sIndex = plvTdoExpected.len;

            while (sEqual != 0 && sIndex-- != 0)
            {
                ucByteVal1 = plvTdoExpected.val[sIndex];
                ucByteVal2 = plvTdoCaptured.val[sIndex];
                if (plvTdoMask.len != 0 && plvTdoMask.val != null)
                {
                    ucByteMask = plvTdoMask.val[sIndex];
                    ucByteVal1 &= ucByteMask;
                    ucByteVal2 &= ucByteMask;
                }
                if (ucByteVal1 != ucByteVal2)
                {
                    sEqual = 0;
                }
            }

            return (sEqual);
        }

        /*****************************************************************************
        * Function:     AddVal
        * Description:  add val1 to val2 and store in resVal;
        *               assumes val1 and val2  are of equal length.
        * Parameters:   plvResVal   - ptr to result.
        *               plvVal1     - ptr of addendum.
        *               plvVal2     - ptr of addendum.
        * Returns:      void.
        *****************************************************************************/
        static void addVal(ref lenval plvResVal, lenval plvVal1, lenval plvVal2)
        {
            byte ucCarry;
            short usSum;
            short usVal1;
            short usVal2;
            short sIndex;

            plvResVal.len = plvVal1.len;         /* set up length of result */

            /* start at least significant bit and add bytes    */
            ucCarry = 0;
            sIndex = plvVal1.len;
            while (sIndex-- != 0)
            {
                usVal1 = plvVal1.val[sIndex];   /* i'th byte of val1 */
                usVal2 = plvVal2.val[sIndex];   /* i'th byte of val2 */

                /* add the two bytes plus carry from previous addition */
                usSum = (short)(usVal1 + usVal2 + ucCarry);

                /* set up carry for next byte */
                ucCarry = (byte)((usSum > 255) ? 1 : 0);

                /* set the i'th byte of the result */
                plvResVal.val[sIndex] = (byte)usSum;
            }
        }

        /*============================================================================
        * File routines 
        ============================================================================*/

        /* readByte:  Implement to source the next byte from your XSVF file location
        read in a byte of data from the prom */
        static byte readByte()//readbyte from input file
        {
            /* pretend reading using a file */
            fileStreamindex++;
            //MainForm.mainForm.progressbar = (int)(fileStreamindex * 100 / filesize);
            //return file[fileStreamindex - 1];
            return (byte)fs.ReadByte();
            /**data=*xsvf_data++;*/
        }

        static string ByteArrayToString(byte[] ba, int startindex = 0, int length = 0)
        {
            if (ba == null) return "";
            string hex = BitConverter.ToString(ba);
            if (startindex == 0 && length == 0) hex = BitConverter.ToString(ba);
            else if (length == 0 && startindex != 0) hex = BitConverter.ToString(ba, startindex);
            else if (length != 0 && startindex != 0) hex = BitConverter.ToString(ba, startindex, length);
            return hex.Replace("-", "");
        }

#if Debug
        void WriteLine(int level, string text, object arg1 = null, object arg2 = null, object arg3 = null)
        {
            if (level <= DebugLevel)
            {
                if (arg1 == null) Console.WriteLine(text);
                else if (arg2 == null) Console.WriteLine(text, arg1);
                else if (arg3 == null) Console.WriteLine(text, arg1, arg2);
                else Console.WriteLine(text, arg1, arg2, arg3);
            }
        }
#endif

        public void lxsvf(string filename, string comma = "0x378", bool deletedll = false)
        {
            Console.WriteLine("\tUsing InpOut32.dll portio driver");
            //Console.WriteLine("XSVF file = {0}", filename);
            int comm = 0x378;
            comm = Convert.ToInt32(comma, 16);
            PortInterop.base_port = comm;
            if (PortInterop.base_port > 0xffff)
            {
                Console.WriteLine("ERROR: invalid IO port.");
                return;
            }
            Console.WriteLine("using base port 0x{0:X}\n", PortInterop.base_port);

            if (!File.Exists(Path.Combine(variables.pathforit, "inpout32.dll")) || PortInterop.IsInpOutOpen() != 1)
            {
                Console.WriteLine("Failed To open dll");
                return;
            }
            if (!File.Exists(filename))
            {
                Console.WriteLine("ERROR:  Cannot open file {0}", filename);
            }
            else
            {
                try
                {
                    fs = new FileStream(filename, FileMode.Open);
                    PortInterop.setPort(PortInterop.TMS, 1);
                    Stopwatch stopwatch = new Stopwatch();

                    filesize = fs.Length;

                    // MainForm.mainForm.progressbar = 0;

                    stopwatch.Start();
                    xsvfExecute();
                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;

                    // Format and display the TimeSpan value.
                    string elapsedTime = String.Format("{0:00}.{1:000}",
                        ts.Seconds, ts.Milliseconds);
                    Console.WriteLine("Execution Time = {0} seconds\n", elapsedTime);


                    //MainForm.mainForm.progressbar = 100;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message.ToString()); }
                finally
                {
                    if (deletedll)
                    {
                        PortInterop.UnloadModule("inpout32.dll");
                        File.Delete(Path.Combine(variables.pathforit, "inpout32.dll"));
                    }
                    fs.Close();
                }
            }
        }
    }
}
