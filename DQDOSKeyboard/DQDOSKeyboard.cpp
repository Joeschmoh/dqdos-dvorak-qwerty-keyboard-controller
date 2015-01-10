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
static const Int32 guiLayoutStringSize = (KL_NAMELENGTH + (KL_NAMELENGTH % 8) + 8);

// Global and shared between all instances
#pragma data_seg(".DQDOSShared")
	
static volatile tKeyboardMode geLastKeyboardMode = FirstRun;
static volatile HKL gPrimaryHKL = 0;
static volatile HKL gSecondaryHKL = 0;

static volatile HINSTANCE ghInstDLL = NULL;
static volatile HHOOK ghFilterHook = NULL;
static volatile bool gIsMyKeyCode[256];

#pragma data_seg()

/*******************************
 * Internal code starts here.  *
 *******************************/

#pragma unmanaged

LRESULT CALLBACK DQDOSKeyFilter(int code, WPARAM wparam, LPARAM lparam)
{
	if (code >= 0)
	{
		// TODO: Filter!
	}

	return CallNextHookEx(ghFilterHook, code, wparam, lparam);
}

bool EnableFilterHook()
{
	if (ghFilterHook != NULL)
		return false;

	ghFilterHook = SetWindowsHookEx(WH_KEYBOARD, DQDOSKeyFilter, ghInstDLL, 0);

	return true;
}

bool DisableFilterHook()
{
	if (ghFilterHook != NULL)
	{
		UnhookWindowsHookEx(ghFilterHook);
	}

	ghFilterHook = NULL;

	return true;
}

#pragma managed

bool ChangeKeyboardLayout(LPCWSTR pwsPrimaryLayoutName, LPCWSTR pwsSecondaryLayoutName)
{
	// Sets the keyboard layouts as requested, ensuring primary is the active layout.
	// Unloads any layouts no longer needed.

	// This may have to be modified for Win8, "Activate" means it sticks for the entire system.
	// Currently coded for Win7.

	HKL OrigHKL = GetKeyboardLayout(0);
	HKL OrigSecHKL = gSecondaryHKL;
	HKL MyHKL = 0;

	// Test inputs
	if ((pwsPrimaryLayoutName == NULL) || (*pwsPrimaryLayoutName == '\0'))
		return false;

	// First, load the secondary layout (if necessary).
	// This could be zero (meaning failure) and we have to watch (i.e. ignore) during filtering.
	if (pwsSecondaryLayoutName != NULL)
	{
		gSecondaryHKL = LoadKeyboardLayout(pwsSecondaryLayoutName, 0);
	}
	else
	{
		gSecondaryHKL = 0;
	}

	// Now load the Primary layout.
	MyHKL = LoadKeyboardLayout(pwsPrimaryLayoutName, KLF_ACTIVATE | KLF_REORDER); // | KLF_REPLACELANG);
	if (MyHKL == 0)
		return false;
	gPrimaryHKL = MyHKL;

	// Tell other processes of the new primary layout and set it as default.
	PostMessage(HWND_BROADCAST, WM_INPUTLANGCHANGEREQUEST, 0, (LPARAM)MyHKL);
	SystemParametersInfo(SPI_SETDEFAULTINPUTLANG, 0, (void *)&MyHKL, SPIF_SENDCHANGE);

	// Unload unneeded layouts if it changed (this ensures all processes accept the new layout).
	if ((OrigHKL != gPrimaryHKL) && (OrigHKL != gSecondaryHKL))
	{
		UnloadKeyboardLayout(OrigHKL);
	}
	if ((pwsSecondaryLayoutName == NULL) && (OrigSecHKL != 0))
	{
		UnloadKeyboardLayout(OrigSecHKL);
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
		
	// First set the layout.
	switch (KeyboardMode)
	{
		case DvorakQwerty:
			Result = ChangeKeyboardLayout(pwsPrimaryLayout, pwsSecondaryLayout);
			EnableFilterHook();
			if (Result == false)
				return false;
			break;

		case DvorakOnly:
			Result = ChangeKeyboardLayout(pwsPrimaryLayout, NULL);
			DisableFilterHook();
			if (Result == false)
				return false;
			break;

		case Disabled:
			Result = ChangeKeyboardLayout(pwsSecondaryLayout, NULL);
			DisableFilterHook();
			if (Result == false)
				return false;
			break;

		default:
			return false;
			break;
	}
	geLastKeyboardMode = KeyboardMode;

	// Second set or release the keyboard hook.
		
	// All done!
	return true;
}

// external
bool DQGetCurrentKeyboardLayout(int iLayoutSize, LPWSTR pwsLayoutName)
{
	HKL hLayout = 0;
	BOOL Result = false;

	if (iLayoutSize < KL_NAMELENGTH + 1)
		return false;

	Result = GetKeyboardLayoutName(pwsLayoutName);
	if (Result == FALSE) // success
		return false;

	return true;
}

} // namespace DQDOSKeyboard

#pragma unmanaged

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason)
	{
	case DLL_PROCESS_DETACH:
		DQDOSKeyboard::DisableFilterHook();
		break;

	case DLL_PROCESS_ATTACH:
		DQDOSKeyboard::ghInstDLL = hinstDLL;
		break;

	default:
		// Currently do nothing.
		break;
	}

	return TRUE;
}
