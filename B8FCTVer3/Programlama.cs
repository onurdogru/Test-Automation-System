using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;

namespace B8FCTVer3
{
    public class ProgramlamaForm
    {
        public ProgAyarForm ProgAyarFrm;
        public Main MainFrm;
        public ProcessForm ProcessFrm;

        /*PROGRAMLAMA + BARKOD DEĞİŞKENLERİ*/
        string[] batchFileFeedback = new string[4];
        public bool isProgrammingStarted = false;

        int versions_number = 0;
        string[] versionsBarcodName = new string[21];
        string[] versionsSlaveName = new string[21];
        int[] stepProgJob = new int[21];
        string computerBatchFileAdress;

        string companyNo;
        string sapNo;
        string productDate;
        string indexNo;
        string productNo;
        string cardType;
        string gerberVer;
        string bomVer;
        string ictRev;
        string fctRev;
        string softwareVer;
        string softwareRev;
        string uniqId;

        string cardStokNum = "";

        public void programlamaInit()
        {
            computerBatchFileAdress = Prog_Ayarlar.Default.Logdosyayolu;

            companyNo = Prog_Ayarlar.Default.companyNo;
            sapNo = Prog_Ayarlar.Default.SAPNo;
            cardType = Prog_Ayarlar.Default.cardNo;
            gerberVer = Prog_Ayarlar.Default.gerberVer;
            bomVer = Prog_Ayarlar.Default.BOMVer;
            ictRev = Prog_Ayarlar.Default.ICTRev;
            fctRev = Prog_Ayarlar.Default.FCTRev;
            softwareVer = Prog_Ayarlar.Default.softwareVer;
            softwareRev = Prog_Ayarlar.Default.softwareRev;

            this.versions_number = Convert.ToInt16(Prog_Ayarlar.Default.barcodeNum);
            this.stepProgJob[1] = Convert.ToInt16(Prog_Ayarlar.Default.step1Job);
            this.stepProgJob[2] = Convert.ToInt16(Prog_Ayarlar.Default.step2Job);
            this.stepProgJob[3] = Convert.ToInt16(Prog_Ayarlar.Default.step3Job);
            this.stepProgJob[4] = Convert.ToInt16(Prog_Ayarlar.Default.step4Job);
            this.stepProgJob[5] = Convert.ToInt16(Prog_Ayarlar.Default.step5Job);
            this.stepProgJob[6] = Convert.ToInt16(Prog_Ayarlar.Default.step6Job);
            this.stepProgJob[7] = Convert.ToInt16(Prog_Ayarlar.Default.step7Job);
            this.stepProgJob[8] = Convert.ToInt16(Prog_Ayarlar.Default.step8Job);
            this.stepProgJob[9] = Convert.ToInt16(Prog_Ayarlar.Default.step9Job);
            this.stepProgJob[10] = Convert.ToInt16(Prog_Ayarlar.Default.step10Job);

            this.versionsBarcodName[1] = Prog_Ayarlar.Default.barcode1;
            this.versionsBarcodName[2] = Prog_Ayarlar.Default.barcode2;
            this.versionsBarcodName[3] = Prog_Ayarlar.Default.barcode3;
            this.versionsBarcodName[4] = Prog_Ayarlar.Default.barcode4;
            this.versionsBarcodName[5] = Prog_Ayarlar.Default.barcode5;
            this.versionsBarcodName[6] = Prog_Ayarlar.Default.barcode6;
            this.versionsBarcodName[7] = Prog_Ayarlar.Default.barcode7;
            this.versionsBarcodName[8] = Prog_Ayarlar.Default.barcode8;
            this.versionsBarcodName[9] = Prog_Ayarlar.Default.barcode9;
            this.versionsBarcodName[10] = Prog_Ayarlar.Default.barcode10;

            this.versionsSlaveName[1] = Prog_Ayarlar.Default.Sbarcode1;
            this.versionsSlaveName[2] = Prog_Ayarlar.Default.Sbarcode2;
            this.versionsSlaveName[3] = Prog_Ayarlar.Default.Sbarcode3;
            this.versionsSlaveName[4] = Prog_Ayarlar.Default.Sbarcode4;
            this.versionsSlaveName[5] = Prog_Ayarlar.Default.Sbarcode5;
            this.versionsSlaveName[6] = Prog_Ayarlar.Default.Sbarcode6;
            this.versionsSlaveName[7] = Prog_Ayarlar.Default.Sbarcode7;
            this.versionsSlaveName[8] = Prog_Ayarlar.Default.Sbarcode8;
            this.versionsSlaveName[9] = Prog_Ayarlar.Default.Sbarcode9;
            this.versionsSlaveName[10] = Prog_Ayarlar.Default.Sbarcode10;

            batchFileFeedback[0] = Prog_Ayarlar.Default.successBatch;
            batchFileFeedback[1] = Prog_Ayarlar.Default.error1Batch;
            batchFileFeedback[2] = Prog_Ayarlar.Default.error2Batch;
            batchFileFeedback[3] = Prog_Ayarlar.Default.error3Batch;
        }

