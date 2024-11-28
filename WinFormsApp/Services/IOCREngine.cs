using PaddleOCRSharp;

namespace WinFormsApp.Services
{
    /// <summary>
    /// OCR引擎依赖注入
    /// </summary>
    public interface IOCREngine
    {
        PaddleOCREngine GetOCREngine();
        PaddleStructureEngine GetStructureEngine();
    }
}
