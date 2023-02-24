using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ZXing;

namespace PokémonQRCodeReader
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        Bitmap bm;
        Graphics g;
        BarcodeReader barcodeReader;
        Result result;
        string lastCode;
        string lastCode1;
        string lastCode2;
        string lastCode3;
        bool stop;
        int count;
        bool stopjogar;
        bool click1;
        bool click2;

        public Form1()
        {
            InitializeComponent();
            button1.Enabled = true;
            button3.Enabled = true;
            click1 = false;
            click2 = false;
        }

        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00000000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010,
        }

        void AutoDo(Bitmap bm, Graphics g, Result result)
        {
            SetCursorPos(400, 1050);    //abre o PokemonTCG
            Thread.Sleep(2);
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

            Clipboard.SetText(result.ToString());
            Thread.Sleep(2);    //insere o codigo
            SendKeys.SendWait("^v");

            Thread.Sleep(30);  //clica para enviar o codigo para a lista
            SetCursorPos(530, 980);
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

            Thread.Sleep(550);  //clica para enviar o codigo para o servidor
            SetCursorPos(1190, 985);
            Thread.Sleep(5);
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

            Thread.Sleep(400);  //coloca o mouse no fechar e captura a tela
            SetCursorPos(824, 985);
            Thread.Sleep(800);
            g.CopyFromScreen(0, 0, 0, 0, bm.Size);
            Thread.Sleep(800);
            Color p = bm.GetPixel(1550, 80);
            Thread.Sleep(50);
            if (p.A == 255 && p.R == 241 && p.G == 108 && p.B == 108)
            {
                count++;
                label1.Text = count.ToString();
            }
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);   //fecha a tela
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

            Thread.Sleep(300);  //fecha o caso do codigo ja ter sido enviado
            SetCursorPos(958, 620);
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

            Thread.Sleep(500);   //clica para inserir o codigo
            SetCursorPos(530, 890);
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

            Thread.Sleep(900);  //volta para o navegador
            SendKeys.SendWait("%{TAB}");

            Thread.Sleep(900);  //coloca o mouse no canto
            SetCursorPos(0, 0);

            if (checkBox1.Checked)
            {
                Thread.Sleep(100);
                SendKeys.SendWait("{F5}");

                Thread.Sleep(2000);
                SendKeys.SendWait("%t");
            }

            return;
        }

        void Capture()
        {
            bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            g = Graphics.FromImage(bm);
            barcodeReader = new BarcodeReader();
            pictureBox1.Image = Properties.Resources.rec;

            while (true)
            {
                Thread.Sleep(5);
                try
                {
                    g.CopyFromScreen(0, 0, 0, 0, bm.Size);
                    result = barcodeReader.Decode(bm);
                }
                catch(Win32Exception e)
                {
                    result = null;
                    Thread.Sleep(5);
                }

                if (stop)
                {
                    pictureBox1.Image = null;
                    return;
                }
                if (result != null && result.ToString()[3] == '-' && result.ToString() != lastCode && result.ToString() != lastCode1 && result.ToString() != lastCode2 && result.ToString() != lastCode3)
                {
                    AutoDo(bm, g, result);

                    lastCode3 = lastCode2;
                    lastCode2 = lastCode1;
                    lastCode1 = lastCode;
                    lastCode = result.ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!click1)
            {
                button3.Enabled = false;

                lastCode = "";
                lastCode1 = "";
                lastCode2 = "";
                lastCode3 = "";

                stop = false;

                count = 0;

                label1.Text = "0";

                Thread t = new Thread(Capture);
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                button1.Text = "Stop";

                click1 = true;
            }
            else
            {
                button3.Enabled = true;

                stop = true;

                button1.Text = "Start";

                click1 = false;
            }
        }

        private void Jogar()
        {
            bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            g = Graphics.FromImage(bm);
            Color pLogin = Color.Empty;
            Color pChallenge = Color.Empty;
            Color pVersus = Color.Empty;
            Color pCoin = Color.Empty;
            Color pGame = Color.Empty;
            Color pEndGame = Color.Empty;
            Color pCard1 = Color.Empty;
            Color pCard2 = Color.Empty;
            Color pOponentTurn = Color.Empty;

            if (checkBox2.Checked)
            {
                SetCursorPos(400, 1050);    //abre o PokemonTCG
                mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
            }

            while (true)
            {
                Thread.Sleep(5);
                try
                {
                    g.CopyFromScreen(0, 0, 0, 0, bm.Size);
                    pLogin = bm.GetPixel(960, 630);
                    pChallenge = bm.GetPixel(940, 940);
                    pVersus = bm.GetPixel(330, 670);
                    pCoin = bm.GetPixel(1140, 890);
                    pGame = bm.GetPixel(1700, 600);
                    pEndGame = bm.GetPixel(20, 1000);
                    pCard1 = bm.GetPixel(970, 600);
                    pCard2 = bm.GetPixel(970, 340);
                    pOponentTurn = bm.GetPixel(90, 177);
                }
                catch (Win32Exception e)
                {
                    Thread.Sleep(5);
                }

                if (pLogin.A == 255 && pLogin.R == 195 && pLogin.G == 162 && pLogin.B == 41)
                {
                    SetCursorPos(960, 600);    //clica em Log In
                    mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                    mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
                }
                else if (pChallenge.A == 255 && pChallenge.R == 121 && pChallenge.G == 211 && pChallenge.B == 113)
                {
                    Thread.Sleep(300);
                    SetCursorPos(960, 950);    //fecha a janela de Challenges
                    mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                    mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

                    Thread.Sleep(300);  //clica no primeiro icone >
                    SetCursorPos(350, 30);
                    mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                    mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

                    Thread.Sleep(300);  //clica em versus
                    SetCursorPos(440, 270);
                    mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                    mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
                }
                else if (pVersus.A == 255 && pVersus.R == 255 && pVersus.G == 255 && pVersus.B == 189)
                {
                    SetCursorPos(960, 670); //clica em Play
                    mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                    mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
                }
                else if (pCoin.A == 255 && pCoin.R == 58 && pCoin.G == 166 && pCoin.B == 53)
                {
                    Thread.Sleep(25000);    //esperar ateh 3seg
                    SetCursorPos(1100, 890);    //clica em coroa ou sim
                    mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                    mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

                    Thread.Sleep(30);   //esperar clicar
                    SetCursorPos(0, 0); //tira de cima do botao
                }
                else if (pGame.A == 255 && pGame.R == 131 && pGame.G == 99 && pGame.B == 72 &&
                        (pCard1.A != 255 || pCard1.R != 248 || pCard1.G != 248 || pCard1.B != 248) &&
                        (pCard2.A != 255 || pCard2.R != 239 || pCard2.G != 20 || pCard2.B != 16) &&
                        pOponentTurn.A == 255 && pOponentTurn.R == 255 && pOponentTurn.G == 230 && pOponentTurn.B == 88)
                {
                    Thread.Sleep(4000); //espera o bot se render

                    Thread.Sleep(200);
                    SetCursorPos(1788, 30); //clica nas configuracoes
                    mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                    mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

                    Thread.Sleep(200);
                    SetCursorPos(1060, 760);    //clica em conceder
                    mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                    mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

                    Thread.Sleep(200);
                    SetCursorPos(1070, 620);    //clica em yes
                    mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                    mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
                }
                else if (pEndGame.A == 255 && pEndGame.R == 23 && pEndGame.G == 80 && pEndGame.B == 133)
                {
                    SetCursorPos(960, 1000);    //clica em done
                    Thread.Sleep(30);
                    mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                    mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
                    Thread.Sleep(8000);    //espera a animacao
                }
                else if (stopjogar)
                {
                    break;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!click2)
            {
                button1.Enabled = false;

                stopjogar = false;

                Thread t = new Thread(Jogar);
                t.Start();

                button3.Text = "End Game";

                click2 = true;
            }
            else
            {
                button1.Enabled = true;
                
                stopjogar = true;

                button3.Text = "Begin Game";

                click2 = false;
            }

            //SetCursorPos(970, 600);
            
            /*
            Thread.Sleep(2000);
            bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            g = Graphics.FromImage(bm);
            g.CopyFromScreen(0, 0, 0, 0, bm.Size);
            Color p;
            p = bm.GetPixel(970, 600);
            MessageBox.Show(p.ToString());*/
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetCursorPos(400, 1050);    //abre o PokemonTCG
                mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

                Thread.Sleep(60);   //clica para inserir o codigo
                SetCursorPos(530, 890);
                mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

                Clipboard.SetText(textBox1.Text);
                Thread.Sleep(5);    //insere o codigo
                SendKeys.SendWait("^v");

                Thread.Sleep(100);  //clica para enviar o codigo para a lista
                SetCursorPos(530, 980);
                mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

                Thread.Sleep(550);  //clica para enviar o codigo para o servidor
                SetCursorPos(1190, 985);
                Thread.Sleep(5);
                mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

                Thread.Sleep(400);  //coloca o mouse no fechar e captura a tela
                SetCursorPos(824, 985);
                Thread.Sleep(800);
                mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);   //fecha a tela
                mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

                Thread.Sleep(300);  //fecha o caso do codigo ja ter sido enviado
                SetCursorPos(958, 620);
                mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

                Thread.Sleep(500);   //clica para inserir o codigo
                SetCursorPos(530, 890);
                mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);

                Thread.Sleep(900);  //volta para o navegador
                SendKeys.SendWait("%{TAB}");

                Thread.Sleep(900);  //coloca o mouse no canto
                SetCursorPos(0, 0);

                if (checkBox1.Checked)
                {
                    Thread.Sleep(100);
                    SendKeys.SendWait("{F5}");

                    Thread.Sleep(2000);
                    SendKeys.SendWait("%t");
                }

                textBox1.Text = "";
            }

            return;
        }
    }
}   