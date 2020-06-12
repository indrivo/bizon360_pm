using System;
using System.Collections.Generic;

namespace Gear.OneDriveCloudStorage
{
    public class ResponseContentModel
    {
        public DateTime ExpirationDateTime { get; set; }

        public List<string> NextExpectedRanges { get; set; }
            = new List<string>();

        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public int Size { get; set; } = 0;

        public object File { get; set; }

        public string UploadUrl { get; set; }

        public Error Error { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
}
