using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NASP
{
     
    public partial class Form1 : Form
    {
        private String _lokacija;
        private AvlStablo _stablo;
        public Form1()
        {
            InitializeComponent();
            numericUpDown1.Maximum = Int32.MaxValue;
            numericUpDown1.Minimum = Int32.MinValue;
            _stablo = new AvlStablo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // izbornik datoteke
            var izbornik = new OpenFileDialog();
            izbornik.Filter = "txt datoteka (*.txt)|*.txt|Sve datoteke (*.*)|*.*";
            izbornik.Title = "Select file";
            if (izbornik.ShowDialog() == DialogResult.OK)
            {
                _lokacija = izbornik.FileName;
            }

            // otvaranje datoteke
            var dat = new Datoteka(_lokacija);
            // čitanje datoteke
            var brojevi = dat.Citaj();
            
            
            // obriši prijašnje stablo
            ObrisiSve();

            //dodaj elemente iz datoteke u stablo
            foreach (var broj in brojevi)
            {
                _stablo.Dodaj(broj);
            }

            // prikaži stablo u sučelju
            PrikazStabla();
        }

        private void PrikazStabla()
        {
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(_stablo.DohvatiCvorove());
            treeView1.ExpandAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _stablo.Dodaj((int)numericUpDown1.Value);
            PrikazStabla();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _stablo.BrisiCvor((int)numericUpDown1.Value);
            PrikazStabla();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ObrisiSve();   
        }

        private void ObrisiSve()
        {
            _stablo = new AvlStablo();
            PrikazStabla();
        }

    }
}
