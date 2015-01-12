/*************************************************************************
 * DQMainForm.cs - Copyright 2015 Derick Snyder                          *
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

// TODO LIST:
// TODO -- Prevent scroll lock light from coming on? Or just resetting it so we are in sync in DQDOS.
// TODO -- Test, review, and clean up code.
// TODO -- Various TODOs Throughout code.
// TODO -- Clean up the GUI or make a much better one. It works, not sure I like it.
// TODO -- Get better icons.
// TODO -- 3 states and then scroll lock mode -- could be confusing as a UI indicator, maybe better ideas in the future.
// TODO -- Figure out Windows key filtering -- looks like you need to use LL Keyboard hook to do this.
// TODO -- Get many more language IDs as "common names". Maybe make the list a file so it can be updated dynamically.
// TODO -- Bubble Icon text when toggling scroll lock Qwerty mode? But means going from C++ DLL back to C# GUI.
// ----------------------
// DONE -- Save state when exiting.
// DONE -- Load state on open, set defaults if no state found.
// DONE -- Be sure it exits cleanly (and saves state) when exiting.
// DONE -- Considering saving state periodically because clean exits don't always happen.
// DONE -- Remove windows filtering since it doesn't work until I can do it properly.
// DONE -- Used Scroll lock -- Is there a way to set a qwerty mode when a key is held down?
// DONE -- Added mutex so that only instance runs at a time.

namespace DQDOS
{
    public partial class DQMainForm : Form
    {
        
        public class SavedSettings
        {
            // This class obviously saves / loads state between executions, but it also it the source of defaults.
            // If the class can't load state, it passes back defaults to the form.

            private static String SavedSettingsFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DQDOS\\";
            private const String SavedSettingsFileName = "DQDOSSettings.xml";

            public String PrimaryLayoutName = "00010409";
            public String SecondaryLayoutName = "00000409";
            public DQDOSKeyboard.tKeyboardMode LastKeyboardMode = DQDOSKeyboard.tKeyboardMode.Disabled;
            public bool IsControlFiltered = true;
            public bool IsAltFiltered = true;
            public bool IsScrollLockQwertyModeEnabled = true;
            public bool IsScrollLockRemoved = true;
            public bool IsAppHidden = false;
            public bool IsHiddenAppWarningShown = false;
            public String LastSaveTime = "";

            public static SavedSettings LoadAppSettings()
            {
                SavedSettings Myself = null;
                FileStream MyFileStream = null;
                XmlSerializer MySerializer = null;
                String FullFileName = SavedSettingsFolder + SavedSettingsFileName;

                try
                {
                    MySerializer = new XmlSerializer(typeof(SavedSettings));
                    MyFileStream = new FileStream(FullFileName, FileMode.Open);

                    Myself = (SavedSettings) MySerializer.Deserialize(MyFileStream);
                }
                catch
                {
                    // do nothing...
                }
                finally
                {
                    if (MyFileStream != null)
                        MyFileStream.Close();

                    if (Myself == null)
                        Myself = new SavedSettings(); // return defaults if we didn't find anything.
                }

                return Myself;
            }

            public bool SaveAppSettings()
            {
                bool IsSuccess = false;
                StreamWriter MyFileWriter = null;
                XmlSerializer MySerializer = null;
                String FullFileName = SavedSettingsFolder + SavedSettingsFileName;
                LastSaveTime = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");

                try
                {
                    System.IO.Directory.CreateDirectory(SavedSettingsFolder);

                    MySerializer = new XmlSerializer(typeof(SavedSettings));
                    MyFileWriter = new StreamWriter(FullFileName, false);

                    MySerializer.Serialize(MyFileWriter, this);

                    IsSuccess = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    IsSuccess = false;
                }
                finally
                {
                    if (MyFileWriter != null)
                    {
                        MyFileWriter.Close();
                    }
                }

                return IsSuccess;
            }
        }

        private bool m_bUserClosing = false;
        private bool m_bSystemClosing = false;
        private bool m_IsHiddentWarningShown = false;
        private DateTime m_LastSettingsSave = DateTime.UtcNow;

        public DQMainForm()
        {
            InitializeComponent();

            this.Icon = Properties.Resources.DQEnabled;
        }
        
        public void DQSaveAppSettings()
        {
            SavedSettings MySavedSettings = new SavedSettings();

            MySavedSettings.LastKeyboardMode = DQDOSKeyboard.GetLastKeyboardMode();
            MySavedSettings.PrimaryLayoutName = DQDOSKeyboardLayout.CommonNameToLayoutName(PrimaryKBTextBox.Text);
            if (MySavedSettings.PrimaryLayoutName == null)
                MySavedSettings.PrimaryLayoutName = PrimaryKBTextBox.Text;
            MySavedSettings.SecondaryLayoutName = DQDOSKeyboardLayout.CommonNameToLayoutName(SecondaryKBTextBox.Text);
            if (MySavedSettings.SecondaryLayoutName == null)
                MySavedSettings.SecondaryLayoutName = SecondaryKBTextBox.Text;
            MySavedSettings.IsControlFiltered = ControlKeyCheckBox.Checked;
            MySavedSettings.IsAltFiltered = AltKeyCheckBox.Checked;
            MySavedSettings.IsScrollLockQwertyModeEnabled = ScrollLockQwertyCheckBox.Checked;
            MySavedSettings.IsScrollLockRemoved = ScrollLockDisabledCheckBox.Checked;
            MySavedSettings.IsAppHidden = ! (this.Visible);
            MySavedSettings.IsHiddenAppWarningShown = m_IsHiddentWarningShown;

            MySavedSettings.SaveAppSettings();
            m_LastSettingsSave = DateTime.UtcNow;
        }

        public void DQLoadAppSettings()
        {
            SavedSettings MySavedSettings = SavedSettings.LoadAppSettings();

            if (MySavedSettings == null)
                throw new Exception("Unable to load default settings or a saved settings file, DQDOS will terminate.");

            m_IsHiddentWarningShown = MySavedSettings.IsHiddenAppWarningShown;
            SetKeyboardLayoutTextBox(MySavedSettings.PrimaryLayoutName, PrimaryKBTextBox);
            SetKeyboardLayoutTextBox(MySavedSettings.SecondaryLayoutName, SecondaryKBTextBox);
            ControlKeyCheckBox.Checked = MySavedSettings.IsControlFiltered;
            AltKeyCheckBox.Checked = MySavedSettings.IsAltFiltered;
            ScrollLockQwertyCheckBox.Checked = MySavedSettings.IsScrollLockQwertyModeEnabled;
            ScrollLockDisabledCheckBox.Checked = MySavedSettings.IsScrollLockRemoved;
            
            SetFilteredControlKeys();

            // This will trigger events that will set the mode too.
            switch (MySavedSettings.LastKeyboardMode)
            {
                case DQDOSKeyboard.tKeyboardMode.Disabled:
                    DQModeDisabledRadio.Checked = true;
                    break;

                case DQDOSKeyboard.tKeyboardMode.DvorakOnly:
                    DQModeDvorakOnlyRadio.Checked = true;
                    break;

                case DQDOSKeyboard.tKeyboardMode.DvorakQwerty:
                    DQModeFullEnableRadio.Checked = true;
                    break;
            }

            if (MySavedSettings.IsAppHidden)
            {
                // The "show" then "hide" seems like a hack, but without showing the form once, my shutdown message box 
                // warning doesn't launch. That is, if the forms stays hidden the whole time, then the NotifyIcon exit 
                // function just exits abruptly but doesn't stop the application message loop because it doesn't call the
                // "closing" and "closed" events.

                this.Opacity = 0;
                ShowGUI();
                HideGUI();
                this.Opacity = 100;
            }
            else
            {
                ShowGUI();
            }

            DQChangeKeyboardMode();
        }

        private void DQMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DQSaveAppSettings();

            Application.Exit();
        }

        public void DQSystemShutdown()
        {
            m_bSystemClosing = true;
            this.Close();
        }

        public void DoHiddenWarningBubble()
        {
            DQNotifyIcon.ShowBalloonTip(15000, "DQDOS", "DQDOS is still running and has moved to your tray. Click the icon to change settings or re-open.", ToolTipIcon.Info);
        }

        private void ShowGUI()
        {
            this.Visible = true;
            openGUIToolStripMenuItem.Text = "Hide GUI";
        }

        private void HideGUI()
        {
            this.Visible = false;
            openGUIToolStripMenuItem.Text = "Show GUI";
            if (m_IsHiddentWarningShown == false)
            {
                DoHiddenWarningBubble();
                m_IsHiddentWarningShown = true;
            }
        }

        private void DQSyncModeRadioToContextMenu()
        {
            dvorakQwertyKeyboardToolStripMenuItem.Checked = DQModeFullEnableRadio.Checked;
            dvorakOnlyToolStripMenuItem.Checked = DQModeDvorakOnlyRadio.Checked;
            disabledQwertyToolStripMenuItem.Checked = DQModeDisabledRadio.Checked;
        }

        private void DQChangeKeyboardMode()
        {
            DQDOSKeyboard.tKeyboardMode OldMode = DQDOSKeyboard.GetLastKeyboardMode();
            DQDOSKeyboard.tKeyboardMode NewMode = DQDOSKeyboard.tKeyboardMode.Disabled;
            Icon NewIcon = null;
            String PrimaryLayout = null; 
            String SecondaryLayout = null;

            PrimaryLayout = DQDOSKeyboardLayout.CommonNameToLayoutName(PrimaryKBTextBox.Text);
            if (PrimaryLayout == null)
                PrimaryLayout = PrimaryKBTextBox.Text;

            SecondaryLayout = DQDOSKeyboardLayout.CommonNameToLayoutName(SecondaryKBTextBox.Text);
            if (SecondaryLayout == null)
                SecondaryLayout = SecondaryKBTextBox.Text;

            if (DQModeDisabledRadio.Checked == true)
            {
                // Keyboard mode "disabled".
                NewMode = DQDOSKeyboard.tKeyboardMode.Disabled;
                NewIcon = Properties.Resources.DQDisabled16x16;
            }
            else if (DQModeDvorakOnlyRadio.Checked == true)
            {
                // Keyboard mode "Dvorak Only".
                NewMode = DQDOSKeyboard.tKeyboardMode.DvorakOnly;
                NewIcon = Properties.Resources.DQEnabled32x32;
            }
            else
            {
                // Keyboard mode "Fully Enabled".
                NewMode = DQDOSKeyboard.tKeyboardMode.DvorakQwerty;
                NewIcon = Properties.Resources.DQEnabled16x16;
            }

            if (NewMode != OldMode) // TODO: Also check whether layouts have changed.
            {
                if (DQDOSKeyboard.SetKeyboardMode(NewMode, PrimaryLayout, SecondaryLayout) == true)
                {
                    DQSyncModeRadioToContextMenu();
                    DQNotifyIcon.Icon = NewIcon;
                    DQNotifyIcon.Visible = true;
                }
            }
        }

        private void DQModeDisabledRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (DQModeDisabledRadio.Checked)
            {
                DQChangeKeyboardMode();
            }
        }

        private void DQModeDvorakOnlyRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (DQModeDvorakOnlyRadio.Checked)
            {
                DQChangeKeyboardMode();
            }
        }

        private void DQModeFullEnableRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (DQModeFullEnableRadio.Checked)
            {
                DQChangeKeyboardMode();
            }
        }

        private void DQHideButton_Click(object sender, EventArgs e)
        {
            HideGUI();
        }

        private void DQQuitButton_Click(object sender, EventArgs e)
        {
            m_bUserClosing = true;
            this.Close();
        }

        private void DQMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult mbrAnswer = DialogResult.No;

            if (m_bSystemClosing == false)
            {
                // Normal close functionality.

                // "X" on main window, Alt-F4, etc., just close window.
                if (m_bUserClosing == false)
                {
                    e.Cancel = true;
                    HideGUI();
                    return;
                }

                // Other close message, including Quit button should confirm and then close.
                mbrAnswer = MessageBox.Show("This will exit and disable DQDOS, are you sure you want to do this?", "DQDOS", MessageBoxButtons.YesNo);
                if (mbrAnswer != DialogResult.Yes)
                {
                    e.Cancel = true;
                    m_bUserClosing = false;
                }
            }
            // else just exit.
        }

        private void DQNotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            // toggle fully-enabled to fully disabled.
            if (DQModeDisabledRadio.Checked == true)
            {
                DQModeFullEnableRadio.Checked = true;
            }
            else
            {
                DQModeDisabledRadio.Checked = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_bUserClosing = true;
            this.Close();
        }

        private void openGUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                ShowGUI();
            }
            else
            {
                HideGUI();
            }
            
        }

        private void dvorakQwertyKeyboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DQModeFullEnableRadio.Checked = true;
        }

        private void dvorakOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DQModeDvorakOnlyRadio.Checked = true;
        }

        private void disabledQwertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DQModeDisabledRadio.Checked = true;
        }

        private void GetCurrentLayoutButton_Click(object sender, EventArgs e)
        {
            String Layout = DQDOSKeyboard.GetCurrentKeyboardLayout();
            SetKeyboardLayoutTextBox(Layout, DQCurrentKBTextBox);
        }

        private void SetKeyboardLayoutTextBox(String Layout, TextBox CurrentTextBox)
        {
            String LayoutName = DQDOSKeyboardLayout.CommonNameToLayoutName(Layout);
            
            if (LayoutName == null)
            {
                CurrentTextBox.Text = DQDOSKeyboardLayout.LayoutNameToCommonName(Layout);
            }
            else
            {
                CurrentTextBox.Text = DQDOSKeyboardLayout.LayoutNameToCommonName(LayoutName);
            }
            
            
        }

        private String GetKeyboardLayoutTextBox(TextBox CurrentTextBox)
        {
            String LayoutName = DQDOSKeyboardLayout.CommonNameToLayoutName(CurrentTextBox.Text);

            if (LayoutName != null)
            {
                return LayoutName;
            }

            return CurrentTextBox.Text;
        }

        private void PrimaryKBTextBox_Leave(object sender, EventArgs e)
        {
            SetKeyboardLayoutTextBox(PrimaryKBTextBox.Text, PrimaryKBTextBox);
        }

        private void SecondardKBTextBox_Leave(object sender, EventArgs e)
        {
            SetKeyboardLayoutTextBox(SecondaryKBTextBox.Text, SecondaryKBTextBox);
        }

        private void SetFilteredControlKeys()
        {
            DQDOSKeyboard.SetFilteredSpecialKeys(ControlKeyCheckBox.Checked, AltKeyCheckBox.Checked, ScrollLockQwertyCheckBox.Checked, ScrollLockDisabledCheckBox.Checked);
        }

        private void ControlKeyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetFilteredControlKeys();
        }

        private void AltKeyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetFilteredControlKeys();
        }

        private void WindowsKeyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetFilteredControlKeys();
        }

        private void PeriodicSaveTimer_Tick(object sender, EventArgs e)
        {
            // TODO: If we want to be really sophisticated, we would save settings soon after they change.
            // But I didn't build in any detection of changes. This was easier.
            if (DateTime.UtcNow.Subtract(m_LastSettingsSave).TotalMinutes >= 60)
                DQSaveAppSettings();
        }

        private void ScrollLockQwertyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetFilteredControlKeys();
        }

        private void ScrollLockDisabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetFilteredControlKeys();
        }

        private void DQNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            MethodInfo mi = null;

            // Taken from Internet so context menu behaves normally.
            if (e.Button == MouseButtons.Left)
            {
                mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                if (mi != null)
                    mi.Invoke(DQNotifyIcon, null);
            }
            
            // This is how you would expect it to work, but the menu doesn't go away from it loses focus.
            //DQNotifyIcon.ContextMenuStrip.Show(this, Control.MousePosition);
        }

        private void DQNotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            m_IsHiddentWarningShown = false;
            ShowGUI();
        }
    }
}
