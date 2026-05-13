using EnergyMonitoring.DTO;

namespace EnergyMonitoring.Modules
{
    public static class DateTimeFunc
    {
        public static DateTime SetDayUtc(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0).ToUniversalTime();
        }

        public static ResultFromTo DataInOnedayUtc(DateTime dt)
        {
            return  new ResultFromTo {
                FromDateTime = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0).ToUniversalTime(),
                ToDateTime = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 0).ToUniversalTime()
            };
        }

        public static DateTime SetHourUtc(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0).ToUniversalTime();
        }

        public static ResultFromTo DataInOneHourUtc(DateTime dt)
        {
            return new ResultFromTo
            {
                FromDateTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0).ToUniversalTime(),
                ToDateTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour,  59, 0).ToUniversalTime()
            };
        }

        public static DateTime SetMinuteUtc(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0).ToUniversalTime();
        }

        public static ResultFromTo DataInOneMinuteUtc(DateTime dt)
        {
            return new ResultFromTo
            {
                FromDateTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0).ToUniversalTime(),
                ToDateTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 59).ToUniversalTime()
            };
        }


        public static string FindQ15Minute(DateTime dt)
        {
            if(dt.Minute >= 0 && dt.Minute <= 14)
            {
                return "Q1";
            }
            else if (dt.Minute >= 15 && dt.Minute <= 29)
            {
                return "Q2";
            }
            else if (dt.Minute >= 30 && dt.Minute <= 44)
            {
                return "Q3";
            }
            else
            {
                return "Q4";
            }
        }


        public static (DateTime,ResultFromTo)  Data15Minute(string q1 , DateTime dt)
        {
            switch (q1)
            {
                case "Q1":
                    return (new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0).ToUniversalTime(),
                        new ResultFromTo {
                        FromDateTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0).ToUniversalTime(),
                        ToDateTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 14, 0).ToUniversalTime()
                        });
                    
                case "Q2":
                    return (new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 15, 0).ToUniversalTime(),
                        new ResultFromTo {
                        FromDateTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 15, 0).ToUniversalTime(),
                        ToDateTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 29, 0).ToUniversalTime()
                        });

                case "Q3":
                    return (new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 30, 0).ToUniversalTime(),
                        new ResultFromTo {
                        FromDateTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 30, 0).ToUniversalTime(),
                        ToDateTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 44 , 0).ToUniversalTime()
                        });
                case "Q4":
                    return (new DateTime(dt.Year ,dt.Month ,dt.Day ,dt.Hour ,45 ,0).ToUniversalTime(),
                        new ResultFromTo {
                        FromDateTime = new DateTime(dt.Year ,dt.Month ,dt.Day ,dt.Hour ,45 ,0).ToUniversalTime(),
                        ToDateTime = new DateTime(dt.Year ,dt.Month ,dt.Day ,dt.Hour ,59 ,0).ToUniversalTime()
                        });

                default:
                    throw new ArgumentException("Invalid quarter specified. Use Q1 to Q4.");
            }
        }


    
        //public static (DateTime, DateTime, DateTime) GetMinuteQ1(DateTime dt)
        //{
        //    return (new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0, DateTimeKind.Utc),
        //            new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 14, 0, DateTimeKind.Utc),
        //            new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0, DateTimeKind.Utc));
        //}
        //public static (DateTime, DateTime, DateTime) GetMinuteQ2(DateTime dt)
        //{
        //    return (new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 15, 0, DateTimeKind.Utc),
        //            new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 29, 0, DateTimeKind.Utc),
        //            new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 15, 0, DateTimeKind.Utc));
        //}
        //public static (DateTime, DateTime, DateTime) GetMinuteQ3(DateTime dt)
        //{
        //    return (new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 30, 0, DateTimeKind.Utc),
        //            new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 44, 0, DateTimeKind.Utc),
        //            new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 30, 0, DateTimeKind.Utc));
        //}
        //public static (DateTime, DateTime, DateTime) GetMinuteQ4(DateTime dt)
        //{
        //    return (new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 45, 0, DateTimeKind.Utc),
        //            new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 59, 0, DateTimeKind.Utc),
        //            new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 45, 0, DateTimeKind.Utc));
        //}


    }
}
