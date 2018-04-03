using System.Threading.Tasks;

namespace FakePI.Lib.Util.Contracts
{
    public interface IFileReader
    {
        bool FileExists(string filename);
        string ReadFile(string filename);
    }
}