﻿using Microsoft.AspNetCore.Mvc;
using OCRCoreService.Services;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using PaddleOCRCore;
using System.Text.RegularExpressions;

namespace OCRCoreService.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]/[action]")]
    public class OCRServiceController : ActionBase
    {
        private readonly ILogger<OCRServiceController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public OCRServiceController(ILogger<OCRServiceController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get()
        {
            return OKResult("接口已正式启用，仅支持64位模式");
        }
        #region 身份证识别
        /// <summary>
        /// 身份证识别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[TypeFilter(typeof(WebApiActionAttribute))]
        public ActionResult GetIdCard([FromServices] IWebHostEnvironment env, [FromServices] IOCREngine engine, [FromBody] RequestOcr request)
        {
            string result = "";
            if (string.IsNullOrEmpty(request.Base64String))
            {
                return (BadResult("识别失败:图片不存在！"));
            }
            string beginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var webSiteUrl = $"UploadFile{Path.DirectorySeparatorChar}OCRService{Path.DirectorySeparatorChar}";
            string fileNameSeg = Guid.NewGuid().ToString() + ".jpg";
            string fileDir = Path.Combine(env.WebRootPath, webSiteUrl);
            string filePath = Path.Combine(fileDir, fileNameSeg);
            if (!System.IO.Directory.Exists(fileDir))
            {
                System.IO.Directory.CreateDirectory(fileDir);
            }
            //OCRResult ocrResult = engine.ocrEngine.DetectText(ImageBeauty.Base64StringToImage(request.Base64String));
            OCRResult ocrResult = engine.ocrEngine.DetectTextBase64(request.Base64String);            
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in ocrResult.TextBlocks)
            {
                if (!string.IsNullOrEmpty(item.Text))
                {
                    //if (stringBuilder.Length > 0)
                    //{
                    //    stringBuilder.Append(Environment.NewLine);
                    //}
                    stringBuilder.Append(item.Text);
            }
            }
            result=stringBuilder.ToString();
            if (request.id_card_side.Equals("front"))
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
                Regex regex = new Regex(@"姓名(?<name>[^\s]+)性别(?<gender>[男女])民族(?<nation>.+?)出生(?<birth>.+?)住址(?<address>.+?)公民身份号码(?<id>\d{18})");

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
                }
                return OKResult(jsonResult);
            }
            else
            {
                var jsonResult = new
                {
                    签发机关 = "",
                    有效期限 = "",
                    text= ""
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
                }
                return OKResult(jsonResult);
            }
        }
        #endregion

        #region 通用文字识别
        /// <summary>
        /// 通用文字识别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[TypeFilter(typeof(WebApiActionAttribute))]
        public ActionResult GetOCRText([FromServices] IWebHostEnvironment env, [FromServices] IOCREngine engine, [FromBody] RequestOcr request)
        {
            string result = "";
            if (string.IsNullOrEmpty(request.Base64String))
            {
                return (BadResult("识别失败:图片不存在！"));
            }
            OCRResult ocrResult = engine.ocrEngine.DetectTextBase64(request.Base64String);
            if (request.ResultType.Equals("text", StringComparison.OrdinalIgnoreCase))
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in ocrResult.TextBlocks)
                {
                    if (!string.IsNullOrEmpty(item.Text))
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append(Environment.NewLine);
                        }
                        stringBuilder.Append(item.Text);
                    }
                }
                result = stringBuilder.ToString();
            }
            else
            {
                result = ocrResult.JsonText;
            }
            return OKResult(result);
        }
        #endregion

        #region 通用表格识别
        /// <summary>
        /// 通用表格识别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[TypeFilter(typeof(WebApiActionAttribute))]
        public ActionResult GetOCRTable([FromServices] IWebHostEnvironment env, [FromServices] IOCREngine engine, [FromBody] RequestOcr request)
        {
            if (string.IsNullOrEmpty(request.Base64String))
            {
                return (BadResult("识别失败:图片不存在！"));
            }
            string ocrResult = engine.structureEngine.StructureDetectBase64(request.Base64String);
            return OKResult(ocrResult);
        }
        #endregion
    }
}
