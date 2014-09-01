using System.Net.Http;

namespace Lacjam.WebApi.Infrastructure
{
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private string fileName { get; set; }


        public CustomMultipartFormDataStreamProvider(string path, string fileName = "")
            : base(path)
        {
            this.fileName = fileName;
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";

            if (!string.IsNullOrEmpty(fileName))
            {
                string fileExtension = "";
                if (headers.ContentDisposition.FileName.Contains("."))
                {
                    fileExtension = headers.ContentDisposition.FileName.Substring(headers.ContentDisposition.FileName.LastIndexOf("."));
                }

                name = this.fileName + fileExtension;

            }

            return name.Replace("\"", string.Empty);
        }
    }
}