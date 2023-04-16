using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNewsArchivator
{
    public class CsakFooldalakStarter
    {
        public CsakFooldalakStarter()
        {
        }

        public void ArchiveFooldalak()
        {
            bool csakFooldal = true;
            CsakMandinerFooldal csakMandinerFooldal = new CsakMandinerFooldal();
            CsakMoszkvaterFooldal csakMoszkvaterFooldal = new CsakMoszkvaterFooldal();

            while (csakFooldal)
            {

                csakMandinerFooldal.ArchiveAll();
                //System.Threading.Thread.Sleep(50000);
                new System.Threading.ManualResetEvent(false).WaitOne(50000);

                csakMoszkvaterFooldal.ArchiveAll();
                //System.Threading.Thread.Sleep(7200000);
                new System.Threading.ManualResetEvent(false).WaitOne(7200000);

                if (DateTime.Now > TimeStamp.Instance.TheDateTime.AddHours(14))
                {
                    csakFooldal = false;
                }
            }

        }
    }
}
