using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QRCoder;
using System.Threading.Tasks;
using System.Drawing;

namespace Learun.Dbsync
{
    public static class QRcodeCreate
    {
/*        /// <summary>
        /// 二维码保存的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 二维码保存地址
        /// </summary>
        public string SavePath { get; set; }*/


       /* public void ExportQRCode(List<QRCode> qRCodes)
        {
            foreach (var qrc in qRCodes)
            {

                QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(0);
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrc.Value, eccLevel))
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    var img = qrCode.GetGraphic(20);
                    img.Save(qrc.SavePath);
                }
            }
        }*/

        public static Bitmap GetQRCodeImage(string qrCode)
        {    //获取含水印的二维码图像对象
            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData data = generator.CreateQrCode(qrCode, QRCodeGenerator.ECCLevel.M);    //qrCode是二维码内容，ECCLevel用于设置容错率
            QRCode code = new QRCode(data);
            //定义二维码中央水印图标，文件路径一定要是绝对路径，如果是Web工程，可用Server.MapPath函数获取绝对路径
            Bitmap qrImage = code.GetGraphic(10);
            //获得含水印的二维码图像信息，如不需要水印可以调用另外函数：
            return qrImage;
        }
    }


}
