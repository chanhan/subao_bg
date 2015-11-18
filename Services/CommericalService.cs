using Common;
using IServices;
using Models;
using Models.ViewModel;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Services
{
    public class CommericalService : RepositoryBase<Commercial>, ICommericalService
    {
        private readonly ISetTypeValService setTypeValService;
        public CommericalService(IDatabaseFactory databaseFactory, IUser IUserService,ISetTypeValService ISetTypeValService)
            : base(databaseFactory, IUserService)
        {
            setTypeValService = ISetTypeValService;
        }
        public Dictionary<string, Tuple<int, List<CommercialImage>>> GetCommercial(string language)
        {
            Dictionary<string, Tuple<int, List<CommercialImage>>> commercial = new Dictionary<string, Tuple<int, List<CommercialImage>>>();
            string left, right;
            if (language.ToUpper() == "ZH")
            {
                left = "PcCommercialLeftS";
                right = "PcCommercialRightS";
            }
            else
            {
                left = "PcCommercialLeftT";
                right = "PcCommercialRightT";
            }
            SetTypeVal typeValLeft = setTypeValService.QueryByCondition(p => p.Type == left).SingleOrDefault();
            SetTypeVal typeValRight = setTypeValService.QueryByCondition(p => p.Type == right).SingleOrDefault();

            if (typeValLeft != null)
            {
                commercial.Add("left", GetImage(typeValLeft.Val));
            }
            if (typeValRight != null)
            {
                commercial.Add("right", GetImage(typeValRight.Val));
            }
            return commercial;
        }
        private Tuple<int, List<CommercialImage>> GetImage(string value)
        {
            Commercial commercial = JSONHelper.JsonDeserialize<Commercial>(value);
            List<CommercialImage> images = new List<CommercialImage>();
            foreach (KeyValuePair<string, string> item in commercial.Commercials)
            {
                string[] linkAndSource = item.Value.Split('#');
                images.Add(new CommercialImage
                {
                    ImageName = item.Key,
                    Link = linkAndSource[0],
                    SourceUrl = linkAndSource.Length > 1 ? linkAndSource[1] : string.Empty
                });
            }
            return Tuple.Create<int, List<CommercialImage>>(commercial.Sec, images);
        }
        /// <summary>
        /// 增删改广告图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="act"></param>
        /// <param name="oldImageName"></param>
        /// <param name="language"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public int EditCommercial(CommercialImage image, string act, HttpPostedFileBase imagaFile, string oldImageName, string language, string direction)
        {
            string type = language.ToUpper() == "ZH" ? (direction.ToUpper() == "LEFT" ? "PcCommercialLeftS" : "PcCommercialRightS") : (direction.ToUpper() == "LEFT" ? "PcCommercialLeftT" : "PcCommercialRightT");
            SetTypeVal typeVal = setTypeValService.QueryByCondition(p => p.Type == type).SingleOrDefault();
            if (typeVal == null) return 0;
            Commercial commercial = JSONHelper.JsonDeserialize<Commercial>(typeVal.Val);
            //更新图片
            string commercialImagePath = AppData.GetCommercialImagePath();
            if (!Directory.Exists(commercialImagePath))
            {
                Directory.CreateDirectory(commercialImagePath);
            }
            if (act == "del" || (act == "update" && imagaFile != null))
            {
                commercial.Commercials.Remove(oldImageName);
                File.Delete(Path.Combine(commercialImagePath, oldImageName));
            }
            if (act == "add" || act == "update")
            {
                if (imagaFile != null)//上传图片
                {
                    image.ImageName = string.Format("{0}{1}.jpg", AppData.CreateRandomCode(5), DateTime.Now.ToString("MM-DD"));
                    Bitmap bmp = new Bitmap(imagaFile.InputStream);//从上传的文件中获取文件流,实例化一个位图
                    bmp.Save(Path.Combine(commercialImagePath, image.ImageName), ImageFormat.Jpeg);//保存文件,将图片上传至服务器指定文件夹.图片在这里转换格式
                    //imagaFile.SaveAs(Path.Combine(commercialImagePath, image.ImageName));
                    commercial.Commercials.Add(image.ImageName, image.ToString());
                }
                else//不上传图片
                {
                    commercial.Commercials[image.ImageName] = image.ToString();
                }
            }
            typeVal.Val = JSONHelper.JsonSerializer(commercial);
            setTypeValService.Update(typeVal);
            return setTypeValService.Commit();
        }
        public int ChangeCommercialSeconds(int seconds, string direction, string language)
        {
            string type = language.ToUpper() == "ZH" ? (direction.ToUpper() == "LEFT" ? "PcCommercialLeftS" : "PcCommercialRightS") : (direction.ToUpper() == "LEFT" ? "PcCommercialLeftT" : "PcCommercialRightT");
            SetTypeVal typeVal = setTypeValService.QueryByCondition(p => p.Type == type).SingleOrDefault();
            if (typeVal == null) return 0;
            Commercial commercial = JSONHelper.JsonDeserialize<Commercial>(typeVal.Val);
            commercial.Sec = seconds;
            typeVal.Val = JSONHelper.JsonSerializer(commercial);
            setTypeValService.Update(typeVal);
            return setTypeValService.Commit();
        }
    }
}
