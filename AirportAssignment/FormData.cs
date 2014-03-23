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
        private ButtonPanelThread p1, p3;
        private Button btn1, btn3;
        private WaitPanelThread p2;
        private Thread thread1, thread2, thread3;
        private Semaphore semaphore;
        private Buffer buffer;
        private Thread semThread;
        private Thread buffThread;
        private Panel pnl1, pnl2, pnl3;

        public Form1()
        {
            InitializeComponent();

            semaphore = new Semaphore();
            buffer = new Buffer();

            p1 = new ButtonPanelThread(new Point(40, 10),
                                 120, true, pnl1,
                                 Color.Blue,
                                 semaphore,
                                 buffer,
                                 btn1);

            p2 = new WaitPanelThread(new Point(40, 10),
                                 100, true, pnl2,
                                 Color.White,
                                 semaphore,
                                 buffer);

            p3 = new ButtonPanelThread(new Point(40, 10),
                                 250, true, pnl3,
                                 Color.Red,
                                 semaphore,
                                 buffer,
                                 btn3);

            semThread = new Thread(new ThreadStart(semaphore.Start));
            buffThread = new Thread(new ThreadStart(buffer.Start));
            thread1 = new Thread(new ThreadStart(p1.Start));
            thread2 = new Thread(new ThreadStart(p2.Start));
            thread3 = new Thread(new ThreadStart(p3.Start));

            this.Closing += new CancelEventHandler(this.Form1_Closing);

            semThread.Start();
            buffThread.Start();
            thread1.Start();
            thread2.Start();
            thread3.Start();
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
            this.Text = "Airport Assignment - Kathryn Lowen 10016148";
            this.Size = new System.Drawing.Size(700, 500);
            this.BackColor = Color.LightGray;
            //this.BackgroundImage =Image.FromFile(@".\stone.gif");

            this.pnl1 = new Panel();
            this.pnl1.Location = new Point(100, 100);
            this.pnl1.Size = new Size(260, 30);
            this.pnl1.BackColor = Color.White;

            this.btn1 = new Button();
            this.btn1.Size = new Size(30, 30);
            this.btn1.BackColor = Color.Pink;
            this.btn1.Location = new System.Drawing.Point(0, 0);

            this.pnl2 = new Panel();
            this.pnl2.Location = new Point(370, 200);
            this.pnl2.Size = new Size(260, 30);
            this.pnl2.BackColor = Color.White;
            //this.pnl2.BackgroundImage =Image.FromFile(@".\stone.gif");

            this.pnl3 = new Panel();
            this.pnl3.Location = new Point(100, 320);
            this.pnl3.Size = new Size(260, 30);
            this.pnl3.BackColor = Color.White;

            this.btn3 = new Button();
            this.btn3.Size = new Size(30, 30);
            this.btn3.BackColor = Color.Pink;
            this.btn3.Location = new System.Drawing.Point(0, 0);

            this.Controls.Add(pnl1);
            this.Controls.Add(pnl2);
            this.Controls.Add(pnl3);
            this.pnl1.Controls.Add(btn1);
            this.pnl3.Controls.Add(btn3);

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
