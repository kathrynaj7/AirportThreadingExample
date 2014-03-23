using System;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;

namespace AirportAssignment
{
    public class ButtonPanelThread
    {
        private Point origin;
        private int delay;
        private Panel panel;
        private bool positiveAxis, isArrival;
        private Plane plane;
        private int xDelta;
        private int yDelta;
        private int length;
        private Semaphore semaphoreThis;
        private Semaphore semaphoreNext;
        private Buffer bufferIn;
        private Buffer bufferOut;
        private Button btn, takeoff, terminal1, terminal2, terminal3;
        private bool locked = true;
        private bool takeoffLock, terminal1Lock, terminal2Lock, terminal3Lock;

        public ButtonPanelThread(Point origin, Plane plane, int delay, bool positiveAxis, bool horizontal, bool isArrival, Panel panel, Semaphore semaphoreThis, Semaphore semaphoreNext, Buffer bufferIn, Buffer bufferOut, Button btn, Button takeoff, Button terminal1, Button terminal2, Button terminal3, int length)
        {
            this.origin = origin;
            this.delay = delay;
            this.positiveAxis = positiveAxis;
            this.panel = panel;
            this.plane = plane;
            this.panel.Paint += new PaintEventHandler(this.panel_Paint);
            if (horizontal)
            {
                this.xDelta = positiveAxis ? +10 : -10;
                this.yDelta = 0;
            }
            else
            {
                this.xDelta = 0;
                this.yDelta = positiveAxis ? +10 : -10;
            }
            this.semaphoreThis = semaphoreThis;
            this.semaphoreNext = semaphoreNext;
            this.bufferIn = bufferIn;
            this.bufferOut = bufferOut;
            this.btn = btn;
            this.isArrival = isArrival;
            if (isArrival)
            {
                this.takeoff = takeoff;
                this.terminal1 = terminal1;
                this.terminal2 = terminal2;
                this.terminal3 = terminal3;
                this.takeoff.Click += new System.EventHandler(this.takeoff_Click);
                this.terminal1.Click += new System.EventHandler(this.terminal1_Click);
                this.terminal2.Click += new System.EventHandler(this.terminal2_Click);
                this.terminal3.Click += new System.EventHandler(this.terminal3_Click);
            }
            this.takeoffLock = true;
            this.terminal1Lock = true;
            this.terminal2Lock = true;
            this.terminal3Lock = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            this.length = length;
        }

        private void btn_Click(object sender, System.EventArgs e)
        {
            locked = !locked;
            this.btn.BackColor = locked ? Color.DarkRed : Color.IndianRed;
            lock (this)
            {
                if (!locked)
                    Monitor.Pulse(this);
            }
        }

        private void takeoff_Click(object sender, System.EventArgs e)
        {
            takeoffLock = !takeoffLock;
            this.terminal1Lock = true;
            this.terminal2Lock = true;
            this.terminal3Lock = true;
            this.takeoff.BackColor = takeoffLock ? Color.SteelBlue : Color.LightSteelBlue;
            this.terminal1.BackColor = terminal1Lock ? Color.SteelBlue : Color.LightSteelBlue;
            this.terminal2.BackColor = terminal2Lock ? Color.SteelBlue : Color.LightSteelBlue;
            this.terminal3.BackColor = terminal3Lock ? Color.SteelBlue : Color.LightSteelBlue;
            if (plane != null)
            {
                this.plane.setDestination(0);
                panel.Invalidate();
            }
        }

        private void terminal1_Click(object sender, System.EventArgs e)
        {
            this.takeoffLock = true;
            terminal1Lock = !terminal1Lock;
            this.terminal2Lock = true;
            this.terminal3Lock = true;
            this.takeoff.BackColor = takeoffLock ? Color.SteelBlue : Color.LightSteelBlue;
            this.terminal1.BackColor = terminal1Lock ? Color.SteelBlue : Color.LightSteelBlue;
            this.terminal2.BackColor = terminal2Lock ? Color.SteelBlue : Color.LightSteelBlue;
            this.terminal3.BackColor = terminal3Lock ? Color.SteelBlue : Color.LightSteelBlue;
            if (plane != null)
            {
                this.plane.setDestination(1);
                panel.Invalidate();
            }
        }

