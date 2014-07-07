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
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Text.RegularExpressions;
using KrasimirTrapper;

    
namespace eTalker
{
    public partial class Form1 : Form
    {
        private string[,][] Phrase;
        private int[] SelectedVerbs;
        private MP3Player mplayer;
        int HowManySelected = 0;
        Random rand=new Random();
        string RusText;
        string EngText;
        string FileName;
        Regex regex_delete_quation = new Regex(@"\s*Вопрос[\s.]*",RegexOptions.IgnoreCase);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form1_Resize(sender, e);
            lbText.Dock = DockStyle.Fill;
            mplayer = new MP3Player();
            mplayer.SongEnd += new MP3Player.SongEndEventHandler(mplayer_SongEnd);

            dataGridView1.DefaultCellStyle.Font = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
#if DEBUG
            Environment.CurrentDirectory = "..\\..";
#endif
            string text = "";
            const string xml_file_name = @"Lesson1.xml";
            if (!File.Exists(xml_file_name))
            {
                string md = Environment.GetFolderPath(Environment.SpecialFolder.Personal) +
                                                    @"\e-talker.ru"; //My Documents path
                if (Directory.Exists(md) == true) 
                    Environment.CurrentDirectory = md;
            }
            try
            {
                text = File.ReadAllText(xml_file_name, System.Text.Encoding.GetEncoding(65001));
                XMLParser(text);
            }
            catch (System.IO.FileNotFoundException)
            {
                openXMLFileToolStripMenuItem.PerformClick();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Start")
            {
                HowManySelected=0;
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                  if(row.Cells[0].Style.ForeColor.R==Color.Black.R) HowManySelected++;
                }
                SelectedVerbs = new int[HowManySelected];
                HowManySelected = 0;
                for (int i=0; i<dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Style.ForeColor.R == Color.Black.R) SelectedVerbs[HowManySelected++]=i;
                }
                if (HowManySelected==0) return;
                if (!rbtnEng.Checked) timer1.Interval = 1500;
                else timer1.Interval = Convert.ToInt32(updwnPause.Text)*1000;
                timer1.Enabled = true;
                timer2.Interval = Convert.ToInt32(updwnPause.Text)*1000;
                btnStart.Text = "Stop";
                dataGridView1.Visible = false;
                rbtnEng.Enabled = false;
                rbtnEngRus.Enabled = false;
                rbtnRusEng.Enabled = false;
                updwnPause.Enabled = false;
                label1.Enabled = false;
                menuStrip1.Visible = false;
                lbText.Visible = true;
                lbText.Enabled = true;
                lbText.Text = "";
            }
            else
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
                btnStart.Text = "Start";
                mplayer.Stop();
                mplayer.Close();
                dataGridView1.Visible = true;
                rbtnEngRus.Enabled = true;
                rbtnRusEng.Enabled = true;
                rbtnEng.Enabled = true;
                updwnPause.Enabled = true;
                label1.Enabled = true;
                menuStrip1.Visible = true;
                lbText.Visible = false;
                lbText.Enabled = false;
                dataGridView1.Focus();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Column2.Width = (int)((dataGridView1.ClientSize.Width-Column1.Width) / 4);
            Column3.Width = Column2.Width;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (HowManySelected <= 0) return;
            int NumberSel = rand.Next(HowManySelected);
            NumberSel = SelectedVerbs[NumberSel];
            int NumberInSel = rand.Next(Phrase[NumberSel,0].Length);
            string dir = Directory.GetCurrentDirectory();
            FileName = Phrase[NumberSel, 2][NumberInSel];
            RusText = Phrase[NumberSel, 1][NumberInSel];
            EngText = Phrase[NumberSel, 0][NumberInSel];
            RusText = regex_delete_quation.Replace(RusText, "");

            if (rbtnEngRus.Checked || rbtnEng.Checked)
            {
                mplayer.Open(FileName + "_en.mp3");
                lbText.Text = EngText;
            }
            else
            {
                mplayer.Open(FileName + "_ru.mp3");
                lbText.Text = RusText;
            }

