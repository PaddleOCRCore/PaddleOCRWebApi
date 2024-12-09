using PaddleOCRCore;

namespace WinFormsApp.Services
{
    /// <summary>
    /// OCR引擎依赖注入
    /// </summary>
    public class OCREngine : IOCREngine
    {
        private static string det_infer = @"\ch_PP-OCRv4_det_infer";
        private static string cls_infer = @"\ch_ppocr_mobile_v2.0_cls_infer";
        private static string rec_infer = @"\ch_PP-OCRv4_rec_infer";
        private static string keys = @"\ppocr_keys.txt";
        private static bool use_gpu = false;
        /// <summary>
        /// 初始化OCR引擎
        /// </summary>
        /// <returns></returns>
        public PaddleOCREngine GetOCREngine()
        {
            //自带轻量版中英文模型V4模型
            OCRModelConfig config = new OCRModelConfig();
            string root = PaddleOCRCore.EngineBase.GetRootDirectory();
            string modelPathroot = root + @"\inference";
            config.det_infer = modelPathroot + det_infer;
            config.cls_infer = modelPathroot + cls_infer;
            config.rec_infer = modelPathroot + rec_infer;
            config.keys = modelPathroot + keys;

            //服务器中英文模型
            //OCRModelConfig config = new OCRModelConfig();
            //string root = PaddleOCRCore.EngineBase.GetRootDirectory();
            //string modelPathroot = root + @"\inference";
            //config.det_infer = modelPathroot + @"\ch_PP-OCRv4_det_server_infer";
            //config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
            //config.rec_infer = modelPathroot + @"\ch_PP-OCRv4_rec_server_infer";
            //config.keys = modelPathroot + @"\ppocr_keysANSI.txt";

            //英文和数字模型V3
            //OCRModelConfig config = new OCRModelConfig();
            //string root = PaddleOCRCore.EngineBase.GetRootDirectory();
            //string modelPathroot = root + @"\en_v3";
            //config.det_infer = modelPathroot + @"\en_PP-OCRv3_det_infer";
            //config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
            //config.rec_infer = modelPathroot + @"\en_PP-OCRv3_rec_infer";
            //config.keys = modelPathroot + @"\en_dict.txt";
            //OCR参数

            OCRParameter oCRParameter = new OCRParameter();
            oCRParameter.use_gpu = use_gpu;
            oCRParameter.cpu_math_library_num_threads = 10;
            oCRParameter.enable_mkldnn = true;
            oCRParameter.cls = false;
            oCRParameter.det = true;
            oCRParameter.use_angle_cls = false;
            oCRParameter.det_db_score_mode = true;
            oCRParameter.max_side_len = 960;
            oCRParameter.rec_img_h = 48;
            oCRParameter.rec_img_w = 320;
            oCRParameter.det_db_thresh = 0.3f;
            oCRParameter.det_db_box_thresh = 0.618f;

            //string ocrJson = "{\"use_gpu\": true,\"gpu_id\": 0,\"gpu_mem\": 4000,\"enable_mkldnn\": true,\"rec_img_h\": 48,\"rec_img_w\": 320,\"cls\":false,\"det\":true,\"use_angle_cls\":false}";
            //初始化通用文字引擎
            return new PaddleOCREngine(config, oCRParameter);
        }

        /// <summary>
        /// 初始化Structure引擎
        /// </summary>
        /// <returns></returns>
        public PaddleStructureEngine GetStructureEngine()
        {
            //自带轻量版中英文模型V4模型
            StructureModelConfig config = new StructureModelConfig();
            string root = PaddleOCRCore.EngineBase.GetRootDirectory();
            string modelPathroot = root + @"\inference";
            config.det_infer = modelPathroot + det_infer;
            config.cls_infer = modelPathroot + cls_infer;
            config.rec_infer = modelPathroot + rec_infer;
            config.keys = modelPathroot + keys;
            config.table_model_dir = modelPathroot + @"\ch_ppstructure_mobile_v2.0_SLANet_infer";
            config.table_char_dict_path = modelPathroot + @"\table_structure_dict_ch.txt";

            StructureParameter oCRParameter = new StructureParameter();
            oCRParameter.use_gpu = use_gpu;
            oCRParameter.cpu_math_library_num_threads = 10;
            oCRParameter.visualize = false;
            oCRParameter.enable_mkldnn = true;
            oCRParameter.cls = false;
            oCRParameter.det = true;
            oCRParameter.use_angle_cls = false;
            oCRParameter.det_db_score_mode = true;
            oCRParameter.max_side_len = 960;
            oCRParameter.rec_img_h = 48;
            oCRParameter.rec_img_w = 320;
            oCRParameter.det_db_thresh = 0.3f;
            oCRParameter.det_db_box_thresh = 0.618f;
            oCRParameter.table_max_len = 488;
            oCRParameter.merge_no_span_structure = true;
            oCRParameter.table_batch_num = 1;
            //string ocrJson = "{\"use_gpu\": true,\"gpu_id\": 0,\"gpu_mem\": 4000,\"enable_mkldnn\": true,\"rec_img_h\": 48,\"rec_img_w\": 320,\"cls\":false,\"det\":true,\"use_angle_cls\":false}";
            //初始化表格识别引擎
            return new PaddleStructureEngine(config, oCRParameter);
        }
    }
}
