using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FakePI.Lib.Util.Contracts;

namespace FakePI.Lib.Util
{
    public class FileReader : IFileReader
    {
        public bool FileExists(string filename) => File.Exists(filename);

        public string ReadFile(string filename) => File.ReadAllText(filename);    
    }
}