        private void terminal2_Click(object sender, System.EventArgs e)
        {
            this.takeoffLock = true;
            this.terminal1Lock = true;
            terminal2Lock = !terminal2Lock;
            this.terminal3Lock = true;
            this.takeoff.BackColor = takeoffLock ? Color.SteelBlue : Color.LightSteelBlue;
            this.terminal1.BackColor = terminal1Lock ? Color.SteelBlue : Color.LightSteelBlue;
            this.terminal2.BackColor = terminal2Lock ? Color.SteelBlue : Color.LightSteelBlue;
            this.terminal3.BackColor = terminal3Lock ? Color.SteelBlue : Color.LightSteelBlue;
            if (plane != null)
            {
                this.plane.setDestination(2);
                panel.Invalidate();
            }
        }

        private void terminal3_Click(object sender, System.EventArgs e)
        {
            this.takeoffLock = true;
            this.terminal1Lock = true;
            this.terminal2Lock = true;
            terminal3Lock = !terminal3Lock;
            this.takeoff.BackColor = takeoffLock ? Color.SteelBlue : Color.LightSteelBlue;
            this.terminal1.BackColor = terminal1Lock ? Color.SteelBlue : Color.LightSteelBlue;
            this.terminal2.BackColor = terminal2Lock ? Color.SteelBlue : Color.LightSteelBlue;
            this.terminal3.BackColor = terminal3Lock ? Color.SteelBlue : Color.LightSteelBlue;
            if (plane != null)
            {
                this.plane.setDestination(3);
                panel.Invalidate();
            }
        }

        public void Start()
        {
            Thread.Sleep(delay);
            for (int k = 1; k <= 100; k++)
            {
                semaphoreThis.Signal();
                if (this.plane != null)
                {
                    semaphoreThis.Wait();
                }
                if (this.plane == null)
                {
                    bufferIn.Read(ref this.plane);
                    this.movePlaneToBottom();
                    panel.Invalidate();
                    for (int i = 1; i < length; i++)
                    {
                        this.movePlane(xDelta, -yDelta);
                        Thread.Sleep(delay);
                        panel.Invalidate();
                    }
                    this.plane.setDestination(0);

                    locked = true;
                    this.btn.BackColor = locked ? Color.DarkRed : Color.IndianRed;
                    lock (this)
                    {
                        if (!locked)
                            Monitor.Pulse(this);
                    }
                }
                this.zeroPlane();
                panel.Invalidate();
                lock (this)
                {
                    while (locked)
                    {
                        Monitor.Wait(this);
                    }
                }
                for (int i = 1; i < length; i++)
                {
                    this.movePlane(xDelta, yDelta);
                    Thread.Sleep(delay);
                    panel.Invalidate();
                }
                semaphoreNext.Wait();
                bufferOut.Write(this.plane);
                this.plane = null;
                panel.Invalidate();
                if (isArrival)
                {
                    this.locked = true;
                    this.btn.BackColor = locked ? Color.DarkRed : Color.IndianRed;
                    this.plane = new Plane(origin, this.randomColorGenerator(), 0);
                    panel.Invalidate();
                }
            }
        }

        private void zeroPlane()
        {
            plane.setPos(origin.X, origin.Y);
        }

        private void movePlane(int xDelta, int yDelta)
        {
            plane.Move(xDelta, yDelta);
        }

        private void movePlaneToBottom()
        {
            this.plane.setPos(this.panel.Size.Width - 20, this.panel.Size.Height - 15);
        }


        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (plane != null)
            {
                SolidBrush brush = new SolidBrush(plane.getColour());
                g.FillRectangle(brush, plane.getPosX(), plane.getPosY(), 10, 10);
                SolidBrush blackBrush = new SolidBrush(Color.White);
                g.DrawString(plane.getStringDestination(), new Font("Arial", 7), blackBrush, new PointF((float)plane.getPosX(), (float)plane.getPosY()));
                brush.Dispose();
            }
            g.Dispose();
        }

        private Color randomColorGenerator()
        {
            Random random = new Random();
            int rand = random.Next(8);
            Color randomColour;

            switch (rand)
            {
                case 1:
                    randomColour = Color.Red;
                    break;
                case 2:
                    randomColour = Color.Magenta;
                    break;
                case 3:
                    randomColour = Color.Pink;
                    break;
                case 4:
                    randomColour = Color.Green;
                    break;
                case 5:
                    randomColour = Color.Orange;
                    break;
                case 6:
                    randomColour = Color.Purple;
                    break;
                case 7:
                    randomColour = Color.Blue;
                    break;
                case 8:
                    randomColour = Color.LightBlue;
                    break;
                default:
                    randomColour = Color.Black;
                    break;
            }

            return randomColour;
        }
    }
}
