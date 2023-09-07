using MetroSuite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;

public partial class MainForm : MetroForm
{
    public MainForm()
    {
        InitializeComponent();
        dateTimePicker1.Value = DateTime.Now;
    }

    public void HandleDragEnter(DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            e.Effect = DragDropEffects.Copy;
        }
    }

    public void HandleDragDrop(DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] filePaths = ((string[])e.Data.GetData(DataFormats.FileDrop));

            foreach (string path in filePaths)
            {
                if (!listBox1.Items.Contains(path.ToLower()))
                {
                    listBox1.Items.Add(path.ToLower());
                }
            }
        }
    }

    private void listBox1_DragEnter(object sender, DragEventArgs e)
    {
        HandleDragEnter(e);
    }

    private void listBox1_DragDrop(object sender, DragEventArgs e)
    {
        HandleDragDrop(e);
    }

    private void MainForm_DragEnter(object sender, DragEventArgs e)
    {
        HandleDragEnter(e);
    }

    private void MainForm_DragDrop(object sender, DragEventArgs e)
    {
        HandleDragDrop(e);
    }

    private void guna2Button3_Click(object sender, System.EventArgs e)
    {
        listBox1.Items.Clear();
    }

    private void guna2Button2_Click(object sender, System.EventArgs e)
    {
        List<string> toRemove = new List<string>();

        foreach (string str in listBox1.SelectedItems)
        {
            toRemove.Add(str);
        }

        foreach (string str in toRemove)
        {
            listBox1.Items.Remove(str);
        }
    }

    private void guna2Button1_Click(object sender, System.EventArgs e)
    {
        if (openFileDialog1.ShowDialog().Equals(DialogResult.OK))
        {
            foreach (string file in openFileDialog1.FileNames)
            {
                if (!listBox1.Items.Contains(file.ToLower()))
                {
                    listBox1.Items.Add(file.ToLower());
                }
            }
        }
    }

    private bool IsTimeValid(string str)
    {
        str = str.Trim().Replace('\t'.ToString(), "").Replace(" ", "");

        if (str == "")
        {
            return false;
        }

        int colons = 0;

        foreach (char c in str)
        {
            if (c.Equals(':'))
            {
                colons++;
            }
        }

        if (colons != 2)
        {
            return false;
        }

        string[] splitted = str.Split(':');
        string strHours = splitted[0], strMinutes = splitted[1], strSeconds = splitted[2];

        if (strHours.Length != 2 || strMinutes.Length != 2 || strSeconds.Length != 2)
        {
            return false;
        }

        if (strHours.StartsWith("0"))
        {
            strHours = strHours.Substring(1);
        }

        if (strMinutes.StartsWith("0"))
        {
            strMinutes = strMinutes.Substring(1);
        }

        if (strSeconds.StartsWith("0"))
        {
            strSeconds = strSeconds.Substring(1);
        }

        if (strHours.StartsWith("-") || strHours.StartsWith("+") || strMinutes.StartsWith("-") || strMinutes.StartsWith("+") || strSeconds.StartsWith("-") || strSeconds.StartsWith("+"))
        {
            return false;
        }

        if (!Information.IsNumeric(strHours) || !Information.IsNumeric(strMinutes) || !Information.IsNumeric(strSeconds))
        {
            return false;
        }

        int hours = int.Parse(strHours), minutes = int.Parse(strMinutes), seconds = int.Parse(strSeconds);

        if (hours > 23 || minutes > 60 || seconds > 60)
        {
            return false;
        }

        return true;
    }

    private void guna2Button4_Click(object sender, System.EventArgs e)
    {
        if (listBox1.Items.Count == 0)
        {
            MessageBox.Show("Please, add some files and/or directories to the list before proceeding.", "FileDatesSpoofer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!guna2CheckBox1.Checked && !guna2CheckBox2.Checked && !guna2CheckBox3.Checked)
        {
            MessageBox.Show("Please, choose at least one option before proceeding.", "FileDatesSpoofer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!IsTimeValid(guna2TextBox1.Text))
        {
            MessageBox.Show("Please, insert the time in the correct format (00:00:00, hh:mm:ss, hours:minutes:seconds) before proceeding.", "FileDatesSpoofer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        string value = guna2TextBox1.Text;
        string[] splitted = value.Split(':');
        string strHours = splitted[0], strMinutes = splitted[1], strSeconds = splitted[2];

        if (strHours.StartsWith("0"))
        {
            strHours = strHours.Substring(1);
        }

        if (strMinutes.StartsWith("0"))
        {
            strMinutes = strMinutes.Substring(1);
        }

        if (strSeconds.StartsWith("0"))
        {
            strSeconds = strSeconds.Substring(1);
        }

        int hours = int.Parse(strHours), minutes = int.Parse(strMinutes), seconds = int.Parse(strSeconds);
        DateTime dateTime = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day, hours, minutes, seconds);

        foreach (string str in listBox1.Items)
        {
            if (Directory.Exists(str))
            {
                FileSystemInfo info = new DirectoryInfo(str);

                if (guna2CheckBox1.Checked)
                {
                    info.CreationTime = dateTime;
                    Directory.SetCreationTime(str, dateTime);
                }

                if (guna2CheckBox2.Checked)
                {
                    info.LastAccessTime = dateTime;
                    Directory.SetLastAccessTime(str, dateTime);
                }

                if (guna2CheckBox3.Checked)
                {
                    info.LastWriteTime = dateTime;
                    Directory.SetLastWriteTime(str, dateTime);
                }
            }
            else if (File.Exists(str))
            {
                FileSystemInfo info = new FileInfo(str);

                if (guna2CheckBox1.Checked)
                {
                    info.CreationTime = dateTime;
                    File.SetCreationTime(str, dateTime);
                }

                if (guna2CheckBox2.Checked)
                {
                    info.LastAccessTime = dateTime;
                    File.SetLastAccessTime(str, dateTime);
                }

                if (guna2CheckBox3.Checked)
                {
                    info.LastWriteTime = dateTime;
                    File.SetLastWriteTime(str, dateTime);
                }
            }
        }

        MessageBox.Show("Succesfully changed all the dates of the files & directories specified!", "FileDatesSpoofer", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}