            if(mplayer.IsOpened) mplayer.Play();
            else
            {
                if (rbtnEng.Checked) timer1.Enabled = true;
                else timer2.Enabled = true;
            }
        }
        void mplayer_SongEnd(Object sender, MP3Player.SongEndEventArgs e)
        {
            if (FileName.Length == 0 || rbtnEng.Checked) timer1.Enabled = true;
            else timer2.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            if (rbtnEngRus.Checked)
            {
                mplayer.Open(FileName + "_ru.mp3");
                lbText.Text = RusText;
            }
            else
            {
                mplayer.Open(FileName + "_en.mp3");
                lbText.Text = EngText;
            }
            if (mplayer.IsOpened) mplayer.Play();
            else timer1.Enabled = true;
            
            FileName = "";
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
            case Keys.Enter:
                e.SuppressKeyPress = true;
                btnStart.PerformClick();
                break;
            case Keys.Space:
            case Keys.Insert:
                Row_Sel_Unsel();
                int num_row=dataGridView1.CurrentRow.Index+1;
                if (num_row < dataGridView1.Rows.Count)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[num_row].Cells[0];
                }
                break;
            case Keys.Home:
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
                break;
            case Keys.End:
                dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0];
                break;
            case Keys.Multiply:
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                  Row_Sel_Unsel(row.Index);
                }
                break;
            case Keys.Add:
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    Row_Sel(row.Index);
                }
                break;
            case Keys.Subtract:
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    Row_Unsel(row.Index);
                }
                break;
            }
        }

        private void Row_Sel()
        {
            int num_row = dataGridView1.CurrentRow.Index;
            Row_Sel(num_row);
        }
        private void Row_Unsel()
        {
            int num_row = dataGridView1.CurrentRow.Index;
            Row_Sel(num_row);
        }

        private void Row_Sel(int num_row)
        {
            DataGridViewCellCollection cells = dataGridView1.Rows[num_row].Cells;

            for (int i = 0; i < 4; i++)
            {
                cells[i].Style.Font = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold);
                cells[i].Style.ForeColor = Color.Black;
                cells[i].Style.SelectionForeColor = Color.Black;
            }
        }
        private void Row_Unsel(int num_row)
        {
            DataGridViewCellCollection cells = dataGridView1.Rows[num_row].Cells;

            for (int i = 0; i < 4; i++)
            {
                cells[i].Style.Font = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Regular);
                cells[i].Style.ForeColor = Color.Gray;
                cells[i].Style.SelectionForeColor = Color.Gray;
            }
        }
        private void Row_Sel_Unsel()
        {
            int num_row = dataGridView1.CurrentRow.Index;
            Row_Sel_Unsel(num_row);
        }
        private void Row_Sel_Unsel(int num_row)
        {
            DataGridViewCellCollection cells = dataGridView1.Rows[num_row].Cells;

            if (cells[0].Style.ForeColor.R == Color.Black.R) Row_Unsel(num_row);
            else Row_Sel(num_row);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>=0)Row_Sel_Unsel();
        }

        void XMLParser(string text)
        {
        string main_dir;

            Regex regex_maindir = new Regex(@"<MainDir>((.|\n)*?)</MainDir>", RegexOptions.IgnoreCase);
            Regex regex_dir = new Regex(@"<Dir>((.|\n)*?)</Dir>", RegexOptions.IgnoreCase);
            Regex regex_verb = new Regex(@"<Verb>((.|\n)*?)</Verb>", RegexOptions.IgnoreCase);
            Regex regex_present = new Regex(@"<PresentIndef>((.|\n)*?)</PresentIndef>", RegexOptions.IgnoreCase);
            Regex regex_past = new Regex(@"<PastIndef>((.|\n)*?)</PastIndef>", RegexOptions.IgnoreCase);
            Regex regex_rus = new Regex(@"[^(?=<Text>)][.\n]*<Rus>((.|\n)*?)</Rus>", RegexOptions.IgnoreCase);
            Regex regex_text = new Regex(@"<Text>((.|\n)*?)</Text>", RegexOptions.IgnoreCase);
            Regex regex_text_eng = new Regex(@"<Eng>((.|\n)*?)</Eng>", RegexOptions.IgnoreCase);
            Regex regex_text_rus = new Regex(@"<Rus>((.|\n)*?)</Rus>", RegexOptions.IgnoreCase);
            Regex regex_text_file = new Regex(@"<File>((.|\n)*?)</File>", RegexOptions.IgnoreCase);
            Regex regex_replace_space = new Regex(@"\s+");
            Regex regex_delete_znaks = new Regex(@"[\[\].,'?!-:]");

            Match match_maindir = regex_maindir.Match(text);
            if (match_maindir.Groups.Count > 1) main_dir = match_maindir.Groups[1].Value;
            else main_dir = "mp3";
            MatchCollection matches = regex_verb.Matches(text);
            Phrase = new string[matches.Count, 3][];
            HowManySelected = matches.Count;
            dataGridView1.RowCount = 0;
            for (int i = 0; i < matches.Count; i++)
            {
                Match match_dir = regex_dir.Match(matches[i].Groups[1].Value);
                Match match_present = regex_present.Match(matches[i].Groups[1].Value);
                Match match_past = regex_past.Match(matches[i].Groups[1].Value);
                Match match_rus = regex_rus.Match(matches[i].Groups[1].Value);
                string verb_dir;
                if (match_dir.Groups.Count > 1) verb_dir = match_dir.Groups[1].Value;
                else
                {
                    verb_dir = match_present.Groups[1].Value;
                    verb_dir = regex_delete_znaks.Replace(verb_dir, "");

                }

                if (match_present.Groups.Count > 1 || match_past.Groups.Count > 1 || match_rus.Groups.Count > 1)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = i + 1;
                    dataGridView1.Rows[i].Cells[1].Value = match_present.Groups[1].Value;
                    dataGridView1.Rows[i].Cells[2].Value = match_past.Groups[1].Value;
                    dataGridView1.Rows[i].Cells[3].Value = match_rus.Groups[1].Value;
                }
                MatchCollection matches_text = regex_text.Matches(matches[i].Groups[1].Value);
                for (int k = 0; k < 3; k++) Phrase[i, k] = new string[matches_text.Count];
                for (int n = 0; n < matches_text.Count; n++)
                {
                    Match match_text_eng = regex_text_eng.Match(matches_text[n].Groups[1].Value);
                    Match match_text_rus = regex_text_rus.Match(matches_text[n].Groups[1].Value);
                    Match match_text_file = regex_text_file.Match(matches_text[n].Groups[1].Value);
                    if (match_text_eng.Groups.Count > 1) Phrase[i, 0][n] = match_text_eng.Groups[1].Value;
                    else Phrase[i, 0][n] = "";
                    if (match_text_rus.Groups.Count > 1) Phrase[i, 1][n] = match_text_rus.Groups[1].Value;
                    else Phrase[i, 1][n] = "";
                    if (match_text_file.Groups.Count > 1) Phrase[i, 2][n] = match_text_file.Groups[1].Value;
                    else if (match_text_eng.Groups.Count > 1)
                    {
                        Phrase[i, 2][n] = regex_replace_space.Replace(Phrase[i, 0][n], "_");
                        Phrase[i, 2][n] = regex_delete_znaks.Replace(Phrase[i, 2][n], "");
                    }
                    else Phrase[i, 2][n] = "";
                    Phrase[i, 2][n] = main_dir+"\\"+match_dir.Groups[1].Value + "\\" + Phrase[i, 2][n];
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openXMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите файл с уроком";
            openFileDialog.Filter = "XML файлы|*.xml";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            Directory.SetCurrentDirectory(Path.GetDirectoryName(openFileDialog.FileName));
            string text;
            text = File.ReadAllText(openFileDialog.FileName, System.Text.Encoding.GetEncoding(65001));
            XMLParser(text);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormHelp formHelp = new FormHelp();
            formHelp.Show();
        }

        private void aboutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FormAbout formAbout = new FormAbout();
            formAbout.ShowDialog();
        }
    }
}   
