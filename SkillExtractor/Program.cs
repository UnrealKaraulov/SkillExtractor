using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SkillExtractor
{
    class Program
    {
        static List<string> abilcodes;

        static void AddNewAbilCode ( int abilcode )
        {
            byte [ ] data = BitConverter.GetBytes( abilcode );
            byte [ ] newdata = new byte [ 5 ] { data [ 3 ] , data [ 2 ] , data [ 1 ] , data [ 0 ] , 0x00 };
            string outstring = Encoding.UTF8.GetString( newdata );
            if ( Regex.Match( outstring , @"[a-zA-Z0-9_-][a-zA-Z0-9_-][a-zA-Z0-9_-][a-zA-Z0-9_-]" ).Success )
            {
                bool needadd = true;
                foreach ( string str in abilcodes )
                {
                    if ( str == outstring )
                    {
                        needadd = false;
                        break;
                    }
                }
                if ( needadd )
                    abilcodes.Add( outstring );
            }
        }


        static void Main ( string [ ] args )
        {
            abilcodes = new List<string>( );
            string filepath = Console.ReadLine( ).Replace( "\"" , "" );
            Console.WriteLine( "Start reading game.c file..." );
            string [ ] filedata = File.ReadAllLines( filepath );
            Console.WriteLine( "End reading game.c file. Len:" + filedata.Length );
            Console.WriteLine( "Start search skills" );

            for ( int i = 0 ; i < filedata.Length ; i++ )
            {
                Console.Write( "\r Process:" + ( ( int ) ( ( float ) i / ( float ) filedata.Length * 100.0f ) ).ToString( ) + "  " );
                try
                {
                    /*  if ( filedata [ i ].IndexOf( "sub_" , StringComparison.Ordinal ) > -1 )
                      {
                    
                          if ( filedata [ i + 1 ].IndexOf( "return" , StringComparison.Ordinal ) > -1 )
                          {*/
                    string data = filedata [ i ];
                    while ( true )
                    {


                        string regex = @"\W(\d+)\W";
                        Match mymath = Regex.Match( data , regex );
                        if ( mymath.Success )
                        {
                            data = data.Replace( mymath.Groups [ 1 ].Value , "" );
                            //  Console.WriteLine( "\rFound    " );
                            int tmpint = int.Parse( mymath.Groups [ 1 ].Value );

                            AddNewAbilCode( tmpint );


                        }
                        else
                            break;
                    }

                    /* i++;
                     i++;
                     i++;

                    
                 }
             }*/
                }
                catch
                {

                }
            }


            File.WriteAllLines( "OutSkills.txt" , abilcodes.ToArray( ) );
            Console.ReadLine( );
        }
    }
}
