using System;
using H3Standard;

namespace testH3
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine( H3.GeoToH3( 47.7, -3, 8 ) );
        }    
    }
}