        public void programlamaStart(string barcode)
        {
            Thread programThread = new Thread(ProgramThreadFunction);
            programThread.Start(barcode);
        }

        public void programlamaStop() 
        {
            
        }

        public void ProgramThreadFunction(object data)
        {
            bool result = false;
            int stepState = 1;
            // Clean console
            this.MainFrm.ConsoleClean();

            string barcode = (string)data;
            string company_no = string.Empty;
            string sap_no = string.Empty;
            string product_date = string.Empty;
            string index_no = string.Empty;
            string product_no = string.Empty;
            string card_type = string.Empty;
            string gerber_ver = string.Empty;
            string bom_ver = string.Empty;
            string ict_rev = string.Empty;
            string fct_rev = string.Empty;
            string software_ver = string.Empty;
            string software_rev = string.Empty;
            //string uniq_id = string.Empty;

            // if barcode is true
            if (BarcodeCheck(barcode))
            {
                this.MainFrm.ConsoleAppendLine("Barkod: " + barcode, Color.Black);
                this.MainFrm.ConsoleNewLine();
                this.MainFrm.ConsoleAppendLine("Barkod kriterlere uygun.", Color.Green);
                this.MainFrm.ConsoleNewLine();

                //barcode = "91358531010045201234560000000123456712070101010104";  //Örnek
                company_no = barcode.Substring(0, 2);
                sap_no = barcode.Substring(2, 10);
                cardStokNum = sap_no;
                product_date = barcode.Substring(12, 4);
                index_no = barcode.Substring(16, 6);
                product_no = barcode.Substring(22, 14);
                card_type = barcode.Substring(36, 2);
                gerber_ver = barcode.Substring(38, 2);
                bom_ver = barcode.Substring(40, 2);
                ict_rev = barcode.Substring(42, 2);
                fct_rev = barcode.Substring(44, 2);
                software_ver = barcode.Substring(46, 2);
                software_rev = barcode.Substring(48, 2);
                //uniq_id = barcode.Substring(50, 22);

                this.MainFrm.ConsoleAppendLine("Firma Kodu: " + company_no, Color.Black);
                this.MainFrm.ConsoleAppendLine("SAP Kodu: " + sap_no, Color.Black);
                this.MainFrm.ConsoleAppendLine("Üretim Tarihi: " + product_date, Color.Black);
                this.MainFrm.ConsoleAppendLine("İndex No: " + index_no, Color.Black);
                this.MainFrm.ConsoleAppendLine("Üretici Kodu: " + product_no, Color.Black);
                this.MainFrm.ConsoleAppendLine("Kart Tipi: " + card_type, Color.Black);
                this.MainFrm.ConsoleAppendLine("Gerber Ver: " + gerber_ver, Color.Black);
                this.MainFrm.ConsoleAppendLine("Bom Ver: " + bom_ver, Color.Black);
                this.MainFrm.ConsoleAppendLine("ICT Rev: " + ict_rev, Color.Black);
                this.MainFrm.ConsoleAppendLine("FCT Rev: " + fct_rev, Color.Black);
                this.MainFrm.ConsoleAppendLine("Software Ver: " + software_ver, Color.Black);
                this.MainFrm.ConsoleAppendLine("Software Rev: " + software_rev, Color.Black);
                //this.MainFrm.ConsoleAppendLineCypress("Uniq ID: " + uniq_id, Color.Black);
                this.MainFrm.ConsoleNewLine();

                this.MainFrm.ConsoleAppendLine(sap_no + " kodlu ürünün programlama işlemi yapılıyor...", Color.Blue);

                if (this.MainFrm.cbProgram.Checked == true)
                {
                    if (ProgramProduct(sap_no))
                    {
                        this.MainFrm.ConsoleNewLine();
                        this.MainFrm.ConsoleAppendLine(sap_no + " kodlu ürünün programlama işlemi başarıyla tamamlanmıştır.", Color.Green);
                        this.MainFrm.ConsoleNewLine();
                        result = true;
                    }
                    else
                    {
                        this.MainFrm.ConsoleNewLine();
                        this.MainFrm.ConsoleAppendLine(sap_no + " kodlu ürünün programlama işlemi sırasında bir hata oluşmuştur!", Color.Red);
                        this.MainFrm.ConsoleNewLine();
                        result = false;
                    }
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                if (barcode.Equals(""))
                {
                    this.MainFrm.ConsoleAppendLine("Barkod: " + barcode, Color.Red);
                    this.MainFrm.ConsoleNewLine();
                    this.MainFrm.ConsoleAppendLine("Barkod Boş Bırakılamaz! Programlama yapılamadı!", Color.Red);
                    this.MainFrm.ConsoleNewLine();
                }
                else
                {
                    this.MainFrm.ConsoleAppendLine("Barkod: " + barcode, Color.Red);
                    this.MainFrm.ConsoleNewLine();
                    this.MainFrm.ConsoleAppendLine("Yanlış barkod! Barkod kriterlere uygun değil! Programlama yapılamadı!", Color.Red);
                    this.MainFrm.ConsoleNewLine();
                }
                ProcessFrm.ProcessFailed(stepState); //stepState = 1
                result = false;
            }

            if (result)
            {
                this.MainFrm.tbState.Invoke(new Action(delegate ()
                {
                    this.MainFrm.tbState.BackColor = Color.Green;
                    this.MainFrm.tbState.Text = "PROGRAMLAMA BAŞARILI";
                    stepState = 1;
                    this.MainFrm.ProcessFrm.ProcessSuccess(stepState);
                    stepState = 0;  //Bir Etkisi Yok
                    this.MainFrm.Invoke(new EventHandler(this.MainFrm.btnFCTInit_Click));
                }));
            }
            else
            {
                this.MainFrm.tbState.Invoke(new Action(delegate ()
                {
                    this.MainFrm.tbState.BackColor = Color.Red;
                    this.MainFrm.tbState.Text = "PROGRAMLAMA BAŞARISIZ";
                }));
                ProcessFrm.ProcessFailed(1);
            }

            this.MainFrm.tbBarcodeLast.Invoke(new Action(delegate () { this.MainFrm.tbBarcodeLast.Text = barcode; }));

            this.MainFrm.tbBarcodeCurrent.Invoke(new Action(delegate ()
            {
                this.MainFrm.tbBarcodeCurrent.Text = "";
                // Focus Barcode textbox
                this.MainFrm.tbBarcodeCurrent.Focus();
                // Select all text in textbox
                this.MainFrm.tbBarcodeCurrent.SelectionStart = 0;
                this.MainFrm.tbBarcodeCurrent.SelectionLength = this.MainFrm.tbBarcodeCurrent.Text.Length;
            }));
            isProgrammingStarted = false;
        }

        /* Gelen Barcode Kontrol Edilir*/
        private bool BarcodeCheck(String barcode)
        {
            // Barcode length should be between 26-150.
            if (barcode.Length < 50 || barcode.Length > 72) return false;

            string company_no = barcode.Substring(0, 2);
            string sap_no = barcode.Substring(2, 10);
            //string product_date = barcode.Substring(12, 4);
            //string index_no = barcode.Substring(16, 6);
            //string product_no = barcode.Substring(22, 14);
            string card_type = barcode.Substring(36, 2);
            string gerber_ver = barcode.Substring(38, 2);
            string bom_ver = barcode.Substring(40, 2);
            string ict_rev = barcode.Substring(42, 2);
            string fct_rev = barcode.Substring(44, 2);
            string software_ver = barcode.Substring(46, 2);
            string software_rev = barcode.Substring(48, 2);
            //string uniq_id = barcode.Substring(50, 22);

            // check company no
            if (!company_no.Equals(companyNo))
            {
                this.MainFrm.ConsoleAppendLine("Firma Kodu !", Color.Red);
                return false;
            }
            /*
            // check sap no
            if (ProgAyarFrm.SAPNo.Enabled)
            {
                if (sap_no.IndexOf(sapNo) != 0)
                {
                    this.MainFrm.ConsoleAppendLine("SAP Kodu !", Color.Red);
                    return false;
                }
            }
            */
            // check card type
            if (card_type.IndexOf(cardType) != 0)
            {
                this.MainFrm.ConsoleAppendLine("Kart Tipi !", Color.Red);
                return false;
            }

            // check gerber ver
            if (!gerber_ver.Equals(gerberVer))
            {
                this.MainFrm.ConsoleAppendLine("Gerber Ver !", Color.Red);
                return false;
            }

            // check bom ver
            if (bom_ver.IndexOf(bomVer) != 0)
            {
                this.MainFrm.ConsoleAppendLine("Bom Ver !", Color.Red);
                return false;
            }
            /*
            // check ict rev
            if (ict_rev.IndexOf(ictRev) != 0)
            {
                this.MainFrm.ConsoleAppendLine("ICT Rev  !", Color.Red);
                return false;
            }

            // check fct rev
            if (!fct_rev.Equals(fctRev))
            {
                this.MainFrm.ConsoleAppendLine("FCT Rev !", Color.Red);
                return false;
            }

            // check software ver
            if (software_ver.IndexOf(softwareVer) != 0)
            {
                this.MainFrm.ConsoleAppendLine("Software Ver !", Color.Red);
                return false;
            }

            // check software rev
            if (software_rev.IndexOf(softwareRev) != 0)
            {
                this.MainFrm.ConsoleAppendLine("Software Rev !", Color.Red);
                return false;
            }
            */
            // There is no problem
            return true;
        }

        /*Gelen Barkodu Ayıklanır*/
        private String GetStringBetweenTwoSubStrings(String source, String sub1, String sub2)
        {
            int pFrom = source.IndexOf(sub1) + sub1.Length;
            int pTo = source.LastIndexOf(sub2);
            String result = source.Substring(pFrom, pTo - pFrom);
            return result;
        }

        private bool ProgramProduct(string product_no)
        {
            bool result = false;
            String batchPath = String.Empty;

            for (int i = 1; i <= versions_number; i++)
            {

                if (versionsBarcodName[i] == product_no)
                {
                    if (stepProgJob[versions_number] == 1)
                    {
                        batchPath = computerBatchFileAdress + product_no + ".bat";    // C:\Users\serkan.baki\Desktop\MIND-BATCH-FILES\
                        result = RunBatch(batchPath, this.MainFrm.projectNameTxt.Text);
                        if (result == false) return result;
                    }
                    else if (stepProgJob[versions_number] == 2)
                    {
                        batchPath = computerBatchFileAdress + product_no + ".bat";    // C:\Users\serkan.baki\Desktop\MIND-BATCH-FILES\
                        result = RunBatch(batchPath, this.MainFrm.projectNameTxt.Text);
                        if (result == false) return result;
                        if (CustomMessageBox.ShowMessage("Slave Yüklemesi Yapılması İçin Önce Programlama Kablosunu Takınız ve Evet'e Tıklayınız", this.MainFrm.projectNameTxt.Text, MessageBoxButtons.YesNo, CustomMessageBoxIcon.Question, Color.Yellow) == DialogResult.Yes)
                        {
                            batchPath = computerBatchFileAdress + versionsSlaveName[i] + ".bat";    // C:\Users\serkan.baki\Desktop\MIND-BATCH-FILES\
                            result = RunBatch(batchPath, this.MainFrm.projectNameTxt.Text);
                            if (result == false) return result;
                        }
                    }
                    break;
                }
            }
            return result;
        }

        /*Seçilen .bat Çalıştırılır- Kontrol Edilir ve Kapatılır*/
        private bool RunBatch(string batch_path, string batch_name)
        {
            bool result = false;

            Process processBatch = new Process();
            processBatch.StartInfo.UseShellExecute = false;
            processBatch.StartInfo.RedirectStandardOutput = true;
            processBatch.StartInfo.CreateNoWindow = true;
            processBatch.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processBatch.StartInfo.FileName = batch_path;
            //processBatch.StartInfo.Arguments = string.Format("");
            processBatch.Start();

            StreamReader strmReader = processBatch.StandardOutput;
            string batchTempRow = string.Empty;
            // get all lines of batch
            while ((batchTempRow = strmReader.ReadLine()) != null)
            {
                // Write batch operation to the console
                this.MainFrm.ConsoleAppendLine(batchTempRow, Color.Black);

                // check programming is successful.
                // if succesfully finished.
                if (Prog_Ayarlar.Default.chBoxSuccess && ((batchTempRow.IndexOf("pause", StringComparison.OrdinalIgnoreCase) >= 0) || (batchTempRow.IndexOf(batchFileFeedback[0], StringComparison.OrdinalIgnoreCase) >= 0)))  // color ae
                {
                    this.MainFrm.ConsoleNewLine();
                    this.MainFrm.ConsoleAppendLine(batch_name + " Programlama İşlemi Başarıyla Tamamlanmıştır!", Color.Green);
                    result = true;
                    break;
                }
                else if (Prog_Ayarlar.Default.chBoxError1 && ((batchTempRow.IndexOf("pause", StringComparison.OrdinalIgnoreCase) >= 0) || (batchTempRow.IndexOf(batchFileFeedback[1], StringComparison.OrdinalIgnoreCase) >= 0))) //Could not start CPU core.
                {
                    this.MainFrm.ConsoleNewLine();
                    this.MainFrm.ConsoleAppendLine(batch_name + " Programlama İşlemi Başarısız1.", Color.Red);  // Programlama Soketi Düzgün Takılı Değil!
                    this.MainFrm.All_FCT_Fail();
                    result = false;
                    break;
                }
                else if (Prog_Ayarlar.Default.chBoxError2 && ((batchTempRow.IndexOf("pause", StringComparison.OrdinalIgnoreCase) >= 0) || (batchTempRow.IndexOf(batchFileFeedback[2], StringComparison.OrdinalIgnoreCase) >= 0)))  // Cannot connect to target.
                {
                    this.MainFrm.ConsoleNewLine();
                    this.MainFrm.ConsoleAppendLine(batch_name + " Programlama İşlemi Başarısız2.", Color.Red); // Programlama Soketi Takılı Değil!
                    this.MainFrm.All_FCT_Fail();
                    result = false;
                    break;
                }
                else if (Prog_Ayarlar.Default.chBoxError3 && ((batchTempRow.IndexOf("pause", StringComparison.OrdinalIgnoreCase) >= 0) || (batchTempRow.IndexOf(batchFileFeedback[3], StringComparison.OrdinalIgnoreCase) >= 0))) //FAILED
                {
                    this.MainFrm.ConsoleNewLine();
                    this.MainFrm.ConsoleAppendLine(batch_name + " Programlama İşlemi Başarısız3.", Color.Red);  //  USB Takılı Değil!
                    this.MainFrm.All_FCT_Fail();
                    result = false;
                    break;
                }
            }

            // if batch didn't closed kill it.
            if (!processBatch.HasExited)
            {
                processBatch.Kill();
            }

            return result;
        }
    }
}
