using System.Collections.Generic;
using System.Web;

namespace FBru.WebAdmin.Helpers
{
    public class Utilities
    {
        private static readonly string[] HttpPostedFileBaseErrorMessages =
        {
            "Size of file must be greater than 0",
            "Invalid image extension. Extensions were accepted: jpg, png, gif"
        };

        public static string HttpPostedFileBaseErrorMessage(int errorCode)
        {
            return HttpPostedFileBaseErrorMessages[errorCode];
        }

        public static int HttpPostedFileBaseFilter(HttpPostedFileBase httpPostedFileBase)
        {
            var contentTypesAccepted = new List<string> {"image/jpeg", "image/png", "image/gif"};
            var contentLength = httpPostedFileBase.ContentLength;
            var contentType = httpPostedFileBase.ContentType;
            if (contentLength <= 0) return 0;
            if (!contentTypesAccepted.Contains(contentType)) return 1;
            return -1;
        }
    }
}