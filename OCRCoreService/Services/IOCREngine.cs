using PaddleOCRCore;

namespace OCRCoreService.Services
{
    /// <summary>
    /// OCR引擎依赖注入
    /// </summary>
    public interface IOCREngine
    {
        PaddleOCREngine ocrEngine{ get; set; }
        PaddleStructureEngine structureEngine { get; set; }
    }
}
