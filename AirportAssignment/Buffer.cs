using System;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;

namespace AirportAssignment
{
    public class Buffer
    {
        private Plane plane;
        private bool empty = true;

        public void Read(ref Plane plane)
        {
            lock (this)
            {
                // Check whether the buffer is empty.
                if (empty)
                    Monitor.Wait(this);
                empty = true;
                plane = this.plane;
                Monitor.Pulse(this);
            }
        }

        public void Write(Plane plane)
        {
            lock (this)
            {
                // Check whether the buffer is full.
                if (!empty)
                    Monitor.Wait(this);
                empty = false;
                this.plane = plane;
                Monitor.Pulse(this);
            }
        }

        public void Start()
        {
        }

    }
}
