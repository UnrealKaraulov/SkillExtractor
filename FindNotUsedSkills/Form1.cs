using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FindNotUsedSkills
{
    public partial class Form1 : Form
    {
        public Form1 ( )
        {
            InitializeComponent( );
        }

        private void button1_Click ( object sender , EventArgs e )
        {
            OpenFileDialog files = new OpenFileDialog( );
            files.ShowDialog( );
            string firstfile = files.FileName;
            FolderBrowserDialog fld = new FolderBrowserDialog( );
            fld.ShowDialog( );
            string finddir = fld.SelectedPath;
            List<string> data1 = new List<string>( );
            foreach ( string file in Directory.GetFiles( finddir , "*.slk" , SearchOption.AllDirectories ) )
            {
                data1.AddRange( File.ReadAllLines( file ) );
            }
            foreach ( string file in Directory.GetFiles( finddir , "*.txt" , SearchOption.AllDirectories ) )
            {
                data1.AddRange( File.ReadAllLines( file ) );
            }

            MessageBox.Show( data1.Count.ToString( ) );

            List<string> outdata = new List<string>( );

            string [ ] data2 = File.ReadAllLines( firstfile );
            MessageBox.Show( data2.Length.ToString( ) );


            foreach(string abil in data2)
            {
                bool needadd = true;
                foreach(string str in data1)
                {
                    if ( str.IndexOf( abil , StringComparison.Ordinal ) > -1 )
                    {
                        needadd = false;
                        break;
                    }
                }
                if (needadd)
                {
                    outdata.Add( abil );
                }
            }

            File.WriteAllLines( "outabils.txt" , outdata.ToArray( ) );
        }
    }
}
