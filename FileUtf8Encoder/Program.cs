using System.Text;

namespace FileUtf8Encoder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: ConvertToUTF8 <file>");
                return;
            }

            string filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);

            Encoding detectEncoding = DetectEncoding(fileBytes);

            if (detectEncoding != null)
            {
                Console.WriteLine($"Detected encoding: {detectEncoding.WebName}");

                string content = detectEncoding.GetString(fileBytes);
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(content);

                File.WriteAllBytes(filePath, utf8Bytes);

                Console.WriteLine($"File converted to UTF-8 (without BOM): {filePath}");
            }
            else
            {
                Console.WriteLine("Unable to detect encoding.");
            }
        }

        static Encoding DetectEncoding(byte[] bytes)
        {
            // Detect BOM
            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                return Encoding.UTF8;
            if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
                return Encoding.BigEndianUnicode;
            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
                return Encoding.Unicode;

            // Detect encoding by checking for valid UTF-8 byte patterns
            if (IsUTF8(bytes))
                return Encoding.UTF8;

            // Try detecting encoding using default encodings
            foreach (var encoding in Encoding.GetEncodings().Select(e => e.GetEncoding()))
            {
                if (IsValidEncoding(bytes, encoding))
                    return encoding;
            }

            return null;
        }

        static bool IsUTF8(byte[] bytes)
        {
            int i = 0;
            while (i < bytes.Length)
            {
                if (bytes[i] <= 0x7F)
                    i += 1;
                else if (bytes[i] >= 0xC2 && bytes[i] <= 0xDF && i + 1 < bytes.Length && bytes[i + 1] >= 0x80 && bytes[i + 1] <= 0xBF)
                    i += 2;
                else if (bytes[i] >= 0xE0 && bytes[i] <= 0xEF && i + 2 < bytes.Length && bytes[i + 1] >= 0x80 && bytes[i + 1] <= 0xBF && bytes[i + 2] >= 0x80 && bytes[i + 2] <= 0xBF)
                    i += 3;
                else if (bytes[i] >= 0xF0 && bytes[i] <= 0xF4 && i + 3 < bytes.Length && bytes[i + 1] >= 0x80 && bytes[i + 1] <= 0xBF && bytes[i + 2] >= 0x80 && bytes[i + 2] <= 0xBF && bytes[i + 3] >= 0x80 && bytes[i + 3] <= 0xBF)
                    i += 4;
                else
                    return false;
            }
            return true;
        }

        static bool IsValidEncoding(byte[] bytes, Encoding encoding)
        {
            try
            {
                encoding.GetString(bytes);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
