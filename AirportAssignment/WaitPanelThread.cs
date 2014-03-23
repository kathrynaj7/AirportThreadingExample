using System;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
namespace AirportAssignment
{
    public class WaitPanelThread
    {
        private Point origin;
        private int delay;
        private Panel panel;
        private bool positiveAxis;
        private Plane plane;
        private int xDelta;
        private int yDelta;
        private Semaphore semaphoreThis;
        private Semaphore semaphoreNext;
        private Semaphore semaphoreTerminal;
        private Buffer bufferPrevious;
        private Buffer bufferNext;
        private Buffer bufferTerminal;
        private int length, terminalID;

        public WaitPanelThread(Point origin, int delay, bool positiveAxis, bool horizontal, Panel panel, Plane plane, Semaphore semaphoreThis, Semaphore semaphoreNext, Semaphore semaphoreTerminal, Buffer bufferPrevious, Buffer bufferNext, Buffer bufferTerminal, int length, int terminalID)
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
            this.semaphoreTerminal = semaphoreTerminal;
            this.bufferPrevious = bufferPrevious;
            this.bufferNext = bufferNext;
            this.bufferTerminal = bufferTerminal;
            this.length = length;
            this.terminalID = terminalID;
        }

        public void Start()
        {
            Thread.Sleep(delay);
            for (int k = 1; k <= 100; k++)
            {
                semaphoreThis.Signal();

                bufferPrevious.Read(ref this.plane);

                this.zeroPlane();

                if (this.plane.getDestination() == 0 && terminalID == 0)
                {
                    for (int i = 1; i <= this.length + 8; i++)
                    {
                        this.movePlane(xDelta, yDelta);
                        panel.Invalidate();
                        Thread.Sleep(delay - (int)(i * 1.8));
                    }
                    this.plane = null;
                    panel.Invalidate();
                }
                else
                {
                    if (terminalID == 0)
                    {
                        for (int i = 1; i <= this.length; i++)
                        {
                            this.movePlane(xDelta, yDelta);
                            panel.Invalidate();
                            Thread.Sleep(15 + (int)(i * 2.0));
                        }
                    }
                    else 
                    {
                        for (int i = 1; i < length; i++)
                        {
                            panel.Invalidate();
                            this.movePlane(xDelta, yDelta);
                            Thread.Sleep(delay);
                        }
                    }
                    if (this.plane.getDestination() == terminalID)
                    {
                        semaphoreTerminal.Wait();
                        bufferTerminal.Write(this.plane);
                        this.plane = null;
                        panel.Invalidate();
                    }
                    else
                    {
                        semaphoreNext.Wait();
                        bufferNext.Write(this.plane);
                        this.plane = null;
                        panel.Invalidate();
                    }
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
    }
}
