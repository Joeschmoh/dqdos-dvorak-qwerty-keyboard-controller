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

template <typename T, size_t N> inline
size_t SizeOfArray(const T(&)[N])
{
	return N;
}

namespace DQDOSKeyboard
{

// Globals
static const Int32 guiLayoutStringSize = (KL_NAMELENGTH + (KL_NAMELENGTH % 8) + 8);

// Global and shared between all instances
#pragma data_seg(".DQDOSShared")
	
// Keyboard layouts and modes.
static volatile tKeyboardMode geLastKeyboardMode = FirstRun;
static volatile HKL gPrimaryHKL = 0;
static volatile HKL gSecondaryHKL = 0;

// Hook pointers.
static volatile HINSTANCE ghInstDLL = NULL;
static volatile HHOOK ghFilterHook = NULL;

// Filter maps and states.
static volatile bool gabDidIMakeKeyCode[256];
static volatile UINT gauiFilterVKeyMap[256];
static volatile UINT gauiFilterScanKeyMap[256];
static volatile UINT guiLastVKeyDown = 0;

// Special key states
static volatile bool gIsControlDown = false;
static volatile bool gIsAltDown = false;
static volatile bool gIsLeftWinDown = false;
static volatile bool gIsRightWinDown = false;

// Should we filter these keys?
static volatile bool gIsControlFiltered = true;
static volatile bool gIsAltFiltered = true;
static volatile bool gIsWinFiltered = true;

#pragma data_seg()

/*******************************
 * Internal code starts here.  *
 *******************************/

#pragma unmanaged

bool IsSpecialKeyPressed()
{
	// First test the special keys
	if (((gIsControlFiltered & gIsControlDown) != false)
		|| ((gIsAltFiltered & gIsAltDown) != false)
		|| ((gIsWinFiltered & (gIsLeftWinDown | gIsRightWinDown)) != false))
	{
		return true;
	}

	return false;
}

bool SendFilteredKeyInput(UINT uiUnFilteredVKey, bool IsKeyDown)
{
	INPUT MyInput[1];
	UINT uiMappedVKey = 0;
	UINT uiMappedSKey = 0;

	if (gauiFilterScanKeyMap[uiUnFilteredVKey] == 0)
		return false;

	uiMappedVKey = gauiFilterVKeyMap[uiUnFilteredVKey];
	uiMappedSKey = gauiFilterScanKeyMap[uiUnFilteredVKey];
	gabDidIMakeKeyCode[uiMappedVKey] = true;

	MyInput[0].type = INPUT_KEYBOARD;
	MyInput[0].ki.wVk = uiMappedVKey;
	MyInput[0].ki.wScan = uiMappedSKey;
	MyInput[0].ki.time = 0;
	MyInput[0].ki.dwExtraInfo = 0;
	
	if (IsKeyDown == false)
	{
		MyInput[0].ki.dwFlags = KEYEVENTF_KEYUP; 
		guiLastVKeyDown = 0;
	}
	else
	{
		MyInput[0].ki.dwFlags = 0;
		guiLastVKeyDown = uiUnFilteredVKey;
	}

	SendInput(1, MyInput, sizeof(MyInput[0]));

	return true;
}

bool FilterTheKeys(int iCode, UINT uiVirtualKey, UINT uiKeyFlags)
{
	bool IsThisKeyDown = (uiKeyFlags & (1 << 31)) == 0;
	
	switch (uiVirtualKey)
	{
		// here we keep up with the special keys.
		case VK_CONTROL:
			gIsControlDown = IsThisKeyDown;
			if ((IsSpecialKeyPressed() == false) && (guiLastVKeyDown != 0))
			{
				SendFilteredKeyInput(guiLastVKeyDown, false);
			}
			break;

		case VK_MENU:
			gIsAltDown = IsThisKeyDown;
			if ((IsSpecialKeyPressed() == false) && (guiLastVKeyDown != 0))
			{
				SendFilteredKeyInput(guiLastVKeyDown, false);
			}
			break;

		case VK_LWIN:
			gIsLeftWinDown = IsThisKeyDown;
			if ((IsSpecialKeyPressed() == false) && (guiLastVKeyDown != 0))
			{
				SendFilteredKeyInput(guiLastVKeyDown, false);
			}
			break;

		case VK_RWIN:
			gIsRightWinDown = IsThisKeyDown;
			if ((IsSpecialKeyPressed() == false) && (guiLastVKeyDown != 0))
			{
				SendFilteredKeyInput(guiLastVKeyDown, false);
			}
			break;

		// here we filter.
		default:
			if (gabDidIMakeKeyCode[uiVirtualKey] != false)
			{
				// We created this key code, ignore it.
				if (iCode != HC_NOREMOVE)
				{
					gabDidIMakeKeyCode[uiVirtualKey] = false;
				}
			}
			else
			{
				if ((IsSpecialKeyPressed() != false) && (gauiFilterVKeyMap[uiVirtualKey] != 0))
				{	
					if (SendFilteredKeyInput(uiVirtualKey, IsThisKeyDown))
					{
						return true;
					}
				}
			}
			break;
	}

	return false;
}

LRESULT CALLBACK DQDOSKeyFilter(int code, WPARAM wparam, LPARAM lparam)
{
	
	if (code >= 0)
	{
		if ((wparam <= 0xff) && (FilterTheKeys(code, (UINT)wparam, (UINT)lparam) != false))
			return 1;
	}

	return CallNextHookEx(ghFilterHook, code, wparam, lparam);
}

bool CreateFilterMap()
{
	// Based on current "primary" and "secondary" layouts, create an array
	// that tells me for a pressed primary virtual scan code what key I should 
	// really press to get proper secondary code.

	int i = 0;
	UINT uiQwKey = 0;
	UINT uiDvKey = 0;

	// First, check for reasons the filter won't work -- Are both keyboard layouts set? Are they different?
	if ((gPrimaryHKL == 0) || (gSecondaryHKL == 0)
		|| (gPrimaryHKL == gSecondaryHKL))
	{
		return false;
	}

	// Next, reset all "I sent that code" flags.
	for (i = 0; i < SizeOfArray(gabDidIMakeKeyCode); i++)
		gabDidIMakeKeyCode[i] = false;

	guiLastVKeyDown = 0;

	// enumerating all VSK codes, most we will just set to zero.
	for (i = 0; i < SizeOfArray(gauiFilterVKeyMap); i++)
	{
		// Be sure array is zeroed out.
		gauiFilterVKeyMap[i] = 0;
		gauiFilterScanKeyMap[i] = 0;
		
		// Lookup filters for the keys we care about -- "ASCII" ones such as A-Z, 0-9, standard punctuation. 
		// TODO: Test mapping every code. Does it matter to limit like this?
		if (((i >= 0x30) && (i <= 0x39))
			|| ((i >= 0x41) && (i <= 0x5A))
			|| ((i >= 0xBA) && (i <= 0xC0))
			|| ((i >= 0xDB) && (i <= 0xDF))
			|| (i == 0xE2))
		{
			uiQwKey = MapVirtualKeyEx((UINT)i, MAPVK_VK_TO_VSC, gPrimaryHKL);
			uiDvKey = MapVirtualKeyEx(uiQwKey, MAPVK_VSC_TO_VK, gSecondaryHKL);

			if ((uiQwKey != 0) && (uiDvKey != 0) && (uiDvKey != i))
			{
				gauiFilterVKeyMap[i] = uiDvKey;
				gauiFilterScanKeyMap[i] = uiQwKey;
			}
		}
	}

	return true;
}

bool DisableFilterHook()
{
	if (ghFilterHook != NULL)
	{
		if (UnhookWindowsHookEx(ghFilterHook) == FALSE)
			return false;
	}

	ghFilterHook = NULL;

	return true;
}

bool EnableFilterHook()
{
	// Turn on the key filter.

	// Disable here for two reasons -- if an error is present, the hook is 
	// NOT set. Also, we avoid weird interim states while the map is created.
	if (DisableFilterHook() == false)
		return false;

	// Create a filter map, if this fails, we don't bother to enable the hook function.
	if (CreateFilterMap() == false)
		return false;

	ghFilterHook = SetWindowsHookEx(WH_KEYBOARD, DQDOSKeyFilter, ghInstDLL, 0);

	if (ghFilterHook == NULL)
		return false;

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
		
	// Set the layout and enable or release the keyboard hook.
	switch (KeyboardMode)
	{
		case DvorakQwerty:
			Result = ChangeKeyboardLayout(pwsPrimaryLayout, pwsSecondaryLayout);
			if (Result == false)
				return false;
			EnableFilterHook();
			break;

		case DvorakOnly:
			DisableFilterHook();
			Result = ChangeKeyboardLayout(pwsPrimaryLayout, NULL);
			if (Result == false)
				return false;
			break;

		case Disabled:
			DisableFilterHook();
			Result = ChangeKeyboardLayout(pwsSecondaryLayout, NULL);
			if (Result == false)
				return false;
			break;

		default:
			return false;
			break;
	}
	geLastKeyboardMode = KeyboardMode;
		
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

// external
bool DQSetFilteredSpecialKeys(bool IsControlFiltered, bool IsAltFiltered, bool IsWinFiltered)
{
	gIsControlFiltered = IsControlFiltered;
	gIsAltFiltered = IsAltFiltered;
	gIsWinFiltered = IsWinFiltered;

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
