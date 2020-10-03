﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace H3Standard
{
    public class H3Lib
    {
        private H3Lib()
        {
            var libraryName = "h3lib.dll";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                libraryName = "h3lib.dll.so";
            }
            UnpackNativeLibrary(libraryName);
        }

        public void Check()
        {
            Console.WriteLine("Just checking");
        }

        public static H3Lib Instance
        {
            get { return _instance;  }
        }

        private static H3Lib _instance = new H3Lib();

        private static void UnpackNativeLibrary(string libraryName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"{assembly.GetName().Name}.{libraryName}";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var memoryStream = new MemoryStream(stream.CanSeek ? (int)stream.Length : 0))
            {
                stream.CopyTo(memoryStream);
                File.WriteAllBytes(libraryName, memoryStream.ToArray());
            }
        }
    }

}