using System.Collections.ObjectModel;

namespace OEHP_WPF_Rework
{
    public class VariableHandler
    {
        public static string SSP { get; set; }
        public static string TCC { get; set; }
        public static string AccountToken { get; set; }
        public static string CryptoKey = "6f70656e65646765686f7374706179DH";
        public static string RcmFinished { get; set; }
        public static string RcmStatusURL = "https://ws.test.paygateway.com/HostPayService/v1/hostpay/transactions/status/";
        public static string PaymentFinishedSignal { get; set; }

        public static string PayPageLiveURL = "https://ws.paygateway.com/HostPayService/v1/hostpay/paypage/";
        public static string EdgeLiveURL = "https://ws.paygateway.com/HostPayService/v1/hostpay/transactions";
        public static string DirectPostLiveURL = "https://ws.paygateway.com/api/v1/transactions";

        public static string queryResultJson { get; set; }


    }
}
