/*************************************************************************
 * DQDQSKeyboard.cs - Copyright 2015 Derick Snyder                       *
 *                                                                       *
 * This file is part of DQDOS - The Dvorak-Qwerty Keyboard Controller.   *
 *                                                                       *
 * DQDOS is free software: you can redistribute it and/or modify         *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation, either version 3 of the License, or     *
 * (at your option) any later version.                                   *
 *                                                                       *
 * DQDOS is distributed in the hope that it will be useful,              *
 * but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 * GNU General Public License for more details.                          *
 *                                                                       *
 * You should have received a copy of the GNU General Public License     *
 * along with DQDOS.  If not, see <http://www.gnu.org/licenses/>.        *
 *                                                                       *
 * This source code is maintained at:                                    *
 *   https://code.google.com/p/dqdos-dvorak-qwerty-keyboard-controller/  *
 *                                                                       *
 * Written by Derick Snyder, the author can be reached at the above      *
 *  source code project site.                                            *
 *                                                                       *
 *************************************************************************/


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

        public static void GetLastKeyboardLayouts(out String PrimaryLayoutName, out String SecondaryLayoutName)
        {
            int iBufferSize = (int)DQGetKeyboardLayoutBufferSize();
            StringBuilder PriMarshalStr = null;
            StringBuilder SecMarshalStr = null;
            bool Result = false;

            PriMarshalStr = new StringBuilder(iBufferSize);
            SecMarshalStr = new StringBuilder(iBufferSize);

            Result = DQGetLastKeyboardLayouts(iBufferSize, PriMarshalStr, SecMarshalStr);
            if (Result != false)
            {
                PrimaryLayoutName = PriMarshalStr.ToString();
                SecondaryLayoutName = SecMarshalStr.ToString();
            }
            else
            {
                PrimaryLayoutName = "";
                SecondaryLayoutName = "";
            }
            
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

        public static bool SetFilteredSpecialKeys(bool IsControlFiltered, bool IsAltFiltered, bool IsWinFiltered, bool IsScrollLockQwertyEnabled, bool IsScrollLockDisabled)
        {
            return DQSetFilteredSpecialKeys(IsControlFiltered, IsAltFiltered, IsWinFiltered, IsScrollLockQwertyEnabled, IsScrollLockDisabled);
        }

        public static bool ResetSpecialKeys()
        {
            return DQResetSpecialKeys();
        }

        public static uint GetNumberAttachedProcs()
        {
            return DQGetNumberAttachedProcs();
        }

        [DllImport("DQDOSKeyboard.dll")]
        private static extern Int32 DQGetKeyboardLayoutBufferSize();

        [DllImport("DQDOSKeyboard.dll")]
        private static extern tKeyboardMode DQGetLastKeyboardMode();

        [DllImport("DQDOSKeyboard.dll")]
        private static extern bool DQSetKeyboardMode(tKeyboardMode Mode, 
            [MarshalAs(UnmanagedType.LPWStr)] String pwsPrimaryLayout, 
            [MarshalAs(UnmanagedType.LPWStr)] String pwsSecondaryLayout);

        [DllImport("DQDOSKeyboard.dll", CharSet = CharSet.Unicode)]
        private static extern bool DQGetLastKeyboardLayouts(int iLayoutSize, StringBuilder pwsPrimaryLayout, StringBuilder pwsSecondaryLayout);

        [DllImport("DQDOSKeyboard.dll", CharSet = CharSet.Unicode)]
        private static extern bool DQGetCurrentKeyboardLayout(int iLayoutSize, StringBuilder pwsLayoutName);

        [DllImport("DQDOSKeyboard.dll")]
        private static extern bool DQSetFilteredSpecialKeys(bool IsControlFiltered, bool IsAltFiltered, bool IsWinFiltered, bool IsScrollLockQwertyEnabled, bool IsScrollLockDisabled);

        [DllImport("DQDOSKeyboard.dll")]
        private static extern bool DQResetSpecialKeys();

        [DllImport("DQDOSKeyboard.dll")]
        private static extern uint DQGetNumberAttachedProcs();
    }
}
