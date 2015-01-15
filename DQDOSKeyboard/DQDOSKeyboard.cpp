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
#include <stdio.h>
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
static WCHAR gwsLastPrimaryLayoutName[guiLayoutStringSize];
static WCHAR gwsLastSecondaryLayoutName[guiLayoutStringSize];

// Hook pointers.
static volatile HINSTANCE ghInstDLL = NULL;
static volatile HHOOK ghFilterHook = NULL;
static volatile UINT guiDLLRefCount = 0;

// Filter maps and states.
static volatile bool gabDidIMakeKeyCode[256];
static volatile UINT gauiFilterVKeyMap[256];
static volatile UINT gauiFilterScanKeyMap[256];
static volatile UINT guiLastVKeyDown = 0;

// Special key states
static volatile bool gIsControlDown = false;
static volatile bool gIsAltDown = false;
static volatile bool gIsWinDown = false;
static volatile bool gIsShiftDown = false;
static volatile bool gIsScrollLockEnabled = false;

// Should we filter these keys?
static volatile bool gIsControlFiltered = true;
static volatile bool gIsAltFiltered = true;
static volatile bool gIsWinFiltered = true;
static volatile bool gIsScrollLockToggleEnabled = true;
static volatile bool gIsScrollLockRemoved = true;

#pragma data_seg()

/*******************************
 * Internal code starts here.  *
 *******************************/

#pragma unmanaged

#ifdef _DEBUG
void DebugLog(const wchar_t *text)
{
	OutputDebugString(text);
}

void DebugLogN(const wchar_t *text, UINT n)
{
	wchar_t buf[1024];
	_snwprintf_s(buf, 1024, _TRUNCATE, L"%s 0x%04x\n", text, n);
	OutputDebugString(buf);
}

void DebugLogN2(const wchar_t *text, UINT n, UINT n2)
{
	wchar_t buf[1024];
	_snwprintf_s(buf, 1024, _TRUNCATE, L"%s 0x%04x 0x%04x\n", text, n, n2);
	OutputDebugString(buf);
}

void DebugLogN3(const wchar_t *text, UINT n, UINT n2, UINT n3)
{
	wchar_t buf[1024];
	_snwprintf_s(buf, 1024, _TRUNCATE, L"%s 0x%04x 0x%04x 0x%04x\n", text, n, n2, n3);
	OutputDebugString(buf);
}

#else
#define DebugLog(x)
#define DebugLogN(x, n)
#define DebugLogN2(x, n, n2)
#define DebugLogN3(x, n, n2, n3)
#endif

