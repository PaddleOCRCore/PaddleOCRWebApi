namespace PaddleOCRCore
{ 
   /// <summary>
   /// 模型配置对象
   /// </summary>
    public  class OCRModelConfig
    {
        /// <summary>
        /// det_infer模型路径
        /// </summary>
        public string det_infer { get; set; }
        /// <summary>
        /// cls_infer模型路径
        /// </summary>
        public string cls_infer { get; set; }
        /// <summary>
        /// rec_infer模型路径
        /// </summary>
        public string rec_infer { get; set; }
        /// <summary>
        /// ppocr_keys.txt文件名全路径
        /// </summary>
        public string keys { get; set; }
    }

    /// <summary>
    /// 表格模型配置对象
    /// </summary>
    public class StructureModelConfig : OCRModelConfig
    {
        /// <summary>
        /// table_model_dir模型路径
        /// </summary>
        public string table_model_dir { get; set; }
        /// <summary>
        /// 表格识别字典
        /// </summary>
        public string table_char_dict_path { get; set; }
    }
}
