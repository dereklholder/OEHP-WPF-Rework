﻿using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace OEHP_WPF_Rework
{
    /// <summary>
    /// OEHP .NET demo done with WPF
    /// Code is for Demonstration/Evaluation purposes only, not intended for Live use
    /// Created by Derek Holder
    /// </summary>
    public partial class MainWindow : Window
    {
        Encoding _encoding;
        IBlockCipherPadding _padding;

        public MainWindow()
        {
            InitializeComponent();
            transactionTypeCollection.Add("CREDIT_CARD");
            transactionTypeCollection.Add("DEBIT_CARD");
            transactionTypeCollection.Add("ACH");
            transactionTypeCollection.Add("INTERAC"); 

            this.transactionTypeCombo.ItemsSource = transactionTypeCollection;

            _encoding = Encoding.ASCII;
            Pkcs7Padding pkcs = new Pkcs7Padding();
            _padding = pkcs;

            var settingsPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.dat").ToString();
            string data = File.ReadAllText(settingsPath);
            string decryptedData = AESDecryption(data, VariableHandler.CryptoKey, true);
            var parts = decryptedData.Split(',');
            VariableHandler.AccountToken = parts[0];
            customParamText.Text = parts[1];

            accountTokenText.Text = VariableHandler.AccountToken;
        }
        public void writeToLog(string logString) //Code for logging functions.
        {
            var logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.IO.Path.Combine("Logging", "Log.txt")).ToString();
            string timeStamp = DateTime.Now.ToString();
            File.AppendAllText(logPath, timeStamp + Environment.NewLine + logString + Environment.NewLine + "--------------------------------------------------" + Environment.NewLine);
        }
        public string TCC = null;

        //Collections for ComboBoxen
        public ObservableCollection<string> transactionTypeCollection = new ObservableCollection<string>();
        public ObservableCollection<string> entryModeCollection = new ObservableCollection<string>();
        public ObservableCollection<string> chargeTypeCollection = new ObservableCollection<string>();
        public ObservableCollection<string> creditTypeCollection = new ObservableCollection<string>();
        public ObservableCollection<string> accountTypeCollection = new ObservableCollection<string>();
        public ObservableCollection<string> tccCollection = new ObservableCollection<string>();

        //Sets available Fields based on Transaction Type Suggested
        private void transactionTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                switch (transactionTypeCombo.SelectedItem.ToString())
                {
                    case "CREDIT_CARD":
                        accountTypeCombo.Visibility = Visibility.Hidden;
                        accountTypeLabel.Visibility = Visibility.Hidden;
                        creditTypeCombo.Visibility = Visibility.Hidden;
                        creditTypeLabel.Visibility = Visibility.Hidden;
                        sigImage.Visibility = Visibility.Visible;
                        returnedSignatureLabel.Visibility = Visibility.Visible;
                        tccCombo.Visibility = Visibility.Hidden;
                        tccLabel.Visibility = Visibility.Hidden;
                        approvalCodeLabel.Visibility = Visibility.Hidden;
                        approvalCodeText.Visibility = Visibility.Hidden;

                        entryModeCollection.Clear();
                        entryModeCollection.Add("EMV");
                        entryModeCollection.Add("HID");
                        entryModeCollection.Add("KEYED");
                        entryModeCombo.ItemsSource = entryModeCollection;

                        chargeTypeCollection.Clear();
                        chargeTypeCollection.Add("SALE");
                        chargeTypeCollection.Add("CREDIT");
                        chargeTypeCollection.Add("VOID");
                        chargeTypeCollection.Add("FORCE_SALE");
                        chargeTypeCollection.Add("AUTH");
                        chargeTypeCollection.Add("CAPTURE");
                        chargeTypeCollection.Add("ADJUSTMENT");
                        chargeTypeCollection.Add("SIGNATURE");
                        chargeTypeCombo.ItemsSource = chargeTypeCollection;

                        break;

                    case "DEBIT_CARD":
                        accountTypeCombo.Visibility = Visibility.Visible;
                        accountTypeLabel.Visibility = Visibility.Visible;
                        creditTypeCombo.Visibility = Visibility.Hidden;
                        creditTypeLabel.Visibility = Visibility.Hidden;
                        sigImage.Visibility = Visibility.Hidden;
                        returnedSignatureLabel.Visibility = Visibility.Hidden;
                        tccCombo.Visibility = Visibility.Hidden;
                        tccLabel.Visibility = Visibility.Hidden;
                        approvalCodeLabel.Visibility = Visibility.Hidden;
                        approvalCodeText.Visibility = Visibility.Hidden;

                        entryModeCollection.Clear();
                        entryModeCollection.Add("EMV");
                        entryModeCollection.Add("HID");
                        entryModeCombo.ItemsSource = entryModeCollection;

                        chargeTypeCollection.Clear();
                        chargeTypeCollection.Add("PURCHASE");
                        chargeTypeCollection.Add("REFUND");
                        chargeTypeCombo.ItemsSource = chargeTypeCollection;

                        break;

                    case "ACH":
                        accountTypeCombo.Visibility = Visibility.Hidden;
                        accountTypeLabel.Visibility = Visibility.Hidden;
                        creditTypeCombo.Visibility = Visibility.Hidden;
                        creditTypeLabel.Visibility = Visibility.Hidden;
                        sigImage.Visibility = Visibility.Hidden;
                        returnedSignatureLabel.Visibility = Visibility.Hidden;
                        tccCombo.Visibility = Visibility.Visible;
                        tccLabel.Visibility = Visibility.Visible;
                        approvalCodeLabel.Visibility = Visibility.Hidden;
                        approvalCodeText.Visibility = Visibility.Hidden;

                        entryModeCollection.Clear();
                        entryModeCollection.Add("KEYED");
                        entryModeCombo.ItemsSource = entryModeCollection;

                        chargeTypeCollection.Clear();
                        chargeTypeCollection.Add("DEBIT");
                        chargeTypeCollection.Add("CREDIT");
                        chargeTypeCombo.ItemsSource = chargeTypeCollection;

                        tccCollection.Clear();
                        tccCollection.Add("PPD");
                        tccCollection.Add("CCD");
                        tccCollection.Add("WEB");
                        tccCollection.Add("TEL");
                        tccCombo.ItemsSource = tccCollection;

                        break;

                    case "INTERAC":
                        accountTypeCombo.Visibility = Visibility.Hidden;
                        accountTypeLabel.Visibility = Visibility.Hidden;
                        creditTypeCombo.Visibility = Visibility.Hidden;
                        creditTypeLabel.Visibility = Visibility.Hidden;
                        sigImage.Visibility = Visibility.Hidden;
                        returnedSignatureLabel.Visibility = Visibility.Hidden;
                        tccCombo.Visibility = Visibility.Hidden;
                        tccLabel.Visibility = Visibility.Hidden;
                        approvalCodeLabel.Visibility = Visibility.Hidden;
                        approvalCodeText.Visibility = Visibility.Hidden;

                        entryModeCollection.Clear();
                        entryModeCollection.Add("EMV");
                        entryModeCollection.Add("HID");
                        entryModeCombo.ItemsSource = entryModeCollection;

                        chargeTypeCollection.Clear();
                        chargeTypeCollection.Add("PURCHASE");
                        chargeTypeCollection.Add("REFUND");
                        chargeTypeCombo.ItemsSource = chargeTypeCollection;

                        break;

                    default:
                        MessageBox.Show("Invalid Transaction Type Input");
                        break;

                }
            }
            catch (System.NullReferenceException ex)
            {
                
            }
            catch (Exception ex)
            {

            }
        }

        //Submit Button Logic for various transaction types.
        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            VariableHandler.SSP = null; //Sets the OTK storage to NULL, used for RCM Status and perhaps further implementations
            string parameters;
            string otk;

            switch (transactionTypeCombo.Text)
            {
                case "CREDIT_CARD":
                    switch (chargeTypeCombo.Text)
                    {
                        case "SALE":
                            //Randomizes the Order, sends Settings over to the ParamBuilder, Then Sends a request to get the OtK  (sealedSetup Parameters) and then
                            // Apends to the PayPage URL to render the page.
                            orderIDText.Text = PaymentEngine.orderIDRandom(8);
                            parameters = PaymentEngine.ParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                entryModeCombo.Text, orderIDText.Text, amountText.Text, customParamText.Text);
                            postParametersText.Text = parameters;
                            writeToLog(parameters);

                            otk = PaymentEngine.webRequest_Post(parameters);

                            hostPayBrowser.Navigate(PaymentEngine.otkURL + otk);
                            break;

                        case "CREDIT":

                            switch (creditTypeCombo.Text)
                            {
                                case "DEPENDENT":
                                    parameters = PaymentEngine.ParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                        entryModeCombo.Text, orderIDText.Text, amountText.Text, customParamText.Text);
                                    postParametersText.Text = parameters;
                                    writeToLog(parameters);
                                    hostPayBrowser.NavigateToString(PaymentEngine.webRequest_Query(parameters));
                                    break;

                                case "INDEPENDENT":
                                    parameters = PaymentEngine.ParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                        entryModeCombo.Text, orderIDText.Text, amountText.Text, customParamText.Text); // Build Parameters for POST
                                    postParametersText.Text = parameters;
                                    writeToLog(parameters);

                                    otk = PaymentEngine.webRequest_Post(parameters);

                                    hostPayBrowser.Navigate(PaymentEngine.otkURL + otk);
                                    break;
                                default:
                                    MessageBox.Show("An Error has occured, Invalid transaction parameters");
                                    break;
                            }
                            break; //End Credit Case

                        case "AUTH":
                            orderIDText.Text = PaymentEngine.orderIDRandom(8);
                            parameters = PaymentEngine.ParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                entryModeCombo.Text, orderIDText.Text, amountText.Text, customParamText.Text);
                            postParametersText.Text = parameters;
                            writeToLog(parameters);

                            otk = PaymentEngine.webRequest_Post(parameters);

                            hostPayBrowser.Navigate(PaymentEngine.otkURL + otk);
                            break;

                        case "VOID":
                            orderIDText.Text = PaymentEngine.orderIDRandom(8);
                            parameters = PaymentEngine.ParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                entryModeCombo.Text, orderIDText.Text, amountText.Text, customParamText.Text);
                            postParametersText.Text = parameters;
                            writeToLog(parameters);

                            hostPayBrowser.NavigateToString(PaymentEngine.webRequest_Query(parameters));
                            break;

                        case "FORCE_SALE":
                            orderIDText.Text = PaymentEngine.orderIDRandom(8);
                            parameters = PaymentEngine.ParamBuilderAppCode(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                entryModeCombo.Text, orderIDText.Text, amountText.Text, approvalCodeText.Text, customParamText.Text);
                            postParametersText.Text = parameters;
                            writeToLog(parameters);

                            hostPayBrowser.NavigateToString(PaymentEngine.webRequest_Query(parameters));
                            break;

                        case "CAPTURE":
                            orderIDText.Text = PaymentEngine.orderIDRandom(8);
                            parameters = PaymentEngine.ParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                entryModeCombo.Text, orderIDText.Text, amountText.Text, customParamText.Text);
                            postParametersText.Text = parameters;
                            writeToLog(parameters);

                            hostPayBrowser.NavigateToString(PaymentEngine.webRequest_Query(parameters));
                            break;

                        case "ADJUSTMENT":
                            orderIDText.Text = PaymentEngine.orderIDRandom(8);
                            parameters = PaymentEngine.ParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                entryModeCombo.Text, orderIDText.Text, amountText.Text, customParamText.Text);
                            postParametersText.Text = parameters;
                            writeToLog(parameters);

                            hostPayBrowser.NavigateToString(PaymentEngine.webRequest_Query(parameters));
                            break;

                        case "SIGNATURE":
                            orderIDText.Text = PaymentEngine.orderIDRandom(8);
                            parameters = PaymentEngine.ParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                entryModeCombo.Text, orderIDText.Text, amountText.Text, customParamText.Text);
                            postParametersText.Text = parameters;
                            writeToLog(parameters);

                            otk = PaymentEngine.webRequest_Post(parameters);

                            hostPayBrowser.Navigate(PaymentEngine.otkURL + otk);
                            break;

                        default:
                            MessageBox.Show("An error has occured, Invalid transaction parameters");
                            break;

                    } // End Credit_CARD request
                    break;

                case "DEBIT_CARD":

                    switch (chargeTypeCombo.Text)
                    {
                        case "REFUND":
                            parameters = PaymentEngine.ParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                entryModeCombo.Text, orderIDText.Text, amountText.Text, accountTypeCombo.Text, customParamText.Text); // Build Parameters for POST
                            postParametersText.Text = parameters;
                            writeToLog(parameters);

                            otk = PaymentEngine.webRequest_Post(parameters);

                            hostPayBrowser.Navigate(PaymentEngine.otkURL + otk); //Navigate Web Browser to Paypage URL + Session Token
                            break;

                        case "PURCHASE":
                            orderIDText.Text = PaymentEngine.orderIDRandom(8);
                            parameters = PaymentEngine.ParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                entryModeCombo.Text, orderIDText.Text, amountText.Text, accountTypeCombo.Text, customParamText.Text); // Build Parameters for POST
                            postParametersText.Text = parameters;
                            writeToLog(parameters);

                            otk = PaymentEngine.webRequest_Post(parameters);

                            hostPayBrowser.Navigate(PaymentEngine.otkURL + otk); //Navigate Web Browser to Paypage URL + Session Token
                            break;

                        default:
                            MessageBox.Show("An error has occured, Invalid Transaction Parameters");
                            break;

                    } //End Debit Card Switch
                    break;

                case "INTERAC":

                    switch (chargeTypeCombo.Text)
                    {
                        case "REFUND":
                            parameters = PaymentEngine.ParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                entryModeCombo.Text, orderIDText.Text, amountText.Text, accountTypeCombo.Text, customParamText.Text); // Build Parameters for POST
                            postParametersText.Text = parameters;
                            writeToLog(parameters);

                            otk = PaymentEngine.webRequest_Post(parameters);

                            hostPayBrowser.Navigate(PaymentEngine.otkURL + otk); //Navigate Web Browser to Paypage URL + Session Token
                            break;

                        case "PURCHASE":
                            orderIDText.Text = PaymentEngine.orderIDRandom(8);
                            parameters = PaymentEngine.ParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                entryModeCombo.Text, orderIDText.Text, amountText.Text, accountTypeCombo.Text, customParamText.Text); // Build Parameters for POST
                            postParametersText.Text = parameters;
                            writeToLog(parameters);

                            otk = PaymentEngine.webRequest_Post(parameters);

                            hostPayBrowser.Navigate(PaymentEngine.otkURL + otk); //Navigate Web Browser to Paypage URL + Session Token
                            break;

                        default:
                            MessageBox.Show("An error has occured, Invalid Transaction Parameters");
                            break;

                    } //End  INterac Switch
                    break;

                case "ACH":
                    switch (chargeTypeCombo.Text)
                    {
                        case "CREDIT":

                            switch (creditTypeCombo.Text)
                            {
                                case "DEPENDENT":
                                    parameters = PaymentEngine.ACHParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                        entryModeCombo.Text, orderIDText.Text, amountText.Text, TCC, customParamText.Text);
                                    postParametersText.Text = parameters;
                                    writeToLog(parameters);

                                    hostPayBrowser.NavigateToString(PaymentEngine.webRequest_Query(parameters));
                                    break;

                                case "INDEPENDENT":
                                    parameters = PaymentEngine.ACHParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                        entryModeCombo.Text, orderIDText.Text, amountText.Text, VariableHandler.TCC, customParamText.Text);
                                    postParametersText.Text = parameters;
                                    writeToLog(parameters);

                                    otk = PaymentEngine.webRequest_Post(parameters);

                                    hostPayBrowser.Navigate(PaymentEngine.otkURL + otk);
                                    break;
                                default:
                                    MessageBox.Show("An Error has occured, Invalid transaction parameters");
                                    break;
                            }
                            break; //End Credit Case

                        case "DEBIT":
                            orderIDText.Text = PaymentEngine.orderIDRandom(8);
                            parameters = PaymentEngine.ACHParamBuilder(accountTokenText.Text, transactionTypeCombo.Text, chargeTypeCombo.Text,
                                entryModeCombo.Text, orderIDText.Text, amountText.Text, VariableHandler.TCC, customParamText.Text);
                            postParametersText.Text = parameters;
                            writeToLog(parameters);

                            otk = PaymentEngine.webRequest_Post(parameters);

                            hostPayBrowser.Navigate(PaymentEngine.otkURL + otk);
                            break;

                        default:
                            MessageBox.Show("An Error has occured, Invalid Transaction Parameters");
                            break;

                    } // End ACH
                    break;

                default:
                    MessageBox.Show("An Error has occured, Invalid Transaction Parameters");
                    break;
            }
        }

        private void chargeTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string chargeTypeNow = chargeTypeCombo.SelectedItem.ToString();
            switch (chargeTypeNow)
            {
                case "CREDIT":
                    approvalCodeLabel.Visibility = Visibility.Hidden;
                    approvalCodeText.Visibility = Visibility.Hidden;
                    switch (transactionTypeCombo.SelectedItem.ToString())
                    {
                        case "CREDIT_CARD":
                            creditTypeCombo.Visibility = Visibility.Visible;
                            creditTypeLabel.Visibility = Visibility.Visible;

                            creditTypeCollection.Clear();
                            creditTypeCollection.Add("INDEPENDENT");
                            creditTypeCollection.Add("DEPENDENT");
                            creditTypeCombo.ItemsSource = creditTypeCollection;


                            break;

                        default:
                            creditTypeCombo.Visibility = Visibility.Hidden;
                            creditTypeLabel.Visibility = Visibility.Hidden;


                            break;

                    }
                    break;

                case "REFUND":
                    orderIDText.IsReadOnly = true;
                    approvalCodeLabel.Visibility = Visibility.Hidden;
                    approvalCodeText.Visibility = Visibility.Hidden;
                    break;

                case "FORCE_SALE":
                    orderIDText.IsReadOnly = false;
                    approvalCodeText.Visibility = Visibility.Visible;
                    approvalCodeLabel.Visibility = Visibility.Visible;
                    break;

                case "VOID":
                    orderIDText.IsReadOnly = false;
                    approvalCodeLabel.Visibility = Visibility.Hidden;
                    approvalCodeText.Visibility = Visibility.Hidden;
                    break;

                case "CAPTURE":
                    orderIDText.IsReadOnly = false;
                    approvalCodeLabel.Visibility = Visibility.Hidden;
                    approvalCodeText.Visibility = Visibility.Hidden;
                    break;

                default:
                    orderIDText.IsReadOnly = true;
                    approvalCodeLabel.Visibility = Visibility.Hidden;
                    approvalCodeText.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void tccCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tccCombo.SelectedItem.ToString())
            {
                case "PPD":
                    VariableHandler.TCC = "50";
                    break;

                case "TEL":
                    VariableHandler.TCC = "51";
                    break;

                case "WEB":
                    VariableHandler.TCC = "52";
                    break;

                case "CCD":
                    VariableHandler.TCC = "53";
                    break;

                default:
                    VariableHandler.TCC = null;
                    break;
            }
        }
        public static string browserContent(WebBrowser wb)
        {
            //mshtml.HTMLDocumentClass dom = (mshtml.HTMLDocumentClass)wb.Document;
            //var innerHtml = dom.body.innerHTML;
            string innerHtml = (wb.Document as mshtml.IHTMLDocument2).body.innerHTML;
            return innerHtml;
        }
        private void hostPayBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            //Performs Query on Every Doc Completed, if it sees the response_code=1 then it it displays the query result. Better implementation to come.

            string parameters;
            string finishedResponse = @"^(.*?(\bresponse_code=1\b)[^$]*)$";
            string queryResult;
            bool performQuery;
            

            if (null != hostPayBrowser.Document)
            {
                switch (transactionTypeCombo.Text)
                {
                    case "CREDIT_CARD":
                        parameters = PaymentEngine.QueryBuilder(accountTokenText.Text, orderIDText.Text,
                    transactionTypeCombo.Text, "QUERY_PAYMENT"); // Build Query
                        queryResult = PaymentEngine.webRequest_Query(parameters);
                        performQuery = Regex.Match(queryResult, finishedResponse).Success;

                        if (performQuery == true)
                        {
                            queryBrowser.NavigateToString(queryResult);
                        }
                        else if (performQuery != true)
                        {
                            queryBrowser.Navigate("about:blank");
                        }
                        
                        queryPostText.Text = parameters;
                        break;

                    case "DEBIT_CARD":
                        parameters = PaymentEngine.QueryBuilder(accountTokenText.Text, orderIDText.Text,
                    transactionTypeCombo.Text, "QUERY_PURCHASE"); // Build Query
                        queryResult = PaymentEngine.webRequest_Query(parameters);
                        performQuery = Regex.Match(queryResult, finishedResponse).Success;

                        if (performQuery == true)
                        {
                            queryBrowser.NavigateToString(queryResult);
                        }
                        else if (performQuery != true)
                        {
                            queryBrowser.Navigate("about:blank");
                        }

                        queryPostText.Text = parameters;
                        break;

                    case "ACH":
                        parameters = PaymentEngine.QueryBuilder(accountTokenText.Text, orderIDText.Text,
                    transactionTypeCombo.Text, "QUERY"); // Build Query
                        queryResult = PaymentEngine.webRequest_Query(parameters);
                        performQuery = Regex.Match(queryResult, finishedResponse).Success;

                        if (performQuery == true)
                        {
                            queryBrowser.NavigateToString(queryResult);
                        }
                        else if (performQuery != true)
                        {
                            queryBrowser.Navigate("about:blank");
                        }

                        queryPostText.Text = parameters;
                        break;

                    default:
                        break;
                }
            }

            //RCM Status Code, Will fire after every DocCompleted event.

            try
            {
                string ssp = VariableHandler.SSP;
                WebRequest wr = WebRequest.Create("https://ws.test.paygateway.com/HostPayService/v1/hostpay/transactions/status/" + ssp);
                wr.Method = "GET";

                Stream objStream;
                objStream = wr.GetResponse().GetResponseStream();

                StreamReader sr = new StreamReader(objStream);

                string rcmStatus = sr.ReadToEnd();
                rcmStatusText.Text = rcmStatus;
            }
            catch (Exception ex)
            {
                writeToLog(ex.ToString());
            }


        }

        private void queryBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            
            try
            {
                StringBuilder s = new StringBuilder();
                s.Append(queryBrowser.ToString());

                writeToLog(s.ToString());
                string queryString = HttpUtility.HtmlDecode((queryBrowser.Document as mshtml.IHTMLDocument2).body.innerHTML);
                NameValueCollection keyPairs = HttpUtility.ParseQueryString(queryString);

                string id = keyPairs.Get("receipt_approval_code");
                string payer_Id = keyPairs.Get("payer_identifier");
                string exp_mm = keyPairs.Get("expire_month");
                string exp_yy = keyPairs.Get("expire_year");
                string span = keyPairs.Get("span");
                string tranType = transactionTypeCombo.Text;
                string label = DateTime.Now.ToShortDateString() + DateTime.Now.ToLongTimeString(); // + DateTime.Now.ToLongDateString();
                StringBuilder forTheLogging = new StringBuilder();
                forTheLogging.Append("Here is the Data Being Attempted to Add to DB" + Environment.NewLine + payer_Id + Environment.NewLine + exp_mm + Environment.NewLine + exp_yy + Environment.NewLine + span + Environment.NewLine + label);
                var dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.IO.Path.Combine("Logging", "db.dat")).ToString();


                StringBuilder dbString = new StringBuilder();
                dbString.Append(id + ',' + payer_Id + ',' + exp_mm + ',' + exp_yy + ',' + span + ',' + label + ',' + tranType + Environment.NewLine);

                File.AppendAllText(dbPath, dbString.ToString());


                writeToLog(forTheLogging.ToString());


            }
            catch (Exception ex)
            {
                writeToLog(ex.ToString());
            }
        }

        private void forceQueryButton_Click(object sender, RoutedEventArgs e)
        {
            string parameters;
            string finishedResponse = @"^(.*?(\b&response_code=1\b)[^$]*)$";
            string queryResult;
            bool performQuery;

            switch (transactionTypeCombo.Text)
            {
                case "CREDIT_CARD":
                    parameters = PaymentEngine.QueryBuilder(accountTokenText.Text, orderIDText.Text,
                transactionTypeCombo.Text, "QUERY_PAYMENT"); // Build Query
                    queryResult = PaymentEngine.webRequest_Query(parameters);
                    performQuery = Regex.Match(queryResult, finishedResponse).Success;

                    if (performQuery == true)
                    {
                        queryBrowser.NavigateToString(queryResult);
                    }
                    else if (performQuery != true)
                    {
                        queryBrowser.NavigateToString("No Transaction to Query");
                    }

                    queryPostText.Text = parameters;
                    break;

                case "DEBIT_CARD":
                    parameters = PaymentEngine.QueryBuilder(accountTokenText.Text, orderIDText.Text,
                transactionTypeCombo.Text, "QUERY_PURCHASE"); // Build Query
                    queryResult = PaymentEngine.webRequest_Query(parameters);
                    performQuery = Regex.Match(queryResult, finishedResponse).Success;

                    if (performQuery == true)
                    {
                        queryBrowser.NavigateToString(queryResult);
                    }
                    else if (performQuery != true)
                    {
                        queryBrowser.NavigateToString("No Transaction to Query");
                    }

                    queryPostText.Text = parameters;
                    break;

                case "ACH":
                    parameters = PaymentEngine.QueryBuilder(accountTokenText.Text, orderIDText.Text,
                transactionTypeCombo.Text, "QUERY"); // Build Query
                    queryResult = PaymentEngine.webRequest_Query(parameters);
                    performQuery = Regex.Match(queryResult, finishedResponse).Success;

                    if (performQuery == true)
                    {
                        queryBrowser.NavigateToString(queryResult);
                    }
                    else if (performQuery != true)
                    {
                        queryBrowser.NavigateToString("No Transaction to Query");
                    }

                    queryPostText.Text = parameters;
                    break;

                default:
                    break;
            }
        }

        private void parseReceipt_Click(object sender, RoutedEventArgs e)
        {
            //Performs additional Query and Displays Receipt

            string parameters;
            string finishedResponse = @"^(.*?(\b&response_code=1\b)[^$]*)$";
            string queryResult;
            bool performQuery;

            switch (transactionTypeCombo.Text)
            {
                case "CREDIT_CARD":
                    parameters = PaymentEngine.QueryBuilder(accountTokenText.Text, orderIDText.Text,
                transactionTypeCombo.Text, "QUERY_PAYMENT"); // Build Query
                    queryResult = PaymentEngine.webRequest_Query(parameters);
                    performQuery = Regex.Match(queryResult, finishedResponse).Success;

                    if (performQuery == true)
                    {
                        
                        NameValueCollection keyPairs = HttpUtility.ParseQueryString(queryResult);
                        string receiptData = HttpUtility.UrlDecode(keyPairs.Get("customer_receipt"));
                        //string receiptFormatted = receiptData.Replace("\n", "\r\n");
                        Receipt r = new Receipt();
                        r.ReceiptText.Text = receiptData.Replace("\n", "\r\n");
                        r.ShowDialog();
                    }
                    else if (performQuery != true)
                    {
                        MessageBox.Show("No Receipt Data to Parse");
                    }

                    queryPostText.Text = parameters;
                    break;

                case "DEBIT_CARD":
                    parameters = PaymentEngine.QueryBuilder(accountTokenText.Text, orderIDText.Text,
                transactionTypeCombo.Text, "QUERY_PURCHASE"); // Build Query
                    queryResult = PaymentEngine.webRequest_Query(parameters);
                    performQuery = Regex.Match(queryResult, finishedResponse).Success;

                    if (performQuery == true)
                    {

                        NameValueCollection keyPairs = HttpUtility.ParseQueryString(queryResult);
                        string receiptData = HttpUtility.UrlDecode(keyPairs.Get("customer_receipt"));
                        //string receiptFormatted = receiptData.Replace("\n", "\r\n");
                        Receipt r = new Receipt();
                        r.ReceiptText.Text = receiptData.Replace("\n", "\r\n");
                        r.ShowDialog();
                    }
                    else if (performQuery != true)
                    {
                        MessageBox.Show("No Receipt Data to Parse");
                    }

                    queryPostText.Text = parameters;
                    break;

                case "ACH":
                    parameters = PaymentEngine.QueryBuilder(accountTokenText.Text, orderIDText.Text,
                transactionTypeCombo.Text, "QUERY"); // Build Query
                    queryResult = PaymentEngine.webRequest_Query(parameters);
                    performQuery = Regex.Match(queryResult, finishedResponse).Success;

                    if (performQuery == true)
                    {

                        NameValueCollection keyPairs = HttpUtility.ParseQueryString(queryResult);
                        string receiptData = HttpUtility.UrlDecode(keyPairs.Get("customer_receipt"));
                        //string receiptFormatted = receiptData.Replace("\n", "\r\n");
                        Receipt r = new Receipt();
                        r.ReceiptText.Text = receiptData.Replace("\n", "\r\n");
                        r.ShowDialog();
                    }
                    else if (performQuery != true)
                    {
                        MessageBox.Show("No Receipt Data to Parse");
                    }

                    queryPostText.Text = parameters;
                    break;

                default:
                    break;
            }
            
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void mpdTransactions_Click(object sender, RoutedEventArgs e)
        {
            MPDTransactions m = new MPDTransactions();
            m.Show();
            this.Close();
        }

        public string AESEncryption(string plain, string key, bool fips) //Encryption for Settings.DAT
        {
            Crypto superSecret = new Crypto(new AesEngine(), _encoding);
            superSecret.SetPadding(_padding);
            return superSecret.Encrypt(plain, key);

        }

        public string AESDecryption(string cipher, string key, bool fips) //Decryption for Settings.DAT
        {
            Crypto superSecret = new Crypto(new AesEngine(), _encoding);
            superSecret.SetPadding(_padding);
            return superSecret.Decrypt(cipher, key);
        }

        private void saveSettings_Click(object sender, RoutedEventArgs e)
        {
            string saveToken = accountTokenText.Text;
            string saveCustom = customParamText.Text;
            StringBuilder sb = new StringBuilder();
            sb.Append(saveToken + "," + saveCustom);
            var settingsPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.dat").ToString();
            File.WriteAllText(settingsPath, AESEncryption(sb.ToString(), VariableHandler.CryptoKey, true));
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            creditTypeCombo.Visibility = Visibility.Hidden;
            creditTypeLabel.Visibility = Visibility.Hidden;
        }

        private void creditTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (creditTypeCombo.SelectedItem.ToString())
            {
                case "INDEPENDENT":
                    orderIDText.IsReadOnly = false;
                    break;
                case "DEPENDENT":
                    orderIDText.IsReadOnly = true;
                    break;
                default:
                    break;

            }
        }
    }
}

