using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace lab_1
{
    public partial class lab : Form
    {
        string file;
        private enum LastActionType { None, Insert, Delete, Paste }
        private LastActionType lastAction = LastActionType.None;
        private string lastSymbol = "";
        private int lastPosition = 0;
        public lab()
        {
            InitializeComponent();
            CreateFileButton.Click += CreateToolStripMenuItem_Click;
            OpenFileButton.Click += OpenToolStripMenuItem_Click;
            SaveFileButton.Click += SaveToolStripMenuItem_Click;
            CancelButton.Click += CancelToolStripMenuItem_Click;
            RepeatButton.Click += DublicationToolStripMenuItem_Click;
            CopyButton.Click += CopyToolStripMenuItem_Click;
            CutButton.Click += CutToolStripMenuItem_Click;
            PasteButton.Click += PasteToolStripMenuItem_Click;
            HelpButton.Click += вызовСправкиToolStripMenuItem_Click;
            AboutButton.Click += оПрограммеToolStripMenuItem_Click;
        }

        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            file = saveFileDialog1.FileName;
            System.IO.File.WriteAllText(file, "");
            MessageBox.Show("Файл создан");
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            file = openFileDialog1.FileName;
            string fileText = System.IO.File.ReadAllText(file);
            richTextBox1.Text = fileText;
            MessageBox.Show("Файл открыт");
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(file))
            {
                System.IO.File.WriteAllText(file, richTextBox1.Text);
                MessageBox.Show("Файл сохранен");
            }
            else
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                file = saveFileDialog1.FileName;
                System.IO.File.WriteAllText(file, "");
                MessageBox.Show("Файл успешно сохранён!");
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            file = saveFileDialog1.FileName;
            System.IO.File.WriteAllText(file, "");
            MessageBox.Show("Файл успешно сохранён!");
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                        $"Сохранить файл \"{file}\"?",
                        "Сохранение файла",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question
                    );
            if (result == DialogResult.Yes)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                file = saveFileDialog1.FileName;
                System.IO.File.WriteAllText(file, "");
                MessageBox.Show("Файл успешно сохранён!");
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            this.Close();
        }

        private void CancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanUndo)
            {
                richTextBox1.Undo();
            }
        }

        private void DublicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (lastAction)
            {
                case LastActionType.Insert:
                    int pos = richTextBox1.SelectionStart;
                    richTextBox1.SelectedText = lastSymbol;
                    richTextBox1.SelectionStart = pos + lastSymbol.Length;
                    break;

                case LastActionType.Delete:
                    if (richTextBox1.Text.Length > 0)
                    {
                        richTextBox1.SelectionStart--;
                        richTextBox1.SelectionLength = 1;
                        richTextBox1.SelectedText = "";
                    }
                    break;

                case LastActionType.Paste:
                    richTextBox1.Paste();
                    break;
            }
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                lastAction = LastActionType.Paste;
                lastSymbol = Clipboard.GetText();
                richTextBox1.Paste();
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                lastAction = LastActionType.Delete;
                lastSymbol = richTextBox1.SelectedText;
                richTextBox1.SelectedText = "";
            }
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            lastPosition = richTextBox1.SelectionStart;
            if (richTextBox1.SelectionLength == 0 && richTextBox1.Text.Length > lastSymbol.Length && lastPosition > 0 && lastAction != LastActionType.Delete && lastAction != LastActionType.Paste
                )
            {
                lastAction = LastActionType.Insert;
                lastSymbol = richTextBox1.Text.Substring(lastPosition - 1, 1);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Back))
            {
                lastAction = LastActionType.Delete;
                if (richTextBox1.Text.Length > 0)
                {
                    if (richTextBox1.SelectionLength > 0)
                    {
                        richTextBox1.SelectedText = "";
                    }
                    else if (richTextBox1.SelectionLength == 0)
                    {
                        richTextBox1.SelectionStart--;
                        richTextBox1.SelectionLength = 1;
                        richTextBox1.SelectedText = "";
                    }
                }
                return true;
            }
            else
            {
                lastAction = LastActionType.Insert;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void вызовСправкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string helpFilePath = "Spravka.chm";
            if (System.IO.File.Exists(helpFilePath))
            {
                Help.ShowHelp(this, helpFilePath);
            };
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Программа: «Анализатор исходного кода».\r\n" +
                "Данная работа была выполнена Капитоновой Анастасией, студенткой 3 курса, группы АВТ-214."
                );
        }
    }
}
