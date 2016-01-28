using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Reflection.Emit;
using System.Web;
using Newtonsoft.Json; // Utilizes JSON.NET for parsing response from the gateway.
/// <summary>
/// General Methods for generating Parameters and sending POSTs to the gateway.
/// Old code Commented and left for reference.
/// </summary>
namespace OEHP_WPF_Rework
{
    public class PaymentEngine  //General Class that Handles the Payment Functions, as well as building the strings used for various transaction Posts
    {
        public struct JsonResponse //Struct for Parsing JSON for use in displaying Payment Page
        {
            public string sealedSetupParameters { get; set; }
            public string actionURL { get; set; }
            public string errorMessage { get; set; }
        }

        //Variables for various URLs used by  Edge, EdgeURL getting the otk, OtK url appended with SSP for pulling up payment page, query URL for Query and Dependent return transactions
        public static string EdgeURL = "https://ws.test.paygateway.com/HostPayService/v1/hostpay/transactions";
        public static string otkURL = "https://ws.test.paygateway.com/HostPayService/v1/hostpay/paypage/";
        public static string queryURL = "https://ws.test.paygateway.com/api/v1/transactions";

        public static string webRequest_Post(string parameters) // Web Request POST to OpenEdge HostPay for Payment Page, Used for all Transactiosn that utilize a payment page.
        {

            try
            {
                WebRequest request = WebRequest.Create(EdgeURL);

                //Set reuqest to POST
                request.Method = "POST";
                //Create Post data and Convert it to a byte array.

                string postData = parameters;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                //set ContentType Property to proper setting for WebRequest
                request.ContentType = "application/x-www-form-urlencoded";

                //set the ContentLength property
                request.ContentLength = byteArray.Length;

                //Get the request stream.
                Stream dataStream = request.GetRequestStream();

                //Write the datat to teh request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);

                //close the stream
                dataStream.Close();

                //Get the response
                WebResponse response = request.GetResponse();

                //Get the stream contaiing content returned byt ghe server.
                dataStream = response.GetResponseStream();

                // Open the Stream using a streamreader for easy access.
                StreamReader reader = new StreamReader(dataStream);

                //Read the Content
                string responseFromServer = reader.ReadToEnd();
                //Cleanign time
                reader.Close();
                response.Close();

                JsonResponse json = new JsonResponse();
                json = JsonConvert.DeserializeObject<JsonResponse>(responseFromServer); //Deserialize JSON response from Server. Utilizing JSON.NET library.

                string ssp = json.sealedSetupParameters; //Pull sealedSetupParameters from JSON response

                if (json.errorMessage == "Account Status is not set")
                {
                    MessageBox.Show("Account Status not Set, please verify OEHP account is set with proper charge types.");
                }
                VariableHandler.SSP = ssp;
                return ssp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error has occured:" + Environment.NewLine + ex);

                return null;
            }

        }

        public static string webRequest_Query(string parameters) // Web Request POST to OpenEdge HostPay for QUERY or DEPENDENT CREDIT
        {
            try
            {
                WebRequest request = WebRequest.Create(queryURL);

                //Set reuqest to POST
                request.Method = "POST";
                //Create Post data nd Convert it to a byte array.

                string postData = parameters;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                //set ContentType Property to proper setting for WebRequest
                request.ContentType = "application/x-www-form-urlencoded";

                //set the ContentLength property
                request.ContentLength = byteArray.Length;

                //Get the request stream.
                Stream dataStream = request.GetRequestStream();

                //Write the datat to teh request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);

                //close the stream
                dataStream.Close();

                //Get the response
                WebResponse response = request.GetResponse();

                //Get the stream contaiing content returned byt ghe server.
                dataStream = response.GetResponseStream();

                // Open the Stream using a streamreader for easy access.
                StreamReader reader = new StreamReader(dataStream);

                //Read the Content
                string responseFromServer = reader.ReadToEnd();
                //Cleanign time
                reader.Close();
                response.Close();

                return responseFromServer;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error has occured:" + Environment.NewLine + ex);
                return null;
            }

        }

