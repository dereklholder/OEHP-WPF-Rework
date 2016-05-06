using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OEHP_WPF_Rework;

namespace OEHPWPFRework.Test
{
    [TestClass]
    public class VariableHandler
    {
        [TestMethod]
        public void SSPReturnsValue()
        {
            //Arrange

            //Act
            OEHP_WPF_Rework.VariableHandler.SSP = "TestValue";
            string result = OEHP_WPF_Rework.VariableHandler.SSP;
            //Assert
            Assert.AreEqual("TestValue", result);

        }
        [TestMethod]
        public void TCCReturnsValue()
        {
            //Arrange

            //Act
            OEHP_WPF_Rework.VariableHandler.TCC = "TestValue";
            string result = OEHP_WPF_Rework.VariableHandler.TCC;
            //Assert
            Assert.AreEqual("TestValue", result);
        }
        [TestMethod]
        public void AccountTokenReturnsValue()
        {
            //Arrange

            //Act
            OEHP_WPF_Rework.VariableHandler.AccountToken = "TestValue";
            string result = OEHP_WPF_Rework.VariableHandler.AccountToken;
            //Assert
            Assert.AreEqual("TestValue", result);
        }
        [TestMethod]
        public void CryptoKeyReturnsValue()
        {
            //Arrange

            //Act
            string result = OEHP_WPF_Rework.VariableHandler.CryptoKey;
            //Assert
            Assert.AreEqual("6f70656e65646765686f7374706179DH", result);
        }
        [TestMethod]
        public void RcmFinishedReturnsValue()
        {
            //Arrange

            //Act
            OEHP_WPF_Rework.VariableHandler.RcmFinished = "TestValue";
            string result = OEHP_WPF_Rework.VariableHandler.RcmFinished;
            //Assert
            Assert.AreEqual("TestValue", result);
        }
        [TestMethod]
        public void RcmStatusURLReturnsValue()
        {
            //Arrange

            //Act
            string result = OEHP_WPF_Rework.VariableHandler.RcmStatusURL;
            //Assert
            Assert.AreEqual("https://ws.test.paygateway.com/HostPayService/v1/hostpay/transactions/status/", result);

        }
        [TestMethod]
        public void PaymentFinishedSignalReturnsValue()
        {
            //Arrange

            //Act
            OEHP_WPF_Rework.VariableHandler.PaymentFinishedSignal = "TestValue";
            string result = OEHP_WPF_Rework.VariableHandler.PaymentFinishedSignal;
            //Assert
            Assert.AreEqual("TestValue", result);
        }
    }
}
