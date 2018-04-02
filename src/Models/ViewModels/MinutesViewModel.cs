using System;

namespace Minutz.Models.ViewModels
{
    public class MinutesViewModel
    {
        public string FileName { get; set; }
        public DateTime IssuedDate { get; set; }
        public byte[] IssuedMinutes { get; set; }
        public string FileUrl { get; set; }
    }
}