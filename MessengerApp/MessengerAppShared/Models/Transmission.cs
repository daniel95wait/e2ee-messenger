using System;
using System.Text;

namespace MessengerAppShared.Models
{
    public class Transmission
    {
        public string Content { get; set; }
        public byte[] Data { get; set; }

        // Contruct from string
        public Transmission(string text)
        {
            // When an empty argmunt was supplied
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException();
            }

            Content = text;
            Data = Encoding.Unicode.GetBytes(text);
        }

        // Construct from byte array
        public Transmission(Byte[] data)
        {
            // When an empty argument was supplied
            if (data.Length == 0)
            {
                throw new ArgumentNullException();
            }

            Data = data;
            Content = Encoding.Unicode.GetString(data);
        }
    }
}
