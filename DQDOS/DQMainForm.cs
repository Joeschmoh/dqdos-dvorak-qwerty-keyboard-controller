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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DQDOS
{
    public partial class DQMainForm : Form
    {

        private bool m_bUserClosing = false;
        private bool m_bSystemClosing = false;

        public DQMainForm()
        {
            InitializeComponent();

            this.Icon = Properties.Resources.DQEnabled;

            // TODO: Load in last state of application.

            // TODO: Make this defaults if no previous state found. 
            DQModeDisabledRadio.Checked = true;
            SetKeyboardLayoutTextBox("Dvorak", PrimaryKBTextBox);
            SetKeyboardLayoutTextBox("Qwerty", SecondaryKBTextBox);
            DQChangeKeyboardMode();
        }

        public void DQSystemShutdown()
        {
            m_bSystemClosing = true;
            this.Close();
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
    }
}
