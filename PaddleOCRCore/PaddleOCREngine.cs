using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System;
using System.Linq;
using System.IO;
using System.Data.Common;

namespace PaddleOCRCore
{
    /// <summary>
    /// PaddleOCR识别引擎对象
    /// </summary>
    public class PaddleOCREngine : EngineBase
    {
        #region PaddleOCR API

        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern bool Initialize(string det_infer, string cls_infer, string rec_infer, string keys, OCRParameter parameter);
        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern bool Initializejson(string det_infer, string cls_infer, string rec_infer, string keys, string parameterjson);

        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern IntPtr Detect(string imagefile);

        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern IntPtr DetectByte(byte[] imagebytedata, long size);

        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern IntPtr DetectBase64(string imagebase64);

        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern int FreeEngine();

        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern bool libModifyParameter(ModifyParameter parameter);
  
        #endregion

        #region 文本识别
        /// <summary>
        /// 使用默认参数初始化OCR引擎对象
        /// </summary>
        public PaddleOCREngine() : this(null,new OCRParameter())
        {
        }
        /// <summary>
        /// 使用默认参数初始化OCR引擎对象
        /// </summary>
        /// <param name="config">模型配置对象，如果为空则按默认值</param>
        public PaddleOCREngine(OCRModelConfig config) : this(config, new OCRParameter())
        {
        }
        /// <summary>
        /// PaddleOCR识别引擎对象初始化
        /// </summary>
        /// <param name="config">模型配置对象，如果为空则按默认值</param>
        /// <param name="parameter">识别参数，为空均按缺省值</param>
        public PaddleOCREngine(OCRModelConfig config, OCRParameter parameter) : base()
        {
           
            if (parameter == null) parameter = new OCRParameter();
            if (config == null)
            {
                string root = GetRootDirectory().TrimEnd('\\');
                config = new OCRModelConfig();
                string modelPathroot = root + @"\inference";
                config.det_infer = modelPathroot + @"\ch_PP-OCRv4_det_infer";
                config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
                config.rec_infer = modelPathroot + @"\ch_PP-OCRv4_rec_infer";
                config.keys = modelPathroot + @"\ppocr_keys.txt";
            }
            if (!Directory.Exists(config.det_infer) && parameter.det) throw new DirectoryNotFoundException(config.det_infer);
            if (!Directory.Exists(config.cls_infer) && parameter.cls) throw new DirectoryNotFoundException(config.cls_infer);
            if (!Directory.Exists(config.rec_infer)) throw new DirectoryNotFoundException(config.rec_infer);
            if (!File.Exists(config.keys)) throw new FileNotFoundException(config.keys);
            bool sucess = Initialize(config.det_infer, config.cls_infer, config.rec_infer, config.keys, parameter);
            if(!sucess)
            {
                throw  new Exception("Initialize err:" + GetLastError());
            }
        }
        /// <summary>
        /// PaddleOCR识别引擎对象初始化
        /// </summary>
        /// <param name="config">模型配置对象，如果为空则按默认值</param>
        /// <param name="parameterjson">识别参数json字符串</param>
        public PaddleOCREngine(OCRModelConfig config, string parameterjson) : base()
        {
            if (config == null)
            {
                string root = GetRootDirectory().TrimEnd('\\');
                config = new OCRModelConfig();
                string modelPathroot = root + @"\inference";
                config.det_infer = modelPathroot + @"\ch_PP-OCRv4_det_infer";
                config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
                config.rec_infer = modelPathroot + @"\ch_PP-OCRv4_rec_infer";
                config.keys = modelPathroot + @"\ppocr_keys.txt";
            }
            if (string.IsNullOrEmpty(parameterjson))
            {
                parameterjson = GetRootDirectory().TrimEnd('\\');
                parameterjson += @"\inference\PaddleOCR.config.json";
                if (!File.Exists(parameterjson)) throw new FileNotFoundException(parameterjson);
                parameterjson = File.ReadAllText(parameterjson);
            }
            var parameter = JsonHelper.DeserializeObject<OCRParameter>(parameterjson);

            if (!Directory.Exists(config.det_infer) && parameter.det) throw new DirectoryNotFoundException(config.det_infer);
            if (!Directory.Exists(config.cls_infer) && parameter.cls) throw new DirectoryNotFoundException(config.cls_infer);
            if (!Directory.Exists(config.rec_infer)) throw new DirectoryNotFoundException(config.rec_infer);
            if (!File.Exists(config.keys)) throw new FileNotFoundException(config.keys);
            parameter = null;
            bool sucess = Initializejson(config.det_infer, config.cls_infer, config.rec_infer, config.keys, parameterjson);
            if (!sucess)
            {
                throw new Exception("Initialize err:" + GetLastError());
            }
        }
        /// <summary>
        /// 对图像文件进行文本识别
        /// </summary>
        /// <param name="imagefile">图像文件</param>
        /// <returns>OCR识别结果</returns>
        public OCRResult DetectText(string imagefile)
        {
            if (!File.Exists(imagefile)) throw new Exception($"文件{imagefile}不存在");
            var ptrResult = Detect(imagefile);
            return ConvertResult(ptrResult);
        }


 
        /// <summary>
        ///对图像对象进行文本识别
        /// </summary>
        /// <param name="image">图像</param>
        /// <returns>OCR识别结果</returns>
        public OCRResult DetectText(Image image)
        {
            if (image == null) throw new ArgumentNullException("image");
            var imagebyte = ImageToBytes(image);
            var result = DetectText(imagebyte);
            imagebyte = null;
            return result;
        }
      
