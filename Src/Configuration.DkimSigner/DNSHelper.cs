using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Configuration.DkimSigner
{
    class DNSHelper
    {
        private const Int16 DNS_TYPE_TEXT = 0x0010;
        private const Int32 DNS_QUERY_BYPASS_CACHE = 0x00000008;
        private const Int32 DNS_ERROR_RCODE_NAME_ERROR = 9003;
        private const Int32 DNS_INFO_NO_RECORDS = 9501;

        public static String GetTxtRecord(String name) {
            var queryResultsSet = IntPtr.Zero;

            try {
                var dnsStatus = DnsQuery(name, DNS_TYPE_TEXT, DNS_QUERY_BYPASS_CACHE, IntPtr.Zero, ref queryResultsSet, IntPtr.Zero);
        
                if (dnsStatus == DNS_ERROR_RCODE_NAME_ERROR || dnsStatus == DNS_INFO_NO_RECORDS)
                    return null;
        
                if (dnsStatus != 0)
                    throw new Win32Exception(dnsStatus);
        
                DnsRecordTxt dnsRecord;
                for (var pointer = queryResultsSet; pointer != IntPtr.Zero; pointer = dnsRecord.pNext) {
                    dnsRecord = (DnsRecordTxt) Marshal.PtrToStructure(pointer, typeof(DnsRecordTxt));

                    if (dnsRecord.wType == DNS_TYPE_TEXT) {
                        List<String> lines = new List<String>();
                        var stringArrayPointer = pointer + Marshal.OffsetOf(typeof(DnsRecordTxt), "pStringArray").ToInt32();
                    
                        for (var i = 0; i < dnsRecord.dwStringCount; ++i) {
                            var stringPointer = (IntPtr) Marshal.PtrToStructure(stringArrayPointer, typeof(IntPtr));
                            lines.Add(Marshal.PtrToStringUni(stringPointer));
                            stringArrayPointer += IntPtr.Size;
                        }

                        return String.Join(Environment.NewLine, lines);
                    }
                }
                return null;
            }
            finally {
                const Int32 DnsFreeRecordList = 1;

                if (queryResultsSet != IntPtr.Zero)
                    DnsRecordListFree(queryResultsSet, DnsFreeRecordList);
            }
        }

        [DllImport("Dnsapi.dll", EntryPoint = "DnsQuery_W", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        static extern Int32 DnsQuery(String lpstrName, Int16 wType, Int32 options, IntPtr pExtra, ref IntPtr ppQueryResultsSet, IntPtr pReserved);

        [DllImport("Dnsapi.dll")]
        static extern void DnsRecordListFree(IntPtr pRecordList, Int32 freeType);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct DnsRecordTxt {
            public IntPtr pNext;
            public String pName;
            public Int16 wType;
            public Int16 wDataLength;
            public Int32 flags;
            public Int32 dwTtl;
            public Int32 dwReserved;
            public Int32 dwStringCount;
            public String pStringArray;
        }
    }
}
