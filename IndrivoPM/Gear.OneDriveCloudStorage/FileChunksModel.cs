using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Gear.OneDriveCloudStorage
{
    /// <summary>
    /// Offset and Count usage described in Microsoft's documentation
    /// for ByteArrayContent(Byte[], Int32, Int32) method.
    /// </summary>
    public class FileChunksModel
    {
        public string AccessToken { get; set; }

        public string UploadUrl { get; }

        public IFormFile File { get; }

        public Queue<RangeOfBytes> Ranges { get; set; }
            = new Queue<RangeOfBytes>();

        public FileChunksModel([Required] string uploadUrl, [Required] IFormFile formFile)
        {
            UploadUrl = uploadUrl;
            File = formFile;
        }

        public int RoundUpQuotient(int number, int divisor)
        {
            return number / divisor + Math.Min(1, number % divisor);
        }

        public void InisializeChunks(int rangeLimit)
        {
            var numberOfRanges = RoundUpQuotient((int)File.Length, rangeLimit);
            for(var offset = 0; offset < numberOfRanges; offset++)
                AddChunk(new RangeOfBytes { Offset = offset * rangeLimit, Count = (int)Math.Min(rangeLimit, File.Length - offset * rangeLimit) });
        }

        /// <summary>
        /// Adds a chunk into the queue.
        /// </summary>
        /// <param name="chunk"></param>
        public void AddChunk(RangeOfBytes chunk)
        {
            Ranges.Enqueue(chunk);
        }

        /// <summary>
        /// Takes content of the response and
        /// adds chunks for new requests.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="numberOfBytes"></param>
        public void AddRangeOfChunksFromContent(ResponseContentModel content, int numberOfBytes)
        {
            if (content.NextExpectedRanges.Count == 0)
                throw new InvalidOperationException("The content doesn't contain any range.");

            foreach (var range in content.NextExpectedRanges)
            {
                var bytes = range.Split('-');
                var leftByte = int.Parse(bytes[0]);
                var rightByte = bytes.Length > 1 ? int.Parse(bytes[1]) : -1;
                var rightOffset = RoundUpQuotient(rightByte, numberOfBytes);
                var leftOffset = RoundUpQuotient(leftByte, numberOfBytes);
                for (var offset = leftOffset; offset <= rightOffset; offset += 2)
                {
                    AddChunk(new RangeOfBytes { Offset = offset * numberOfBytes, Count = (int)Math.Min(numberOfBytes, File.Length - offset * numberOfBytes) });
                }
            }
        }

        /// <summary>
        /// Gets the first chunk from the queue.
        /// The chunk isn't erased.
        /// </summary>
        /// <returns></returns>
        public RangeOfBytes GetChunk()
        {
            if (Ranges.Count == 0)
                throw new InvalidOperationException("There are no more chunks in the queue.");
            return Ranges.Peek();
        }

        /// <summary>
        /// The response of this method should be setted as a parameter
        /// to Content-Range header. The used chunk is erased.
        /// </summary>
        /// <returns></returns>
        public string GetContentRange()
        {
            if (Ranges.Count == 0)
                throw new InvalidOperationException("There are no more chunks in the queue.");

            var range = Ranges.Peek();
            Ranges.Dequeue();
            return $"bytes {range.Offset}-{range.Offset + range.Count - 1}/{File.Length}";
        }
    }

    public class RangeOfBytes
    {
        public int Offset { get; set; }

        public int Count { get; set; }
    }
}
