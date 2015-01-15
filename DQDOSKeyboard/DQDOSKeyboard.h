/************************************************************************
* DQDOSKeyboard.h - Copyright 2015 Derick Snyder                        *
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

#pragma once

using namespace System;

#define DLLEXPORT extern "C" __declspec(dllexport)

namespace DQDOSKeyboard 
{
	typedef enum {FirstRun = -1, Disabled = 0, DvorakOnly, DvorakQwerty} tKeyboardMode;

	DLLEXPORT Int32 DQGetKeyboardLayoutBufferSize();
	DLLEXPORT tKeyboardMode DQGetLastKeyboardMode();
	DLLEXPORT bool DQGetLastKeyboardLayouts(int iLayoutSize, LPWSTR pwsPrimaryLayout, LPWSTR pwsSecondaryLayout);
	DLLEXPORT bool DQSetKeyboardMode(tKeyboardMode KeyboardMode, LPCWSTR pwsPrimaryLayout, LPCWSTR pwsSecondaryLayout);
	DLLEXPORT bool DQGetCurrentKeyboardLayout(int iLayoutSize, LPWSTR pwsLayoutName);
	DLLEXPORT bool DQSetFilteredSpecialKeys(bool IsControlFiltered, bool IsAltFiltered, bool IsWinFiltered, bool IsScrollLockQwertyEnabled, bool IsScrollLockDisabled);
	DLLEXPORT UINT DQGetNumberAttachedProcs();
}
