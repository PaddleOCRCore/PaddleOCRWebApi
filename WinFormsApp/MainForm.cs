using Newtonsoft.Json;
using PaddleOCRCore;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WinFormsApp.Services;

namespace WinFormsApp
{
    public partial class MainForm : Form
    {
        StringBuilder message = new StringBuilder();
        PaddleOCREngine ocrEngine;
        PaddleStructureEngine structureEngine;
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonInit_Click(object sender, EventArgs e)
        {
            try
            {
                IOCREngine engine = new OCREngine();
                ocrEngine = engine.GetOCREngine();
                structureEngine = engine.GetStructureEngine();
                message.Append("��ʼ���ɹ���\r\n");
                textBoxResult.Text = message.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string result = "";
                string filePath = Path.Combine(AppContext.BaseDirectory, "inference", "1231.jpeg");
                //message.Append(filePath);
                textBoxResult.Text = message.ToString();
                OCRResult ocrResult = ocrEngine.DetectText(filePath);
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in ocrResult.TextBlocks)
                {
                    //if (stringBuilder.Length > 0)
                    //{
                    //    stringBuilder.Append(Environment.NewLine);
                    //}
                    stringBuilder.Append(item.Text);
                }
                result = stringBuilder.ToString();
                string id_card_side = "front";
                if (id_card_side.Equals("front"))
                {
                    var jsonResult = new
                    {
                        ���� = "",
                        �Ա� = "",
                        ���� = "",
                        ���� = "",
                        סַ = "",
                        ������ݺ��� = "",
                        text = ""
                    };
                    // ����������ʽ
                    Regex regex = new Regex(@"����(?<name>[^\s]+)�Ա�(?<gender>[��Ů])����(?<nation>.+?)����(?<birth>.+?)סַ(?<address>.+?)�������(?<zheng>.+?)����(?<id>\d{18})");
                    // ִ��ƥ��
                    Match match = regex.Match(result);
                    if (match.Success)
                    {
                        // ��ȡ��Ϣ
                        string name = match.Groups["name"].Value;
                        string gender = match.Groups["gender"].Value;
                        string nation = match.Groups["nation"].Value;
                        string birth = match.Groups["birth"].Value;
                        string address = match.Groups["address"].Value;
                        string idNumber = match.Groups["id"].Value;
                        // ����JSON����
                        jsonResult = new
                        {
                            ���� = name,
                            �Ա� = gender,
                            ���� = nation,
                            ���� = birth,
                            סַ = address,
                            ������ݺ��� = idNumber,
                            text = result
                        };
                        result = JsonConvert.SerializeObject(jsonResult, Formatting.Indented);
                    }
                    else
                    {
                        message.Append("������ʽƥ��ʧ��\r\n");
                    }
                }
                else if(id_card_side.Equals("back"))
                {
                    var jsonResult = new
                    {
                        ǩ������ = "",
                        ��Ч���� = "",
                        text = ""
                    };
                    // ����������ʽ
                    Regex regex = new Regex(@"ǩ������(?<issuingAuthority>.+?)��Ч����(?<validityPeriod>.+)$");

                    // ִ��ƥ��
                    Match match = regex.Match(result);
                    if (match.Success)
                    {
                        // ��ȡ��Ϣ
                        string issuingAuthority = match.Groups["issuingAuthority"].Value;
                        string validityPeriod = match.Groups["validityPeriod"].Value;
                        // ����JSON����
                        jsonResult = new
                        {
                            ǩ������ = issuingAuthority,
                            ��Ч���� = validityPeriod,
                            text = result
                        };
                        result = JsonConvert.SerializeObject(jsonResult, Formatting.Indented);
                    }
                    else
                    {
                        message.Append("������ʽƥ��ʧ��\r\n");
                    }
                }
                message.Append(result);
                //message.Insert(0, $"ʶ����{ocrResult.TextBlocks.Count()}�У�\r\n");
                textBoxResult.Text = message.ToString();
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                textBoxResult.Text = message.ToString();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                textBoxResult.Text = message.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, "inference", "123.png");
            string base64 = GetBase64FromImage(filePath);
            textBoxResult.Text = base64;
        }
        #region ͼƬ·��תBase64
        public static string GetBase64FromImage(string strPath)
        {
            string strbaser64 = "";
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(strPath, FileMode.Open)))
                {
                    FileInfo fi = new FileInfo(strPath);
                    byte[] bytes = reader.ReadBytes((int)fi.Length);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        strbaser64 = Convert.ToBase64String(bytes);
                        return strbaser64;
                    }
                }
            }
            catch (Exception)
            {
                return strbaser64;
            }
        }
        #endregion
    }
}
