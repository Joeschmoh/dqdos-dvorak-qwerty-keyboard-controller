using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DQDOS
{
    public static class DQDOSKeyboardLayout
    {
        private static String[,] LayoutTable = new String[,] {   
            {"00010409", "Dvorak (English US)"},
            {"00000409", "Qwerty (English US)"} 
        };

        public static String CommonNameToLayoutName(String CommonName)
        {
            //Regex MySearch = new Regex(CommonName);
            int i = 0;

            for (i = 0; i < LayoutTable.GetLength(0); i++)
            {
                //if (MySearch.IsMatch(LayoutTable[i,1]))
                if (LayoutTable[i,1].ToUpper().Contains(CommonName.ToUpper()))
                {
                    return LayoutTable[i,0];
                }
            }

            return null;
        }

        public static String LayoutNameToCommonName(String LayoutName)
        {
            //Regex MySearch = new Regex(LayoutName);
            int i = 0;

            for (i = 0; i < LayoutTable.GetLength(0); i++)
            {
                //if (MySearch.IsMatch(LayoutTable[i, 0]))
                if (LayoutTable[i,0].ToUpper().Contains(LayoutName.ToUpper()))
                {
                    return LayoutTable[i, 1];
                }
            }

            return LayoutName;
        }
    }

    public static class DQDOSKeyboard
    {
        public enum tKeyboardMode {FirstRun = -1, Disabled = 0, DvorakOnly, DvorakQwerty };

        public static tKeyboardMode GetLastKeyboardMode()
        {
            return DQGetLastKeyboardMode();
        }

        public static bool SetKeyboardMode(tKeyboardMode Mode, String PrimaryLayout, String SecondaryLayout)
        {
            return DQSetKeyboardMode(Mode, PrimaryLayout, SecondaryLayout);
        }

        public static String GetCurrentKeyboardLayout()
        {
            int iBufferSize = (int) DQGetKeyboardLayoutBufferSize();
            StringBuilder MarshalStr = null;
            bool Result = false;

            MarshalStr = new StringBuilder(iBufferSize);

            Result = DQGetCurrentKeyboardLayout(iBufferSize, MarshalStr);
            if (Result != false)
            {
                return MarshalStr.ToString();
            }
            
            return null;
        }

        [DllImport("DQDOSKeyboard.dll")]
        private static extern Int32 DQGetKeyboardLayoutBufferSize();

        [DllImport("DQDOSKeyboard.dll")]
        private static extern tKeyboardMode DQGetLastKeyboardMode();

        [DllImport("DQDOSKeyboard.dll")]
        private static extern bool DQSetKeyboardMode(tKeyboardMode Mode, 
            [MarshalAs(UnmanagedType.LPWStr)] String pwsPrimaryLayout, 
            [MarshalAs(UnmanagedType.LPWStr)] String pwsSecondaryLayout);

        [DllImport("DQDOSKeyboard.dll")]
        private static extern bool DQGetCurrentKeyboardLayout(int iLayoutSize, StringBuilder pwsLayoutName);

        //[DllImport("DQDOSKeyboard.dll")]
        //private static extern bool DQSetPrimaryKeyboardLayout([MarshalAs(UnmanagedType.LPStr)] String pcLayout);

        //[DllImport("DQDOSKeyboard.dll")]
        //private static extern bool DQSetSecondaryKeyboardLayout([MarshalAs(UnmanagedType.LPStr)] String pcLayout);
    }
}
