using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace XmlParser
{
    public partial class XmlToDatabase : Form
    {
        string filePath = String.Empty;
        string fileName = String.Empty;
        BankaSubeTumListe list = null;

        public XmlToDatabase()
        {
            InitializeComponent();
            Load += XmlToDatabase_Load;          
        }

        void XmlToDatabase_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            DbOperations.CreateTable();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (filePath != String.Empty)
            {
                try
                {
                    list = SerializeConfig<BankaSubeTumListe>.DeSerialize(filePath);
                    DbOperations.AddDatabase(list);
                    if (DbOperations.result)
                        MessageBox.Show("Veritabanına başarıyla kaydedildi.");
                }
                catch (Exception)
                {
                    MessageBox.Show("Dosyadan veriler alınamadı.");
                }               
            }
            else
            {
                MessageBox.Show("Lütfen bir dosya seçiniz...");
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "XML Files (*.xml)|*.xml";
            file.FilterIndex = 0;
            file.DefaultExt = "xml";

            if (file.ShowDialog() == DialogResult.OK)
            {
                filePath = file.FileName;
                fileName = file.SafeFileName;
                btnSave.Enabled = true;
            }            
        }
    }
}
