using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApplication6
{
    public partial class Form1 : Form
    {   //WIM FRANKENA
        //BIM/ITSM 2
        //306355
        //FRAN@STUDENT.NHL.NL
        //23-02-2016

        //PUBLIC VARIABLES
        Image photo_X = Properties.Resources.PhotoX;
        Image photo_O = Properties.Resources.PhotoO;
        PictureBox[,] arrayPicturebox = new PictureBox[3, 3];
        Random rnd = new Random();
        int intTeller = 0;
        bool spel = true;
        OleDbConnection cn = new OleDbConnection();
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataReader dr;

        public Form1()
        { InitializeComponent(); }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Deze void zorgt ervoor dat het speelveld gegenereert word tijdens het laden/openen van de applicatie.
            //Ook maakt het connectie met de database die in de resources toegevoegt is.
            MaakSpeelVeld();
            cn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=DatabaseTTT.accdb;
Persist Security Info=False;";
            cmd.Connection = cn;
        }

        private void MaakSpeelVeld()
        {
            //Aanmaken speelveld
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    PictureBox picture = new PictureBox
                    {
                        Name = "pictureBox" + i + "_" + j,
                        Size = new Size(156, 156),
                        Location = new Point(12 + (i * 153), 12 + (j * 153)),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.White,
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    picture.Click += new EventHandler(SpelerZet);
                    arrayPicturebox[i, j] = picture;
                    this.Controls.Add(picture);
                }
            }
        }
        private void ResetSpel()
        {
            //Deze method reset het spel.
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    arrayPicturebox[i, j].Image = null;
                }
            }
        }
        private void SpelStop()
        {
            //Deze method stopt het spel wanneer er gewonnen, verloren of gelijkspel is.
            spel = false;
            intTeller = intTeller - 1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    arrayPicturebox[i, j].Enabled = false;
                }
            }
        }
        public void SpelerZet(object sender, EventArgs e)
        {
            //Deze method voert de speler zet/beurt uit.
            //Hij plaatst een plaatje met X in de geklikte picturebox.
            //Vervolgens roept hij de winstcheckspeler() method aan om te kijken of je gewonnen heb.
            //En dan roept hij de RandomZet() methode aan om de zet van de computer te doen.

            PictureBox dezePictureBox = (PictureBox)sender;
            if (dezePictureBox.Image == null)
            {
                dezePictureBox.Image = photo_X;
                spel = true;
                intTeller = intTeller + 1;
                winstcheckspeler();
                RandomZet();
            }
        }
        public void winstcheckspeler()
        {
            // ** DIT IS VOOR DE SPELER **
            // Als je drie op een rij hebt, win je.
            // Dit kan horizontaal/verticaal/diagonaal
            // Als je vijf zetten gedaan hebt is het gelijkspel.
            // Als de speler gewonnen heeft wordt er een 'WIN' toegevoegd in de database.
            // Als er gelijkspel is wordt er een 'DRAW' toegevoegd in de database.
            string q = "INSERT INTO Boter_Kaas_Eieren (WIN,LOSE,DRAW) VALUES (1,0,0)";

            //HORIZONTAL CHECK
            if (arrayPicturebox[0, 0].Image == photo_X && arrayPicturebox[1, 0].Image == photo_X && arrayPicturebox[2, 0].Image == photo_X)
            {
                MessageBox.Show("U heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            if (arrayPicturebox[0, 1].Image == photo_X && arrayPicturebox[1, 1].Image == photo_X && arrayPicturebox[2, 1].Image == photo_X)
            {
                MessageBox.Show("U heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            if (arrayPicturebox[0, 2].Image == photo_X && arrayPicturebox[1, 2].Image == photo_X && arrayPicturebox[2, 2].Image == photo_X)
            {
                MessageBox.Show("U heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            //VERTICAL CHECK
            if (arrayPicturebox[0, 0].Image == photo_X && arrayPicturebox[0, 1].Image == photo_X && arrayPicturebox[0, 2].Image == photo_X)
            {
                MessageBox.Show("U heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            if (arrayPicturebox[1, 0].Image == photo_X && arrayPicturebox[1, 1].Image == photo_X && arrayPicturebox[1, 2].Image == photo_X)
            {
                MessageBox.Show("U heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            if (arrayPicturebox[2, 0].Image == photo_X && arrayPicturebox[2, 1].Image == photo_X && arrayPicturebox[2, 2].Image == photo_X)
            {
                MessageBox.Show("U heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            //DIAGONAL CHECK 1 (Linksboven naar rechtsonder)
            if (arrayPicturebox[0, 0].Image == photo_X && arrayPicturebox[1, 1].Image == photo_X && arrayPicturebox[2, 2].Image == photo_X)
            {
                MessageBox.Show("U heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            //DIAGONAL CHECK 2 (Linksonder naar rechtsboven)
            if (arrayPicturebox[2, 0].Image == photo_X && arrayPicturebox[1, 1].Image == photo_X && arrayPicturebox[0, 2].Image == photo_X)
            {
                MessageBox.Show("U heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            //gelijkspel teller
            if (intTeller == 5)
            {
                MessageBox.Show("Gelijkspel!");
                string p = "INSERT INTO Boter_Kaas_Eieren (WIN,LOSE,DRAW) VALUES (0,0,1)";
                dosomething2(p);
                SpelStop();
            }
        }
        public void winstcheckcomputer()
        {
            // ** DIT IS VOOR DE COMPUTER **
            // Als de computer drie op een rij heeft, verlies je.
            // Dit kan horizontaal/verticaal/diagonaal
            // Als de computer gewonnen heeft wordt er een 'LOSE' toegevoegd in de database.

            string q = "INSERT INTO Boter_Kaas_Eieren (WIN,LOSE,DRAW) VALUES (0,1,0)";

            //HORIZONTAL CHECK
            if (arrayPicturebox[0, 0].Image == photo_O && arrayPicturebox[1, 0].Image == photo_O && arrayPicturebox[2, 0].Image == photo_O)
            {
                MessageBox.Show("De computer heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            if (arrayPicturebox[0, 1].Image == photo_O && arrayPicturebox[1, 1].Image == photo_O && arrayPicturebox[2, 1].Image == photo_O)
            {
                MessageBox.Show("De computer heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            if (arrayPicturebox[0, 2].Image == photo_O && arrayPicturebox[1, 2].Image == photo_O && arrayPicturebox[2, 2].Image == photo_O)
            {
                MessageBox.Show("De computer heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            //VERTICAL CHECK
            if (arrayPicturebox[0, 0].Image == photo_O && arrayPicturebox[0, 1].Image == photo_O && arrayPicturebox[0, 2].Image == photo_O)
            {
                MessageBox.Show("De computer heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            if (arrayPicturebox[1, 0].Image == photo_O && arrayPicturebox[1, 1].Image == photo_O && arrayPicturebox[1, 2].Image == photo_O)
            {
                MessageBox.Show("De computer heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            if (arrayPicturebox[2, 0].Image == photo_O && arrayPicturebox[2, 1].Image == photo_O && arrayPicturebox[2, 2].Image == photo_O)
            {
                MessageBox.Show("De computer heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            //DIAGONAL CHECK 1 (Linksboven naar rechtsonder)
            if (arrayPicturebox[0, 0].Image == photo_O && arrayPicturebox[1, 1].Image == photo_O && arrayPicturebox[2, 2].Image == photo_O)
            {
                MessageBox.Show("De computer heeft gewonnen!");
                dosomething(q);
                SpelStop();
            }
            //DIAGONAL CHECK 2 (Linksonder naar rechtsboven)
            if (arrayPicturebox[2, 0].Image == photo_O && arrayPicturebox[1, 1].Image == photo_O && arrayPicturebox[0, 2].Image == photo_O)
            {
                MessageBox.Show("De computer heeft gewonnen!");
                                dosomething(q);
                SpelStop();
            }
        }
        private void RandomZet()
        {
            //Deze method zorgt ervoor dat de computer automatisch een zet doet nadat de speler dat gedaan heeft.
            //Hij checkt hierbij of het random vakje leeg is, is dit niet zo dan zoekt hij een ander.
            try
            {
                while (spel == true)
                {
                    int computerZet = rnd.Next(1, 10);

                    if (computerZet == 1 && arrayPicturebox[0, 0].Image == null)
                    {
                        arrayPicturebox[0, 0].Image = photo_O;
                        winstcheckcomputer();
                        spel = false;
                        return;
                    }
                    else if (computerZet == 2 && arrayPicturebox[1, 0].Image == null)
                    {
                        arrayPicturebox[1, 0].Image = photo_O;
                        winstcheckcomputer();
                        spel = false;

                        return;
                    }
                    else if (computerZet == 3 && arrayPicturebox[2, 0].Image == null)
                    {
                        arrayPicturebox[2, 0].Image = photo_O;
                        winstcheckcomputer();
                        spel = false;

                        return;
                    }
                    else if (computerZet == 4 && arrayPicturebox[0, 1].Image == null)
                    {
                        arrayPicturebox[0, 1].Image = photo_O;
                        winstcheckcomputer();
                        spel = false;

                        return;
                    }
                    else if (computerZet == 5 && arrayPicturebox[1, 1].Image == null)
                    {
                        arrayPicturebox[1, 1].Image = photo_O;
                        winstcheckcomputer();
                        spel = false;

                        return;
                    }
                    else if (computerZet == 6 && arrayPicturebox[2, 1].Image == null)
                    {
                        arrayPicturebox[2, 1].Image = photo_O;
                        winstcheckcomputer();
                        spel = false;

                        return;
                    }
                    else if (computerZet == 7 && arrayPicturebox[0, 2].Image == null)
                    {
                        arrayPicturebox[0, 2].Image = photo_O;
                        winstcheckcomputer();
                        spel = false;

                        return;
                    }
                    else if (computerZet == 8 && arrayPicturebox[1, 2].Image == null)
                    {
                        arrayPicturebox[1, 2].Image = photo_O;
                        winstcheckcomputer();
                        spel = false;

                        return;
                    }
                    else if (computerZet == 9 && arrayPicturebox[2, 2].Image == null)
                    {
                        arrayPicturebox[2, 2].Image = photo_O;
                        winstcheckcomputer();
                        spel = false;

                        return;
                    }
                    else
                    {
                        spel = true;
                    }
                }
            }
            catch
            {
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //New game knop.
            //Reset de pictureboxes en zet het aantal beurten op 0.
            ResetSpel();
            intTeller = 0;
            spel = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    arrayPicturebox[i, j].Enabled = true;
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //EXIT knop.
            //Sluit de applicatie.
            this.Close();
        }

        private void dosomething(String q)
        {
            //Deze method zorgt ervoor dat SQL commandos uitgevoert worden.
            try
            {
                cn.Open();
                cmd.CommandText = q;
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception e)
            {
                cn.Close();
                MessageBox.Show(e.Message.ToString());
            }
        }
        private void dosomething2(String p)
        {
            //Deze method zorgt ervoor dat SQL commandos uitgevoert worden.
            try
            {
                cn.Open();
                cmd.CommandText = p;
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception e)
            {
                cn.Close();
                MessageBox.Show(e.Message.ToString());
            }
        }
        private void loaddata()
        {
            //Deze method leest de database(de snapshot) en zorgt ervoor dat het veld herladen wordt.
            try
            {
                string q = "select * from Speelveld";
                cmd.CommandText = q;
                cn.Open();
                dr = cmd.ExecuteReader();
                List<int> list1 = new List<int>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        list1.Add(dr.GetInt32(0));
                        list1.Add(dr.GetInt32(1));
                        list1.Add(dr.GetInt32(2));
                        list1.Add(dr.GetInt32(3));
                        list1.Add(dr.GetInt32(4));
                        list1.Add(dr.GetInt32(5));
                        list1.Add(dr.GetInt32(6));
                        list1.Add(dr.GetInt32(7));
                        list1.Add(dr.GetInt32(8));
                        int VakPB1 = list1[0];
                        int VakPB2 = list1[1];
                        int VakPB3 = list1[2];
                        int VakPB4 = list1[3];
                        int VakPB5 = list1[4];
                        int VakPB6 = list1[5];
                        int VakPB7 = list1[6];
                        int VakPB8 = list1[7];
                        int VakPB9 = list1[8];

                        if (VakPB1.Equals(1))
                        {
                            arrayPicturebox[0, 0].Image = photo_X;
                        }
                        if (VakPB1.Equals(2))
                        {
                            arrayPicturebox[0, 0].Image = photo_O;
                        }
                        if (VakPB1.Equals(0))
                        {
                            arrayPicturebox[0, 0].Image = null;
                        }

                        if (VakPB2.Equals(1))
                        {
                            arrayPicturebox[0, 1].Image = photo_X;
                        }
                        if (VakPB2.Equals(2))
                        {
                            arrayPicturebox[0, 1].Image = photo_O;
                        }
                        if (VakPB2.Equals(0))
                        {
                            arrayPicturebox[0, 1].Image = null;
                        }

                        if (VakPB3.Equals(1))
                        {
                            arrayPicturebox[0, 2].Image = photo_X;
                        }
                        if (VakPB3.Equals(2))
                        {
                            arrayPicturebox[0, 2].Image = photo_O;
                        }
                        if (VakPB3.Equals(0))
                        {
                            arrayPicturebox[0, 2].Image = null;
                        }

                        if (VakPB4.Equals(1))
                        {
                            arrayPicturebox[1, 0].Image = photo_X;
                        }
                        if (VakPB4.Equals(2))
                        {
                            arrayPicturebox[1, 0].Image = photo_O;
                        }
                        if (VakPB4.Equals(0))
                        {
                            arrayPicturebox[1, 0].Image = null;
                        }

                        if (VakPB5.Equals(1))
                        {
                            arrayPicturebox[1, 1].Image = photo_X;
                        }
                        if (VakPB5.Equals(2))
                        {
                            arrayPicturebox[1, 1].Image = photo_O;
                        }
                        if (VakPB5.Equals(0))
                        {
                            arrayPicturebox[1, 1].Image = null;
                        }

                        if (VakPB6.Equals(1))
                        {
                            arrayPicturebox[1, 2].Image = photo_X;
                        }
                        if (VakPB6.Equals(2))
                        {
                            arrayPicturebox[1, 2].Image = photo_O;
                        }
                        if (VakPB6.Equals(0))
                        {
                            arrayPicturebox[1, 2].Image = null;
                        }

                        if (VakPB7.Equals(1))
                        {
                            arrayPicturebox[2, 0].Image = photo_X;
                        }
                        if (VakPB7.Equals(2))
                        {
                            arrayPicturebox[2, 0].Image = photo_O;
                        }
                        if (VakPB7.Equals(0))
                        {
                            arrayPicturebox[2, 0].Image = null;
                        }

                        if (VakPB8.Equals(1))
                        {
                            arrayPicturebox[2, 1].Image = photo_X;
                        }
                        if (VakPB8.Equals(2))
                        {
                            arrayPicturebox[2, 1].Image = photo_O;
                        }
                        if (VakPB8.Equals(0))
                        {
                            arrayPicturebox[2, 1].Image = null;
                        }

                        if (VakPB9.Equals(1))
                        {
                            arrayPicturebox[2, 2].Image = photo_X;
                        }
                        if (VakPB9.Equals(2))
                        {
                            arrayPicturebox[2, 2].Image = photo_O;
                        }
                        if (VakPB9.Equals(0))
                        {
                            arrayPicturebox[2, 2].Image = null;
                        }
                    }

                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message.ToString());
            }

        }
        private void snapshot()
        {
            //Deze method maakt de snapshot van het spel, en zet deze vervolgens in de database.
            //Hij zet hiervoor de arrayPicturebox om in 0 (leeg), 1 (X) of 2 (O).
            try
            {
                if (arrayPicturebox[0, 0].Image == photo_X)
                {
                    string q = "UPDATE Speelveld SET Pb_1 = 1";
                    dosomething(q);
                }
                if (arrayPicturebox[0, 0].Image == photo_O)
                {
                    string q = "UPDATE Speelveld SET Pb_1 = 2";
                    dosomething(q);
                }
                if (arrayPicturebox[0, 0].Image == null)
                {
                    string q = "UPDATE Speelveld SET Pb_1 = 0";
                    dosomething(q);
                }
                if (arrayPicturebox[0, 1].Image == photo_X)
                {
                    string q = "UPDATE Speelveld SET Pb_2 = 1";
                    dosomething(q);
                }
                if (arrayPicturebox[0, 1].Image == photo_O)
                {
                    string q = "UPDATE Speelveld SET Pb_2 = 2";
                    dosomething(q);
                }
                if (arrayPicturebox[0, 1].Image == null)
                {
                    string q = "UPDATE Speelveld SET Pb_2 = 0";
                    dosomething(q);
                }
                if (arrayPicturebox[0, 2].Image == photo_X)
                {
                    string q = "UPDATE Speelveld SET Pb_3 = 1";
                    dosomething(q);
                }
                if (arrayPicturebox[0, 2].Image == photo_O)
                {
                    string q = "UPDATE Speelveld SET Pb_3 = 2";
                    dosomething(q);
                }
                if (arrayPicturebox[0, 2].Image == null)
                {
                    string q = "UPDATE Speelveld SET Pb_3 = 0";
                    dosomething(q);
                }
                if (arrayPicturebox[1, 0].Image == photo_X)
                {
                    string q = "UPDATE Speelveld SET Pb_4 = 1";
                    dosomething(q);
                }
                if (arrayPicturebox[1, 0].Image == photo_O)
                {
                    string q = "UPDATE Speelveld SET Pb_4 = 2";
                    dosomething(q);
                }
                if (arrayPicturebox[1, 0].Image == null)
                {
                    string q = "UPDATE Speelveld SET Pb_4 = 0";
                    dosomething(q);
                }
                if (arrayPicturebox[1, 1].Image == photo_X)
                {
                    string q = "UPDATE Speelveld SET Pb_5 = 1";
                    dosomething(q);
                }
                if (arrayPicturebox[1, 1].Image == photo_O)
                {
                    string q = "UPDATE Speelveld SET Pb_5 = 2";
                    dosomething(q);
                }
                if (arrayPicturebox[1, 1].Image == null)
                {
                    string q = "UPDATE Speelveld SET Pb_5 = 0";
                    dosomething(q);
                }
                if (arrayPicturebox[1, 2].Image == photo_X)
                {
                    string q = "UPDATE Speelveld SET Pb_6 = 1";
                    dosomething(q);
                }
                if (arrayPicturebox[1, 2].Image == photo_O)
                {
                    string q = "UPDATE Speelveld SET Pb_6 = 2";
                    dosomething(q);
                }
                if (arrayPicturebox[1, 2].Image == null)
                {
                    string q = "UPDATE Speelveld SET Pb_6 = 0";
                    dosomething(q);
                }
                if (arrayPicturebox[2, 0].Image == photo_X)
                {
                    string q = "UPDATE Speelveld SET Pb_7 = 1";
                    dosomething(q);
                }
                if (arrayPicturebox[2, 0].Image == photo_O)
                {
                    string q = "UPDATE Speelveld SET Pb_7 = 2";
                    dosomething(q);
                }
                if (arrayPicturebox[2, 0].Image == null)
                {
                    string q = "UPDATE Speelveld SET Pb_7 = 0";
                    dosomething(q);
                }
                if (arrayPicturebox[2, 1].Image == photo_X)
                {
                    string q = "UPDATE Speelveld SET Pb_8 = 1";
                    dosomething(q);
                }
                if (arrayPicturebox[2, 1].Image == photo_O)
                {
                    string q = "UPDATE Speelveld SET Pb_8 = 2";
                    dosomething(q);
                }
                if (arrayPicturebox[2, 1].Image == null)
                {
                    string q = "UPDATE Speelveld SET Pb_8 = 0";
                    dosomething(q);
                }
                if (arrayPicturebox[2, 2].Image == photo_X)
                {
                    string q = "UPDATE Speelveld SET Pb_9 = 1";
                    dosomething(q);
                }
                if (arrayPicturebox[2, 2].Image == photo_O)
                {
                    string q = "UPDATE Speelveld SET Pb_9 = 2";
                    dosomething(q);
                }
                if (arrayPicturebox[2, 2].Image == null)
                {
                    string q = "UPDATE Speelveld SET Pb_9 = 0";
                    dosomething(q);
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Snapshot knop
            snapshot();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Load game knop
            loaddata();
        }
    }
}

