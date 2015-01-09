﻿/*************************************************************************
 * Program.cs - Copyright 2015 Derick Snyder                             *
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DQDOS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DQMainForm());
        }
    }
}
