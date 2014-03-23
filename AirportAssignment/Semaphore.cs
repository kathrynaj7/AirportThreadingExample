using System;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;

namespace AirportAssignment
{
    public class Semaphore
    {
        private int count = 0;

        public void Wait()
        {
            lock (this)
            {
                while (count == 0)
                    Monitor.Wait(this);
                count = 0;
            }
        }

        public void Signal()
        {
            lock (this)
            {
                count = 1;
                Monitor.Pulse(this);
            }
        }

        public void Start()
        {
        }

    }
}