        /// <summary>
        ///文本识别
        /// </summary>
        /// <param name="imagebyte">图像内存流</param>
        /// <returns>OCR识别结果</returns>
        public OCRResult DetectText(byte[] imagebyte)
        {
            if (imagebyte == null) throw new ArgumentNullException("imagebyte");
            var ptrResult = DetectByte(imagebyte, imagebyte.LongLength);
            return ConvertResult(ptrResult);
        }

        /// <summary>
        ///文本识别
        /// </summary>
        /// <param name="imagebase64">图像base64</param>
        /// <returns>OCR识别结果</returns>
        public OCRResult DetectTextBase64(string imagebase64)
        {
            if (imagebase64 == null || imagebase64 == "") throw new ArgumentNullException("imagebase64");
            IntPtr ptrResult = DetectBase64(imagebase64);
            return ConvertResult(ptrResult);
        }

        /// <summary>
        /// 结果解析
        /// </summary>
        /// <param name="ptrResult"></param>
        /// <returns></returns>
        private OCRResult ConvertResult(IntPtr ptrResult)
        {
            OCRResult result = new OCRResult();
            if (ptrResult == IntPtr.Zero)
            {
                var err = GetLastError();
                if (!string.IsNullOrEmpty(err))
                {
                    throw new Exception("内部遇到错误：" + err);
                }
                return result;
            }
            string json = string.Empty;
            try
            {
                json = Marshal.PtrToStringUni(ptrResult);
                List<TextBlock> textBlocks = JsonHelper.DeserializeObject<List<TextBlock>>(json);
                result.JsonText = json;
                result.TextBlocks = textBlocks;
                Marshal.FreeHGlobal(ptrResult);
            }
            catch (Exception ex)
            {
                throw new Exception("OCR结果Json反序列化失败:"+ json, ex);
            }
            return result;
        }

        #endregion

        #region 表格识别



        /// <summary>
        ///结构化文本识别
        /// </summary>
        /// <param name="image">图像</param>
        /// <returns>表格识别结果</returns>
        [Obsolete]
        public OCRStructureResult DetectStructure(Image image)
        {

            if (image == null) throw new ArgumentNullException("image");
            var imagebyte = ImageToBytes(image);
            OCRResult result = DetectText(imagebyte);
            List<TextBlock> blocks = result.TextBlocks;
            if (blocks == null || blocks.Count == 0) return new OCRStructureResult();
            var listys = getzeroindexs(blocks.OrderBy(x => x.BoxPoints[0].Y).Select(x => x.BoxPoints[0].Y).ToArray(), 10);
            var listxs = getzeroindexs(blocks.OrderBy(x => x.BoxPoints[0].X).Select(x => x.BoxPoints[0].X).ToArray(), 10);

            int rowcount = listys.Count;
            int colcount = listxs.Count;
            OCRStructureResult structureResult = new OCRStructureResult();
            structureResult.TextBlocks = blocks;
            structureResult.RowCount = rowcount;
            structureResult.ColCount = colcount;
            structureResult.Cells = new List<StructureCells>();
            for (int i = 0; i < rowcount; i++)
            {
                int y_min = blocks.OrderBy(x => x.BoxPoints[0].Y).OrderBy(x => x.BoxPoints[0].Y).ToList()[listys[i]].BoxPoints[0].Y;
                int y_max = 99999;
                if (i < rowcount - 1)
                {
                    y_max = blocks.OrderBy(x => x.BoxPoints[0].Y).ToList()[listys[i + 1]].BoxPoints[0].Y;
                }

                for (int j = 0; j < colcount; j++)
                {
                    int x_min = blocks.OrderBy(x => x.BoxPoints[0].X).ToList()[listxs[j]].BoxPoints[0].X;
                    int x_max = 99999;

                    if (j < colcount - 1)
                    {
                        x_max = blocks.OrderBy(x => x.BoxPoints[0].X).ToList()[listxs[j + 1]].BoxPoints[0].X;
                    }

                    var textBlocks = blocks.Where(x => x.BoxPoints[0].X < x_max && x.BoxPoints[0].X >= x_min && x.BoxPoints[0].Y < y_max && x.BoxPoints[0].Y >= y_min).OrderBy(u => u.BoxPoints[0].X);
                    var texts = textBlocks.Select(x => x.Text).ToArray();

                    StructureCells cell = new StructureCells();
                    cell.Row = i;
                    cell.Col = j;

#if NET35
                    cell.Text = string.Join("", texts);
#else
                    cell.Text = string.Join<string>("", texts);
#endif


                    cell.TextBlocks = textBlocks.ToList();
                    structureResult.Cells.Add(cell);
                }
            }
            return structureResult;
        }

        /// <summary>
        /// 计算表格分割
        /// </summary>
        /// <param name="pixellist"></param>
        /// <param name="thresholdtozero"></param>
        /// <returns></returns>
        private List<int> getzeroindexs(int[] pixellist, int thresholdtozero = 10)
        {
            List<int> zerolist = new List<int>();
            zerolist.Add(0);
            for (int i = 0; i < pixellist.Length; i++)
            {
                if ((i < pixellist.Length - 1)
                    && (Math.Abs(pixellist[i + 1] - pixellist[i])) > thresholdtozero)
                {
                    //突增点
                    zerolist.Add(i + 1);
                }
            }
            return zerolist;
        }


        #endregion

        /// <summary>
        /// 在初始化后动态修改参数
        /// </summary>
        /// <param name="parameter">可修改参数对象</param>
        /// <returns>是否成功，在初始化前调用会导致失败</returns>
        public bool ModifyParameter(ModifyParameter parameter)
        {
            return libModifyParameter(parameter);
        }
       
        #region Dispose
        /// <summary>
        /// 释放对象
        /// </summary>
        public override void Dispose()
        {
            FreeEngine();
        }
        #endregion
    }
}