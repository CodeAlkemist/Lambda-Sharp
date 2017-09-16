// In short apache 2 license, check LICENSE file for the legalese

namespace System.IO
{
    public static class FileStreamExtensions
    {
        /// <summary>
        /// Get chunks of your Stream this function does not change the Stream.Position
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="offset">The number of chunks of the given size to offset the read</param>
        /// <param name="bufferSize">The size of the buffer in bytes</param>
        /// <param name="padding">If a chunk ends up smaller than it should pad it with 0's</param>
        /// <returns>The buffer containing the file chunk</returns>
        public static byte[] GetChunk<T>(this T stream, int offset, int bufferSize, bool padding = true) where T:Stream
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            stream = stream.CanRead && stream.CanSeek ? stream : throw new ArgumentException($"The stream {nameof(stream)} cannot be used because its not readable and or not seekable");
            var pos = stream.Position;
            var buffer = new byte[bufferSize];
            var bytesRead = stream.Read(buffer, offset * bufferSize, bufferSize);
            stream.Position = pos;
            return buffer;
        }
    }
}
