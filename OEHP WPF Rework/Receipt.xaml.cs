using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Drawing;


namespace OEHP_WPF_Rework
{
    /// <summary>
    /// Interaction logic for Receipt.xaml
    /// </summary>
    public partial class Receipt : Window
    {
        public Receipt()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            string s = ReceiptText.Text;

            PrintDialog printDialog = new PrintDialog();
            if ((bool)printDialog.ShowDialog().GetValueOrDefault())
            {
                FlowDocument fD = new FlowDocument();
                foreach (string line in ReceiptText.Text.Split('\n'))
                {
                    Paragraph p = new Paragraph();
                    p.Margin = new Thickness(0);
                    p.Inlines.Add(new Run(line));
                    fD.Blocks.Add(p);
                }

                DocumentPaginator paginator = ((IDocumentPaginatorSource)fD).DocumentPaginator;
                printDialog.PrintDocument(paginator, "oehpReceipt");
            }
        }
        
    }
}
