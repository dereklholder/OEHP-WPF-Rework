using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace OEHP_WPF_Rework
{
    /// <summary>
    /// Interaction logic for MPDTransactions.xaml
    /// </summary>
    public partial class MPDTransactions : Window
    {
        public MPDTransactions()
        {
            InitializeComponent();

            
        }
        //Collections
        public ObservableCollection<string> chargeTypeCollection = new ObservableCollection<string>();
        public ObservableCollection<string> tccCollection = new ObservableCollection<string>();
        public ObservableCollection<string> transactionTypeCollection = new ObservableCollection<string>();

        private void standardTransactions_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void transactionTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (transactionTypeCombo.SelectedItem.ToString())
            {
                case "ACH":
                    tccCombo.Visibility = Visibility.Visible;
                    tccLabel.Visibility = Visibility.Visible;
                    chargeTypeCollection.Clear();
                    chargeTypeCollection.Add("DEBIT");
                    chargeTypeCollection.Add("CREDIT");
                    chargeTypeCollection.Add("DELETE_CUSTOMER");
                    chargeTypeCombo.ItemsSource = chargeTypeCollection;

                    tccCollection.Clear();
                    tccCollection.Add("PPD");
                    tccCollection.Add("TEL");
                    tccCollection.Add("WEB");
                    tccCollection.Add("CCD");
                    tccCombo.Items.Clear();
                    tccCombo.ItemsSource = tccCollection;
                    break;

                case "CREDIT_CARD":
                    tccCombo.Visibility = Visibility.Hidden;
                    tccLabel.Visibility = Visibility.Hidden;
                    
                    chargeTypeCollection.Clear();
                    chargeTypeCollection.Add("SALE");
                    chargeTypeCollection.Add("CREDIT");
                    chargeTypeCollection.Add("FORCE_SALE");
                    chargeTypeCollection.Add("DELETE_CUSTOMER");
                    chargeTypeCollection.Add("AUTH");
                    chargeTypeCombo.ItemsSource = chargeTypeCollection;
                    break;

                default:
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
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tccCombo.Visibility = Visibility.Hidden;
            tccLabel.Visibility = Visibility.Hidden;

            accountTokenText.Text = VariableHandler.AccountToken;

            DataContext = DBService.ReadFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.IO.Path.Combine("Logging", "db.dat")).ToString());

            transactionTypeCollection.Clear();
            transactionTypeCollection.Add("CREDIT_CARD");
            transactionTypeCollection.Add("ACH");
            transactionTypeCombo.ItemsSource = transactionTypeCollection;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                orderIDText.Text = PaymentEngine.orderIDRandom(8);
                string parameters = PaymentEngine.mpdBuilder(accountTokenText.Text, orderIDText.Text, transactionTypeCombo.Text,
                    chargeTypeCombo.Text, amountText.Text, payerIDText.Text, spanText.Text, null, null);
                postParametersText.Text = parameters;
                writeToLog(parameters);

                hostPayBrowser.NavigateToString(PaymentEngine.webRequest_Query(parameters));
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error has Occured, please check the log.");
                writeToLog(ex.ToString());
            }
        }
        public void writeToLog(string logString) //Code for logging functions.
        {
            var logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.IO.Path.Combine("Logging", "Log.txt")).ToString();
            string timeStamp = DateTime.Now.ToString();
            System.IO.File.AppendAllText(logPath, timeStamp + Environment.NewLine + logString + Environment.NewLine + "--------------------------------------------------" + Environment.NewLine);
        }

        private void hostPayBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //string script = "document.body.style.overflow='hidden'";
            //WebBrowser hostPayBrowser = (WebBrowser)sender;
            //hostPayBrowser.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }
    }
}
