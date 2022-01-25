using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRunner
{
    internal interface IISD
    {
        int Open();
        int Close();
        int PowerUp();
        int GetID();
        void PowerDown();
        void Reset();
        int ISD_Read_Flash(string filename);
        int ISD_Erase_Flash();
        void ISD_Write_Flash(string filename);
        Boolean ISD_Verify_Flash(string filename);
        void ISD_Play(UInt16 index);
        void ISD_Exec(UInt16 index);
    }
}
