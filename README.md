## 项目介绍
基于百度飞桨PaddleOCR的C++代码修改并封装的动态链接库，支持文本识别、文本检测、表格识别等功能。本项目实现了.net8环境下利用CPU或GPU进行身份证正反面的OCR识别的WebApi，部署后可供其它应用程序调用。

## 运行环境
项目运行环境为VS2022+.net8，其它.net版本可自行修改：

1、下载paddle_inference3.0版本，解压后将paddle_inference.dll放到编译的根目录

https://paddle-inference-lib.bj.bcebos.com/3.0.0-beta2/cxx_c/Windows/GPU/x86-64_cuda12.3_cudnn9.0.0_trt8.6.1.6_mkl_avx_vs2019/paddle_inference.zip

2、进QQ群475159576，下载群文件的PaddleOCRLib.zip，解压后放到编译的根目录中

3、核心文件PaddleOCR.dll为C++动态链接库，支持CPU/GPU模式(GPU需接gitee说明安装对应环境)

## GPU版本环境

OpenCV 4.7

Paddle version: 3.0.0-beta1

CUDA version: 12.0

CUDNN version: v8.9

CXX compiler version: 19.29.30154.0

WITH_TENSORRT: ON

TensorRT version: v8.6.1.6

cuda下载
https://developer.nvidia.com/cuda-11-8-0-download-archive

cudnn下载
https://developer.nvidia.cn/rdp/cudnn-archive

TensorRT下载
https://developer.nvidia.com/nvidia-tensorrt-download

paddle_inference下载
https://www.paddlepaddle.org.cn/inference/v3.0/guides/install/download_lib.html#windows

https://paddle-inference-lib.bj.bcebos.com/3.0.0-beta2/cxx_c/Windows/GPU/x86-64_cuda12.3_cudnn9.0.0_trt8.6.1.6_mkl_avx_vs2019/paddle_inference.zip

OCR识别官方模型库下载

https://gitee.com/paddlepaddle/PaddleOCR/blob/release/2.7/doc/doc_ch/models_list.md

## 开发交流群

欢迎加入QQ群475159576交流,或者添加QQ：2380243976

<img src="https://gitee.com/corallite/PaddleOCRWebApi/raw/master/PaddleOCRCore/PaddleOCRRuntime/qq.png" width="382px;" />