bool inline IsSpecialKeyPressed()
{
	// First test the special keys
	if (((gIsControlFiltered & gIsControlDown) != false)
		|| ((gIsAltFiltered & gIsAltDown) != false)
		|| ((gIsWinFiltered & gIsWinDown) != false)
		|| ((gIsScrollLockToggleEnabled & gIsScrollLockEnabled) != false))
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

bool FilterTheKeysLL(WPARAM KeyMsg, KBDLLHOOKSTRUCT* KbdInfo)
{
	if (KbdInfo == NULL)
		return false;

	bool IsThisKeyDown = (KbdInfo->flags & LLKHF_UP) == 0 ? true : false;

	DebugLogN3(L"Hook encountered -- KeyMsg, vkCode, Keydown: ", (UINT) KeyMsg, KbdInfo->vkCode, (UINT) IsThisKeyDown);
	
	switch (KbdInfo->vkCode)
	{
		// here we keep up with the special keys.
	case VK_LCONTROL:
	case VK_RCONTROL:
	case VK_CONTROL:
		gIsControlDown = IsThisKeyDown;
		if ((IsSpecialKeyPressed() == false) && (guiLastVKeyDown != 0))
		{
			SendFilteredKeyInput(guiLastVKeyDown, false);
		}
		break;

	case VK_LMENU:
	case VK_RMENU:
	case VK_MENU:
		gIsAltDown = IsThisKeyDown;
		if ((IsSpecialKeyPressed() == false) && (guiLastVKeyDown != 0))
		{
			SendFilteredKeyInput(guiLastVKeyDown, false);
		}
		break;

	case VK_LWIN:
	case VK_RWIN:
		gIsWinDown = IsThisKeyDown;
		if ((IsSpecialKeyPressed() == false) && (guiLastVKeyDown != 0))
		{
			SendFilteredKeyInput(guiLastVKeyDown, false);
		}
		break;

	case VK_LSHIFT:
	case VK_RSHIFT:
	case VK_SHIFT:
		gIsShiftDown = IsThisKeyDown;
		break;

	case VK_SCROLL:
		if (!gIsShiftDown)
		{
			//No shift key.
			if (IsThisKeyDown != false)
				gIsScrollLockEnabled = !gIsScrollLockEnabled;
			if (gIsScrollLockRemoved)
				return 1;
		}
		break;

		// here we filter.
	default:
		if (gabDidIMakeKeyCode[KbdInfo->vkCode] != false)
		{
			gabDidIMakeKeyCode[KbdInfo->vkCode] = false;
		}
		else
		{
			if ((IsSpecialKeyPressed() != false) && (gauiFilterVKeyMap[KbdInfo->vkCode] != 0))
			{
				if (SendFilteredKeyInput(KbdInfo->vkCode, IsThisKeyDown))
				{
					return true;
				}
			}
		}
		break;
	}

	return false;
}

LRESULT CALLBACK DQDOSKeyFilterLL(int iCode, WPARAM wParam, LPARAM lParam)
{
	if (iCode == HC_ACTION)
	{
		KBDLLHOOKSTRUCT* KbdInfo = reinterpret_cast<KBDLLHOOKSTRUCT*>(lParam);

		if ((KbdInfo->vkCode <= 0xff) && (FilterTheKeysLL(wParam, KbdInfo) != false))
			return 1;
	}

	return CallNextHookEx(ghFilterHook, iCode, wParam, lParam);
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
		
		// Lookup filters for various keys  
		if ((i >= 0x30) && (i <= 0xfe))
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

	// Input validation, we need a DLL instance and the hook should be disabled already.
	if ((ghFilterHook != NULL) || (ghInstDLL == 0))
		return false;

	// Create a filter map, if this fails, we don't bother to enable the hook function.
	if (CreateFilterMap() == false)
		return false;

	gIsScrollLockEnabled = false;

	ghFilterHook = SetWindowsHookEx(WH_KEYBOARD_LL, DQDOSKeyFilterLL, ghInstDLL, 0);
	
	if (ghFilterHook == NULL)
		return false;

	return true;
}

#pragma managed

bool ChangeKeyboardLayout(LPCWSTR pwsPrimaryLayoutName, LPCWSTR pwsSecondaryLayoutName, bool bEnableFilter)
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

	// Disable any current hook first.
	if (DisableFilterHook() == false)
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

	// Layouts are set, now we can enable the hook (which also creates the map).
	if ((bEnableFilter) && (gSecondaryHKL != 0))
		EnableFilterHook();

	// Unload unneeded layouts if it changed (this ensures all processes see only the new layout, otherwise
	// they sometimes like to pick the secondary layout).
	if (gSecondaryHKL != 0)
	{
		UnloadKeyboardLayout(gSecondaryHKL);
	}
	if ((OrigHKL != 0) && (OrigHKL != gPrimaryHKL) && (OrigHKL != gSecondaryHKL))
	{
		UnloadKeyboardLayout(OrigHKL);
	}
	if ((pwsSecondaryLayoutName == NULL) && (OrigSecHKL != 0))
	{
		UnloadKeyboardLayout(OrigSecHKL);
	}
	
	wcscpy_s(gwsLastPrimaryLayoutName, SizeOfArray(gwsLastPrimaryLayoutName), pwsPrimaryLayoutName);
	if (pwsSecondaryLayoutName != NULL)
	{
		wcscpy_s(gwsLastSecondaryLayoutName, SizeOfArray(gwsLastSecondaryLayoutName), pwsSecondaryLayoutName);
	}
	else
	{
		gwsLastSecondaryLayoutName[0] = L'\0';
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
bool DQGetLastKeyboardLayouts(int iLayoutSize, LPWSTR pwsPrimaryLayout, LPWSTR pwsSecondaryLayout)
{
	if (iLayoutSize < KL_NAMELENGTH + 1)
		return false;

	wcscpy_s(pwsPrimaryLayout, iLayoutSize, gwsLastPrimaryLayoutName);
	wcscpy_s(pwsPrimaryLayout, iLayoutSize, gwsLastSecondaryLayoutName);

	return true;
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
			Result = ChangeKeyboardLayout(pwsPrimaryLayout, pwsSecondaryLayout, true);
			if (Result == false)
				return false;
			break;

		case DvorakOnly:
			Result = ChangeKeyboardLayout(pwsPrimaryLayout, NULL, false);
			if (Result == false)
				return false;
			break;

		case Disabled:
			Result = ChangeKeyboardLayout(pwsSecondaryLayout, NULL, false);
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
bool DQSetFilteredSpecialKeys(bool IsControlFiltered, bool IsAltFiltered, bool IsWinFiltered, bool IsScrollLockQwertyEnabled, bool IsScrollLockDisabled)
{
	gIsControlFiltered = IsControlFiltered;
	gIsAltFiltered = IsAltFiltered;
	gIsWinFiltered = IsWinFiltered;
	gIsScrollLockToggleEnabled = IsScrollLockQwertyEnabled;
	gIsScrollLockRemoved = IsScrollLockDisabled;

	return true;
}

//external
UINT DQGetNumberAttachedProcs()
{
	return guiDLLRefCount;
}


} // namespace DQDOSKeyboard

#pragma unmanaged

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason)
	{
	case DLL_PROCESS_DETACH:
		// Do nothing, when main process is closed, the DLL will be unhooked.
		if (DQDOSKeyboard::guiDLLRefCount > 1)
		{
			DQDOSKeyboard::guiDLLRefCount--;
		}
		else
		{
			DQDOSKeyboard::guiDLLRefCount = 0;
			DQDOSKeyboard::DisableFilterHook();
		}
		break;

	case DLL_PROCESS_ATTACH:
		DQDOSKeyboard::guiDLLRefCount++;
		if (DQDOSKeyboard::guiDLLRefCount == 1)
		{
			DQDOSKeyboard::ghInstDLL = hinstDLL;
			DQDOSKeyboard::gwsLastPrimaryLayoutName[0] = L'\0';
			DQDOSKeyboard::gwsLastSecondaryLayoutName[0] = L'\0';
		}
		break;

	default:
		// Currently do nothing.
		break;
	}

	return TRUE;
}
