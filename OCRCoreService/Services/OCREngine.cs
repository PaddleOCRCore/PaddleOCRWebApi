using PaddleOCRSharp;

namespace OCRCoreService.Services
{
    /// <summary>
    /// OCR引擎依赖注入
    /// </summary>
    public class OCREngine : IOCREngine
    {
        public PaddleOCREngine ocrEngine{ get; set; }
        public PaddleStructureEngine structureEngine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public OCREngine()
        {
            ocrEngine = GetOCREngine();
            structureEngine = GetStructureEngine();
        }
        /// <summary>
        /// 初始化OCR引擎
        /// </summary>
        /// <returns></returns>
        private PaddleOCREngine GetOCREngine()
        {
            //自带轻量版中英文模型V4模型
            OCRModelConfig config = null;

            //服务器中英文模型
            //OCRModelConfig config = new OCRModelConfig();
            //string root = System.IO.Path.GetDirectoryName(typeof(OCRModelConfig).Assembly.Location);
            //string modelPathroot = root + @"\inference";
            //config.det_infer = modelPathroot + @"\ch_PP-OCRv4_det_server_infer";
            //config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
            //config.rec_infer = modelPathroot + @"\ch_PP-OCRv4_rec_server_infer";
            //config.keys = modelPathroot + @"\ppocr_keys.txt";

            //英文和数字模型V3
            //OCRModelConfig config = new OCRModelConfig();
            //string root = PaddleOCRSharp.EngineBase.GetRootDirectory();
            //string modelPathroot = root + @"\en_v3";
            //config.det_infer = modelPathroot + @"\en_PP-OCRv3_det_infer";
            //config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
            //config.rec_infer = modelPathroot + @"\en_PP-OCRv3_rec_infer";
            //config.keys = modelPathroot + @"\en_dict.txt";
            //OCR参数

            OCRParameter oCRParameter = new OCRParameter();
            oCRParameter.use_gpu = true;
            oCRParameter.cpu_math_library_num_threads = 10;//预测并发线程数
            oCRParameter.visualize = false;
            oCRParameter.enable_mkldnn = true;
            oCRParameter.cls = false; //是否执行文字方向分类；默认false
            oCRParameter.det = true;//是否开启文本框检测，用于检测文本块
            oCRParameter.use_angle_cls = false;//是否开启方向检测，用于检测识别180旋转
            oCRParameter.det_db_score_mode = true;//是否使用多段线，即文字区域是用多段线还是用矩形，
            oCRParameter.max_side_len = 960;
            oCRParameter.rec_img_h = 48;
            oCRParameter.rec_img_w = 320;
            oCRParameter.det_db_thresh = 0.3f;
            oCRParameter.det_db_box_thresh = 0.618f;
            //string ocrJson = "{\"use_gpu\": true,\"gpu_id\": 0,\"gpu_mem\": 4000,\"enable_mkldnn\": true,\"rec_img_h\": 48,\"rec_img_w\": 320,\"cls\":false,\"det\":true,\"use_angle_cls\":false}";
            //初始化OCR引擎
            return new PaddleOCREngine(config, oCRParameter);
        }

        /// <summary>
        /// 初始化Structure引擎
        /// </summary>
        /// <returns></returns>
        private PaddleStructureEngine GetStructureEngine()
        {
            //自带轻量版中英文模型V4模型
            StructureModelConfig config = null;

            //服务器中英文模型
            //OCRModelConfig config = new OCRModelConfig();
            //string root = System.IO.Path.GetDirectoryName(typeof(OCRModelConfig).Assembly.Location);
            //string modelPathroot = root + @"\inference";
            //config.det_infer = modelPathroot + @"\ch_PP-OCRv4_det_server_infer";
            //config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
            //config.rec_infer = modelPathroot + @"\ch_PP-OCRv4_rec_server_infer";
            //config.keys = modelPathroot + @"\ppocr_keys.txt";

            //英文和数字模型V3
            //OCRModelConfig config = new OCRModelConfig();
            //string root = PaddleOCRSharp.EngineBase.GetRootDirectory();
            //string modelPathroot = root + @"\en_v3";
            //config.det_infer = modelPathroot + @"\en_PP-OCRv3_det_infer";
            //config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
            //config.rec_infer = modelPathroot + @"\en_PP-OCRv3_rec_infer";
            //config.keys = modelPathroot + @"\en_dict.txt";
            //OCR参数

            StructureParameter oCRParameter = new StructureParameter();
            oCRParameter.use_gpu = true;
            oCRParameter.cpu_math_library_num_threads = 10;//预测并发线程数
            oCRParameter.visualize = false;
            oCRParameter.enable_mkldnn = true;
            oCRParameter.cls = false; //是否执行文字方向分类；默认false
            oCRParameter.det = true;//是否开启文本框检测，用于检测文本块
            oCRParameter.use_angle_cls = false;//是否开启方向检测，用于检测识别180旋转
            oCRParameter.det_db_score_mode = true;//是否使用多段线，即文字区域是用多段线还是用矩形，
            oCRParameter.max_side_len = 960;
            oCRParameter.rec_img_h = 48;
            oCRParameter.rec_img_w = 320;
            oCRParameter.det_db_thresh = 0.3f;
            oCRParameter.det_db_box_thresh = 0.618f;
            oCRParameter.table_max_len = 488;
            oCRParameter.merge_no_span_structure = true;
            oCRParameter.table_batch_num = 1;
            //string ocrJson = "{\"use_gpu\": true,\"gpu_id\": 0,\"gpu_mem\": 4000,\"enable_mkldnn\": true,\"rec_img_h\": 48,\"rec_img_w\": 320,\"cls\":false,\"det\":true,\"use_angle_cls\":false}";
            //初始化OCR引擎
            return new PaddleStructureEngine(config, oCRParameter);
        }
    }
}
