using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace iel.line.Models
{
    public class RequestLineToken
    {
        [Required]
        public string GrantType { get; set; }
        [Required]
        public string Code { get; set; }
        public object State { get; set; }
        //public string redirect_uri { get; }
        //public string client_id { get; }
        //public string client_secret { get; }

    }
    public class RequestLineMessage
    {
        [Required, MaxLength(1000)]
        public string Message { get; set; }
        // 	HTTP/HTTPS URL	Maximum size of 240×240px JPEG
        public string ImageThumbnail { get; set; }
        // HTTP/HTTPS URL	Maximum size of 2048×2048px JPEG
        public string ImageFullsize { get; set; }
        public string ImageFile { get; set; }
        public string StickerPackageId { get; set; }
        public string StickerId { get; set; }
        public bool NotificationDisabled { get; set; }
        public string Token { get; set; }
        //public string redirect_uri { get; }
        //public string client_id { get; }
        //public string client_secret { get; }

    }
}
