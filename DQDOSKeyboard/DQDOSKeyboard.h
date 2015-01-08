// DQDOSKeyboard.h

#pragma once

using namespace System;

#define DLLEXPORT extern "C" __declspec(dllexport)

namespace DQDOSKeyboard 
{
	typedef enum {Disabled = 0, DvorakOnly, DvorakQwerty} tKeyboardMode;

	DLLEXPORT Int32 DQGetKeyboardLayoutBufferSize();
	DLLEXPORT tKeyboardMode DQGetLastKeyboardMode();
	DLLEXPORT bool DQSetKeyboardMode(tKeyboardMode KeyboardMode, LPCWSTR pwsPrimaryLayout, LPCWSTR pwsSecondaryLayout);
	DLLEXPORT bool DQGetCurrentKeyboardLayout(int iLayoutSize, LPWSTR pwsLayoutName);
}
