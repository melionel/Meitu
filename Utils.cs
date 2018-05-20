using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meitu
{
    public class Utils
    {
        public static List<Goods> PredefinedGoodsList = new List<Goods>
        {
            new Goods("Cosme Decorte黛珂AQ 肌耀未来洁面乳130g", 198, 0, 0, "", "", "Decorte"), // 0
            new Goods("Cosme Decorte黛珂蜂蜜滋养深层卸妆膏170g", 208, 0, 0, "", "", "Decorte"), // 1
            new Goods("Cosme Decorte黛珂高机能紫苏化妆水150ml", 324, 0, 0, "", "", "Decorte"), // 2
            new Goods("Cosme Decorte黛珂AQMW白檀舞蝶蜜粉20g#10", 348, 0, 0, "", "", "Decorte"), // 3
            new Goods("Cosme Decorte黛珂保湿赋活眼霜15g", 498, 0, 0, "", "", "Decorte"), // 4
            new Goods("Cosme Decorte黛珂AQMW白檀平衡化妆水200ml", 598, 0, 0, "", "", "Decorte"), // 5
            new Goods("Cosme Decorte黛珂完美精致保湿洁面露200ml", 658, 0, 0, "", "", "Decorte"), // 6
            new Goods("Laneige兰芝4色眼影盘9g 璀璨花火节日限量版", 188, 0, 0, "", "", "Laneige"), // 7
            new Goods("Givency 纪梵希小羊皮细管唇膏2.2g#204 Rose Perfecto", 239, 0, 0, "", "", "Givency"), // 8
            new Goods("VDL修颜妆前乳/隔离霜SPF32 PA+++30ml #02粉色", 115, 12.88, 0, "", "", "VDL"), // 9
            new Goods("Kiehl's 科颜氏金盏花爽肤水250ml 节日限量版", 278, 31.14, 0, "", "", "Kiehls"), // 10
            new Goods("REAL TECHNIQUES 斜角修容刷", 58, 6.5, 0, "", "", "REAL TECHNIQUES"), // 11
            new Goods("HONEY TRAP赫妮特腮红刷", 49, 0, 0, "", "", "HONEY TRAP"), // 12
            new Goods("Foreo Luna 露娜 电动洁面仪Mini2粉色 关晓彤同款", 888, 0, 0, "", "", "Foreo Luna", GoodsType.SkinCare), // 13
            new Goods("Shiseido资生堂 安耐晒防晒露SPF50+PA++++60ml 2018版", 189, 21.17, 0, "", "", "Shiseido", GoodsType.SkinCare), // 14
            new Goods("PHILIPS 飞利浦钻石亮白声波牙刷 HX9362/67粉色", 948, 0, 0, "", "", "PHILIPS", GoodsType.HealthCare), // 15
            new Goods("Shiseido资生堂 Fino发膜230g*2件", 124, 0, 0, "", "", "Shiseido"), // 16
            new Goods("YSL圣罗兰迷魅纯漾口红3.8g#01方管", 298, 0, 0, "", "", "YSL"), // 17
            new Goods("YSL圣罗兰迷魅纯漾口红3.8g#17方管", 288, 0, 0, "", "", "YSL"), //18
            new Goods("YSL圣罗兰纯色唇釉6ml#202", 288, 0, 0, "", "", "YSL") // 19
        };
 
        public static List<Coupon> PredefinedCouponsList = new List<Coupon>
        {
            // Decorte, 999 - 200
            new Coupon(200, 999, new List<Goods>{}, (goods, time) => (goods.Brand == "Decorte")),
            // Decorte, 399 - 80
            new Coupon(80, 399, new List<Goods>{PredefinedGoodsList[0], PredefinedGoodsList[1], PredefinedGoodsList[3], PredefinedGoodsList[5]}, (goods, time) => (goods.Brand == "Decorte")),
            // 会员5月专享， 599 - 120
            new Coupon(120, 599, new List<Goods>{PredefinedGoodsList[10], PredefinedGoodsList[18], PredefinedGoodsList[19], PredefinedGoodsList[17]}, (goods, time) => (time < new DateTime(2018, 6, 1) && time > new DateTime(2018, 4, 30))),
            // 调研福利券， 299 - 20
            new Coupon(20, 299, new List<Goods>{}, (goods, time) => (true)),
            // 无门槛优惠券， 10 - 5
            new Coupon(5, 10, new List<Goods>{}, (goods, time) => (true)),
            // 节日优惠券，母亲节彩妆， 399 - 50
            new Coupon(50, 399, new List<Goods>{}, (goods, time) => (time.Date == new DateTime(2018, 5, 13).Date && new List<Goods>{PredefinedGoodsList[8],PredefinedGoodsList[9]}.Contains(goods))),
            // 节日优惠券，母亲节护肤， 499 - 60
            new Coupon(60, 499, new List<Goods>{}, (goods, time) => (time.Date == new DateTime(2018, 5, 13).Date && new List<Goods>{PredefinedGoodsList[13]}.Contains(goods))),
            // 节日优惠券，母亲节个护，999 - 120
            new Coupon(120, 999, new List<Goods>{PredefinedGoodsList[16]}, (goods, time) => (time.Date == new DateTime(2018, 5, 13).Date && new List<Goods>{PredefinedGoodsList[15],PredefinedGoodsList[16]}.Contains(goods))),
            // 节日优惠券，母亲节彩妆，999 - 120
            new Coupon(120, 999, new List<Goods>{PredefinedGoodsList[16]}, (goods, time) => (time.Date == new DateTime(2018, 5, 13).Date && new List<Goods>{PredefinedGoodsList[17],PredefinedGoodsList[18],PredefinedGoodsList[19]}.Contains(goods))),
        }; 

        public static void PrintOneOrder(Order order)
        {
            Console.WriteLine("All goods:");
            foreach (var g in order.GoodsList)
            {
                Console.WriteLine(g.ToString());
            }

            if (order.RecomendedGoodsList.Count > 0)
            {
                Console.WriteLine("Recomended goods:");
                foreach (var g in order.RecomendedGoodsList)
                {
                    Console.WriteLine(g.ToString());
                }
            }

            if (order.CouponList.Count == 0)
            {
                Console.WriteLine("No coupon used.");
            }
            else
            {
                Console.WriteLine("All coupons:");
                foreach (var c in order.CouponList)
                {
                    Console.WriteLine(c.ToString());
                }
            }

            Console.WriteLine("Order total price:" + order.GetTotalPrice());
            Console.WriteLine("Order total discount:" + order.GetTotalDiscount());
            Console.WriteLine("Order total tax:" + order.GetTotalTax());
            Console.WriteLine("Order total income:" + order.GetTotalIncome());
            Console.WriteLine("Order final price:" + order.GetFinalPrice());
            Console.WriteLine(string.Format("Order discount rate:{0:0.000}", order.GetDiscountRate()));
        }
    }
}
