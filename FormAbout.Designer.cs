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

namespace eTalker
{
    partial class FormAbout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.lbProductName = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.lbCopyright = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.llbWebSite = new System.Windows.Forms.LinkLabel();
            this.tbBrief = new System.Windows.Forms.TextBox();
            this.rtbGPLBrief = new System.Windows.Forms.RichTextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbProductName
            // 
            this.lbProductName.AutoSize = true;
            this.lbProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbProductName.Location = new System.Drawing.Point(127, 9);
            this.lbProductName.Name = "lbProductName";
            this.lbProductName.Size = new System.Drawing.Size(41, 13);
            this.lbProductName.TabIndex = 1;
            this.lbProductName.Text = "label2";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbVersion.Location = new System.Drawing.Point(127, 28);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(41, 13);
            this.lbVersion.TabIndex = 2;
            this.lbVersion.Text = "label3";
            // 
            // lbCopyright
            // 
            this.lbCopyright.AutoSize = true;
            this.lbCopyright.Location = new System.Drawing.Point(9, 176);
            this.lbCopyright.Name = "lbCopyright";
            this.lbCopyright.Size = new System.Drawing.Size(35, 13);
            this.lbCopyright.TabIndex = 3;
            this.lbCopyright.Text = "label4";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Название продукта:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Версия:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Адрес в интернете:";
            // 
            // llbWebSite
            // 
            this.llbWebSite.AutoSize = true;
            this.llbWebSite.Location = new System.Drawing.Point(127, 47);
            this.llbWebSite.Name = "llbWebSite";
            this.llbWebSite.Size = new System.Drawing.Size(55, 13);
            this.llbWebSite.TabIndex = 9;
            this.llbWebSite.TabStop = true;
            this.llbWebSite.Text = "linkLabel1";
            this.llbWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbWebSite_LinkClicked);
            // 
            // tbBrief
            // 
            this.tbBrief.BackColor = System.Drawing.SystemColors.Control;
            this.tbBrief.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbBrief.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBrief.Location = new System.Drawing.Point(15, 69);
            this.tbBrief.Multiline = true;
            this.tbBrief.Name = "tbBrief";
            this.tbBrief.Size = new System.Drawing.Size(298, 104);
            this.tbBrief.TabIndex = 11;
            this.tbBrief.TabStop = false;
            this.tbBrief.Text = resources.GetString("tbBrief.Text");
            // 
            // rtbGPLBrief
            // 
            this.rtbGPLBrief.Location = new System.Drawing.Point(12, 200);
            this.rtbGPLBrief.Name = "rtbGPLBrief";
            this.rtbGPLBrief.ReadOnly = true;
            this.rtbGPLBrief.ShortcutsEnabled = false;
            this.rtbGPLBrief.Size = new System.Drawing.Size(295, 100);
            this.rtbGPLBrief.TabIndex = 12;
            this.rtbGPLBrief.TabStop = false;
            this.rtbGPLBrief.Text = "";
            this.rtbGPLBrief.ZoomFactor = 2F;
            this.rtbGPLBrief.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbGPLBrief_LinkClicked);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOk.Location = new System.Drawing.Point(121, 311);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FormAbout
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOk;
            this.ClientSize = new System.Drawing.Size(319, 343);
            this.ControlBox = false;
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.rtbGPLBrief);
            this.Controls.Add(this.tbBrief);
            this.Controls.Add(this.llbWebSite);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbCopyright);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.lbProductName);
            this.Name = "FormAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = " About";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbProductName;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Label lbCopyright;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel llbWebSite;
        private System.Windows.Forms.TextBox tbBrief;
        private System.Windows.Forms.RichTextBox rtbGPLBrief;
        private System.Windows.Forms.Button btnOk;

    }
}