/************************************************************************
* DQDOSKeyboard.cs - Copyright 2015 Derick Snyder                       *
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
* along with DQDOS.  If not, see <http://*www.gnu.org/licenses/>.        *
*                                                                       *
* This source code is maintained at:                                    *
*   https://code.google.com/p/dqdos-dvorak-qwerty-keyboard-controller/  *
*                                                                       *
* Written by Derick Snyder, the author can be reached at the above      *
*  source code project site.                                            *
*                                                                       *
*************************************************************************/


// This is the main DLL file.

#include "stdafx.h"

#include <cstdlib>
#include <Windows.h>

#include "DQDOSKeyboard.h"

namespace DQDOSKeyboard 
{
	// Globals
	const Int32 guiLayoutStringSize = (KL_NAMELENGTH + (KL_NAMELENGTH % 8) + 8);
	tKeyboardMode geLastKeyboardMode = FirstRun;

	// Internal code
	bool ChangeKeyboardLayout(LPCWSTR pwsLayoutName)
	{
		// Make the given layout the only keyboard layout, and set it to default.
		// This only changes the user's keyboard layout, doesn't do the filtering.
		
		HKL OrigHKL = GetKeyboardLayout(0);
		HKL MyHKL = 0;

		if ((pwsLayoutName == NULL) || (*pwsLayoutName == 0))
			return false;
		
		MyHKL = LoadKeyboardLayout(pwsLayoutName, KLF_ACTIVATE); // | KLF_SUBSTITUTE_OK | KLF_REPLACELANG);
		if (MyHKL == 0)
			return false;

		PostMessage(HWND_BROADCAST, WM_INPUTLANGCHANGEREQUEST, 0, (LPARAM) MyHKL);

		// Purposefully ignoring any error codes that might happen from here out.
		SystemParametersInfo(SPI_SETDEFAULTINPUTLANG, 0, (void *)&MyHKL, SPIF_SENDCHANGE);
		if (OrigHKL != MyHKL)
		{
			UnloadKeyboardLayout(OrigHKL);
		}

		return true;
	}

	// external
	Int32 DQGetKeyboardLayoutBufferSize()
	{
		return guiLayoutStringSize;
	}

	// external
	tKeyboardMode DQGetLastKeyboardMode()
	{
		return geLastKeyboardMode;
	}

	// external
	bool DQSetKeyboardMode(tKeyboardMode KeyboardMode, LPCWSTR pwsPrimaryLayout, LPCWSTR pwsSecondaryLayout)
	{
		bool Result = false;

		if ((pwsPrimaryLayout == NULL) || (pwsSecondaryLayout == NULL)
			|| (*pwsPrimaryLayout == 0) || (*pwsSecondaryLayout == 0))
		{
			return false;
		}
		
		switch (KeyboardMode)
		{
			case DvorakQwerty:
				Result = ChangeKeyboardLayout(pwsPrimaryLayout);
				if (Result == false)
					return false;
				break;

			case DvorakOnly:
				Result = ChangeKeyboardLayout(pwsPrimaryLayout);
				if (Result == false)
					return false;
				break;

			case Disabled:
				Result = ChangeKeyboardLayout(pwsSecondaryLayout);
				if (Result == false)
					return false;
				break;

			default:
				return false;
				break;
		}

		geLastKeyboardMode = KeyboardMode;
		return true;
	}

	// external
	bool DQGetCurrentKeyboardLayout(int iLayoutSize, LPSTR psLayoutName)
	{
		HKL hLayout = 0;
		BOOL Result = false;

		if (iLayoutSize < KL_NAMELENGTH + 1)
			return false;

		Result = GetKeyboardLayoutNameA(psLayoutName);
		if (Result == FALSE) // success
			return false;

		return true;
	}
	
}
