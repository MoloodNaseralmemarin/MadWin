using System;
using System.Collections.Generic;
using System.Text;

namespace Shop2City.Core.DTOs.Payments
{
    public class MetadataViewModel
    {
        public string mobile { get; set; }
        public string email { get; set; }
    }

    public class RequestViewModel
    {
        public string merchant_id { get; set; }
        public int amount { get; set; }
        public string callback_url { get; set; }
        public string description { get; set; }
        public MetadataViewModel metadata { get; set; }
    }

    public class Data
    {
        public int code { get; set; }
        public string message { get; set; }
        public string authority { get; set; }
        public string fee_type { get; set; }
        public int fee { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public List<object> errors { get; set; }
    }


    public class VerifyViewModel
    {
        public string merchant_id { get; set; }
        public int amount { get; set; }
        public string authority { get; set; }
    }


    public class DataVerifyViewModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public string card_hash { get; set; }
        public string card_pan { get; set; }
        public long ref_id { get; set; }
        public string fee_type { get; set; }
        public int fee { get; set; }
    }

    public class RootVerifyViewModel
    {
        public DataVerifyViewModel data { get; set; }
        public List<object> errors { get; set; }
    }




}
