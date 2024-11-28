using System.Runtime.InteropServices;
namespace PaddleOCRCore
{
    /// <summary>
    /// OCR识别参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class StructureParameter:OCRParameter
    {
        /// <summary>
        /// 输入图像长宽大于488时，等比例缩放图像,默认488
        /// </summary>
        public int table_max_len { get; set; } = 488;
        /// <summary>
        /// 是否合并空单元格
        /// </summary>
        [field: MarshalAs(UnmanagedType.I1)]
        public bool merge_no_span_structure { get; set; } = true;
        /// <summary>
        /// 批量识别数量
        /// </summary>
        public int table_batch_num { get; set; } = 1;
         

    }
}


