using System;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;

namespace AirportAssignment
{
    public class Form1 : Form
    {
        private Container components = null;
        private ButtonPanelThread terminal1, terminal2, terminal3, arrivals1;
        private Button btnTerminal1, btnTerminal2, btnTerminal3, btnArrivals1, btnSendTo0, btnSendTo1, btnSendTo2, btnSendTo3;
        private WaitPanelThread taxiBetween1and2, taxiBetween2and3, taxiBetween3andToRunway, taxiToRunway, runway, taxiFromRunway;
        private Thread thread1, thread2, thread3, thread4, threadBetween1and2, threadBetween2and3, threadBetween3andToRunway, 
            threadTaxiToRunway, threadRunway, threadTaxiFromRunway;
        private Semaphore semaphoreTerminal1, semaphoreTerminal2, semaphoreTerminal3, semaphoreArrivals1, semaphoreTaxiBetween1and2, semaphoreTaxiBetween2and3, 
            semaphoreTaxiBetween3andToRunway, semaphoreTaxiToRunway, semaphoreRunway, semaphoreTaxiFromRunway;
        private Buffer bufferTerminal1, bufferTerminal2, bufferTerminal3, bufferArrivals1, bufferTaxiBetween1and2, bufferTaxiBetween2and3, 
            bufferTaxiBetween3andToRunway, bufferTaxiToRunway, bufferRunway, bufferTaxiFromRunway;
        private Thread semaphoreThreadTerminal1, semaphoreThreadTerminal2, semaphoreThreadTerminal3, semaphoreThreadArrivals1, semaphoreThreadTaxiBetween1and2, 
            semaphoreThreadTaxiBetween2and3, semaphoreThreadTaxiBetween3andToRunway, semaphoreThreadTaxiToRunway, semaphoreThreadRunway, semaphoreThreadTaxiFromRunway;
        private Thread bufferThreadTerminal1, bufferThreadTerminal2, bufferThreadTerminal3, bufferThreadArrivals1, bufferThreadTaxiBetween1and2, 
            bufferThreadTaxiBetween2and3, bufferThreadTaxiBetween3andToRunway, bufferThreadTaxiToRunway, bufferThreadRunway, bufferThreadTaxiFromRunway;
        private Panel pnlTerminal1, pnlTerminal2, pnlTaxiBetween1and2, pnlTerminal3, pnlArrivals1, pnlTaxiBetween2and3, pnlTaxiBetween3andToRunway, 
            pnlTaxiToRunway, pnlRunway, pnlTaxiFromRunway;
        private Plane[] planes;

