using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace Common
{
    public class AppData
    {
        /// <summary>
        /// 登录权限
        /// </summary>
        public static readonly string LoginRanetks = WebConfigurationManager.AppSettings["loginAccess"];

        /// <summary>
        /// 查看比分修改记录权限
        /// </summary>
        public static readonly string Ranetks = WebConfigurationManager.AppSettings["showRecordAccess"];
        /// <summary>
        /// 取得赛事状态及文字
        /// </summary>
        /// <returns></returns>
        public static List<GameStatus> GetGameStatus()
        {
            List<GameStatus> GameStatus = new List<GameStatus>();
            GameStatus.Add(new GameStatus
            {
                Status = "X",
                StatusText = "未開賽"
            });
            GameStatus.Add(new GameStatus
            {
                Status = "S",
                StatusText = "已開賽"
            });
            GameStatus.Add(new GameStatus
            {
                Status = "E",
                StatusText = "結束"
            });
            return GameStatus;
        }

        /// <summary>
        /// 取得赛事状态及文字
        /// </summary>
        /// <returns></returns>
        public static List<GameStatus> GetAllGameStatus()
        {
            List<GameStatus> GameStatus = new List<GameStatus>();
            GameStatus.Add(new GameStatus
            {
                Status = "X",
                StatusText = "未開賽"
            });
            GameStatus.Add(new GameStatus
            {
                Status = "S",
                StatusText = "已開賽"
            });
            GameStatus.Add(new GameStatus
            {
                Status = "E",
                StatusText = "結束"
            });
            GameStatus.Add(new GameStatus
          {
              Status = "D",
              StatusText = "延遲"
          });
            GameStatus.Add(new GameStatus
          {
              Status = "P",
              StatusText = "中止"
          });
            GameStatus.Add(new GameStatus
          {
              Status = "C",
              StatusText = "取消"
          });
            return GameStatus;
        }

        /// <summary>
        /// 取得修改比分选项
        /// </summary>
        /// <returns></returns>
        public static List<ModifyItem> GetModifyItems()
        {
            List<ModifyItem> ModifyItem = new List<ModifyItem>();
            ModifyItem.Add(new ModifyItem
            {
                ItemValue = 1,
                ItemText = "狀態"
            });
            ModifyItem.Add(new ModifyItem
            {
                ItemValue = 2,
                ItemText = "開賽時間"
            });
            ModifyItem.Add(new ModifyItem
            {
                ItemValue = 3,
                ItemText = "比分"
            });
            ModifyItem.Add(new ModifyItem
            {
                ItemValue = 4,
                ItemText = "全部"
            });

            return ModifyItem;
        }
        public static string GetModifyItemsText(int itemValue)
        {
            string ItemText = String.Empty;
            switch (itemValue)
            {
                case 1:
                    ItemText = "狀態";
                    break;
                case 2:
                    ItemText = "開賽時間";
                    break;
                case 3:
                    ItemText = "比分";
                    break;
                case 4:
                    ItemText = "全部";
                    break;
                case 5:
                    ItemText = "刪除";
                    break;

            }
            return ItemText;
        }
        /// <summary>
        /// 取得狀態文字翻譯
        /// </summary>
        /// <param name="state">狀態</param>
        /// <returns>狀態文字</returns>
        public static string GetStatesText(string state)
        {
            string text = String.Empty;

            switch (state)
            {
                case "X":
                    text = "未開賽";
                    break;
                case "E":
                    text = "結束";
                    break;
                case "D":
                    text = "延遲";
                    break;
                case "P":
                    text = "中止";
                    break;
                case "S":
                    text = "已開賽";
                    break;
                case "C":
                    text = "取消";
                    break;
                default:
                    text = state;
                    break;
            }

            return text;
        }

        public static string GetCtrlStateText(int ctrlStates, string ctrlAdmin, string webID)
        {
            string ctrlState = string.Empty;
            switch (ctrlStates)
            {
                case 1:
                    if (string.IsNullOrEmpty(ctrlAdmin))
                    {
                        ctrlState = "操盤中";
                    }
                    else
                    {
                        ctrlState = ctrlAdmin;
                    }
                    break;
                case 2:
                    ctrlState = String.Format("自動操盤({0})", webID);
                    break;
                case 3:
                case 4:
                    ctrlState = "半自半操";
                    break;
                default:
                    ctrlState = "未操盤";
                    break;
            }
            return ctrlState;
        }
        public static List<CtrlStatus> GetCtrlState(bool isBasketball = false)
        {
            List<CtrlStatus> ModifyItem = new List<CtrlStatus>();
            ModifyItem.Add(new CtrlStatus
            {
                CtrlValue = "0",
                CtrlText = "未操盤"
            });
            ModifyItem.Add(new CtrlStatus
            {
                CtrlValue = "1",
                CtrlText = "操盤中"
            });
            ModifyItem.Add(new CtrlStatus
            {
                CtrlValue = "2",
                CtrlText = "自動操盤"
            });
            if (isBasketball)
            {
                ModifyItem.Add(new CtrlStatus
                {
                    CtrlValue = "3",
                    CtrlText = "半操盤"
                });
            }
            return ModifyItem;
        }
        #region Title對應表
        /// <summary>
        /// Title對應表
        /// </summary>
        /// <param name="code">Title代碼</param>
        /// <returns>Title中文名稱</returns>
        public static string GetGameTypeName(string code)
        {
            string gameName = code;
            code = code.ToUpper();
            switch (code)
            {
                #region 棒球
                case "BBUS":
                    gameName = "美國職棒";
                    break;
                case "BB3A":
                    gameName = "美國職棒3A聯盟";
                    break;
                case "BB3AIL":
                    gameName = "美國職棒 3A 國際聯盟";
                    break;
                case "BB3APCL":
                    gameName = "美國職棒 3A 太平洋岸聯盟";
                    break;
                case "BBJP":
                    gameName = "日本職棒";
                    break;
                case "BBTW":
                    gameName = "台灣職棒";
                    break;
                case "BBTW2":
                    gameName = "世界少棒錦標賽";
                    break;
                case "BBTW3":
                    gameName = "U18世界盃棒球賽";
                    break;
                case "BBKR":
                    gameName = "韓國職棒";
                    break;
                case "BBMX":
                    gameName = "墨西哥冬季聯盟";
                    break;
                case "BBMX2":
                    gameName = "墨西哥夏季聯盟";
                    break;
                case "BBAU":
                    gameName = "澳洲棒球聯盟(ABL)";
                    break;
                case "BBTW7":
                    return "PL 爆米花夏季棒球聯盟";
                case "BBTW4":
                    return "2013 經典棒球隊抗賽";
                case "BBTW5":
                    return "2013 亞洲職棒大賽";
                case "BBTW6":
                    return "2013 亞洲冬季棒球聯盟";
                case "BBTW8":
                    return "亞運會棒球";
                case "BBNL":
                    return "荷兰棒球";
                case"BBQT":
                    return "其他棒球";
                #endregion
                #region 籃球
                case "BKUS":
                    gameName = "美國職籃 - 男子";
                    break;
                case "BKUSW":
                    gameName = "美國職籃 - 女子";
                    break;
                case "BKJP":
                    gameName = "日本職籃";
                    break;
                case "BKEL":
                    gameName = "歐聯職籃";
                    break;
                case "BKEL2":
                    gameName = "歐洲籃球聯盟盃";
                    break;
                case "BKKR":
                    gameName = "韓國職籃 - 男子";
                    break;
                case "BKKRW":
                    gameName = "韓國職籃 - 女子";
                    break;
                case "BKCN":
                    gameName = "中國職籃";
                    break;
                case "BKTW":
                    gameName = "台灣職籃";
                    break;
                case "BKAU":
                    gameName = "澳洲職籃";
                    break;
                case "BKVTB":
                    gameName = "VTB籃球聯賽";
                    break;
                case "BKFIBA":
                    gameName = "亞洲籃球錦標賽 (FIBA)";
                    break;
                case "BKEBT":
                    gameName = "歐洲籃球錦標賽 (FIBA)";
                    break;
                case "BKOS":
                    gameName = "奧訊籃球";
                    break;
                case "BKBF":
                    gameName = "BF籃球";
                    break;
                case "BKNCAA":
                    gameName = "美國大學男子籃球";
                    break;
                case "BKBBL":
                    gameName = "德國籃球甲級聯賽(BBL)";
                    break;

                #endregion
                #region 足球
                case "SB":
                    gameName = "足球";
                    break;
                #endregion
                #region 美足
                case "AFUS":
                    gameName = "美式足球";
                    break;
                #endregion
                #region 冰球
                case "IHUS":
                    gameName = "國家冰球";
                    break;
                case "IHRU":
                    gameName = "俄羅斯冰球";
                    break;
                case "IHUS2":
                    gameName = "美国冰球";
                    break;
                case "IHBF":
                    gameName = "BF冰球";
                    break;
                #endregion
                #region 網球
                case "TNAU":
                    gameName = "澳大立亞網球公開賽";
                    break;
                case "TNFR":
                    gameName = "法國網球公開賽";
                    break;
                case "TNWC":
                    gameName = "溫布爾頓網球公開賽";
                    break;
                case "TNUS":
                    gameName = "美國網球公開賽";
                    break;
                case "TNDC":
                    gameName = "台維斯盃";
                    break;
                case "TNWM":
                    gameName = "網球大師公開賽";
                    break;
                case "TNWA":
                    gameName = "女子網球聯合會";
                    break;
                case "TN":
                    gameName = "網球";
                    break;
                #endregion
                #region 終極格鬥
                case "UFC":
                    gameName = "終極格鬥";
                    break;
                #endregion
                case "AAAA":
                    gameName = "全部";
                    break;
                case "ACMA":
                    gameName = "帳號管理";
                    break;
                case "MARQUEE":
                    gameName = "讯息管理";
                    break;
                case "NAME":
                    gameName = "名稱對照";
                    break;
                default :
                    break;
            }
            return gameName;
        }
        #endregion

        #region 获取员工等级描述
        /// <summary>
        /// 获取员工等级描述
        /// </summary>
        /// <param name="rank">等级代码</param>
        /// <returns>返回等级描述</returns>
        public static string GetEmployeeRankName(string rank)
        {
            string rankName = string.Empty;
            switch (rank)
            {
                case "9":
                    rankName = "工程師";
                    break;
                case "4":
                    rankName = "建資料";
                    break;
                case "3":
                    rankName = "管理者";
                    break;
                case "1":
                    rankName = "操盤員";
                    break;
            }
            return rankName;
        }
        #endregion


        public static string GetImagePath(string gameType)
        {
            return string.Format(WebConfigurationManager.AppSettings["ImagePath"], gameType.Substring(0, 2));
        }
        public static string GetCommercialImagePath()
        {
            return WebConfigurationManager.AppSettings["CommercialImage"];
        }
        public static string GetTeamImagePath(string gameType, int teamID)
        {
            return Path.Combine(GetImagePath(gameType), teamID + ".png");
        }
        public static void UpLoadImage(string path, Stream stream, string imagePath)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            System.Drawing.Image.GetThumbnailImageAbort callBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
            Bitmap image = new Bitmap(stream);
            // 計算維持比例的縮圖大小
            int[] thumbnailScale = getThumbnailImageScale(170, 73, image.Width, image.Height);
            // 產生縮圖
            System.Drawing.Image smallImage = image.GetThumbnailImage(thumbnailScale[0], thumbnailScale[1], callBack, IntPtr.Zero);
            if (File.Exists(imagePath)) { File.Delete(imagePath); }
            // 將縮圖存檔
            smallImage.Save(imagePath);
        }
        public static string GetBase64StringForImage(string imageName)
        {
            string path = Path.Combine(GetCommercialImagePath(), imageName);
            byte[] imgData = null;
            string base64String = string.Empty;
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        imgData = br.ReadBytes((int)fs.Length);
                    }
                }
                // 轉換成 base64
                if (imgData != null && imgData.Length > 0)
                {
                    base64String = Convert.ToBase64String(imgData, 0, imgData.Length);
                }
                if (!String.IsNullOrEmpty(base64String))
                {
                    base64String = "data:image/jpg;base64," + base64String;
                }
            }
            return base64String;
        }
        private static bool ThumbnailCallback()
        {
            return false;
        }
        // 計算維持比例的縮圖大小
        private static int[] getThumbnailImageScale(int maxWidth, int maxHeight, int oldWidth, int oldHeight)
        {
            int[] result = new int[] { 0, 0 };
            float widthDividend, heightDividend, commonDividend;

            widthDividend = (float)oldWidth / (float)maxWidth;
            heightDividend = (float)oldHeight / (float)maxHeight;

            commonDividend = (heightDividend > widthDividend) ? heightDividend : widthDividend;
            result[0] = (int)(oldWidth / commonDividend);
            result[1] = (int)(oldHeight / commonDividend);

            return result;
        }
        //產生亂數檔名
        public static string CreateRandomCode(int number)
        {
            //產生亂數檔名
            string allChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < number; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(36);
                if (temp != -1 && temp == t)
                {
                    return CreateRandomCode(number);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }

        public static List<string> GetAreaAndController(string gameType)
        {

            List<string> list = new List<string>();
            switch (gameType)
            {
                #region  篮球
                case "BKUS":
                case "BKUSW":
                case "BKNCAA":
                case "BKJP":
                case "BKEL":
                case "BKEL2":
                case "BKKR":
                case "BKKRW":
                case "BKCN":
                case "BKTW":
                case "BKAU":
                case "BKACB":
                case "BKCR":
                case "BKBBL":
                case "BKVTB":
                case "BKFIBA":
                case "BKEBT":
                    list.Add("Basketball");//Area
                    list.Add("Log");//Controller
                    break;
                #endregion
                #region 奥讯篮球
                case "BKOS":
                    list.Add("Basketball");
                    list.Add("BKOSLog");
                    break;
                #endregion
                #region 棒球
                case "BBUS":
                case "BB3A":
                case "BB3AIL":
                case "BB3APCL":
                case "BBJP":
                case "BBTW":
                case "BBTW7":
                case "BBKR":
                case "BBMX":
                case "BBMX2":
                case "BBAU":
                case "BBTW2":
                case "BBTW3":
                case "BBTW4":
                case "BBTW5":
                case "BBTW6":
                case "BBTW8":
                case "BBNL":
                case "BBQT":
                    list.Add("Baseball");
                    list.Add("Log");
                    break;
                #endregion
                #region 足球
                case "SB":
                    list.Add("Football");
                    list.Add("Log");
                    break;
                #endregion
                #region 美足
                case "AFUS"://美足
                    list.Add("AmericanFootball");
                    list.Add("Log");
                    break;
                #endregion
                #region 冰球
                case "IHUS":
                case "IHRU":
                case "IHUS2":
                case "IHBF"://BF冰球
                    list.Add("IceHockey");
                    list.Add("Log");
                    break;
                #endregion
                //#region BF冰球
                //    list.Add("IceHockey");
                //    list.Add("BFLog");
                //    break;
                //#endregion
                #region 网球
                case "TN":
                case "TNAU":
                case "TNFR":
                case "TNWC":
                case "TNUS":
                case "TNDC":
                case "TNWM":
                case "TNWA":
                    list.Add("Tennis");
                    list.Add("Log");
                    break;
                #endregion
                #region 帐号管理
                case "AcMa":
                    list.Add("Employee");
                    list.Add("Log");
                    break;
                #endregion
                #region 系统设定
                case "Marquee"://讯息管理
                case "NameControl":
                case"IPAccess":
                case"SetTypeVal"://修改首页
                    list.Add("SystemSet");
                    list.Add("Log");
                    break;
                #endregion
            }
            return list;
        }
        public static string GetAction(int itemCategory)
        {
            switch (itemCategory)
            {
                case 1:
                    return "Alliance";
                case 2:
                    return "Team";
                case 4:
                    return "Schedule";
                case 5:
                    return "Account";
                case 6:
                    return "Message";
                case 7:
                    return "IpAddress";
                case 8:
                    return "Name";
                case 9:
                    return "SetTypeVal";
            }
            return string.Empty;
        }

        public static string GetMarqueeText(string gameType)
        {
            switch (gameType)
            {
                case "AAAA":

                    return "全站公告";
                case "BB":

                    return "棒球公告";
                case "BK":

                    return "籃球公告";
                case "SB":

                    return "足球公告";
                case "AF":

                    return "美足公告";
                case "IH":
                    return "冰球公告";
                default:
                    return gameType;
            }
        }

        public static string GetExerciseType(string type)
        { 
            switch(type)
            {
                case "BB": return "國際棒球";
                case "TW": return "台棒";
                case "BK": return "籃球";
                case "SB": return "足球";
                case "AF": return "美足";
                case "IH": return "冰球";
                case "TN": return "網球";
                case "OT": return "其他運動";
                default: return type;
            }
        }
        public static string GetLangText(string code)
        {
            switch (code)
            {
                case "en":
                    return "English";
                case "tw":
                    return "繁體";
                case "cn":
                    return "简体";
                case "ko":
                    return "한국의";
                case "ja":
                    return "日本の";
                default:
                    return code;
            }
        }
        public static string GetCategoryText(string code)
        {
            switch (code)
            {
                case "0":
                    return "遊戲";
                case "1":
                    return "姓";
                case "2":
                    return "名";
                default:
                    return code;
            }
        }
        public static string GetAppTypeText(string code)
        {
            switch (code)
            {
                case "Content":
                    return "內容";
                case "TeamName":
                    return "隊名";
                case "First":
                    return "First";
                case "Last":
                    return "Last";
                default:
                    return code;
            }
        }
        public static string GetScorllingTypeText(int code)
        {
            switch (code)
            {
                case 1:
                    return "電腦版";
                case 2:
                    return "手機版";
                default:
                    return code.ToString();
            }
        }
        public static void WriteLog(string path,string content)
        {

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd-HH-mm")+".txt");
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(content);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
        public static string GetLanguageText(string  code)
        {
            switch (code)
            {
                case "zh-tw":
                    return "繁體中文";
                case "zh-cn":
                    return "簡體中文";
                default:
                    return code;
            }
        }

        public static string GetVisibleText(int code)
        {
            switch (code)
            {
                case 0:
                    return "不啟用";
                case 1:
                    return "啟用";
                default:
                    return code.ToString();
            }
        }
        public static string GetOrderByText(int code)
        {
            switch (code)
            {
                case 1:
                    return "最優先";
                case 2:
                    return "一般";
                case 3:
                    return "低";
                default:
                    return code.ToString();
            }
        }
    }


    public class GameStatus
    {
        public string Status { get; set; }
        public string StatusText { get; set; }

    }
    public class CtrlStatus
    {
        public string CtrlValue { get; set; }
        public string CtrlText { get; set; }

    }
    public class ModifyItem
    {
        public int ItemValue { get; set; }
        public string ItemText { get; set; }

    }

    public enum ActionItem : int
    {
        AllAction,
        Add,
        Delete,
        Update,

    }
    public enum CategoryItem : int
    {
        AllCategory,
        Alliance,
        Team,
        TeamMember,
        Schedule,
        Account,
        Message,
        IP,
        NameList,
        HomePage,
    }

    /// <summary>
    /// 赛程资料中的存储选项
    /// </summary>
    public enum SetItem : int
    {
        NULL,
        /// <summary>
        /// 删除选择项目
        /// </summary>
        DeleteSelectionProject,
        /// <summary>
        /// 存储补赛设定
        /// </summary>
        StorageReschedulingSetting,
        /// <summary>
        /// 存储走势图设定
        /// </summary>
        ChartStorageSet,
    }
}
