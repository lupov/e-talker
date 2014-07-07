/**************************************************************************
 * Copiright(C) 2014 Sergey Lupov, Natalia Fradkina
 *
 * e-talker(v.1.X.X) is an application for studying english. 
 * Prepared english phrases are spoken in a random order.
 * You have to recall the translation of the phrase
 * and after a short pause they are spoken by the program automatically.
 *
 * Web Site: http://e-talker.ru	
 * E-mail: sergey.lupov {at} pselab.ru
 *
 * This file is part of e-talker.
 * 
 * e-talker is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Foobar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 ***************************************************************************/

using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace eTalker
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
