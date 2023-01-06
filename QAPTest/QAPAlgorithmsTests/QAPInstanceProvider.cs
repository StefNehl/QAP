using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPTest.QAPAlgorithmsTests
{
    public class QAPInstanceProvider
    {
        public static async Task<QAPInstance> GetChr12a()
        {
            var folderName = "QAPLIB";
            var fileName = "chr12a.dat";

            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            return await qapReader.ReadFileAsync(folderName, fileName);
        }

        public static async Task<QAPInstance> GetTestN3()
        {
            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            return qapReader.ReadFileAsync("Small", "TestN3.dat").Result;
        }
    }
}
