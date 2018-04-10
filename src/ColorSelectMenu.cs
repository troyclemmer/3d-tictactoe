using System;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
//using System.Drawing.Drawing2D;

namespace SeniorProject
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	/// 

	public class ColorSelectMenu : Form
	{
		private System.Windows.Forms.ListBox lstColor;
		private string []data;
		private Color []color;
        Game game;
        private Button button1;
        private TextBox textBox1;
        private Label label1;


		private System.ComponentModel.Container components = null;

        public ColorSelectMenu(Game game_, List<string> colorList)
		{
			InitializeComponent();
            game = game_;

            data = new String[colorList.Count];
            color = new Color[colorList.Count];
            fillColorListBox(colorList);
		}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool baseResult = base.ProcessCmdKey(ref msg, keyData);

            if (keyData == Keys.Enter)
            {
                submit();
            }

            return baseResult;
        }

        private void fillColorListBox(List<string> colorList_)
        {
            //counter for index reference
            int i = 0;

            //add each color to the list
            foreach (string c in colorList_)
            {
                data[i] = c;
                color[i] = Color.FromName(c);
                i++;
            }

            lstColor.DataSource = data;

        }

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lstColor = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstColor
            // 
            this.lstColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lstColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lstColor.ForeColor = System.Drawing.Color.White;
            this.lstColor.Location = new System.Drawing.Point(12, 61);
            this.lstColor.Name = "lstColor";
            this.lstColor.Size = new System.Drawing.Size(202, 251);
            this.lstColor.TabIndex = 0;
            this.lstColor.TabStop = false;
            this.lstColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DrawItemHandler);
            this.lstColor.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.MeasureItemHandler);
            this.lstColor.SelectedIndexChanged += new System.EventHandler(this.lstColor_SelectedIndexChanged);
            this.lstColor.DoubleClick += new System.EventHandler(this.lstColor_DoubleClick);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderSize = 3;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MidnightBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(12, 318);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(202, 30);
            this.button1.TabIndex = 1;
            this.button1.TabStop = false;
            this.button1.Text = "DONE";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(71, 12);
            this.textBox1.MaxLength = 18;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(143, 29);
            this.textBox1.TabIndex = 2;
            this.textBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Name:";
            // 
            // ColorSelectMenu
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(226, 360);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lstColor);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "ColorSelectMenu";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Player Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		private void DrawItemHandler(object sender,  DrawItemEventArgs e)
		{
			e.DrawBackground();
			e.DrawFocusRectangle();
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                // If item is selected, draw a red selection box
                e.Graphics.FillRectangle(Brushes.MidnightBlue, e.Bounds);

                //draw colored text
                //e.Graphics.DrawString(data[e.Index], new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold), new SolidBrush(color[e.Index]), e.Bounds);
            }
            //else
            //{
                //draw white text if not selected
                //e.Graphics.DrawString(this.lstColor.Items[e.Index].ToString(), this.lstColor.Font, Brushes.White, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                
                //draw colored text
                e.Graphics.DrawString(data[e.Index], new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold), new SolidBrush(color[e.Index]), e.Bounds);
            //}
	
		}
		private void MeasureItemHandler(object sender, MeasureItemEventArgs e)
		{
			e.ItemHeight= 22;
		}

        private void submit()
        {
            string name = textBox1.Text.Trim();
            textBox1.Text = name;

            if (name.Length < 1)
            {
                game.sendError("Please give yourself a name.");
                textBox1.Focus();
            }
            else
            {
                try
                {
                    HumanPlayer player = new HumanPlayer(name, lstColor.SelectedItem.ToString());
                    //add the human player to the game
                    game.addPlayer(player);

                    this.Close();
                }
                catch (Exception)
                {
                    game.sendError("Ran out of color choices.");
                    Application.Exit();
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            submit();
        }

        private void lstColor_DoubleClick(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void lstColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            Rectangle rc = lstColor.GetItemRectangle(lstColor.SelectedIndex);
            LinearGradientBrush brush = new LinearGradientBrush(
                rc, Color.Transparent, Color.MidnightBlue, LinearGradientMode.ForwardDiagonal);
            Graphics g = Graphics.FromHwnd(lstColor.Handle);

            g.FillRectangle(brush, rc);
             */

        }
         

	}
}