        //ACH ParamBuilder
        public static string ACHParamBuilder(string accountToken, string transactionType, string chargeType,
            string entryMode, string orderID, string chargeAmount, string tcc, string customParameters) // Builds Parameters for WebPost
        {

            string entryModeBuilder = "entry_mode=" + entryMode;
            string transactionTypeBuilder = "transaction_type=" + transactionType;
            string chargeTypeBuilder = "charge_type=" + chargeType;
            string chargeAmountBuilder = "charge_total=" + chargeAmount;
            string orderIDBuilder = "order_id=" + orderID;
            string accountTokenBuilder = "account_token=" + accountToken;
            string tccBuilder = "transaction_condition_code=" + tcc;

            StringBuilder parameters = new StringBuilder();
            parameters.Append(accountTokenBuilder
                        + "&" + transactionTypeBuilder
                        + "&" + entryModeBuilder
                        + "&" + chargeTypeBuilder
                        + "&" + chargeAmountBuilder
                        + "&" + orderIDBuilder
                        + "&" + "duplicate_check=NO_CHECK"
                        + "&" + tccBuilder
                        + customParameters)
                        ;

            return parameters.ToString();

        }

        //Credit ParamBuilder
        public static string ParamBuilder(string accountToken, string transactionType, string chargeType,
            string entryMode, string orderID, string chargeAmount, string customParameters) // Builds Parameters for WebPost
        {

            string entryModeBuilder = "entry_mode=" + entryMode;
            string transactionTypeBuilder = "transaction_type=" + transactionType;
            string chargeTypeBuilder = "charge_type=" + chargeType;
            string chargeAmountBuilder = "charge_total=" + chargeAmount;
            string orderIDBuilder = "order_id=" + orderID;
            string accountTokenBuilder = "account_token=" + accountToken;

            StringBuilder parameters = new StringBuilder();
            parameters.Append(accountTokenBuilder
                        + "&" + transactionTypeBuilder
                        + "&" + entryModeBuilder
                        + "&" + chargeTypeBuilder
                        + "&" + chargeAmountBuilder
                        + "&" + orderIDBuilder
                        + "&" + "duplicate_check=NO_CHECK"
                        + customParameters);

            return parameters.ToString();

        }

        //Credit ParamBuilder
        public static string ParamBuilderAppCode(string accountToken, string transactionType, string chargeType,
            string entryMode, string orderID, string chargeAmount, string approvalCode, string customParameters) // Builds Parameters for WebPost
        {

            string entryModeBuilder = "entry_mode=" + entryMode;
            string transactionTypeBuilder = "transaction_type=" + transactionType;
            string chargeTypeBuilder = "charge_type=" + chargeType;
            string chargeAmountBuilder = "charge_total=" + chargeAmount;
            string orderIDBuilder = "order_id=" + orderID;
            string accountTokenBuilder = "account_token=" + accountToken;
            string approvalCodeBuilder = "bank_approval_code=" + approvalCode;

            StringBuilder parameters = new StringBuilder();
            parameters.Append(accountTokenBuilder
                                + "&" + transactionTypeBuilder
                                + "&" + entryModeBuilder
                                + "&" + chargeTypeBuilder
                                + "&" + chargeAmountBuilder
                                + "&" + orderIDBuilder
                                + "&" + approvalCodeBuilder
                                + "&" + approvalCodeBuilder
                                + "&" + "duplicate_check=NO_CHECK"
                                + customParameters)
                                ;
            ;
            return parameters.ToString();

        }

