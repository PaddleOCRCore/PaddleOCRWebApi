using System;
using System.Runtime.InteropServices;

//说明：OCRParameter类的属性定义顺序不可随便更改，用户向PdddleOCR.dll传入参数

namespace PaddleOCRCore
{
    /// <summary>
    /// OCR识别参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class OCRParameter
	{
        #region 通用参数设置
        /// <summary>
        /// 是否使用GPU；默认false
        /// </summary>
        [field: MarshalAs(UnmanagedType.I1)] 
		public bool use_gpu { get; set; } = true;
        /// <summary>
        /// GPU id，使用GPU时有效；默认0;
        /// </summary>
        [field: MarshalAs(UnmanagedType.I4)]
        public int gpu_id { get; set; } = 0;
        /// <summary>
        /// 申请的GPU内存;默认4000
        /// </summary>
        [field: MarshalAs(UnmanagedType.I4)]
        public int gpu_mem { get; set; } = 4000;

		/// <summary>
		/// CPU预测时的线程数；默认10
		/// </summary>
		public int cpu_math_library_num_threads { get; set; } = 10;
		/// <summary>
		/// 是否使用mkldnn库；默认true
		/// </summary>
		[field:MarshalAs(UnmanagedType.I1)]
		public bool enable_mkldnn { get; set; } =true;
        #endregion

        #region 文字识别设置
        /// <summary>
        ///是否执行文字检测；默认true 
        /// </summary>
        [field: MarshalAs(UnmanagedType.I1)] 
		public bool det { get; set; } = true;
        /// <summary>
        /// 是否执行文字识别；默认true
        /// </summary>
        [field: MarshalAs(UnmanagedType.I1)] 
		public bool rec { get; set; } = true;
        /// <summary>
        /// 是否执行文字方向分类；默认false
        /// </summary>
        [field: MarshalAs(UnmanagedType.I1)] 
		public bool cls { get; set; } = false;
        #endregion

        #region 检测模型设置

        /// <summary>
        /// 输入图像长宽大于960时，等比例缩放图像，使得图像最长边为960,；默认960
        /// </summary>
        public int max_side_len { get; set; } = 960;
		/// <summary>
		/// 用于过滤DB预测的二值化图像，设置为0.-0.3对结果影响不明显；默认0.3
		/// </summary>
		public float det_db_thresh { get; set; } = 0.3f;
		/// <summary>
		/// DB后处理过滤box的阈值，如果检测存在漏框情况，可酌情减小；默认0.5
		/// </summary>
		public float det_db_box_thresh { get; set; } = 0.5f;
		/// <summary>
		/// 表示文本框的紧致程度，越小则文本框更靠近文本;默认1.6
		/// </summary>
		public float det_db_unclip_ratio { get; set; } = 1.6f;
		/// <summary>
		/// 是否在输出映射上使用膨胀,默认false
		/// </summary>
		[field: MarshalAs(UnmanagedType.I1)] 
		public bool use_dilation { get; set; } = false;
		/// <summary>
		/// true:使用多边形框计算bbox score，false:使用矩形框计算。矩形框计算速度更快，多边形框对弯曲文本区域计算更准确。
		/// </summary>
		[field: MarshalAs(UnmanagedType.I1)]	
		public bool det_db_score_mode { get; set; } = true;
        /// <summary>
        /// 是否对结果进行可视化，为1时，预测结果会保存在output字段指定的文件夹下和输入图像同名的图像上。默认false
        /// </summary>

        [field: MarshalAs(UnmanagedType.I1)] 
		public bool visualize { get; set; } = false;

        #endregion

        #region 方向分类器设置

        /// <summary>
        /// 是否使用方向分类器,默认false
        /// </summary>
        [field: MarshalAs(UnmanagedType.I1)] 
		public bool use_angle_cls { get; set; } = false;
		/// <summary>
		/// 方向分类器的得分阈值，默认0.9
		/// </summary>
		public float cls_thresh { get; set; } = 0.9f;
		/// <summary>
		/// 方向分类器batchsize，默认1
		/// </summary>
		public int cls_batch_num { get; set; } = 1;
        #endregion


        #region 文字识别模型设置
        /// <summary>
        /// 识别模型batchsize，默认6
        /// </summary>
        public int rec_batch_num { get; set; } = 6;
		/// <summary>
		/// 识别模型输入图像高度，默认48
		/// </summary>
		public int rec_img_h { get; set; } = 48;
		/// <summary>
		/// 识别模型输入图像宽度，默认320
		/// </summary>
		public int rec_img_w { get; set; } = 320;
		#endregion
		/// <summary>
		/// 是否显示预测结果，默认false
		/// </summary>
		[field: MarshalAs(UnmanagedType.I1)] 
		public bool show_img_vis { get; set; } = false;

        /// <summary>
        /// 使用GPU预测时，是否启动tensorrt，默认false
        /// </summary>

        [field: MarshalAs(UnmanagedType.I1)]
        public bool use_tensorrt { get; set; } = false;
    }
    /// <summary>
    /// OCR可修改参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential,Pack =1)]
    public class AsyncParameter
    {
        /// <summary>
        ///动态修改是否检测。在OCRParameter.det=true时，m_det可动态关闭参数det
        /// </summary>
        [field: MarshalAs(UnmanagedType.I1)]
        public bool m_det { get; set; } = true;
        /// <summary>
        ///动态修改是否识别。在OCRParameter.rec=true时，m_rec可动态关闭参数rec
        /// </summary>
        [field: MarshalAs(UnmanagedType.I1)]
        public bool m_rec { get; set; } = true;
        /// <summary>
        /// 输入图像长宽大于960时，等比例缩放图像，使得图像最长边为960,；默认960
        /// </summary>
        public int m_max_side_len { get; set; } = 960;
        /// <summary>
        /// 用于过滤DB预测的二值化图像，设置为0.-0.3对结果影响不明显；默认0.3。当m_det=true时有效
        /// </summary>
        public float m_det_db_thresh { get; set; } = 0.3f;
        /// <summary>
        /// DB后处理过滤box的阈值，如果检测存在漏框情况，可酌情减小；默认0.5。当m_det=true时有效
        /// </summary>
        public float m_det_db_box_thresh { get; set; } = 0.5f;
        /// <summary>
        /// 表示文本框的紧致程度，越小则文本框更靠近文本;默认1.6。当m_det=true时有效
        /// </summary>
        public float m_det_db_unclip_ratio { get; set; } = 1.6f;

    }
}


