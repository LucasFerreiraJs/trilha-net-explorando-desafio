using System.Globalization;

namespace DesafioProjetoHospedagem.Models
{
    public class Suite
    {
        public const string ActiveStatus = "S";
        public const string InactiveStatus = "N";

        public Suite() { }

        public Suite(
            int? id,
            string suiteType,
            int capacity,
            int dailyValue,
            string status = ActiveStatus)
        {
            Id = id;
            SuiteType = suiteType;
            Capacity = capacity;
            DailyValue = dailyValue;
            Status = status;
        }
        public Suite( string suiteType, int capacity, int  dailyValue)
        {
            SuiteType = suiteType;
            Capacity = capacity;
            DailyValue = dailyValue;
        }

        public int? Id { get; set; }
        public string SuiteType { get; set; }
        public int Capacity { get; set; }
        public int DailyValue { get; set; }
        public string Status { get; set; } = ActiveStatus;


        public decimal DailyValueToDecimal(){

            return DailyValue / 100m;
        }
        public string FormatedDailyValue(){
            return DailyValueToDecimal().ToString("0.00", CultureInfo.InvariantCulture);

        }
    }
}
