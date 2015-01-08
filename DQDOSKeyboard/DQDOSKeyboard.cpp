// This is the main DLL file.

#include "stdafx.h"

#include <cstdlib>
#include <Windows.h>

#include "DQDOSKeyboard.h"

namespace DQDOSKeyboard 
{
	// Globals
	const Int32 guiLayoutStringSize = (KL_NAMELENGTH + (KL_NAMELENGTH % 8) + 8);
	tKeyboardMode geLastKeyboardMode = Disabled;

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
	
}
