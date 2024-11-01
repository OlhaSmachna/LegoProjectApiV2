using System.Security.Cryptography;
using System.Text;

namespace LegoProjectApiV2.Tools
{
    public class Hasher
    {
        public static byte[] CreateHash_SHA512(string stringToHash)
        {
            SHA512 hasher = SHA512.Create();

            return hasher.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
        }

        public static bool CheckPassword(byte[] sourcePasswordHash, string testPasswordString)
        {
            if (sourcePasswordHash == null
                 || sourcePasswordHash.Length == 0
                 || testPasswordString == null)
                return false;

            byte[] testPasswordHash = CreateHash_SHA512(testPasswordString);

            return CheckHashes(sourcePasswordHash, testPasswordHash);
        }

        public static bool CheckHashes(byte[] sourceHash, byte[] targetHash)
        {
            bool areHashesEqual = false;

            if (sourceHash.Length == targetHash.Length)
            {
                areHashesEqual = true;

                for (int i = 0; i < sourceHash.Length; i++)
                {
                    if (sourceHash[i] != targetHash[i])
                    {
                        areHashesEqual = false;
                        break;
                    }
                }
            }

            return areHashesEqual;
        }
    }
}
