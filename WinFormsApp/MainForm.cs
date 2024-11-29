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
                message.Append("初始化成功！\r\n");
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
                        姓名 = "",
                        性别 = "",
                        民族 = "",
                        出生 = "",
                        住址 = "",
                        公民身份号码 = "",
                        text = ""
                    };
                    // 定义正则表达式
                    Regex regex = new Regex(@"姓名(?<name>[^\s]+)性别(?<gender>[男女])民族(?<nation>.+?)出生(?<birth>.+?)住址(?<address>.+?)公民身份(?<zheng>.+?)号码(?<id>\d{18})");
                    // 执行匹配
                    Match match = regex.Match(result);
                    if (match.Success)
                    {
                        // 提取信息
                        string name = match.Groups["name"].Value;
                        string gender = match.Groups["gender"].Value;
                        string nation = match.Groups["nation"].Value;
                        string birth = match.Groups["birth"].Value;
                        string address = match.Groups["address"].Value;
                        string idNumber = match.Groups["id"].Value;
                        // 构建JSON对象
                        jsonResult = new
                        {
                            姓名 = name,
                            性别 = gender,
                            民族 = nation,
                            出生 = birth,
                            住址 = address,
                            公民身份号码 = idNumber,
                            text = result
                        };
                        result = JsonConvert.SerializeObject(jsonResult, Formatting.Indented);
                    }
                    else
                    {
                        message.Append("正则表达式匹配失败\r\n");
                    }
                }
                else if(id_card_side.Equals("back"))
                {
                    var jsonResult = new
                    {
                        签发机关 = "",
                        有效期限 = "",
                        text = ""
                    };
                    // 定义正则表达式
                    Regex regex = new Regex(@"签发机关(?<issuingAuthority>.+?)有效期限(?<validityPeriod>.+)$");

                    // 执行匹配
                    Match match = regex.Match(result);
                    if (match.Success)
                    {
                        // 提取信息
                        string issuingAuthority = match.Groups["issuingAuthority"].Value;
                        string validityPeriod = match.Groups["validityPeriod"].Value;
                        // 构建JSON对象
                        jsonResult = new
                        {
                            签发机关 = issuingAuthority,
                            有效期限 = validityPeriod,
                            text = result
                        };
                        result = JsonConvert.SerializeObject(jsonResult, Formatting.Indented);
                    }
                    else
                    {
                        message.Append("正则表达式匹配失败\r\n");
                    }
                }
                message.Append(result);
                //message.Insert(0, $"识别结果{ocrResult.TextBlocks.Count()}行：\r\n");
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
        #region 图片路径转Base64
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