        public Form1()
        {
            InitializeComponent();
            planes = new Plane[4];

            planes[0] = new Plane(new Point(0, 0), Color.Red, 0);
            planes[1] = new Plane(new Point(0, 0), Color.Blue, 0);
            planes[2] = new Plane(new Point(0, 0), Color.Green, 0);
            planes[3] = new Plane(new Point(0, 0), Color.Magenta, 1);

            semaphoreTerminal1 = new Semaphore();
            semaphoreTerminal2 = new Semaphore();
            semaphoreTerminal3 = new Semaphore();
            semaphoreArrivals1 = new Semaphore();
            semaphoreTaxiBetween1and2 = new Semaphore();
            semaphoreTaxiBetween2and3 = new Semaphore();
            semaphoreTaxiBetween3andToRunway = new Semaphore();
            semaphoreTaxiToRunway = new Semaphore();
            semaphoreRunway = new Semaphore();
            semaphoreTaxiFromRunway = new Semaphore();
            bufferTerminal1 = new Buffer();
            bufferTerminal2 = new Buffer();
            bufferTerminal3 = new Buffer();
            bufferArrivals1 = new Buffer();
            bufferTaxiBetween1and2 = new Buffer();
            bufferTaxiBetween2and3 = new Buffer();
            bufferTaxiBetween3andToRunway = new Buffer();
            bufferTaxiToRunway = new Buffer();
            bufferRunway = new Buffer();
            bufferTaxiFromRunway = new Buffer();

            terminal1 = new ButtonPanelThread(new Point(10, 40), planes[0], 150, true, false, false, pnlTerminal1, semaphoreTerminal1, semaphoreTaxiBetween1and2, bufferTerminal1, bufferTaxiBetween1and2, btnTerminal1, null, null, null, null, 11);
            terminal2 = new ButtonPanelThread(new Point(10, 40), planes[1], 150, true, false, false, pnlTerminal2, semaphoreTerminal2, semaphoreTaxiBetween2and3, bufferTerminal2, bufferTaxiBetween2and3, btnTerminal2, null, null, null, null, 11);
            terminal3 = new ButtonPanelThread(new Point(10, 40), planes[2], 150, true, false, false, pnlTerminal3, semaphoreTerminal3, semaphoreTaxiBetween3andToRunway, bufferTerminal3, bufferTaxiBetween3andToRunway, btnTerminal3, null, null, null, null, 11);
            arrivals1 = new ButtonPanelThread(new Point(30, 10), planes[3], 150, false, true, true, pnlArrivals1, semaphoreArrivals1, semaphoreRunway, null, bufferRunway, btnArrivals1, btnSendTo0, btnSendTo1, btnSendTo2, btnSendTo3, 4);

            taxiBetween1and2 = new WaitPanelThread(new Point(0, 10), 100, true, true, pnlTaxiBetween1and2, null, semaphoreTaxiBetween1and2, semaphoreTaxiBetween2and3, semaphoreTerminal2, bufferTaxiBetween1and2, bufferTaxiBetween2and3, bufferTerminal2, 13, 2);
            taxiBetween2and3 = new WaitPanelThread(new Point(0, 10), 100, true, true, pnlTaxiBetween2and3, null, semaphoreTaxiBetween2and3, semaphoreTaxiBetween3andToRunway, semaphoreTerminal3, bufferTaxiBetween2and3, bufferTaxiBetween3andToRunway, bufferTerminal3, 13, 3);
            taxiBetween3andToRunway = new WaitPanelThread(new Point(0, 10), 100, true, true, pnlTaxiBetween3andToRunway, null, semaphoreTaxiBetween3andToRunway, semaphoreTaxiToRunway, null, bufferTaxiBetween3andToRunway, bufferTaxiToRunway, null, 13, 4);
            taxiToRunway = new WaitPanelThread(new Point(10, 0), 100, true, false, pnlTaxiToRunway, null, semaphoreTaxiToRunway, semaphoreRunway, null, bufferTaxiToRunway, bufferRunway, null, 16, 4);
            runway = new WaitPanelThread(new Point(530, 10), 100, false, true, pnlRunway, null, semaphoreRunway, semaphoreTaxiFromRunway, null, bufferRunway, bufferTaxiFromRunway, null, 43, 0);
            taxiFromRunway = new WaitPanelThread(new Point(10, 150), 100, false, false, pnlTaxiFromRunway, null, semaphoreTaxiFromRunway, semaphoreTaxiBetween1and2, semaphoreTerminal1, bufferTaxiFromRunway, bufferTaxiBetween1and2, bufferTerminal1, 15, 1);

            semaphoreThreadTerminal1 = new Thread(new ThreadStart(semaphoreTerminal1.Start));
            semaphoreThreadTerminal2 = new Thread(new ThreadStart(semaphoreTerminal2.Start));
            semaphoreThreadTerminal3 = new Thread(new ThreadStart(semaphoreTerminal3.Start));
            semaphoreThreadArrivals1 = new Thread(new ThreadStart(semaphoreArrivals1.Start));
            semaphoreThreadTaxiBetween1and2 = new Thread(new ThreadStart(semaphoreTaxiBetween1and2.Start));
            semaphoreThreadTaxiBetween2and3 = new Thread(new ThreadStart(semaphoreTaxiBetween2and3.Start));
            semaphoreThreadTaxiBetween3andToRunway = new Thread(new ThreadStart(semaphoreTaxiBetween3andToRunway.Start));
            semaphoreThreadTaxiToRunway = new Thread(new ThreadStart(semaphoreTaxiToRunway.Start));
            semaphoreThreadRunway = new Thread(new ThreadStart(semaphoreRunway.Start));
            semaphoreThreadTaxiFromRunway = new Thread(new ThreadStart(semaphoreTaxiFromRunway.Start));
            bufferThreadTerminal1 = new Thread(new ThreadStart(bufferTerminal1.Start));
            bufferThreadTerminal2 = new Thread(new ThreadStart(bufferTerminal2.Start));
            bufferThreadTerminal3 = new Thread(new ThreadStart(bufferTerminal3.Start));
            bufferThreadArrivals1 = new Thread(new ThreadStart(bufferArrivals1.Start));
            bufferThreadTaxiBetween1and2 = new Thread(new ThreadStart(bufferTaxiBetween1and2.Start));
            bufferThreadTaxiBetween2and3 = new Thread(new ThreadStart(bufferTaxiBetween2and3.Start));
            bufferThreadTaxiBetween3andToRunway = new Thread(new ThreadStart(bufferTaxiBetween3andToRunway.Start));
            bufferThreadTaxiToRunway = new Thread(new ThreadStart(bufferTaxiToRunway.Start));
            bufferThreadRunway = new Thread(new ThreadStart(bufferRunway.Start));
            bufferThreadTaxiFromRunway = new Thread(new ThreadStart(bufferTaxiFromRunway.Start));

            thread1 = new Thread(new ThreadStart(terminal1.Start));
            thread2 = new Thread(new ThreadStart(terminal2.Start));
            thread3 = new Thread(new ThreadStart(terminal3.Start));
            thread4 = new Thread(new ThreadStart(arrivals1.Start));
            threadBetween1and2 = new Thread(new ThreadStart(taxiBetween1and2.Start));
            threadBetween2and3 = new Thread(new ThreadStart(taxiBetween2and3.Start));
            threadBetween3andToRunway = new Thread(new ThreadStart(taxiBetween3andToRunway.Start));
            threadTaxiToRunway = new Thread(new ThreadStart(taxiToRunway.Start));
            threadRunway = new Thread(new ThreadStart(runway.Start));
            threadTaxiFromRunway = new Thread(new ThreadStart(taxiFromRunway.Start));

            this.Closing += new CancelEventHandler(this.Form1_Closing);

            semaphoreThreadTerminal1.Start();
            semaphoreThreadTerminal2.Start();
            semaphoreThreadTerminal3.Start();
            semaphoreThreadArrivals1.Start();
            semaphoreThreadTaxiBetween1and2.Start();
            semaphoreThreadTaxiBetween2and3.Start();
            semaphoreThreadTaxiBetween3andToRunway.Start();
            semaphoreThreadTaxiToRunway.Start();
            semaphoreThreadRunway.Start();
            semaphoreThreadTaxiFromRunway.Start();
            bufferThreadTerminal1.Start();
            bufferThreadTerminal2.Start();
            bufferThreadTerminal3.Start();
            bufferThreadArrivals1.Start();
            bufferThreadTaxiBetween1and2.Start();
            bufferThreadTaxiBetween2and3.Start();
            bufferThreadTaxiBetween3andToRunway.Start();
            bufferThreadTaxiToRunway.Start();
            bufferThreadRunway.Start();
            bufferThreadTaxiFromRunway.Start();
            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            threadBetween1and2.Start();
            threadBetween2and3.Start();
            threadBetween3andToRunway.Start();
            threadTaxiToRunway.Start();
            threadRunway.Start();
            threadTaxiFromRunway.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Text = "Airport Assignment                          Kathryn Lowen 10016148";
            this.Size = new System.Drawing.Size(700, 500);
            this.BackColor = Color.LightGray;

            this.pnlTerminal1 = new Panel();
            this.pnlTerminal1.Location = new Point(120, 30);
            this.pnlTerminal1.Size = new Size(30, 150);
            this.pnlTerminal1.BackColor = Color.DarkGray;
            this.btnTerminal1 = new Button();
            this.btnTerminal1.Size = new Size(30, 30);
            this.btnTerminal1.BackColor = Color.DarkRed;
            this.btnTerminal1.Location = new System.Drawing.Point(0, 0);

            this.pnlTerminal2 = new Panel();
            this.pnlTerminal2.Location = new Point(250, 30);
            this.pnlTerminal2.Size = new Size(30, 150);
            this.pnlTerminal2.BackColor = Color.DarkGray;
            this.btnTerminal2 = new Button();
            this.btnTerminal2.Size = new Size(30, 30);
            this.btnTerminal2.BackColor = Color.DarkRed;
            this.btnTerminal2.Location = new System.Drawing.Point(0, 0);

            this.pnlTerminal3 = new Panel();
            this.pnlTerminal3.Location = new Point(380, 30);
            this.pnlTerminal3.Size = new Size(30, 150);
            this.pnlTerminal3.BackColor = Color.DarkGray;
            this.btnTerminal3 = new Button();
            this.btnTerminal3.Size = new Size(30, 30);
            this.btnTerminal3.BackColor = Color.DarkRed;
            this.btnTerminal3.Location = new System.Drawing.Point(0, 0);

            this.pnlArrivals1 = new Panel();
            this.pnlArrivals1.Location = new Point(540, 340);
            this.pnlArrivals1.Size = new Size(80, 30);
            this.pnlArrivals1.BackColor = Color.DarkGray;
            this.btnArrivals1 = new Button();
            this.btnArrivals1.Size = new Size(30, 30);
            this.btnArrivals1.BackColor = Color.DarkRed;
            this.btnArrivals1.Location = new System.Drawing.Point(50, 0);

            this.btnSendTo0 = new Button();
            this.btnSendTo0.Location = new Point(510, 395);
            this.btnSendTo0.Size = new Size(70, 20);
            this.btnSendTo0.Text = "Takeoff";
            this.btnSendTo0.Font = new Font("Arial", 8, FontStyle.Regular);
            this.btnSendTo0.TextAlign = ContentAlignment.MiddleCenter;
            this.btnSendTo0.BackColor = Color.SteelBlue;

            this.btnSendTo1 = new Button();
            this.btnSendTo1.Location = new Point(590, 395);
            this.btnSendTo1.Size = new Size(70, 20);
            this.btnSendTo1.Text = "Terminal 1";
            this.btnSendTo1.Font = new Font("Arial", 8, FontStyle.Regular);
            this.btnSendTo1.TextAlign = ContentAlignment.MiddleCenter;
            this.btnSendTo1.BackColor = Color.SteelBlue;

            this.btnSendTo2 = new Button();
            this.btnSendTo2.Location = new Point(510, 420);
            this.btnSendTo2.Size = new Size(70, 20);
            this.btnSendTo2.Text = "Terminal 2";
            this.btnSendTo2.Font = new Font("Arial", 8, FontStyle.Regular);
            this.btnSendTo2.TextAlign = ContentAlignment.MiddleCenter;
            this.btnSendTo2.BackColor = Color.SteelBlue;

            this.btnSendTo3 = new Button();
            this.btnSendTo3.Location = new Point(590, 420);
            this.btnSendTo3.Size = new Size(70, 20);
            this.btnSendTo3.Text = "Terminal 3";
            this.btnSendTo3.Font = new Font("Arial", 8, FontStyle.Regular);
            this.btnSendTo3.TextAlign = ContentAlignment.MiddleCenter;
            this.btnSendTo3.BackColor = Color.SteelBlue;

            this.pnlTaxiBetween1and2 = new Panel();
            this.pnlTaxiBetween1and2.Location = new Point(120, 180);
            this.pnlTaxiBetween1and2.Size = new Size(130, 30);
            this.pnlTaxiBetween1and2.BackColor = Color.DarkGray;

            this.pnlTaxiBetween2and3 = new Panel();
            this.pnlTaxiBetween2and3.Location = new Point(250, 180);
            this.pnlTaxiBetween2and3.Size = new Size(130, 30);
            this.pnlTaxiBetween2and3.BackColor = Color.DarkGray;

            this.pnlTaxiBetween3andToRunway = new Panel();
            this.pnlTaxiBetween3andToRunway.Location = new Point(380, 180);
            this.pnlTaxiBetween3andToRunway.Size = new Size(130, 30);
            this.pnlTaxiBetween3andToRunway.BackColor = Color.DarkGray;

            this.pnlTaxiToRunway = new Panel();
            this.pnlTaxiToRunway.Location = new Point(510, 180);
            this.pnlTaxiToRunway.Size = new Size(30, 160);
            this.pnlTaxiToRunway.BackColor = Color.DarkGray;

            this.pnlRunway = new Panel();
            this.pnlRunway.Location = new Point(0, 340);
            this.pnlRunway.Size = new Size(540, 30);
            this.pnlRunway.BackColor = Color.DarkGray;

            this.pnlTaxiFromRunway = new Panel();
            this.pnlTaxiFromRunway.Location = new Point(90, 180);
            this.pnlTaxiFromRunway.Size = new Size(30, 160);
            this.pnlTaxiFromRunway.BackColor = Color.DarkGray;

            this.Controls.Add(pnlTerminal1);
            this.Controls.Add(pnlTerminal2);
            this.Controls.Add(pnlTerminal3);
            this.Controls.Add(pnlArrivals1);
            this.Controls.Add(pnlTaxiBetween1and2);
            this.Controls.Add(pnlTaxiBetween2and3);
            this.Controls.Add(pnlTaxiBetween3andToRunway);
            this.Controls.Add(pnlTaxiToRunway);
            this.Controls.Add(pnlRunway);
            this.Controls.Add(pnlTaxiFromRunway);
            this.Controls.Add(btnSendTo0);
            this.Controls.Add(btnSendTo1);
            this.Controls.Add(btnSendTo2);
            this.Controls.Add(btnSendTo3);

            this.pnlTerminal1.Controls.Add(btnTerminal1);
            this.pnlTerminal2.Controls.Add(btnTerminal2);
            this.pnlTerminal3.Controls.Add(btnTerminal3);
            this.pnlArrivals1.Controls.Add(btnArrivals1);

            // Wire Closing event.      
            this.Closing += new CancelEventHandler(this.Form1_Closing);
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            // Environment is a System class.
            // Kill off all threads on exit.
            Environment.Exit(Environment.ExitCode);
        }
    }
}
