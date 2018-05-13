using System;
using System.IO;

namespace Parking
{
    public class Logger
    {
        private readonly InternalTimer timer;
        private Parking Parking { get; }
        private readonly string fileName = @"Transactions.log";

        public Logger(Parking parking, int interaval)
        {
            timer = new InternalTimer(interaval, WriteLogFile);

            Parking = parking;

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        private void WriteLogFile()
        {
            try
            {
                using (var file = new StreamWriter(fileName, true))
                {
                    var tmpQueue = Parking.Trasactions.ToArray();

                    foreach (var list in tmpQueue)
                    {
                        if (list.Count == 0) return;
                        var dateTime = list[0].DateTime;
                        var sum = 0M;

                        foreach (var trasaction in list)
                        {
                            sum += trasaction.Debit;
                        }

                        file.WriteLine(dateTime + "     {0:C}", sum);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string PrintLogFile()
        {
            if (File.Exists(fileName))
            {
                try
                {
                    var readText = File.ReadAllText(fileName);

                    if (!string.IsNullOrEmpty(readText))
                    {
                       return readText;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }

        }
    }
}