        //Debit Param Builder Overload
        public static string ParamBuilder(string accountToken, string transactionType, string chargeType,
            string entryMode, string orderID, string chargeAmount, string accountType, string customParameters) // Builds Parameters for WebPost
        {

            string entryModeBuilder = "entry_mode=" + entryMode;
            string transactionTypeBuilder = "transaction_type=" + transactionType;
            string chargeTypeBuilder = "charge_type=" + chargeType;
            string chargeAmountBuilder = "charge_total=" + chargeAmount;
            string orderIDBuilder = "order_id=" + orderID;
            string accountTokenBuilder = "account_token=" + accountToken;
            string accountTypeStatus = accountType;
            string customParamBuilder = customParameters;
            string accountTypeBuilder = null;
            //string parameters = null;
            StringBuilder parameters = new StringBuilder();
            bool usesAccountType = false;

            switch (accountTypeStatus)
            {
                case "DEFAULT":
                    accountTypeBuilder = "account_type=2";
                    usesAccountType = false;
                    break;
                case "CASH_BENEFIT":
                    accountTypeBuilder = "account_type=3";
                    usesAccountType = true;
                    break;
                case "FOOD_STAMP":
                    accountTypeBuilder = "account_type=4";
                    usesAccountType = true;
                    break;
                default:
                    break;
            }


            if (usesAccountType == false)
            {
                parameters.Append(accountTokenBuilder
                                    + "&" + transactionTypeBuilder
                                    + "&" + entryModeBuilder
                                    + "&" + chargeTypeBuilder
                                    + "&" + chargeAmountBuilder
                                    + "&" + orderIDBuilder
                                    + "&" + "duplicate_check=NO_CHECK"
                                    + customParamBuilder)
                ;
            }
            if (usesAccountType == true)
            {
                parameters.Append(accountTokenBuilder
                                    + "&" + transactionTypeBuilder
                                    + "&" + entryModeBuilder
                                    + "&" + chargeTypeBuilder
                                    + "&" + chargeAmountBuilder
                                    + "&" + orderIDBuilder
                                    + "&" + "duplicate_check=NO_CHECK"
                                    + "&" + accountTypeBuilder
                                    + customParamBuilder)
                ;
            }

            return parameters.ToString();

        }

        //Query ParamBuilder
        public static string QueryBuilder(string accountToken, string orderID, string transactionType, string chargeType) //Build Query POST
        {
            string accountTokenBuilder = "account_token=" + accountToken;
            string transactionTypeBuilder = "transaction_type=" + transactionType;
            string chargeTypeBuilder = "charge_type=" + chargeType;
            string orderIDBuilder = "order_id=" + orderID;

            StringBuilder parameters = new StringBuilder();
            parameters.Append(accountTokenBuilder
                                + "&" + transactionTypeBuilder
                                + "&" + chargeTypeBuilder
                                + "&" + orderIDBuilder
                                + "&" + "full_detail_flag=true");


            return parameters.ToString();

        }

        //MPD ParamBuilder
        public static string mpdBuilder(string accountToken, string orderID, string transactionType, string chargeType,
            string chargeAmount, string payer_id, string span, string expYY, string expMM)
        {
            string accountTokenBuilder = "account_token=" + accountToken;
            string transactionTypeBuilder = "transaction_type=" + transactionType;
            string chargeTypeBuilder = "charge_type=" + chargeType;
            string orderIDBuilder = "order_id=" + orderID;
            string payerIDBuilder = "payer_identifier=" + payer_id;
            string spanBuilder = "span=" + span;
            //string expYYBuilder = "expire_year=" + expYY; Update Customer not yet implemented into API, 
            //string expMMBuilder = "expire_month=" + expMM;
            string amountBuilder = "charge_total=" + chargeAmount;
            StringBuilder parameters = new StringBuilder();
            parameters.Append(accountTokenBuilder
                                + "&" + transactionTypeBuilder
                                + "&" + chargeTypeBuilder
                                + "&" + orderIDBuilder
                                + "&" + payerIDBuilder
                                + "&" + spanBuilder
                                //+ "&" + expMMBuilder
                                //+ "&" + expYYBuilder
                                + "&" + amountBuilder
                                + "&" + "managed_payer_data=true"
                                /*+ "&" + "duplicate_check=NO_CHECK"*/);


            return parameters.ToString();
        }

        public static string orderIDRandom(int size) //Code for creating Randomized OrderIDs
        {
            Random random = new Random((int)DateTime.Now.Ticks); // Use Timestamp to Seed Random Number
            StringBuilder builder = new StringBuilder();
            Int32 ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65));
                builder.Append(ch.ToString());
            }
            return builder.ToString();
        }
    }
}
