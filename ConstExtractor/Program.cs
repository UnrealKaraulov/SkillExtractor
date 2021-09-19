using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;

namespace ConstExtractor
{
    class Program
    {


        static void Main ( string [ ] args )
        {
            IniFile ini = new IniFile( ".\\outconfig.txt" );
            string filepath = Console.ReadLine( ).Replace( "\"" , "" );
            Console.WriteLine( "Start reading game.c file..." );
            string [ ] filedata = File.ReadAllLines( filepath );
            Console.WriteLine( "End reading game.c file. Len:" + filedata.Length );
            Console.WriteLine( "Start search skills" );

            string RegexGetName = @"sub_.*?\(int\)\""(.*?)\"",\s\(int\)\""(.*?)\""";

            for ( int i = 0 ; i < filedata.Length ; i++ )
            {
                Match ConstNameReader = Regex.Match( filedata [ i ] , RegexGetName );
                if (ConstNameReader.Success && filedata[i].IndexOf("6F455110") == -1 && filedata[i].IndexOf("6F71D9D0") == -1)
                {
                    ini.IniWriteValue( ConstNameReader.Groups [ 1 ].Value , ConstNameReader.Groups [ 2 ].Value , "" );
                }
            }

        }
    }

    class IniFile
    {
        public string path;

        [DllImport( "kernel32" )]
        private static extern long WritePrivateProfileString ( string section ,
            string key , string val , string filePath );
        [DllImport( "kernel32" )]
        private static extern int GetPrivateProfileString ( string section ,
                 string key , string def , StringBuilder retVal ,
            int size , string filePath );

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile ( string INIPath )
        {
            path = INIPath;
        }
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// Section name
        /// <PARAM name="Key"></PARAM>
        /// Key Name
        /// <PARAM name="Value"></PARAM>
        /// Value Name
        public void IniWriteValue ( string Section , string Key , string Value )
        {
            WritePrivateProfileString( Section , Key , Value , this.path );
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string IniReadValue ( string Section , string Key )
        {
            StringBuilder temp = new StringBuilder( 255 );
            int i = GetPrivateProfileString( Section , Key , "" , temp ,
                                            255 , this.path );
            return temp.ToString( );

        }
    }
}
