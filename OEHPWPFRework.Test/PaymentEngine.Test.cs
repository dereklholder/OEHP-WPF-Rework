using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OEHP_WPF_Rework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace OEHPWPFRework.Test
{
    [TestClass]
    public class PaymentEngineTest
    {
        [TestMethod]
        public void SealedSetupParametersReturnsValue()
        {
            //Arrange
            PaymentEngine.JsonResponse json = new PaymentEngine.JsonResponse();
            //Act
            string jsonString = "{\"sealedSetupParameters\":\"TestValue\"}";
            json = JsonConvert.DeserializeObject<PaymentEngine.JsonResponse>(jsonString);
            //Assert
            Assert.AreEqual("TestValue", json.sealedSetupParameters);
        }
        [TestMethod]
        public void actionUrlReturnsValue()
        {
            //Arrange
            PaymentEngine.JsonResponse json = new PaymentEngine.JsonResponse();
            //Act
            string jsonString = "{\"actionUrl\":\"TestValue\"}";
            json = JsonConvert.DeserializeObject<PaymentEngine.JsonResponse>(jsonString);
            //Assert
            Assert.AreEqual("TestValue", json.actionUrl);
        }
        [TestMethod]
        public void errorMessageReturnsValue()
        {
            //Arrange
            PaymentEngine.JsonResponse json = new PaymentEngine.JsonResponse();
            //Act
            string jsonString = "{\"errorMessage\":\"TestValue\"}";
            json = JsonConvert.DeserializeObject<PaymentEngine.JsonResponse>(jsonString);
            //Assert
            Assert.AreEqual("TestValue", json.errorMessage);
        }
        [TestMethod]
        public void webRequestPostReturnsValue()
        {
            //NYI
        }

    }
}
