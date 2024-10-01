using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SortFAT {
        public partial class Form1 : Form {
        string basedir;
           
        public Form1() {
            InitializeComponent();
            listBox1.KeyDown += new KeyEventHandler(listBox1_KeyDown);
        }

        private void Form1_Load(object sender, EventArgs e) {
            // preencher listbox com drives do sistema
            foreach (var drive in System.IO.DriveInfo.GetDrives()) {
                if (drive.DriveType == System.IO.DriveType.Removable) {
                    listBox2.Items.Add(drive.Name + " - " + drive.VolumeLabel);
                }
            }
            //seleciona o primeiro item da listbox
            listBox2.SelectedIndex = 0;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e) {
            // preencher listbox com todos os arquivos do drive selecionado
            listBox1.Items.Clear();
            basedir = listBox2.SelectedItem.ToString().Substring(0, 3);

            foreach (var file in System.IO.Directory.GetFiles(basedir,"*.*",System.IO.SearchOption.AllDirectories)) {
                if (file.Contains("System Volume Information")) continue;
                listBox1.Items.Add(file.Substring(3));
            }

            button2_Click(null, null);
        }


        // permitir reordenar os itens da listbox com as setas de direção
        private void listBox1_KeyDown(object sender, KeyEventArgs e) {
            // verifica se ctrl está pressionado
            if (!e.Control) return;

            if (listBox1.SelectedItem == null) return;

            int selectedIndex = listBox1.SelectedIndex;
            if (e.KeyCode == Keys.Up && selectedIndex > 0) {
                // Move item up
                var item = listBox1.SelectedItem;
                listBox1.Items.RemoveAt(selectedIndex);
                listBox1.Items.Insert(selectedIndex - 1, item);
                listBox1.SelectedIndex = selectedIndex;
            } else if (e.KeyCode == Keys.Down && selectedIndex < listBox1.Items.Count - 1) {
                // Move item down
                var item = listBox1.SelectedItem;
                listBox1.Items.RemoveAt(selectedIndex);
                listBox1.Items.Insert(selectedIndex + 1, item);
                listBox1.SelectedIndex = selectedIndex;
            }
        }

               

        private void button1_Click(object sender, EventArgs e) {
            // para cada item da listbox, definir o nome curto
            for (int i = 0; i < listBox1.Items.Count; i++) {
                string fileori = basedir + listBox1.Items[i].ToString();

                string extensao = System.IO.Path.GetExtension(fileori);
                string path = System.IO.Path.GetFullPath(fileori);
                string filename = System.IO.Path.GetFileNameWithoutExtension(fileori);
                filename = filename.TrimStart(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_', ' ' });

                string num = (i + 1).ToString("000");

                string novonome = basedir + num + "- " + filename + extensao;

                //renomar arquivo
                System.IO.File.Move(fileori, novonome);
            }

            // mostra aviso de salvo
            MessageBox.Show("Salvo!");

            listBox2_SelectedIndexChanged(null, null);
        }

        private void button2_Click(object sender, EventArgs e) {
            // ordena o listbox pelo nome do item
            List<string> items = new List<string>();
            foreach (var item in listBox1.Items) {
                items.Add(item.ToString());
            }

            items.Sort();

            listBox1.Items.Clear();
            foreach (var item in items) {
                listBox1.Items.Add(item);
            }


        }
    }
}
